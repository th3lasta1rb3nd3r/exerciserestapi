namespace Simple.Model.Errors;

public record class UserNotFoundError() : IErrorWithMessage
{
    public string Message => $"User not found.";
}
