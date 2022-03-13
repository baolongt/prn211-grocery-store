using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRN211_Grocery_store.Models;
using PRN211_Grocery_store.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PRN211_Grocery_store.Controllers
{
    public class HomeController : Controller
    {
        ProductRepository productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            productRepository = new ProductRepository();
        }

        public IActionResult Index()
        {
            ViewBag.products = productRepository.GetProducts();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
