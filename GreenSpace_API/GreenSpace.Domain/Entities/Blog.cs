using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? Author { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? ImageId { get; set; }

    public virtual Image? Image { get; set; }
}
