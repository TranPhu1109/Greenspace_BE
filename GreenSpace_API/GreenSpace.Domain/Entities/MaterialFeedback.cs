using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class MaterialFeedback
{
    public Guid UserId { get; set; }

    public int ProductId { get; set; }

    public int? Rating { get; set; }

    public string? Description { get; set; }

    public virtual Material Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
