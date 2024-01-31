using Simple.Model.Errors;
using Simple.Model.Inputs;
using Simple.Model;

namespace Simple.Service.Extensions;

internal static class AddressInputValidator
{
    internal static List<IErrorWithMessage> Validate(this AddressInput input)
    {
        var errors = new List<IErrorWithMessage>();

        if (input.Street is null)
        {
            errors.Add(new EmptyFieldError(nameof(input.Street)));
        }
        if (input.City is null)
        {
            errors.Add(new EmptyFieldError(nameof(input.City)));
        }

        return errors;
    }
}
