using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleShopMVC.Models;
using System.Security.Claims;

namespace MotorcycleShopMVC.Controllers
{
    [Authorize] // Yêu cầu người dùng phải đăng nhập
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper lấy UserId:
        // Ưu tiên Session -> fallback Cookie Claims
        private int GetCurrentUserId()
        {
            // Session
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrWhiteSpace(userIdStr) &&
                int.TryParse(userIdStr, out var sessionUserId))
            {
                return sessionUserId;
            }

            // Cookie Claims fallback
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(claimId) &&
                int.TryParse(claimId, out var claimUserId))
            {
                return claimUserId;
            }

            return 0;
        }

        // GET: /Wishlist
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new { returnUrl = "/Wishlist" });
            }

            // Lấy tất cả sản phẩm trong wishlist của user
            var wishlistItems = await _context.Wishlists
                .Where(w => w.UserId == userId)

                .Include(w => w.Motorcycle)
                    .ThenInclude(m => m.Brand)

                .Include(w => w.Part)
                    .ThenInclude(p => p.Brand)

                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();

            return View(wishlistItems);
        }

        // POST: /Wishlist/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int? motorcycleId,
            int? partId)
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
            {
                return Unauthorized(
                    "Vui lòng đăng nhập để thêm vào danh sách yêu thích.");
            }

            if (motorcycleId == null && partId == null)
            {
                return BadRequest("Không có sản phẩm để thêm.");
            }

            // Kiểm tra đã tồn tại chưa
            var alreadyExists = await _context.Wishlists.AnyAsync(w =>
    w.UserId == userId &&
    (
        (motorcycleId != null && w.MotorcycleId == motorcycleId) ||
        (partId != null && w.PartId == partId)
    ));

            if (alreadyExists)
            {
                TempData["WishlistMessage"] =
                    "Sản phẩm đã có trong danh sách yêu thích của bạn.";

                return Redirect(
                    Request.Headers["Referer"].ToString() ?? "/");
            }

            var wishlistItem = new Wishlist
            {
                UserId = userId,
                MotorcycleId = motorcycleId,
                PartId = partId,
                CreatedAt = DateTime.Now
            };

            _context.Wishlists.Add(wishlistItem);

            await _context.SaveChangesAsync();

            TempData["WishlistMessage"] =
                "Đã thêm sản phẩm vào danh sách yêu thích!";

            // Quay lại trang trước
            return Redirect(
                Request.Headers["Referer"].ToString() ?? "/");
        }

        // POST: /Wishlist/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
                return Unauthorized();

            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w =>
                    w.WishlistId == id &&
                    w.UserId == userId);

            if (wishlistItem == null)
                return NotFound();

            _context.Wishlists.Remove(wishlistItem);

            await _context.SaveChangesAsync();

            TempData["WishlistMessage"] =
                "Đã xóa sản phẩm khỏi danh sách yêu thích.";

            var referer = Request.Headers["Referer"].ToString();

            // Nếu đang ở trang wishlist thì reload wishlist
            if (!string.IsNullOrWhiteSpace(referer) &&
                referer.Contains("/Wishlist"))
            {
                return RedirectToAction(nameof(Index));
            }

            // Không thì quay lại trang trước
            return Redirect(referer ?? "/");
        }
    }
}