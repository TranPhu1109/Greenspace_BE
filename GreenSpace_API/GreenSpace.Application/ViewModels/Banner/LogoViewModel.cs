using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Banner
{
    public class LogoViewModel
    {
        public Guid Id { get; set; }
        public string? ImageLogo { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public Guid? CreatedBy { get; set; } = Guid.Empty;
        public DateTime? ModificationDate { get; set; } = null;
        public Guid? ModificatedBy { get; set; } = default!;
    }
}
