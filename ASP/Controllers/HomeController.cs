using DuAnThucTap_vs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace DuAnThucTap_vs.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // ✅ Inject DbContext
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var featuredProduct = _context.Products
        .Include(x => x.Brand)
        .FirstOrDefault(x => x.IsFeatured);

            var model = new HomeViewModel
            {
                Products = _context.Products
                            .Where(x => (bool)x.IsActive)
                            .OrderByDescending(x => x.ProductId)
                            .Take(6)
                            .ToList(),

                Brands = _context.Brands
                            .Where(x => (bool)x.IsActive)
                            .ToList(),

                Categories = _context.Categories
                            .Where(x => (bool)x.IsActive)
                            .ToList(),

                News = _context.News
                            .Where(x => (bool)x.IsActive)
                            .OrderByDescending(x => x.CreatedDate)
                            .Take(4)
                            .ToList()
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Project(int page = 1)
        {
            int pageSize = 7;

            var totalProjects = _context.Projects
                .Where(p => p.IsActive == true)
                .Count();

            var projects = _context.Projects
                .Where(p => p.IsActive == true)
                .OrderByDescending(p => p.ProjectId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProjects / pageSize);

            return View(projects); // Views/Home/Project.cshtml
        }

        public IActionResult ProjectDetail(int id)
        {
            var project = _context.Projects
                .FirstOrDefault(p => p.ProjectId == id && p.IsActive == true);

            if (project == null)
                return NotFound();

            return View(project); // Views/Home/ProjectDetail.cshtml
        }



        public IActionResult Service()
        {
            return View();
        }

        // 🔥 TRANG TIN TỨC – LOAD DB
        public IActionResult News(int page = 1)
        {
            int pageSize = 5;

            // Tin nổi bật (bài mới nhất)
            var featuredNews = _context.News
                .Where(n => n.IsActive == true)
                .OrderByDescending(n => n.CreatedDate)
                .FirstOrDefault();

            // Danh sách tin còn lại
            var query = _context.News
                .Where(n => n.IsActive == true && n.NewsId != featuredNews.NewsId)
                .OrderByDescending(n => n.CreatedDate);

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var newsList = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.FeaturedNews = featuredNews;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(newsList);
        }



        public IActionResult Recruitment()
        {
            return View();
        }

        public IActionResult NewsDetail(int id)
        {
            var news = _context.News.FirstOrDefault(n => n.NewsId == id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news); // 👉 Views/Home/NewsDetail.cshtml
        }

        [Route("category/{slug}")]
        public IActionResult Category(string slug)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                    .ThenInclude(p => p.Brand)
                .FirstOrDefault(c => c.Slug == slug);

            if (category == null)
                return NotFound();

            return View(category);
        }


        [Route("product/{id}")]
        public IActionResult ProductDetail(int id)
        {
            var product = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View("Product-detail", product);
        }

        public IActionResult Apply()
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
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
