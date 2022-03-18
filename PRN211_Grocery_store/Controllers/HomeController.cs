using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRN211_Grocery_store.Models;
using PRN211_Grocery_store.Models.ViewModel;
using PRN211_Grocery_store.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Utils;
namespace PRN211_Grocery_store.Controllers
{
    public class HomeController : Controller
    {
        ProductRepository productRepository;
        OrderRepository orderRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            productRepository = new ProductRepository();
            orderRepository = new OrderRepository();
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

        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("cart") != null)
            {
                return View((List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart")));
            }
            return View();
        }

        public IActionResult AddToCart(int productId)
        {
            var product = productRepository.GetProductById(productId);
            List<Item> cart = null;
            // check cart in session
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart = (List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart"));
            }
            else
            {
                cart = new List<Item>();
            }

            // check product in cart
            Item item = cart.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                ++item.Quantity;
            }
            else
            {
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
            }
            // set cart to session
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart"));
            Item item = cart.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                cart.Remove(item);
            }
            // set cart to session
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Cart");
        }

        public IActionResult IncreaseQuantity(int productId)
        {
            List<Item> cart = (List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart"));
            Item item = cart.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                ++item.Quantity;
            }
            // set cart to session
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Cart");
        }

        public IActionResult DecreaseQuantity(int productId)
        {
            List<Item> cart = (List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart"));
            Item item = cart.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    --item.Quantity;
                }
                else
                {
                    cart.Remove(item);
                }
            }
            // set cart to session
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            List<Item> cart = (List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart"));
            Order order = new()
            {
                Username = "test123",
                CreatedDate = DateTime.Now,
                Status = "Pending",
                OrderDetails = CartMapper.Instance.MapToOrderDetail(cart)
            };
            orderRepository.AddNew(order);
            // delete cart
            cart.Clear();
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
