using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleShopMVC.Models;

namespace MotorcycleShopMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? brandId)
        {
            var brands = await _context.Brands.ToListAsync();

            var motorcycles = _context.Motorcycles
                .Include(m => m.Brand)
                .OrderByDescending(m => m.CreatedAt)
                .AsQueryable();

            if (brandId != null)
            {
                motorcycles = motorcycles.Where(m => m.BrandId == brandId);
            }

            var result = await motorcycles.Take(12).ToListAsync();

            ViewBag.Brands = brands;
            ViewBag.SelectedBrand = brandId;

            return View(result);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}