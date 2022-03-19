using Microsoft.AspNetCore.Mvc;
using PRN211_Grocery_store.Data;
using PRN211_Grocery_store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRN211_Grocery_store.Data.Entity;
using PRN211_Grocery_store.Models.DAO;

namespace PRN211_Grocery_store.Controllers
{
    public class RegisterController : Controller { 
         private readonly ApplicationDBContext _context;

    public RegisterController(ApplicationDBContext context)
    {
        _context = context;
    }

        public  IActionResult Register([Bind("Username,Password,ConfirmPassword,Name,Email,Phone")] UserRegister userRegister )
        {
              
            if (userRegister == null)
            {
                return NotFound();
            }
            User user = new User
            {
                Username = userRegister.Username,
                Password = userRegister.Password,
                Name = userRegister.Name,
                Email = userRegister.Email,
                Phone = userRegister.Phone,
                IsAdmin = false,
                
            };

            var checkUser = UserRegisterDAO.Instance.CheckUserDuplicate(user.Email);
            if (checkUser == null && ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(userRegister);
        
        }
    }
}
