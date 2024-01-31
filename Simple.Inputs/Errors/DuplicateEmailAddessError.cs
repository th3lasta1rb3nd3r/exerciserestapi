namespace Simple.Model.Errors;

public record class DuplicateEmailAddessError() : IErrorWithMessage
{
    public string Message => "Duplicate email address.";
}
