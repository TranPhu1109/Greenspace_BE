namespace GreenSpace.Domain.Entities;

public class Payment : BaseEntity
{
    public string PaymentMethod { get; set; } = default!;

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
