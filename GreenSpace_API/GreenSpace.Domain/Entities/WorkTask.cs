namespace GreenSpace.Domain.Entities;

public class WorkTask :BaseEntity
{
    public Guid ServiceOrderId { get; set; }

    public Guid UserId { get; set; }

    public int Status { get; set; }
    public string Note { get; set; } = string.Empty;

    public ServiceOrder ServiceOrder { get; set; } = null!;

    public User User { get; set; } = null!;
}
