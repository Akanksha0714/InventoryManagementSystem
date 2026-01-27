using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Quantity cannot be negative")]
        public int QuantityInStock { get; set; }

        // Dropdown select karnyasathi ID
        [Required(ErrorMessage = "Please select a Category")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        // List madhe Nav dakhavnyasathi (Table madhe nahi yenar he column)
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Please select a Supplier")]
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }

        // List madhe Nav dakhavnyasathi
        public string? SupplierName { get; set; }
    }
}