using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.MongoDbs.Notifications
{
    public class NotificationCreateModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsSeen { get; set; } = false;
        public string ImageURL { get; set; } = string.Empty;
        public int Source { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
