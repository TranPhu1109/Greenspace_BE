namespace GreenSpace.Domain.Entities;

public class Product :BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public decimal Price { get; set; } 

    public int Stock { get; set; }

    public string Description { get; set; } = string.Empty;

    public Guid ImageId { get; set; }

    public int Size { get; set; }

    public string DesignImage1URL { get; set; } = string.Empty;

    public Category Category { get; set; } = default!;

    public Image Image { get; set; } = default!;

    public ICollection<ProductFeedback> MaterialFeedbacks { get; set; } = new List<ProductFeedback>();

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();
    public ICollection<ComplaintDetail> ComplaintDetails { get; set; } = new List<ComplaintDetail>();
}
