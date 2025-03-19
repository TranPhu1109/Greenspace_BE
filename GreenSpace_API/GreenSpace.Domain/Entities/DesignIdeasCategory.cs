﻿namespace GreenSpace.Domain.Entities;

public class DesignIdeasCategory : BaseEntity
{

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<DesignIdea> DesignIdeas { get; set; } = new List<DesignIdea>();

}
