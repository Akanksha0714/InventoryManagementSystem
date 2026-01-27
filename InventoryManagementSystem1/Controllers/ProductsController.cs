using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem1.Models;

namespace InventoryManagementSystem1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: Products
        // =========================
        public async Task<IActionResult> Index(
     string searchString,
     Guid? categoryId,
     Guid? supplierId)
        {
            // dropdown data
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Suppliers = _context.Suppliers.ToList();

            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            // 🔍 Search by product name
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchString));
            }

            // 📂 Filter by category
            if (categoryId.HasValue)
            {
                products = products.Where(p =>
                    p.CategoryID == categoryId);
            }

            // 🏭 Filter by supplier
            if (supplierId.HasValue)
            {
                products = products.Where(p =>
                    p.SupplierID == supplierId);
            }

            return View(await products.ToListAsync());
        }


        // =========================
        // GET: Products/Create
        // =========================
        public IActionResult Create()
        {
            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryID", "CategoryName");

            ViewData["SupplierID"] =
                new SelectList(_context.Suppliers, "SupplierID", "SupplierName");

            return View();
        }



        // =========================
        // POST: Products/Create
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                // 🔥 DEBUG: validation errors पाहण्यासाठी (dev time)
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"{state.Key} : {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                product.ProductID = Guid.NewGuid();
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // ✅ SUCCESS MESSAGE (THIS IS THE CHANGE)
                TempData["success"] = "Product added successfully!";

                return RedirectToAction(nameof(Index));
            }

            // dropdown data reload
            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);

            ViewData["SupplierID"] =
                new SelectList(_context.Suppliers, "SupplierID", "SupplierName", product.SupplierID);

            return View(product);
        }




        // =========================
        // GET: Products/Edit/5
        // =========================
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);

            ViewData["SupplierID"] =
                new SelectList(_context.Suppliers, "SupplierID", "SupplierName", product.SupplierID);

            return View(product);
        }

        // =========================
        // POST: Products/Edit/5
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            if (id != product.ProductID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);

            ViewData["SupplierID"] =
                new SelectList(_context.Suppliers, "SupplierID", "SupplierName", product.SupplierID);

            return View(product);
        }

        // =========================
        // GET: Products/Delete/5
        // =========================
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }


        // =========================
        // POST: Products/Delete/5
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Product deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // Helper Method
        // =========================
        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}

