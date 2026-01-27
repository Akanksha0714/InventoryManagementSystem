using System.Collections.Generic;
using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InventoryManagementSystem.Controllers
{
    public class SupplierController : Controller
    {
        // फक्त एकदाच _dal व्हेरिएबल डिक्लेअर केला आहे
        private readonly ProductDAL _dal;

        public SupplierController(IConfiguration configuration)
        {
            _dal = new ProductDAL(configuration);
        }

        public IActionResult Index()
        {
            List<Supplier> suppliers = _dal.GetSuppliers();
            return View(suppliers);
        }

        public IActionResult Create()
        {
            return View();
        }

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

        public IActionResult Edit(int id)
        {
            var supplier = _dal.GetSupplierById(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

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

        public IActionResult Delete(int id)
        {
            var supplier = _dal.GetSupplierById(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteSupplier(id);
            return RedirectToAction(nameof(Index));
        }
    }
}