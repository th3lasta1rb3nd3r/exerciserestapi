using Simple.Model.Errors;
using Simple.Model.Inputs;
using Simple.Model;
using System.Text.RegularExpressions;

namespace Simple.Service.Extensions;

internal static class UserInputValidator
{
    internal static List<IErrorWithMessage> Validate(this UserInput input)
    {
        var errors = new List<IErrorWithMessage>();

        if (input.FirstName is null)
        {
            errors.Add(new EmptyFieldError(nameof(input.FirstName)));
        }
        if (input.LastName is null)
        {
            errors.Add(new EmptyFieldError(nameof(input.LastName)));
        }

        if (input.EmailAddress is null)
        {
            errors.Add(new EmptyFieldError(nameof(input.EmailAddress)));
        }
        else if (!IsValidEmail(input.EmailAddress))
        {
            errors.Add(new InvalidEmailAddressError());
        }

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        Regex regex = new(pattern);
        return regex.IsMatch(email);
    }
}
