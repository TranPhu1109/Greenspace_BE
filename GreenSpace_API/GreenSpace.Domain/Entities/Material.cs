namespace GreenSpace.Domain.Entities;

public class Material :BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public double Price { get; set; } 

    public int Stock { get; set; }

    public string Description { get; set; } = string.Empty;

    public Guid ImageId { get; set; }

    public int Size { get; set; }

    public Category Category { get; set; } = default!;

    public Image Image { get; set; } = default!;

    public ICollection<MaterialFeedback> MaterialFeedbacks { get; set; } = new List<MaterialFeedback>();

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();
}
