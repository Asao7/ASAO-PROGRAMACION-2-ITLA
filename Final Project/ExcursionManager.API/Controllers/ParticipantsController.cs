using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExcursionManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService _service;

        public ParticipantsController(IParticipantService service)
        {
            _service = service;
        }

        // GET api/participants
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var participants = await _service.GetAllAsync();
            return Ok(participants);
        }

        // GET api/participants/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var participant = await _service.GetByIdAsync(id);
            if (participant == null) return NotFound($"Participant {id} not found.");
            return Ok(participant);
        }

        // POST api/participants
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateParticipantDto dto)
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

        // PUT api/participants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateParticipantDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                if (!result) return NotFound($"Participant {id} not found.");
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/participants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound($"Participant {id} not found.");
            return NoContent();
        }
    }
}
