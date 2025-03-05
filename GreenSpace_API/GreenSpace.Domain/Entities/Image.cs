using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Image
{
    public int ImageId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<DesignIdea> DesignIdeas { get; set; } = new List<DesignIdea>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
