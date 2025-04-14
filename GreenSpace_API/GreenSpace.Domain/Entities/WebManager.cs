using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class WebManager : BaseEntity
{
    public string? ImageBanner { get; set; }

    public string? ImageLogo { get; set; }
}
