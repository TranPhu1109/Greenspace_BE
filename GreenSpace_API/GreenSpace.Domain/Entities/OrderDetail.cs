namespace GreenSpace.Domain.Entities;

public class OrderDetail : BaseEntity
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal? Price { get; set; } = default!;

    public Order Order { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
