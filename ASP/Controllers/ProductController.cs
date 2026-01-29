using DuAnThucTap_vs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuAnThucTap_vs.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // DANH SÁCH SẢN PHẨM THEO DANH MỤC
        // ===============================
        public IActionResult Category(string slug)
        {
            var category = _context.Categories
            .Include(c => c.Products)
                .ThenInclude(p => p.Brand)
            .FirstOrDefault(c => c.Slug == slug);


            if (category == null)
                return NotFound();

            return View("~/Views/Home/Category.cshtml", category);
        }

        // ===============================
        // CHI TIẾT SẢN PHẨM
        // ===============================
        public IActionResult Detail(int id)
        {
            var product = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View("~/Views/Home/Product-detail.cshtml", product);
        }
    }
}
