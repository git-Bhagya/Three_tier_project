using CI.Entities.Data;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CI.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly CiPlatformContext _db;

        public LoginController(ILogger<LoginController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {

                var status = _db.Users.Where(u => u.Email == lvm.Email && u.Password == lvm.Password).FirstOrDefault();
                if (status != null)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, status.Email) },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, status.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, status.LastName));
                    identity.AddClaim(new Claim(ClaimTypes.Email, status.Email));


                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("EmailId", status.Email);
                    return RedirectToAction("Platform", "Home");
                }
                else
                {
                    TempData["Error"] = "Enter Correct details";
                    return RedirectToAction("Login", "Login");
                }
            }

            return View();
        }
    }
}
