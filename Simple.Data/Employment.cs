using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Data;

public class Employment : Entity<int>
{
    public string Company { get; set; } = default!;
    public short MonthsOfExperience { get; set; } = default!;
    public double Salary { get; set; } = default!;
    public DateTime StartDate { get; set; } = default!;
    public DateTime? EndDate { get; set; }
    public int UserId { get; set; } = default!;

    [NotMapped]
    public User User { get; set; } = default!;
}
