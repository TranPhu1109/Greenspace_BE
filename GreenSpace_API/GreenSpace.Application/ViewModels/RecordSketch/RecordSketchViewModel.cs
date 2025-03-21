using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.RecordSketch
{
    public class RecordSketchViewModel
    {
        public int phase { get; set; }
        public bool isSelected { get; set; }
        public Guid ServiceOrderId { get; set; }
        public Guid ImageId { get; set; }
        public Image? Image { get; set; }
    }
}
