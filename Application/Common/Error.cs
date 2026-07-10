namespace TicketHub.Application.Common;

public static class Error
{
    public const string IdIsEmptyError = "Id is empty";
    public const string NotFoundError = "Entity not found";
    public const string CreationError = "Could not create entity";
    public const string Invalid = "Entity is either expired or inactive";
}
