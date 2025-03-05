using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DesignIdea> DesignIdeas { get; set; } = new List<DesignIdea>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
