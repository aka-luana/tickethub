# TicketHub — Plano de Refatoração

## Objetivo do projeto

Estudar estratégias de controle de concorrência (locks) usando o caso clássico do Ticketmaster:
múltiplos usuários tentando comprar o mesmo ingresso ao mesmo tempo.

Após a base estar consolidada, o plano é:
- Fazer um front simples gerado com IA
- Rodar testes de carga (k6/Gatling) simulando compra simultânea do mesmo ingresso
- Observar e comparar o comportamento de cada estratégia de lock

---

## Diagnóstico do estado atual

### O que está bom
- Estrutura Clean Architecture + CQRS com MediatR bem organizada
- Padrão `Result<T>` para tratamento de erros sem exceptions
- DTOs com DataAnnotations
- Controllers REST versionados

### Problemas críticos

**1. Entidades com responsabilidades confusas**

Existem dois conceitos de "reserva" sobrepostos e usados inconsistentemente:
- `Reservation` com `OrderStatus` (Pending/Paid/Cancelled) → parece um pedido final
- `TicketReservation` com `ReservationStatus` (Reserved/Confirmed/Expired) → parece um hold temporário

O fluxo correto do Ticketmaster é: **Hold temporário → Pagamento → Order confirmada**.
Precisamos tornar isso explícito nas entidades.

**2. Nada persiste nada**
- `EventRepository`: todos os métodos jogam `NotImplementedException`
- Sem banco, sem in-memory, nada
- `IReservationRepository` nem está registrado no DI

**3. Handlers de reserva não implementados**
- `CreateReservationCommandHandler` joga `null` (literalmente)
- `ConfirmReservationCommandHandler` e `CancelReservationCommandHandler` idem

**4. Sem camada de concorrência** — o core do estudo
- `TicketType.AvailableQuantity` não tem proteção alguma contra race condition
- Não há mecanismo de lock (otimista, pessimista ou distribuído)

---

## Fluxo de domínio clarificado

```
POST /reservations/hold      → cria SeatHold (10min TTL, decrementa AvailableQuantity)
POST /reservations/confirm   → SeatHold vira Order (pagamento confirmado)
DELETE /reservations/{id}    → libera SeatHold, devolve AvailableQuantity
[Background Job]             → expira SeatHolds vencidos, devolve AvailableQuantity
```

---

## Sequência de implementação

### Passo 1 — Renomear entidades

| Antes | Depois | Motivo |
|---|---|---|
| `TicketReservation` | `SeatHold` | Nome mais claro para o hold temporário |
| `Reservation` | `Order` | É a compra confirmada, não uma reserva |
| `ReservationStatus` | `SeatHoldStatus` | Status: Active / Expired / Converted |
| `OrderStatus` | mantém | Pending / Paid / Cancelled faz sentido |

Atualizar todas as referências: handlers, repositórios, controllers, DTOs.

---

### Passo 2 — EF Core + PostgreSQL

Adicionar os pacotes:
```
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.EntityFrameworkCore.Design
```

Criar `TicketHubDbContext` em `Infrastructure/` com:
- `DbSet<Event>`
- `DbSet<TicketType>`
- `DbSet<SeatHold>`
- `DbSet<Order>`
- `DbSet<User>`

Adicionar **concurrency token** em `TicketType`:
```csharp
public uint Version { get; set; } // xmin do PostgreSQL — lock otimista
```

Configurar no `OnModelCreating` e rodar a primeira migration.

---

### Passo 3 — Implementar EventRepository

Substituir os `NotImplementedException` por implementações reais usando o `DbContext`.

Métodos:
- `GetByIdAsync` → `FindAsync`
- `GetAllAsync` → `ToListAsync`
- `AddAsync` → `AddAsync` + `SaveChangesAsync`
- `UpdateAsync` → `Update` + `SaveChangesAsync`

---

### Passo 4 — Criar e implementar SeatHoldRepository e OrderRepository

