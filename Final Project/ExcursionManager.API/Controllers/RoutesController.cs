using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Route = ExcursionManager.Domain.Entities.Route;

namespace ExcursionManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IRepository<Route> _repository;

        public RoutesController(IRepository<Route> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var routes = await _repository.GetAllAsync();
            return Ok(routes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRouteDto dto)
        {
            var route = new Route(dto.Name, dto.Difficulty, dto.StartPoint, dto.EndPoint);
            var id = await _repository.CreateAsync(route);
            return Ok(new { id });
        }

        // 🔴 ESTE ERA EL QUE FALTABA
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateRouteDto
    {
        public string Name { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string StartPoint { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
    }
}