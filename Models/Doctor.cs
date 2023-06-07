using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIprojectDoctorPatient.Models
{
    public class Doctor
    {
        [Key]
        public int Doctor_Id { get; set; }
        public string? Doctor_Name { get; set; }
        public string? Specialization { get; set; }
        public int Doctor_No { get; set; }
        public byte[]? ImageData { get; set; } // New property for image data

        public ICollection<Patient>? Patients { get; set; }
    }
}
