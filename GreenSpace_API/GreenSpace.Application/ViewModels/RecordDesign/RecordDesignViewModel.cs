using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.RecordDesign
{
    public class RecordDesignViewModel
    {
        public Guid Id { get; set; }
        public int phase { get; set; }
        public bool isSelected { get; set; }
        public Guid ServiceOrderId { get; set; }
        public Guid ImageId { get; set; }
        public ImageCreateModel? Image { get; set; }
    }
}
