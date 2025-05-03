using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ST10263027_PROG7311_POE.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment (if needed)
        public int ProductId { get; set; }

        [Column("productName")] // Maps to the exact column name in SQL
        public string ProductName { get; set; }

        [Column("productCategory")]
        public string ProductCategory { get; set; }

        [Column("productionDate")]
        public DateTime? ProductionDate { get; set; } // Nullable

        [Column("FarmerId")]
        public int? FarmerId { get; set; } // Nullable (matches SQL)

        // Navigation property to Farmer (optional)
        [ForeignKey("FarmerId")]
        public virtual Farmer Farmer { get; set; }
    }
}
