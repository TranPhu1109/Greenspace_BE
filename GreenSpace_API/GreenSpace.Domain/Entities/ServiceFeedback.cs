namespace GreenSpace.Domain.Entities;

public class ServiceFeedback :BaseEntity
{
    public Guid UserId { get; set; }

    public Guid DesignIdeaId { get; set; }

    public int Rating { get; set; } = default!;

    public string Description { get; set; } = string.Empty;
    public string Reply { get; set; } = string.Empty;

    public DesignIdea DesignIdea { get; set; } = null!;

    public User User { get; set; } = null!;
}
