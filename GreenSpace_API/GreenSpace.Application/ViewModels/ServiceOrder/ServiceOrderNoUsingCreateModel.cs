using GreenSpace.Application.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
     public class ServiceOrderNoUsingCreateModel
    {
        public Guid UserId { get; set; }


        public string Address { get; set; } = string.Empty;

        public string CusPhone { get; set; } = string.Empty;

        public double? Length { get; set; } = default!;
        public double? Width { get; set; } = default!;
        public double? DesignPrice { get; set; } = default!;

        public double? MaterialPrice { get; set; } = default!;
        public double TotalCost { get; set; } = default!;

        public string Description { get; set; } = string.Empty;
        public ImageCreateModel Image { get; set; } = new ImageCreateModel();
    }
}
