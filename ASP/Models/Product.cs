using System;
using System.Collections.Generic;

namespace DuAnThucTap_vs.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? Specification { get; set; }

    public string? Application { get; set; }

    public bool IsFeatured { get; set; }
    public bool? IsActive { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }
}
