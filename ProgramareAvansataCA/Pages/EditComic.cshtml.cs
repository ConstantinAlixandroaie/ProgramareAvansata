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
    public class EditComicModel : PageModel
    {
        private readonly ComicBookController _comicsController;
        [BindProperty]
        public ComicBookViewModel ComicBook { get; set; }

        [BindProperty]
        public bool IsById { get; set; }

        public EditComicModel(ComicsDbContext ctx)
        {
            _comicsController = new ComicBookController(ctx);
        }
        public async Task<IActionResult> OnGet(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

           ComicBook = await _comicsController.GetByIdAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostEdit()
        {
            await _comicsController.Edit(ComicBook);
            return RedirectToPage("/Comics");
        }
    }
}