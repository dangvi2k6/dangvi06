using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MotorcycleShopMVC.Models;
using System.Security.Claims;

namespace MotorcycleShopMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy UserId:
        // Ưu tiên Session -> fallback Cookie Claims
        private int GetUserId()
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

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
                return cart;

            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();

            return cart;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Motorcycle)

                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Part)

                .FirstOrDefaultAsync(c => c.UserId == userId);

            var items = cart?.CartItems ?? new List<CartItem>();

            return View(items);
        }

        // Add motorcycle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMotorcycle(
            int motorcycleId,
            int quantity = 1,
            string? returnUrl = null)
        {
            if (quantity < 1)
                quantity = 1;

            int userId = GetUserId();

            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await GetOrCreateCartAsync(userId);

            // Kiểm tra motorcycle tồn tại
            var motorcycle = await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.MotorcycleId == motorcycleId);

            if (motorcycle == null)
                return NotFound();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cart.CartId &&
                    ci.MotorcycleId == motorcycleId);

            if (item == null)
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.CartId,
                    MotorcycleId = motorcycleId,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            // Ưu tiên returnUrl
            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // fallback referer
            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrWhiteSpace(referer))
                return Redirect(referer);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/AddPart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPart(
            int partId,
            int quantity = 1,
            string? returnUrl = null)
        {
            if (quantity < 1)
                quantity = 1;

            var userId = GetUserId();

            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var cart = await GetOrCreateCartAsync(userId);

            var part = await _context.Parts
                .FirstOrDefaultAsync(p => p.PartId == partId);

            if (part == null)
                return NotFound();

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cart.CartId &&
                    ci.PartId == partId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.CartId,
                    MotorcycleId = null,
                    PartId = partId,
                    Quantity = quantity
                });
            }

            cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            // Ưu tiên returnUrl
            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // fallback referer
            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrWhiteSpace(referer))
                return Redirect(referer);

            return RedirectToAction(nameof(Index));
        }

        // Tăng số lượng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            int userId = GetUserId();

            var cart = await GetOrCreateCartAsync(userId);

            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartItemId == cartItemId &&
                    ci.CartId == cart.CartId);

            if (item == null)
                return NotFound();

            item.Quantity += 1;

            cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Giảm số lượng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            var userId = GetUserId();

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
                return NotFound();

            if (cartItem.Cart.UserId != userId)
                return Forbid();

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
            }
            else
            {
                _context.CartItems.Remove(cartItem);
            }

            cartItem.Cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Xóa item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = GetUserId();

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
                return NotFound();

            if (cartItem.Cart.UserId != userId)
                return Forbid();

            _context.CartItems.Remove(cartItem);

            cartItem.Cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Clear cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return RedirectToAction(nameof(Index));

            _context.CartItems.RemoveRange(cart.CartItems);

            cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // API lấy tổng số lượng cho icon cart
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = GetUserId();

            if (userId == 0)
                return Json(0);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return Json(0);

            var count = cart.CartItems.Sum(x => x.Quantity);

            return Json(count);
        }
    }
}