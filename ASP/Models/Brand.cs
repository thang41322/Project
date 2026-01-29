using System;
using System.Collections.Generic;

namespace DuAnThucTap_vs.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? Country { get; set; }

    public string? LogoUrl { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
