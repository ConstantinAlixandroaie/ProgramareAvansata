using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgramareAvansataCA.Controllers;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ProgramareAvansataCA.Pages
{
    public class EditComicModel : PageModel
    {
        private readonly ComicBookController _comicsController;
        private readonly ComicsDbContext _ctx;

        [BindProperty]
        public ComicBookViewModel ComicBook { get; set; }
        public int comicId;
        public EditComicModel(ComicsDbContext ctx)
        {
            _comicsController = new ComicBookController(ctx);
            _ctx = ctx;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

           ComicBook = await _comicsController.GetByIdAsync(id);
            comicId = ComicBook.Id;
            return Page();
            
        }
        public async Task<IActionResult> OnPostEdit(int id)
        {
            await _comicsController.Edit(ComicBook);

            return RedirectToPage("/Comics");
        }
    }
}