Substituir `IReservationRepository` por duas interfaces separadas:
- `ISeatHoldRepository` com: `GetByIdAsync`, `CreateAsync`, `ExpireAsync`
- `IOrderRepository` com: `GetByIdAsync`, `CreateAsync`, `CancelAsync`

Implementar ambas com EF Core.
Registrar no DI em `Program.cs`.

---

### Passo 5 — Implementar os handlers de reserva (baseline sem lock)

Começar com **nenhuma proteção de concorrência** — esse é o baseline que vai "quebrar" nos testes de carga.

`CreateSeatHoldCommandHandler`:
1. Busca o `TicketType` pelo id
2. Verifica se `AvailableQuantity >= quantity`
3. Decrementa `AvailableQuantity`
4. Cria o `SeatHold` com `ExpiresAt = now + 10min`
5. Salva tudo

`ConfirmSeatHoldCommandHandler`:
1. Busca o `SeatHold`
2. Valida que não está expirado
3. Cria a `Order` com status `Pending`
4. Marca o `SeatHold` como `Converted`
5. Salva tudo

`CancelSeatHoldCommandHandler`:
1. Busca o `SeatHold`
2. Devolve a `AvailableQuantity` ao `TicketType`
3. Marca o `SeatHold` como `Expired`
4. Salva tudo

---

### Passo 6 — Background Job de expiração

Criar `SeatHoldExpirationService : BackgroundService` em `Infrastructure/`:
- Roda a cada 30 segundos
- Busca `SeatHold` com `Status == Active && ExpiresAt < DateTime.UtcNow`
- Para cada um: devolve `AvailableQuantity` ao `TicketType`, marca como `Expired`
- Salva as alterações

Registrar em `Program.cs` com `AddHostedService<SeatHoldExpirationService>()`.

---

### Passo 7 — Response DTOs

Controllers hoje retornam entidades de domínio diretamente. Criar DTOs de resposta para:
- `EventResponseDTO`
- `SeatHoldResponseDTO`
- `OrderResponseDTO`

Mapear manualmente dentro dos handlers ou controllers (sem AutoMapper por ora).

---

### Passo 8 — Middleware de exception handling

Criar um `ExceptionHandlingMiddleware` em `Api/` que:
- Captura exceptions não tratadas
- Retorna `ProblemDetails` padronizado (RFC 7807)
- Loga o erro

---

### Passo 9 — Estratégias de lock (o estudo em si)

Com a base funcionando e os testes de carga quebrando o baseline, implementar e comparar:

#### Estratégia A: Sem lock (baseline)
O que já foi feito no Passo 5. Vai vender mais ingressos do que existe.

#### Estratégia B: Lock Otimista
- EF Core usa o `xmin` do PostgreSQL como concurrency token em `TicketType`
- Se dois updates colidem, EF lança `DbUpdateConcurrencyException`
- Handler faz retry com backoff
- **Custo**: retries sob alta carga. **Ganho**: sem bloqueio de linha no banco.

#### Estratégia C: Lock Pessimista
- Antes de decrementar, executa `SELECT FOR UPDATE` via raw SQL
- Bloqueia a linha de `TicketType` enquanto a transação está aberta
- Outras requisições ficam em fila no banco
- **Custo**: throughput menor. **Ganho**: zero conflitos, zero retries.

#### Estratégia D (bônus): Redis Distributed Lock
- Usar `StackExchange.Redis` para adquirir lock antes de qualquer operação
- Útil quando há múltiplas instâncias da API
- **Custo**: dependência externa, latência de rede. **Ganho**: escala horizontalmente.

---

## O que NÃO mudar

- Estrutura de pastas Clean Architecture (está bem)
- Padrão `Result<T>` (está bom)
- CQRS com MediatR (está bom)
- Naming conventions dos controllers e rotas

---

## Stack final

- .NET 10
- PostgreSQL
- Entity Framework Core (Npgsql)
- MediatR
- Swagger
- k6 ou Gatling para testes de carga (fase final)
