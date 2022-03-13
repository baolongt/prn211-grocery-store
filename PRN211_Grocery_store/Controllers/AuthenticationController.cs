using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PRN211_Grocery_store.Data;

namespace PRN211_Grocery_store.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AuthenticationController(ApplicationDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            [FromForm] string email,
            [FromForm] string password)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }
            try
            {
                ViewData["Email"] = email;
                var user = _context.Users
                    .SingleOrDefault(user =>
                                user.email.ToLower().Equals(email.ToLower())
                                && user.password.Equals(password));

                if (user == null)
                {
                    throw new Exception("Login failed!! Please try again.");
                }

                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Name, user.name)
                };
                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                await HttpContext.SignInAsync(userPrincipal);
                return RedirectToAction("Index", "Products");
            }
            catch (Exception ex)
            {
                ViewData["Login"] = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Authentication");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
