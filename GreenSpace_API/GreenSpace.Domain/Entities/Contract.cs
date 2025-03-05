using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Contract
{
    public int ContractId { get; set; }

    public Guid? UserId { get; set; }

    public string? Description { get; set; }

    public virtual User? User { get; set; }
}
