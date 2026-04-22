using ExcursionManager.Application.DTOs;
using ExcursionManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExcursionManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuidesController : ControllerBase
    {
        private readonly IGuideService _service;

        public GuidesController(IGuideService service)
        {
            _service = service;
        }

        // GET api/guides
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var guides = await _service.GetAllAsync();
            return Ok(guides);
        }

        // GET api/guides/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var guides = await _service.GetActiveAsync();
            return Ok(guides);
        }

        // GET api/guides/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var guide = await _service.GetByIdAsync(id);
            if (guide == null) return NotFound($"Guide {id} not found.");
            return Ok(guide);
        }

        // GET api/guides/search?name=carlos
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var guides = await _service.SearchAsync(name);
            return Ok(guides);
        }

        // GET api/guides/search/specialty?specialty=mountaineering
        [HttpGet("search/specialty")]
        public async Task<IActionResult> SearchBySpecialty([FromQuery] string specialty)
        {
            var guides = await _service.SearchBySpecialtyAsync(specialty);
            return Ok(guides);
        }

        // POST api/guides
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGuideDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // PUT api/guides/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateGuideDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound($"Guide {id} not found.");
            return NoContent();
        }

        // DELETE api/guides/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound($"Guide {id} not found.");
            return NoContent();
        }
    }
}
