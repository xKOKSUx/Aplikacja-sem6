using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Context;
using WebApplication1.Hubs;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<CharacterHub> _hub;

        public IndexModel(AppDbContext db, IHubContext<CharacterHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public int DeleteId { get; set; }

        [BindProperty]
        public int EditId { get; set; }

        [BindProperty]
        public string EditName { get; set; } = string.Empty;

        public List<Character> Characters { get; set; } = new();

        public void OnGet(int? editId)
        {
            Characters = _db.Character.ToList();
            EditId = editId ?? 0;
        }

        // Dodawanie
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Characters = _db.Character.ToList();
                return Page();
            }

            _db.Character.Add(new Character { Name = Name });
            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("Refresh");

            return RedirectToPage();
        }

        // Usuwanie
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var character = await _db.Character.FindAsync(DeleteId);
            if (character != null)
            {
                _db.Character.Remove(character);
                await _db.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("Refresh");
            }

            return RedirectToPage();
        }

        // Zapisywanie edycji
        public async Task<IActionResult> OnPostSaveAsync()
        {
            var character = await _db.Character.FindAsync(EditId);
            if (character != null && !string.IsNullOrWhiteSpace(EditName))
            {
                character.Name = EditName;
                await _db.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("Refresh");
            }

            return RedirectToPage();
        }
    }
}
