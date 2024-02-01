namespace Simple.Model.Inputs;

public record EmploymentInput(string Company, short MonthsOfExperience, double Salary, DateTime StartDate, DateTime? EndDate);