using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem1.Models
{
    public class Supplier
    {
        public Guid SupplierID { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        public string SupplierName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage = "Contact number must be exactly 10 digits")]
        public string ContactNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
