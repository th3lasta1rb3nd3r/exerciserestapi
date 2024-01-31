namespace Simple.Data;

public class Employment : Entity<int>
{
    public string Company { get; set; } = default!;
    public uint MonthsOfExperience { get; set; } = default!;
    public double Salary { get; set; } = default!;
    public DateTime StartDate { get; set; } = default!;
    public DateTime? EndDate { get; set; }
    public int UserId { get; set; } = default!;

    public User User { get; set; } = default!;
}
