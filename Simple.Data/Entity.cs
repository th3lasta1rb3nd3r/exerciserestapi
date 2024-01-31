using System.ComponentModel.DataAnnotations;

namespace Simple.Data;

public abstract class Entity<TKey>
{
    [Key]
    public virtual TKey Id { get; set; } = default!;
}