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
        public List<ComicBookViewModel> ComicBooks { get; set; }

        [BindProperty]
        public ComicBookViewModel SingleComicBook { get; set; }

        [BindProperty]
        public bool IsById { get; set; }

        public EditComicModel(ComicsDbContext ctx)
        {
            _comicsController = new ComicBookController(ctx);
        }

    }
}