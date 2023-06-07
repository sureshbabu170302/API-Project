using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIprojectDoctorPatient.Models;
using Microsoft.AspNetCore.Authorization;

namespace APIprojectDoctorPatient.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DPContext _context;

        public PatientsController(DPContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
          if (_context.Patients == null)
          {
              return NotFound();
          } 
            return await _context.Patients.Include(x => x.doctor).ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
          if (_context.Patients == null)
          {
              return NotFound();
          }
            var patient = await _context.Patients.Include(x=>x.doctor).FirstOrDefaultAsync(x=>x.Patient_Id==id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.Patient_Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound(patient);
                }
                else
                {
                    throw;
                }
            }

            return Ok("Patient details Updated");

        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
          if (_context.Patients == null)
          {
              return Problem("Entity set 'DPContext.Patients'  is null.");
          }
            Doctor dt = await _context.doctors.FindAsync(patient.doctor.Doctor_Id);
            patient.doctor = dt;
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Patient_Id }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            if (_context.Patients == null)
            {
                return NotFound();
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("doctors/specialization/{specialization}")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpecialization(string specialization)
        {
            if (_context.doctors == null)
            {
                return NotFound();
            }

            var doctors = await _context.doctors
                .Where(d => d.Specialization == specialization)
                .GroupBy(d => d.Specialization)
                .Select(g => new { Specialization = g.Key, Doctors = g.ToList() })
                .ToListAsync();

            if (!doctors.Any())
            {
                return NotFound();
            }

            return Ok(doctors);
        }


        private bool PatientExists(int id)
        {
            return (_context.Patients?.Any(e => e.Patient_Id == id)).GetValueOrDefault();
        }
    }
}
