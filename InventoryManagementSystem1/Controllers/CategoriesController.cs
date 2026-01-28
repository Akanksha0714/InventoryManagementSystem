using InventoryManagementSystem1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem1.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
               
                category.CategoryID = Guid.NewGuid();

                _context.Categories.Add(category);
                _context.SaveChanges();

         
                TempData["success"] = "Category added successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

     
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category category)
        {
            if (id != category.CategoryID)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();

          
                TempData["success"] = "Category updated successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

     
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);

            return View(category);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                
                TempData["success"] = "Category deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

