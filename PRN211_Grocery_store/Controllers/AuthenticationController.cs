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
using Microsoft.AspNetCore.Http;
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
            return View("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }
            try
            {
                ViewData["Email"] = email;
                var user = _context.Users
                    .SingleOrDefault(user =>
                                user.Email.ToLower().Equals(email.ToLower())
                                && user.Password.Equals(password));

                if (user == null)
                {
                    throw new Exception("Login failed!! Please try again.");
                }
                HttpContext.Session.SetString("username", user.Username);
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier , user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                };
                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                await HttpContext.SignInAsync(userPrincipal);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewData["Login"] = ex.Message;
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("cart");
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Authentication");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
