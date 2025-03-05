using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public Guid? UserId { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }
}
