namespace Simple.Model.Errors;

public record class WrongDateRangeError(string StartDateName, string EndDateName) : IErrorWithMessage
{
    public string Message => $"{StartDateName} must be greater than {EndDateName}.";
}
