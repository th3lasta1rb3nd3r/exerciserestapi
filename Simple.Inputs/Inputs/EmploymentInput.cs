namespace Simple.Model.Inputs;

public record EmploymentInput(string Company, uint MonthsOfExperience, double Salary, DateTime StartDate, DateTime? EndDate);