using Microsoft.AspNetCore.Mvc;

namespace MotorcycleShopMVC.Controllers
{
    public class SupportController : Controller
    {
        // GET: /Support
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Support/Policy
        public IActionResult Policy()
        {
            // Placeholder for a detailed policy page
            return View();
        }

        // GET: /Support/Faq
        public IActionResult Faq()
        {
            // Placeholder for a detailed FAQ page
            return View();
        }
    }
}