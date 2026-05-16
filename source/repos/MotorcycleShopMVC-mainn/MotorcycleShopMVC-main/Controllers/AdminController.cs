using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleShopMVC.Filters;
using MotorcycleShopMVC.Models;
using MotorcycleShopMVC.Models.ViewModels;

namespace MotorcycleShopMVC.Controllers
{
    [RoleAuthorize("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // DASHBOARD
        // =========================

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers =
                    await _context.Users.CountAsync(),

                TotalMotorcycles =
                    await _context.Motorcycles.CountAsync(),

                TotalOrders =
                    await _context.Orders.CountAsync(),

                TotalRevenue =
                    await _context.Orders
                        .Where(o => o.Status == "Completed")
                        .SumAsync(o => (decimal?)o.TotalAmount)
                        ?? 0
            };

            return View(model);
        }

        // =========================
        // USERS
        // =========================

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        // =========================
        // LOCK USER
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            // Không cho khóa chính mình
            if (User.Identity?.Name == user.Email)
            {
                TempData["Error"] =
                    "Bạn không thể tự khóa tài khoản của mình.";

                return RedirectToAction(nameof(Users));
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "User đã bị khóa.";

            return RedirectToAction(nameof(Users));
        }

        // =========================
        // UNLOCK USER
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.IsActive = true;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "User đã được mở khóa.";

            return RedirectToAction(nameof(Users));
        }

        // =========================
        // CHANGE ROLE
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            // Validate role
            var validRoles = new[]
            {
                "Customer",
                "Vendor",
                "Admin"
            };

            if (!validRoles.Contains(role))
            {
                TempData["Error"] =
                    "Role không hợp lệ.";

                return RedirectToAction(nameof(Users));
            }

            user.Role = role;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Role đã được cập nhật.";

            return RedirectToAction(nameof(Users));
        }

        // =========================
        // DELETE USER
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            // Không cho xóa chính mình
            if (User.Identity?.Name == user.Email)
            {
                TempData["Error"] =
                    "Bạn không thể tự xóa tài khoản của mình.";

                return RedirectToAction(nameof(Users));
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "User đã bị xóa.";

            return RedirectToAction(nameof(Users));
        }

        // =========================
        // ORDERS
        // =========================

        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        // =========================
        // REVENUE
        // =========================

        public IActionResult Revenue()
        {
            return View();
        }
    }
}
