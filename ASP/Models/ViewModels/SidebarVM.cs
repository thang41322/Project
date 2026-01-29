namespace DuAnThucTap_vs.Models.ViewModels
{
    using DuAnThucTap_vs.Models;
    using System.Collections.Generic;

    public class SidebarVM
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<News> HotNews { get; set; }
    }

}
