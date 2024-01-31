using Simple.Data;
using Simple.Model;
using Simple.Model.Errors;
using Simple.Model.Inputs;

namespace Simple.Service.Extensions;

internal static class AddUserInputValidator
{
    internal static List<IErrorWithMessage> Validate(this AddUserInput input, SimpleDataDbContext sampleDataDbContext)
    {
        if (input.User == null)
        {
            return [new EmptyFieldError(nameof(input.User))];
        }

        var errors = input.User.Validate();

        if (input.User.EmailAddress is not null &&
            sampleDataDbContext.Users.Any(e => e.EmailAddress == input.User.EmailAddress))
        {
            errors.Add(new DuplicateEmailAddessError());
        }

        if (input.Address is not null)
        {
            errors.AddRange(input.Address.Validate());
        }

        if (input.Employments is not null)
        {
            errors.AddRange(input.Employments.Validate());
        }

        return errors;
    }
}
