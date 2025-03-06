namespace GreenSpace.Domain.Entities;

public class Category : BaseEntity
{

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<DesignIdea> DesignIdeas { get; set; } = new List<DesignIdea>();

    public ICollection<Material> Materials { get; set; } = new List<Material>();
}
