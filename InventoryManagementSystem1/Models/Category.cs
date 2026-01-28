using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem1.Models
{
    public class Category
    {
        
            [Key]
            public Guid CategoryID { get; set; }

            [Required]   
            public string CategoryName { get; set; }
        


    }
}
