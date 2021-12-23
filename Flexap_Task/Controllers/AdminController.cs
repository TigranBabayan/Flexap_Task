using Flexap_Task.Data;
using Flexap_Task.Models;
using Flexap_Task.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Flexap_Task.Helpers;



namespace Flexap_Task.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationContext _context;
        public AdminController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AdminLogin login)
        {
            if (ModelState.IsValid)
            {
                Admin admin = _context.Admin.FirstOrDefault(a => a.Email == login.Email && a.Password ==EncryptDecryptHelper.EncryptPlainTextToCipherText(login.Password));
                if (admin != null)
                {
                    HttpContext.Session.SetString("Email", login.Email);
                    return RedirectToAction("Index", "Products");
                }
            }

            return View(login);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(AdminRegistration register)
        {
            if (ModelState.IsValid)
            {
                Admin admin = _context.Admin.FirstOrDefault(a => a.Email == register.Email);
                if (admin == null)
                {
                    if (register.Password.Equals(register.ConfirmPassword))
                    {
                        _context.Admin.Add(new Admin { Email = register.Email, Password = EncryptDecryptHelper.EncryptPlainTextToCipherText(register.Password) });
                        _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Login", "Admin");
                }
            }


            return View(register);

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");

            return RedirectToAction("Login", "Admin");
        }
    }
}
