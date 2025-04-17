using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.OrderProducts
{
    public class CreateOrderModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal ShipPrice { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
