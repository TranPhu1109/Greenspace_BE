using GreenSpace.Domain.Entities;

namespace GreenSpace.Application.ViewModels.OrderProducts
{
    public class CreateOrderProductModel
    {
        public Guid UserId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal ShipPrice { get; set; }
        public List<ItemProductViewModel>? Products { get; set; }
    }
}
