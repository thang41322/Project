using System;
using System.Collections.Generic;

namespace DuAnThucTap_vs.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Slug { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
