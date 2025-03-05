using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class ServiceFeedback
{
    public Guid UserId { get; set; }

    public int DesignIdeaId { get; set; }

    public int? Rating { get; set; }

    public string? Description { get; set; }

    public virtual DesignIdea DesignIdea { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
