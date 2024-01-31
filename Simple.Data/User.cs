namespace Simple.Data;

public class User : Entity<int>
{
    public User() => Employments = [];

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    
    public Address? Address { get; set; }
    public List<Employment> Employments { get; set; }
}
