using DuAnThucTap_vs.Models;
using System.Collections.Generic;

public class HomeViewModel
{
    public Product FeaturedProduct { get; set; }

    public List<Product> Products { get; set; }
    public List<Brand> Brands { get; set; }
    public List<Category> Categories { get; set; }
    public List<News> News { get; set; }
    //public List<Category> Categories { get; set; }
    public List<Product> FeaturedProducts { get; set; }
    public List<News> HotNews { get; set; }
}
