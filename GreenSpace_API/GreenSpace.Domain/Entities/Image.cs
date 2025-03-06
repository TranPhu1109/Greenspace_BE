namespace GreenSpace.Domain.Entities;

public class Image : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;

    public string Image2 { get; set; } = string.Empty;

    public string Image3 { get; set; } = string.Empty;

    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public ICollection<DesignIdea> DesignIdeas { get; set; } = new List<DesignIdea>();

    public ICollection<Material> Materials { get; set; } = new List<Material>();
}
