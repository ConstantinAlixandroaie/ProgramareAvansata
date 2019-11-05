using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgramareAvansataCA.Controllers;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;

namespace ProgramareAvansataCA.Pages
{
    public class AddComicModel : PageModel
    {
        private readonly ComicBookController _comicController;
        [BindProperty]
        public ComicBookViewModel comicBook { get; set; }
        public AddComicModel(ComicsDbContext ctx)
        {
            _comicController = new ComicBookController(ctx);
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");
            return Page();
        }
        public async Task<IActionResult> OnPostAdd()
        {
            await _comicController.Add(comicBook);
            return RedirectToPage("/Comics");
        }

    }
}