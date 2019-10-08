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
    public class ProfileModel : PageModel
    {
        private readonly UserController _userController;

        [BindProperty]
        public UserViewModel ViewModel { get; set; }

        public ProfileModel(ComicsDbContext ctx)
        {
            _userController = new UserController(ctx);
        }

        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

            var idString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var id = int.Parse(idString);

            ViewModel = await _userController.GetByIdAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnDelete(int id)
        {
            await _userController.RemoveCollectionFromUser(id, ViewModel);
            return Page();
        }
    }
}