namespace GreenSpace.Domain.Entities;

public class ProductDetail : BaseEntity
{
    public Guid ProductId { get; set; }

    public Guid DesignIdeaId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DesignIdea DesignIdea { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
