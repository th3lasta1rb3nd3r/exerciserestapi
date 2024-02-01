using Simple.Model.Errors;
using Simple.Model.Inputs;
using Simple.Model;
using Simple.Data;

namespace Simple.Service.Extensions;

internal static class UpdateUserInputValidator
{
    internal static List<IErrorWithMessage> Validate(this UpdateUserInput input, SimpleDataDbContext simpleDataDbContext)
    {
        if (input is null)
        {
            return [new EmptyFieldError(nameof(input))];
        }

        var errors = new List<IErrorWithMessage>();
        if (!simpleDataDbContext.Users.Any(e => e.Id == input.UserId))
        {
            errors.Add(new UserNotFoundError());
        }
        else
        {
            errors.AddRange(input.User.Validate());
        }

        if(input.User.EmailAddress is not null && 
            simpleDataDbContext.Users.Any(e => e.Id != input.UserId && e.EmailAddress == input.User.EmailAddress))
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
