namespace GreenSpace.Domain.Entities;

public class ProductFeedback :BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public int? Rating { get; set; } = null;

    public string Description { get; set; } = string.Empty;

    public Product Product { get; set; } = null!;

    public User User { get; set; } = null!;
}
