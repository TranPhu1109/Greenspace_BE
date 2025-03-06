namespace GreenSpace.Domain.Entities;

public class ProductDetail : BaseEntity
{
    public Guid ProductId { get; set; }

    public Guid DesignIdeaId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public DesignIdea DesignIdea { get; set; } = null!;

    public Material Product { get; set; } = null!;
}
