﻿namespace GreenSpace.Domain.Entities;

public class Role : BaseEntity
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}
