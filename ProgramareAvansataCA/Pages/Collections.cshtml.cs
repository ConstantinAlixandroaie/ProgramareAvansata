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
    public class CollectionsModel : PageModel
    {
        private readonly CollectionController _collectionController;
        private readonly UserController _userController;

        [BindProperty]
        public List<CollectionViewModel> Collections { get; set; }

        [BindProperty]
        public CollectionViewModel SingleCollection { get; set; }

        [BindProperty]
        public bool IsById { get; set; }

        [BindProperty]
        public bool IsInUserCollection { get; set; }

        public CollectionsModel(ComicsDbContext ctx)
        {
            _collectionController = new CollectionController(ctx);
            _userController = new UserController(ctx);
        }


        public async Task<IActionResult> OnGet(int? qid = null)
        {
            if (qid != null)
                return await OnGetWithId(qid.Value);

            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");


            Collections = await _collectionController.GetAsync();

            return Page();
        }
        public async Task<IActionResult> OnGetWithId(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");

            SingleCollection = await _collectionController.GetByIdAsync(id);
            IsById = true;

            var idString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var uid = int.Parse(idString);

            var usr = await _userController.GetByIdAsync(uid);

            IsInUserCollection = await _collectionController.IsInUserCollection(id, usr.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToUserCollections(int id)
        {
            var idString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var uid = int.Parse(idString);

            var usr = await _userController.GetByIdAsync(uid);


            await _userController.AddCollectionToUser(id, usr);

            return Page();
        }
    }
}