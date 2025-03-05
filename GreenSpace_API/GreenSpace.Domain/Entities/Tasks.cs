using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Tasks
{
    public int ServiceOrderId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Status { get; set; }

    public virtual ServiceOrder ServiceOrder { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
