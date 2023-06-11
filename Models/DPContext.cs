using Microsoft.EntityFrameworkCore;

namespace APIprojectDoctorPatient.Models
{
    public class DPContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DPContext(DbContextOptions<DPContext> options) : base(options) 
        {

        }
    }
}
