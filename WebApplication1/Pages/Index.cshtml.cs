using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public string Name { get; set; } = string.Empty;

        public List<Character> Characters { get; set; } = new();

        public void OnGet()
        {
            Characters = _db.Character.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Page();
            }

            var character = new Character { Name = Name };
            _db.Character.Add(character);
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
