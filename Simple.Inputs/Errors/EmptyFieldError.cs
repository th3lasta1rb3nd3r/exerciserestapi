namespace Simple.Model.Errors;

public record EmptyFieldError(string FieldName) : IErrorWithMessage
{
    public string Message => $"{FieldName} is field.";
}
