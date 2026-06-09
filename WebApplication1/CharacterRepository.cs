using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1
{
    public interface ICharacterRepository
    {
        Task AddCharacter(Character character);
        Task<List<Character>> GetAllCharacters();
        Task<Character?> GetById(int id);
        Task UpdateCharacter(Character character);
        Task<bool> DeleteCharacter(int id);
    }

    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCharacter(Character character)
        {
            _context.Character.Add(character);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            return await _context.Character.ToListAsync();
        }

        public async Task<Character?> GetById(int id)
        {
            return await _context.Character.FindAsync(id);
        }

        public async Task UpdateCharacter(Character character)
        {
            _context.Character.Update(character);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCharacter(int id)
        {
            var entity = await _context.Character.FindAsync(id);
            if (entity == null) return false;
            _context.Character.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


/*Reszta jest w innych plikach ale wyglada tak
 
 builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CharacterDb"));


    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Character> Character => Set<Character>();
}
 */