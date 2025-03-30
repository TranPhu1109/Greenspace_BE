namespace GreenSpace.Domain.Entities;

public class Contract : BaseEntity
{
    public Guid UserId { get; set; } 

    public string Description { get; set; } = string.Empty;
    public Guid ServiceOrderId { get; set; }

    public User User { get; set; } = default!;
}
