using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ProductDAL _dal;

        public SupplierController(IConfiguration configuration)
        {
            // ProductDAL la Supplier data access sathi vapart ahot
            _dal = new ProductDAL(configuration);
        }

        // GET: Supplier/Index (Supplier List)
        public IActionResult Index()
        {
            var suppliers = _dal.GetSuppliers();
            return View(suppliers);
        }

        // GET: Supplier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _dal.AddSupplier(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Supplier/Edit/{id}
        public IActionResult Edit(int id)
        {
            var supplier = _dal.GetSupplierById(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // POST: Supplier/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Supplier supplier)
        {
            if (id != supplier.SupplierID) return NotFound();

            if (ModelState.IsValid)
            {
                _dal.UpdateSupplier(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Supplier/Delete/{id}
        public IActionResult Delete(int id)
        {
            var supplier = _dal.GetSupplierById(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // POST: Supplier/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteSupplier(id);
            return RedirectToAction(nameof(Index));
        }
    }
}




//namespace InventoryManagementSystem.Models
//{
//    public class Supplier
//    {
//        public int SupplierID { get; set; }

//        [Required(ErrorMessage = "Supplier Name is required")]
//        public string SupplierName { get; set; }
//    }
//}
