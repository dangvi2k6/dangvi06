using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleShopMVC.Models;
using MotorcycleShopMVC.Filters;

namespace MotorcycleShopMVC.Controllers
{
    public class MotorcycleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotorcycleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX + FILTER + PAGINATION (GIỮ CODE XỊN CỦA BẠN)
        public async Task<IActionResult> Index(
            string searchString,
            int? brandId,
            int? typeId,
            int? yearFrom,
            string priceRange,
            string engineRange,
            int? pageNumber)
        {
            ViewData["BrandId"] = new SelectList(await _context.Brands.ToListAsync(), "BrandId", "BrandName", brandId);
            ViewData["TypeId"] = new SelectList(await _context.VehicleTypes.ToListAsync(), "TypeId", "TypeName", typeId);
            ViewData["YearFrom"] = new SelectList(await _context.Motorcycles
                .Select(m => m.YearFrom)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync());

            var priceRanges = new List<SelectListItem>
            {
                new SelectListItem { Value = "0-20000000", Text = "Dưới 20 triệu" },
                new SelectListItem { Value = "20000000-50000000", Text = "20 - 50 triệu" },
                new SelectListItem { Value = "50000000-100000000", Text = "50 - 100 triệu" },
                new SelectListItem { Value = "100000000-9999999999", Text = "Trên 100 triệu" }
            };
            ViewData["PriceRanges"] = new SelectList(priceRanges, "Value", "Text", priceRange);

            var engineRanges = new List<SelectListItem>
            {
                new SelectListItem { Value = "0-100", Text = "Dưới 100cc" },
                new SelectListItem { Value = "100-175", Text = "100cc - 175cc" },
                new SelectListItem { Value = "175-9999", Text = "Trên 175cc" }
            };
            ViewData["EngineRanges"] = new SelectList(engineRanges, "Value", "Text", engineRange);

            var motorcycles = _context.Motorcycles
                .Include(m => m.Brand)
                .Include(m => m.Type)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                motorcycles = motorcycles.Where(s => s.ModelName.Contains(searchString));

            if (brandId.HasValue)
                motorcycles = motorcycles.Where(m => m.BrandId == brandId.Value);

            if (typeId.HasValue)
                motorcycles = motorcycles.Where(m => m.TypeId == typeId.Value);

            if (yearFrom.HasValue)
                motorcycles = motorcycles.Where(m => m.YearFrom == yearFrom.Value);

            if (!string.IsNullOrEmpty(priceRange))
            {
                var prices = priceRange.Split('-').Select(decimal.Parse).ToList();
                motorcycles = motorcycles.Where(m => m.Price >= prices[0] && m.Price <= prices[1]);
            }

            if (!string.IsNullOrEmpty(engineRange))
            {
                var capacities = engineRange.Split('-').Select(int.Parse).ToList();
                motorcycles = motorcycles.Where(m => m.EngineCapacity >= capacities[0] && m.EngineCapacity < capacities[1]);
            }

            int pageSize = 9;
            var paginatedList = await PaginatedList<Motorcycle>.CreateAsync(
                motorcycles.AsNoTracking().OrderByDescending(m => m.CreatedAt),
                pageNumber ?? 1,
                pageSize
            );

            return View(paginatedList);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var motorcycle = await _context.Motorcycles
                .Include(m => m.Brand)
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.MotorcycleId == id);

            if (motorcycle == null) return NotFound();

            return View(motorcycle);
        }

        [RoleAuthorize("Vendor", "Admin")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["TypeId"] = new SelectList(_context.VehicleTypes, "TypeId", "TypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Create(Motorcycle motorcycle)
        {
            if (ModelState.IsValid)
            {
                motorcycle.CreatedAt = DateTime.Now;
                motorcycle.UpdatedAt = DateTime.Now;

                _context.Add(motorcycle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", motorcycle.BrandId);
            ViewData["TypeId"] = new SelectList(_context.VehicleTypes, "TypeId", "TypeName", motorcycle.TypeId);


            return View(motorcycle);
        }

        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var motorcycle = await _context.Motorcycles.FindAsync(id);
            if (motorcycle == null) return NotFound();

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", motorcycle.BrandId);
            ViewData["TypeId"] = new SelectList(_context.VehicleTypes, "TypeId", "TypeName", motorcycle.TypeId);

            return View(motorcycle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Edit(int id, Motorcycle motorcycle)
        {
            if (id != motorcycle.MotorcycleId) return NotFound();

            if (ModelState.IsValid)
            {
                motorcycle.UpdatedAt = DateTime.Now;
                _context.Update(motorcycle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(motorcycle);
        }

        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var motorcycle = await _context.Motorcycles
                .Include(m => m.Brand)
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.MotorcycleId == id);

            if (motorcycle == null) return NotFound();

            return View(motorcycle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motorcycle = await _context.Motorcycles.FindAsync(id);
            if (motorcycle != null)
                _context.Motorcycles.Remove(motorcycle);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}