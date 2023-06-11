using APIprojectDoctorPatient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIprojectDoctorPatient.Controllers
{
    //[Authorize(Roles = "Customer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DPContext _DContext;

        public DoctorController(DPContext Context)
        {
            _DContext = Context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> Get()
        {
            try
            {
                return await _DContext.doctors.Include(d => d.Patients).ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> Get(int id)
        {
            try
            {
                Doctor dt = await _DContext.doctors.SingleOrDefaultAsync(x => x.Doctor_Id == id);
                if (dt == null)
                {
                    return NotFound($"Doctor with id = {id} not found.");
                }
                return dt;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> Post(Doctor dt)
        {
            try
            {
                await _DContext.doctors.AddAsync(dt);
                _DContext.SaveChanges();
                return Ok(dt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error inserting data into the database: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Doctor dt)
        {
            try
            {
                if (id != dt.Doctor_Id)
                {
                    return BadRequest("Doctor id mismatch.");
                }

                _DContext.Entry(dt).State = EntityState.Modified;
                await _DContext.SaveChangesAsync();
                return Ok(dt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating data in the database: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var dp = await _DContext.doctors.FindAsync(id);
                if (dp == null)
                {
                    return NotFound($"Doctor with id = {id} not found.");
                }
                _DContext.doctors.Remove(dp);
                await _DContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting data from the database: {ex.Message}");
            }
        }

        [HttpGet("{id}/patients/count")]
        public async Task<ActionResult<int>> GetPatientCountByDoctorId(int id)
        {
            try
            {
                if (_DContext.doctors == null)
                {
                    return NotFound($"Doctor with id = {id} not found.");
                }
                int count = await _DContext.Patients.CountAsync(p => p.doctor.Doctor_Id == id);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetDoctorImage(int id)
        {
            try
            {
                Doctor doctor = await _DContext.doctors.FindAsync(id);
                if (doctor == null)
                {
                    return NotFound($"Doctor with id = {id} not found.");
                }

                if (doctor.ImageData == null)
                {
                    return NotFound("No image found for this doctor.");
                }

                return File(doctor.ImageData, "image/jpg"); // Assuming image format is JPEG
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving image from the database: {ex.Message}");
            }
        }

    }
}
