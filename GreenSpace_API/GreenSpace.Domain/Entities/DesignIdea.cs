namespace GreenSpace.Domain.Entities;

public class DesignIdea : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public double DesignPrice { get; set; } = default!;
    public double MaterialPrice { get; set; } = default!;
    public double TotalPrice { get; set; } = default!;

    public Guid ImageId { get; set; }

    public Guid DesignIdeasCategoryId { get; set; }

    public DesignIdeasCategory DesignIdeasCategory { get; set; } = default!;

    public Image Image { get; set; } = default!;

    public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public ICollection<ServiceFeedback> ServiceFeedbacks { get; set; } = new List<ServiceFeedback>();
}
