
using System.ComponentModel.DataAnnotations;

namespace APIprojectDoctorPatient.Models
{
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }
        public string? Customer_Name { get; set; } = string.Empty;
        public string? Customer_Email { get; set; }
        public string? Customer_Password { get; set; }
    }
}
