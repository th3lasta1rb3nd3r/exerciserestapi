namespace Simple.Model.Errors;

public record class EmploymentNotFoundError() : IErrorWithMessage
{
    public string Message => $"Employment not found.";
}
