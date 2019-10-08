using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgramareAvansataCA.Controllers;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;

namespace ProgramareAvansataCA.Pages
{
    public class ComicsModel : PageModel
    {
        private readonly ComicBookController _comicsController;

        [BindProperty]
        public List<ComicBookViewModel> ComicBooks { get; set; }

        [BindProperty]
        public ComicBookViewModel SingleComicBook { get; set; }

        [BindProperty]
        public bool IsById { get; set; }

        public ComicsModel(ComicsDbContext ctx)
        {
            _comicsController = new ComicBookController(ctx);
        }


        public async Task<IActionResult> OnGet(int? qid = null)
        {
            if (qid != null)
                return await OnGetWithId(qid.Value);

            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

            ComicBooks = await _comicsController.GetAsync();

            return Page();
        }
        public async Task<IActionResult> OnGetWithId(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

            SingleComicBook = await _comicsController.GetByIdAsync(id);
            IsById = true;

            return Page();
        }
    }
}