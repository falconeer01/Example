using System.ComponentModel.DataAnnotations;
using ExampleAPI.Core;
namespace ExampleAPI.Entities;

public class Stock : Entity<Guid>
{
    public required short Amount { get; set; }
    public required Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
}
