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
using Microsoft.AspNetCore.Authorization;

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

        public IActionResult Search([FromForm] string searchValue)
        {
            ViewBag.products = productRepository.SearchProduct(searchValue);
            ViewData["searchValue"] = searchValue;
            return View("Index");
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
                if (item.Product.Quantity > item.Quantity)
                {
                    ++item.Quantity;
                }
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
            HttpContext.Session.SetInt32("itemNum", cart.Count());
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
            HttpContext.Session.SetInt32("itemNum", cart.Count());
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
            HttpContext.Session.SetInt32("itemNum", cart.Count());
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
            HttpContext.Session.SetInt32("itemNum", cart.Count());
            return RedirectToAction("Cart");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            List<Item> cart = (List<Item>)JsonConvert.DeserializeObject<IEnumerable<Item>>(HttpContext.Session.GetString("cart"));
            // add new order
            Order order = new()
            {
                Username = HttpContext.Session.GetString("username"),
                CreatedDate = DateTime.Now,
                Status = "Pending",
                OrderDetails = CartMapper.Instance.MapToOrderDetail(cart)
            };
            orderRepository.AddNew(order);
            // decrease item's quantity
            foreach (Item item in cart)
            {
                Product product = item.Product;
                product.Quantity -= item.Quantity;
                productRepository.Update(product);
            }
            // delete cart
            cart.Clear();
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            HttpContext.Session.SetInt32("itemNum", 0);
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
