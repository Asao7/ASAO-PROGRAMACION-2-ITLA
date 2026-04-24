using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExcursionManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcursionsController : ControllerBase
    {
        private readonly IExcursionService _service;

        public ExcursionsController(IExcursionService service)
        {
            _service = service;
        }

        // GET api/excursions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var excursions = await _service.GetAllAsync();
            return Ok(excursions);
        }

        // GET api/excursions/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var excursions = await _service.GetAvailableAsync();
            return Ok(excursions);
        }

        // GET api/excursions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var excursion = await _service.GetByIdAsync(id);
            if (excursion == null) return NotFound($"Excursion {id} not found.");
            return Ok(excursion);
        }

        // POST api/excursions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExcursionDto dto)
        {
            try
            {
                var id = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/excursions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExcursionDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound($"Excursion {id} not found.");
            return NoContent();
        }

        // PATCH api/excursions/5/cancel
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAsync(id);
            if (!result) return NotFound($"Excursion {id} not found.");
            return NoContent();
        }

        // DELETE api/excursions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound($"Excursion {id} not found.");
            return NoContent();
        }
    }
}