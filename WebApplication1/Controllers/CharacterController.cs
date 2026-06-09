using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _repo;

        public CharactersController(ICharacterRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetAll()
        {
            var list = await _repo.GetAllCharacters();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetById(int id)
        {
            var character = await _repo.GetById(id);
            if (character == null) return NotFound();
            return Ok(character);
        }

        [HttpPost]
        public async Task<ActionResult<Character>> Create([FromBody] Character character)
        {

            await _repo.AddCharacter(character);
            return CreatedAtAction(nameof(GetById), new { id = character.Id }, character);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Character character)
        {
            if (id != character.Id) return BadRequest();

            var existing = await _repo.GetById(id);
            if (existing == null) return NotFound();

            existing.Name = character.Name;

            await _repo.UpdateCharacter(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removed = await _repo.DeleteCharacter(id);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}