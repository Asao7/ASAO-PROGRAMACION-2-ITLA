using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExcursionManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        // GET api/reservations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _service.GetAllAsync();
            return Ok(reservations);
        }

        // GET api/reservations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _service.GetByIdAsync(id);
            if (reservation == null) return NotFound($"Reservation {id} not found.");
            return Ok(reservation);
        }

        // GET api/reservations/excursion/5
        [HttpGet("excursion/{excursionId}")]
        public async Task<IActionResult> GetByExcursion(int excursionId)
        {
            var reservations = await _service.GetByExcursionAsync(excursionId);
            return Ok(reservations);
        }

        // POST api/reservations
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            try
            {
                var id = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        // PATCH api/reservations/5/confirm
        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _service.ConfirmAsync(id);
            if (!result) return NotFound($"Reservation {id} not found.");
            return NoContent();
        }

        // PATCH api/reservations/5/cancel
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAsync(id);
            if (!result) return NotFound($"Reservation {id} not found.");
            return NoContent();
        }

        // PATCH api/reservations/5/attendance
        [HttpPatch("{id}/attendance")]
        public async Task<IActionResult> MarkAttendance(int id)
        {
            var result = await _service.MarkAttendanceAsync(id);
            if (!result) return NotFound($"Reservation {id} not found.");
            return NoContent();
        }

        // DELETE api/reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Cancel first to release the spot
            await _service.CancelAsync(id);
            return NoContent();
        }
    }
}
