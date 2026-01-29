using System;
using System.Collections.Generic;

namespace DuAnThucTap_vs.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string? Title { get; set; }

    public string? Thumbnail { get; set; }

    public string? ShortDescription { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }
}
