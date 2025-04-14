using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Document
{
    public class DocumentViewModel
    {
        public Guid Id { get; set; }
        public string Document1 { get; set; } = string.Empty;
        public DateTime? ModificationDate { get; set; } = null;
        public Guid? ModificatedBy { get; set; } = default!;
    }
}
