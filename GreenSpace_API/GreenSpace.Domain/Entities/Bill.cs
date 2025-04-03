namespace GreenSpace.Domain.Entities;

public class Bill : BaseEntity
{
    public Guid? ServiceOrderId { get; set; }

    public Guid? OrderId { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public Order? Order { get; set; }

    public ServiceOrder? ServiceOrder { get; set; }
    public Guid UsersWalletId { get; set; }
    public UsersWallet? UsersWallet { get; set; }
}
