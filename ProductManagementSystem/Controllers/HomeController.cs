using System;
using System.Web.Mvc;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                ViewBag.ProductCount = Product.GetAllProducts().Count;
                ViewBag.CategoryCount = Category.GetAllCategories().Count;
                ViewBag.LowStockCount = Product.GetLowStockProducts().Count;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading dashboard: " + ex.Message;
                return View();
            }
        }

        // GET: Home/About
        public ActionResult About()
        {
            return View();
        }
    }
} 