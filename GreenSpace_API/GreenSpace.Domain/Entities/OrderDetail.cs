namespace GreenSpace.Domain.Entities;

public class OrderDetail : BaseEntity
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public double? Price { get; set; } = default!;

    public Order Order { get; set; } = null!;

    public Material Product { get; set; } = null!;
}
