namespace GreenSpace.Domain.Entities;

public class Contract : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public Guid ServiceOrderId { get; set; }

    public User User { get; set; } = default!;
}
