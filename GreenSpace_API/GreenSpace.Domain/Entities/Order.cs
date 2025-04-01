namespace GreenSpace.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal? TotalAmount { get; set; } = default!;

    public Guid PaymentId { get; set; } = default!;

    public int Status { get; set; }

    public decimal? ShipPrice { get; set; }

    public string Address { get; set; } = string.Empty;

    public string? Phone { get; set; } = default!;

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public Payment? Payment { get; set; }

    public User User { get; set; } = default!;
}
