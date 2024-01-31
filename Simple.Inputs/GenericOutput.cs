namespace Simple.Model
{
    public record GenericOutput<T>(List<IErrorWithMessage>? Errors, T Value);
}
