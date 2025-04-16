using GreenSpace.Domain.Enum;

namespace GreenSpace.Domain.Entities;

public class ServiceOrder :BaseEntity
{
    public Guid UserId { get; set; }

    public Guid? DesignIdeaId { get; set; }

    public string Address { get; set; } = string.Empty;

    public string CusPhone { get; set; } = string.Empty;
    public double? Length { get; set; } = default!;
    public double? Width { get; set; } = default!;
    public string ServiceType { get; set; } = string.Empty;
    public decimal TotalCost { get; set; } = default!;

    public Guid? PaymentId { get; set; }

    public bool IsCustom { get; set; } = false;

    public decimal? DesignPrice { get; set; } = default!;

    public decimal? MaterialPrice { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    public Guid? ImageId { get; set; } = default!;
    //public Guid? RecordDesignId { get; set; } = default!;
    //public Guid? RecordSketchId { get; set; } = default!;
    public string DeliveryCode { get; set; } = string.Empty;

    public string Report { get; set; } = string.Empty;

    public int Status { get; set; } = default;
    public decimal DepositPercentage { get; set; } = decimal.One;
    public decimal RefundPercentage { get; set; } = decimal.One;
    public User User { get; set; } = default!;
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();

    public ICollection<RecordDesign> RecordDesigns { get; set; } = new List<RecordDesign>();
    public ICollection<RecordSketch> RecordSketches { get; set; } = new List<RecordSketch>();

    public ICollection<WorkTask> WorkTask { get; set; } = new List<WorkTask>();
    public ICollection<ServiceFeedback> ServiceFeedbacks { get; set; } = new List<ServiceFeedback>();
    public Image Image { get; set; } = default!;
}
