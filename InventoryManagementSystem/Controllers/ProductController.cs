using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Dropdown sathi he lagel

// 🎯 KORRAKT NAMESPACE ITHE TAKA:
namespace InventoryManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDAL _dal;

        public ProductController(IConfiguration configuration)
        {
            // IConfiguration vaprun ProductDAL la connection string deto
            _dal = new ProductDAL(configuration);
        }

        // GET: List Products
        public IActionResult Index()
        {
            var products = _dal.GetAllProducts();
            return View(products);
        }

        // --- Create (GET) Method ---
        public IActionResult Create()
        {
            // Categories ani Suppliers DAL kadun fetch kara
            var categories = _dal.GetCategories();
            var suppliers = _dal.GetSuppliers();

            // ViewBag vaprun data View kade pathva (Dropdown sathi)
            ViewBag.CategoriesList = new SelectList(categories, "CategoryID", "CategoryName");
            ViewBag.SuppliersList = new SelectList(suppliers, "SupplierID", "SupplierName");

            return View();
        }

        // --- Create (POST) Method ---
        [HttpPost]
        [ValidateAntiForgeryToken] // Security sathi he use kara
        public IActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Data barobar asel tar DAL vaprun database madhe save kara
                _dal.AddProduct(model);
                return RedirectToAction("Index");
            }

            // Validation fail zali tar parat dropdowns load karave lagtil
            ViewBag.CategoriesList = new SelectList(_dal.GetCategories(), "CategoryID", "CategoryName", model.CategoryID);
            ViewBag.SuppliersList = new SelectList(_dal.GetSuppliers(), "SupplierID", "SupplierName", model.SupplierID);

            return View(model);
        }

        // ** (TODO: Implement Edit and Delete methods) **
    }
}