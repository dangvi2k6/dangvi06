using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleShopMVC.Filters;
using MotorcycleShopMVC.Models;
using MotorcycleShopMVC.Models.ViewModels;
using System.Security.Claims;

namespace MotorcycleShopMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            //var userIdStr = HttpContext.Session.GetString("UserId");
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out var userId) ? userId : 0;
        }

        // Hiển thị trang thanh toán
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var cart = await _context.Carts
                .Include(c => c.CartItems).ThenInclude(ci => ci.Motorcycle)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Part)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return RedirectToAction("Index", "Cart");

            var model = new CheckoutViewModel
            {
                CartItems = cart.CartItems.ToList(),
                TotalAmount = cart.CartItems.Sum(i => i.Quantity * (i.Motorcycle?.Price ?? i.Part?.Price ?? 0))
            };

            return View(model);
        }

        // Xử lý đặt hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var userId = GetUserId();
            var cart = await _context.Carts
                .Include(c => c.CartItems).ThenInclude(ci => ci.Motorcycle)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Part)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any()) return RedirectToAction("Index", "Home");

            // Tạo Order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ShippingAddress = model.ShippingAddress,
                PaymentMethod = model.PaymentMethod,
                TotalAmount = cart.CartItems.Sum(i => i.Quantity * (i.Motorcycle?.Price ?? i.Part?.Price ?? 0)),
                Status = "pending",
                PaymentStatus = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo các OrderItem
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    MotorcycleId = item.MotorcycleId,
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    Price = item.Motorcycle?.Price ?? item.Part?.Price ?? 0
                };
                _context.OrderItems.Add(orderItem);
            }

            // Xóa giỏ hàng
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success", new { id = order.OrderId });
        }

        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        // ==========================================
        // 1. LỊCH SỬ ĐƠN HÀNG (Dành cho Khách hàng)
        // ==========================================
        public async Task<IActionResult> History()
        {
            var userId = GetUserId();
            // Lấy danh sách đơn hàng của user đang đăng nhập
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // ==========================================
        // 2. QUẢN LÝ ĐƠN HÀNG (Dành cho Admin/Nhân viên)
        // ==========================================
        [RoleAuthorize("Vendor", "Admin")]
        public async Task<IActionResult> Manage()
        {
            // Lấy tất cả đơn hàng trong hệ thống, kèm theo thông tin User
            var orders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // Action để Admin cập nhật trạng thái đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string status, string paymentStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                order.PaymentStatus = paymentStatus;
                order.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            // Quay lại trang Manage sau khi cập nhật xong
            return RedirectToAction(nameof(Manage));
        }
    }
}