namespace MotorcycleShopMVC.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalMotorcycles { get; set; }

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}