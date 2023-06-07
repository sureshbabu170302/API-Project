using System.ComponentModel.DataAnnotations;

namespace APIprojectDoctorPatient.Models
{
    public class Patient
    {
        [Key]

        public int Patient_Id { get; set; }

        public string? Patient_Name { get; set; }

        public string? Disease { get; set; }

        public string? Email { get; set; }

        public int Patient_No { get; set; }

        public Doctor? doctor { get; set; }
    }
}
