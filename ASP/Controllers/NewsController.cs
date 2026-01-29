using DuAnThucTap_vs.Models;
using Microsoft.AspNetCore.Mvc;

namespace DuAnThucTap_vs.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() // hoặc tên action tương ứng
        {
            var allNews = _context.News
            .OrderByDescending(n => n.CreatedDate)
            .ToList();

            // Bỏ tin đầu tiên (tin nổi bật)
            var smallNews = allNews.Skip(1).ToList();

            return View(smallNews);
        }
    }

}
