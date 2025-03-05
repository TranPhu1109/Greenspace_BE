using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Material
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public double? Price { get; set; }

    public int? Stock { get; set; }

    public string? Description { get; set; }

    public int? ImageId { get; set; }

    public bool? IsDelete { get; set; }

    public int? Size { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Image? Image { get; set; }

    public virtual ICollection<MaterialFeedback> MaterialFeedbacks { get; set; } = new List<MaterialFeedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public virtual ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();
}
