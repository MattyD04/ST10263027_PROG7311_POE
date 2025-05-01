using System.ComponentModel.DataAnnotations;

namespace ST10263027_PROG7311_POE.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
