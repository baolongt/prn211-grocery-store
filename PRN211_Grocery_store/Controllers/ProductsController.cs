using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Grocery_store.Data;
using PRN211_Grocery_store.Data.Entity;

namespace PRN211_Grocery_store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ProductsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Products.Include(p => p.Category);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Enable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = _context.Products.Find(id);
            if (product != null)
            {
                product.IsDelete = false;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Disable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = _context.Products.Find(id);
            if (product != null)
            {
                product.IsDelete = true;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Price,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct([Bind("Id,CategoryId,Name,Price,Quantity")] Product product)
        {
            if (product == null)
            {
                return NotFound();
            }
            Product product2 = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (ModelState.IsValid) { 
                try
                {
                    product2.CategoryId = product.CategoryId;
                    product2.Name = product.Name;
                    product2.Quantity = product.Quantity;
                    product2.Price = product.Price;
                    _context.Products.Update(product2);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
