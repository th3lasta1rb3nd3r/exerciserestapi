namespace Simple.Model.Errors;

public record class AddressNotFoundError() : IErrorWithMessage
{
    public string Message => $"Address not found.";
}
