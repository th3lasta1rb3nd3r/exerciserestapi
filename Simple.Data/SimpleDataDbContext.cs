using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Simple.Data;

public class SimpleDataDbContext(DbContextOptions<SimpleDataDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Address> Addresses { get; set; } = default!;
    public DbSet<Employment> Employments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Assembly assemblyWithConfigurations = GetType().Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);
    }
}
