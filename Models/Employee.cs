using System.ComponentModel.DataAnnotations;

namespace ST10263027_PROG7311_POE.Models
{
    //Employee model class representing the Employee entity
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } //Primary key for Employee entity
        public string userName { get; set; } //Username (email) of the employee
        public string password { get; set; } //Password of the employee
    }
}
//***********************************************End of file*****************************************//
