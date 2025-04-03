using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.OrderDetail
{
    public class OrderDetailViewModel
    {
        public Guid ProductId { get; set; }



        public int Quantity { get; set; }

        public double Price { get; set; }

        public string CategoryName { get; set; } = default!;
    }
}
