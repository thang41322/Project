using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP.Models;

namespace ASP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


   

}
