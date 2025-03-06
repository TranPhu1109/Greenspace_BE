namespace GreenSpace.Domain.Entities;

public class Blog : BaseEntity
{

    public string Author { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid ImageId { get; set; }

    public Image Image { get; set; } = default!;
}
