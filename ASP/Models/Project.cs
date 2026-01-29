using System;
using System.Collections.Generic;

namespace DuAnThucTap_vs.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public string? ImageUrl { get; set; }

    public string? Customer { get; set; }

    public string? Location { get; set; }

    public string? Time { get; set; }

    public string? ShortDescription { get; set; }

    public string? Content { get; set; }

    public bool? IsActive { get; set; }
}
