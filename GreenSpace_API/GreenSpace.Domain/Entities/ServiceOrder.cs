namespace GreenSpace.Domain.Entities;

public class ServiceOrder :BaseEntity
{
    public Guid UserId { get; set; }

    public Guid? DesignIdeaId { get; set; }

    public string Address { get; set; } = string.Empty;

    public string CusPhone { get; set; } = string.Empty;
    public double? Length { get; set; } = default!;
    public double? Width { get; set; } = default!;

    public double TotalCost { get; set; } = default!;

    public Guid? PaymentId { get; set; }

    public bool IsCustom { get; set; } = false;

    public double? DesignPrice { get; set; } = default!;

    public double? MaterialPrice { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    public Guid? ImageId { get; set; } = default!;
    public Guid? RecordDesignId { get; set; } = default!;
    public Guid? RecordSketchId { get; set; } = default!;

    public int Status { get; set; } = default;
    public User User { get; set; } = default!;
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();

 

    public ICollection<WorkTask> WorkTask { get; set; } = new List<WorkTask>();
    public ICollection<ServiceFeedback> ServiceFeedbacks { get; set; } = new List<ServiceFeedback>();
    public Image Image { get; set; } = default!;
}
