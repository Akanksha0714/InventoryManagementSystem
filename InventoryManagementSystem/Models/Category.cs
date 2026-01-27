using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InventoryManagementSystem.Models
{
    //namespace InventoryManagementSystem.Controllers
    {
        public class CategoryController : Controller
    {
        private readonly ProductDAL _dal;

        public CategoryController(IConfiguration configuration)
        {
            _dal = new ProductDAL(configuration);
        }

        // ======================
        // GET: Category/Index
        // ======================
        public IActionResult Index()
        {
            var categories = _dal.GetCategories();
            return View(categories);
        }

        // ======================
        // GET: Category/Create
        // ======================
        public IActionResult Create()
        {
            return View();
        }

        // ======================
        // POST: Category/Create;
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _dal.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // ======================
        // GET: Category/Edit/{id}
        // ======================
        public IActionResult Edit(int id)
        {
            var category = _dal.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // ======================
        // POST: Category/Edit
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.CategoryID)
                return NotFound();

            if (ModelState.IsValid)
            {
                _dal.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // ======================
        // GET: Category/Delete/{id}
        // ======================
        public IActionResult Delete(int id)
        {
            var category = _dal.GetCategoryById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ======================
        // POST: Category/Delete/{id}
        // ======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
}
