namespace Simple.Data;

public class Address : Entity<int>
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public int? PostCode { get; set; }
    public User User { get; set; } = default!;
}