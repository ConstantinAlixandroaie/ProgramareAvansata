using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgramareAvansataCA.Controllers;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;

namespace ProgramareAvansataCA.Pages
{
    public class LoginModel : PageModel
    {
        private AuthenticationController _authController;

        [BindProperty]
        public LoginViewModel Login { get; set; }

        public LoginModel(ComicsDbContext ctx)
        {
            _authController = new AuthenticationController(ctx);
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Home Page.  
                return RedirectToPage("/Profile");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLogIn()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var authResult = await _authController.Authenticate(Login);
            if (authResult.IsAuthenticated)
            {
                await SignInUser(authResult.Username, authResult.UserId.ToString(), true);
                return RedirectToPage("/Profile");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            return Page();
        }

        private async Task SignInUser(string username, string id, bool isPersistent)
        {
            // Initialization.  
            var claims = new List<Claim>();

            try
            {
                // Setting  
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.  
                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }
        }
    }
}