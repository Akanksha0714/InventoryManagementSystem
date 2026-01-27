using InventoryManagementSystem1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.TotalProducts = _context.Products.Count();
        ViewBag.TotalCategories = _context.Categories.Count();
        ViewBag.TotalSuppliers = _context.Suppliers.Count();
        ViewBag.TotalStock = _context.Products.Sum(p => p.QuantityInStock);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
