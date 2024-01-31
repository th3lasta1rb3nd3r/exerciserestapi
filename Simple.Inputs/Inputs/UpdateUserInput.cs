namespace Simple.Model.Inputs;

public record UpdateUserInput(int UserId, UserInput User, AddressInput? Address, List<EmploymentInput>? Employments);
