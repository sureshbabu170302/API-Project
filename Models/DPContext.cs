using Microsoft.EntityFrameworkCore;

namespace APIprojectDoctorPatient.Models
{
    public class DPContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }

        public DbSet<User> Users { get; set; }

        public DPContext(DbContextOptions<DPContext> options) : base(options) 
        {

        }
    }
}
