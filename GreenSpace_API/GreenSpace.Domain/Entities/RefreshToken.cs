using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class RefreshToken
{
    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public string? JwtId { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public DateTime? IssuedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public virtual User User { get; set; } = null!;
}
