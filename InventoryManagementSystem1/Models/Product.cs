using InventoryManagementSystem1.Models;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public Guid ProductID { get; set; }

    [Required]
    public string ProductName { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    [Required]
    public Guid CategoryID { get; set; }

    [Required]
    public Guid SupplierID { get; set; }

    
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
}

