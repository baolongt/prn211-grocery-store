using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Grocery_store.Data;
using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Models.Repository;
namespace PRN211_Grocery_store.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDBContext _context;
        private OrderRepository _orderRepository;

        public OrdersController(ApplicationDBContext context)
        {
            _context = context;
            _orderRepository = new OrderRepository();
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.Session.GetString("username");
            return View(_orderRepository.GetOrderByUsername(username));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //TODO: Change username - get from session
            var order = _orderRepository.Get((int)id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.details = order.OrderDetails.ToList();
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,createdDate,username,status")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
