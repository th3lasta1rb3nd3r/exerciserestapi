using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Data;

public class Address : Entity<int>
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string? PostCode { get; set; }

    [NotMapped]
    public User User { get; set; } = default!;
}