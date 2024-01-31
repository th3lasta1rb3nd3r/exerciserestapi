namespace Simple.Model.Errors;

public record InvalidEmailAddressError() : IErrorWithMessage
{
    public string Message => "Invalid email address.";
}
