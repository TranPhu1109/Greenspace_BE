using GreenSpace.Application.ViewModels.Images;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
    public class ServiceOrderCreateModel
    {
        public Guid UserId { get; set; }

        public Guid? DesignIdeaId { get; set; }

        public string Address { get; set; } = string.Empty;

        public string CusPhone { get; set; } = string.Empty;
        public bool IsCustom { get; set; } = false;
        public double? Length { get; set; } = default!;
        public double? Width { get; set; } = default!;
        public double? DesignPrice { get; set; } = default!;

        public double? MaterialPrice { get; set; } = default!;
        public double TotalCost { get; set; } = default!;

        public string Description { get; set; } = string.Empty;
        public ImageCreateModel Image { get; set; } = new ImageCreateModel();
        public List<ListProductViewModel> Products { get; set; } = new List<ListProductViewModel>();
    }
    public class ListProductViewModel
    {
        public Guid ProductId { get; set; } = Guid.Empty;
        public int Quantity { get; set; } = default!;
    }
}


