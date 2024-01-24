using System.ComponentModel.DataAnnotations;
using ExampleAPI.Core;
using Microsoft.EntityFrameworkCore.Query;

namespace ExampleAPI.Entities;

public class Order : Entity<Guid>
{
    public required Guid UserId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public virtual User User { get; set; }

    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
    }

    internal IIncludableQueryable<Order, object> Include(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}
