using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ST10263027_PROG7311_POE.Models
{
    // Product model class representing the Product entity
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment (if needed...recommended by Claude AI)
        public int ProductId { get; set; }

        [Column("productName")] 
        public string ProductName { get; set; } // Name of the product

        [Column("productCategory")]
        public string ProductCategory { get; set; } // Category of the product

        [Column("productionDate")]
        public DateTime? ProductionDate { get; set; } // Date of production

        [Column("FarmerId")]
        public int? FarmerId { get; set; } // Foreign key to the Farmer entity for matching products with farmers


    }
}
//***********************************************End of file*****************************************//
