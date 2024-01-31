using Simple.Model.Errors;
using Simple.Model.Inputs;
using Simple.Model;

namespace Simple.Service.Extensions;

internal static class EmploymentInputValidator
{
    internal static List<IErrorWithMessage> Validate(this List<EmploymentInput> inputs)
    {
        var errors = new List<IErrorWithMessage>();

        foreach (var input in inputs)
        {
            if (input.Company is null)
            {
                errors.Add(new EmptyFieldError(nameof(input.Company)));
            }

            if (input.MonthsOfExperience == 0)
            {
                errors.Add(new EmptyFieldError(nameof(input.MonthsOfExperience)));
            }

            if (input.Salary == 0)
            {
                errors.Add(new EmptyFieldError(nameof(input.Salary)));
            }

            if (input.StartDate == DateTime.MinValue)
            {
                errors.Add(new EmptyFieldError(nameof(input.StartDate)));
            }

            if (input.EndDate is not null && input.StartDate > input.EndDate)
            {
                errors.Add(new WrongDateRangeError(nameof(input.StartDate), nameof(input.EndDate)));
            }
        }

        return errors;
    }
}
