namespace GreenSpace.Domain.Entities;

public class DesignIdea : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal DesignPrice { get; set; } = default!;
    public decimal MaterialPrice { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public string DesignImage1URL { get; set; } = string.Empty;
    public string DesignImage2URL { get; set; } = string.Empty;
    public string DesignImage3URL { get; set; } = string.Empty;
    public Guid ImageId { get; set; }

    public Guid DesignIdeasCategoryId { get; set; }

    public DesignIdeasCategory DesignIdeasCategory { get; set; } = default!;

    public Image Image { get; set; } = default!;

    public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public ICollection<ServiceFeedback> ServiceFeedbacks { get; set; } = new List<ServiceFeedback>();
}
