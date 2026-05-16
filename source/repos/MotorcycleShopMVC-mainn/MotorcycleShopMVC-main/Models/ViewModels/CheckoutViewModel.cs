using System.ComponentModel.DataAnnotations;
using MotorcycleShopMVC.Models;

namespace MotorcycleShopMVC.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string PaymentMethod { get; set; } = "COD";
    }
}