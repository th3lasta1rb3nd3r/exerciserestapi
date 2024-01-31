namespace Simple.Model.Inputs;

public record AddUserInput(UserInput User, AddressInput? Address, List<EmploymentInput>? Employments);