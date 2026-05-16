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
    public class PartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Part
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Parts
                .Include(p => p.Brand)
                .Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Part/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PartId == id);

            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Part/Create
        [RoleAuthorize("Vendor", "Admin")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId");
            ViewData["CategoryId"] = new SelectList(_context.PartCategories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Part/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Create([Bind("PartId,PartName,CategoryId,BrandId,Price,StockQuantity,Description,ImagePath,WarrantyMonths,CreatedAt,UpdatedAt")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", part.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.PartCategories, "CategoryId", "CategoryId", part.CategoryId);
            return View(part);
        }

        // GET: Part/Edit/5
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", part.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.PartCategories, "CategoryId", "CategoryId", part.CategoryId);
            return View(part);
        }

        // POST: Part/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("PartId,PartName,CategoryId,BrandId,Price,StockQuantity,Description,ImagePath,WarrantyMonths,CreatedAt,UpdatedAt")] Part part)
        {
            if (id != part.PartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.PartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", part.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.PartCategories, "CategoryId", "CategoryId", part.CategoryId);
            return View(part);
        }

        // GET: Part/Delete/5
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PartId == id);

            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // POST: Part/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.PartId == id);
        }
    }
}