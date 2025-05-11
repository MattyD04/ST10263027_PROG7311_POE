using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ST10263027_PROG7311_POE.Models
{
    //Farmer model class representing the Farmer entity
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; } //Primary key for Farmer entity

        [Column(TypeName = "nvarchar(MAX)")]
        public string? FarmerUserName { get; set; } //Username (email) of the farmer

        [Column(TypeName = "nvarchar(MAX)")]
        public string? FarmerPassword { get; set; } //Password of the farmer

        [Column(TypeName = "nvarchar(MAX)")]
        public string? FarmerContactNum { get; set; } //Contact number of the farmer
    }
}
//***********************************************End of file*****************************************//
