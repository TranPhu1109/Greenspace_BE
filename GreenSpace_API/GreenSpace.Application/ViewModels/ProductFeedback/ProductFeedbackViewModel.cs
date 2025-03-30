using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ProductFeedback
{
    public class ProductFeedbackViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty; 
        public string ProductName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public int? Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 
    }
}
