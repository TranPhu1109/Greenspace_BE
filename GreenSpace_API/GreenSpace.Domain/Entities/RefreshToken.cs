using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public string JwtId { get; set; } = default!;

    public bool IsUsed { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime IssuedAt { get; set; } = DateTime.Now;

    public DateTime? ExpiredAt { get; set; } = default!;

    public User User { get; set; } = null!;
}
