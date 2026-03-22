using Microsoft.AspNetCore.Mvc;
using Pets.API.DTOs;
using Pets.Domain.Entities;
using Pets.Domain.Repository;

namespace Pets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _repository;

        public PetsController(IPetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _repository.GetByIdAsync(id);

            if (pet == null)
                return NotFound();

            return pet;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePet(CreatePetDto dto)
        {
            var pet = new Pet
            {
                Name = dto.Name,
                Age = dto.Age,
                Species = dto.Species
            };

            await _repository.AddAsync(pet);

            return Ok(pet);
        }
    }
}