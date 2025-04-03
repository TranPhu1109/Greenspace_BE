namespace GreenSpace.Domain.Entities;

public class Payment : BaseEntity
{
    public string PaymentMethod { get; set; } = default!;

}
