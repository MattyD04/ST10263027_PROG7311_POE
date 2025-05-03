using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ST10263027_PROG7311_POE.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? FarmerUserName { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? FarmerPassword { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? FarmerContactNum { get; set; }
    }
}
