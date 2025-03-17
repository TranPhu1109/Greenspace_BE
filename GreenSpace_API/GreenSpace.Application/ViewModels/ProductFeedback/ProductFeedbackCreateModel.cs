using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ProductFeedback
{
    public class ProductFeedbackCreateModel
    {
        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public int? Rating { get; set; } = null;

        public string Description { get; set; } = string.Empty;
    }
}
