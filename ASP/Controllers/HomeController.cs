using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP.Models;
using Microsoft.Extensions.Configuration;

namespace ASP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _config;

    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
    public IActionResult Services()
    {
        return View("~/Views/Home/Services.cshtml");
    }
     public IActionResult About()
    {
        return View("~/Views/Home/About.cshtml");
    }
    public IActionResult Projects()
    {
        return View("~/Views/Home/Projects.cshtml");
    }
    public IActionResult News()
    {
        return View("~/Views/Home/News.cshtml");
    }
     public IActionResult Product()
    {
        return View("~/Views/Home/Product.cshtml");
    }




    public async Task<IActionResult> ProductsFromSql()
    {
        var products = new List<Product>();
        var connStr = _config.GetConnectionString("DefaultConnection");
        using (var conn = new Microsoft.Data.SqlClient.SqlConnection(connStr))
        {
            await conn.OpenAsync();
            using var cmd = new Microsoft.Data.SqlClient.SqlCommand("SELECT id, Name FROM Products", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new Product { Id = reader.GetInt32(0), Name = reader.GetString(1)});
            }
        }
        return Json(products);
    }
    public IActionResult Recruitment()
    {
        return View("~/Views/Home/Recruitment.cshtml");
    }public IActionResult Contact()
    {
        return View("~/Views/Home/Contact.cshtml");
    }

    

    public IActionResult Index()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


   

}
