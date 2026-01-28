using InventoryManagementSystem1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return View(suppliers);
        }

       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.SupplierID = Guid.NewGuid();
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

               
                TempData["success"] = "Supplier added successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Supplier supplier)
        {
            if (id != supplier.SupplierID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();

         
                    TempData["success"] = "Supplier updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();

  
                TempData["success"] = "Supplier deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(Guid id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
