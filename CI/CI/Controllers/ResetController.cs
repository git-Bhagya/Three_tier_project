using Microsoft.AspNetCore.Mvc;
using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class ResetController : Controller
    {
        private readonly ILogger<ResetController> _logger;
        private readonly CiPlatformContext _db;


        public ResetController(ILogger<ResetController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Reset(string Email, string Token)
        {
            var validEmail = _db.PasswordResets.FirstOrDefault(m => m.Email == Email && m.Token == Token);
            if (validEmail != null)
            {
                var tempResetPassword = new TempResetPasswordModel()
                {
                    Email = Email,
                    Token = Token,
                };
                return View(tempResetPassword);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Reset(TempResetPasswordModel tempResetPasswordModel)
        {
            var validEmail = _db.Users.FirstOrDefault(m => m.Email == tempResetPasswordModel.Email);
            if (validEmail != null)
            {
                validEmail.Password = tempResetPasswordModel.NewPassword;
                _db.SaveChanges();
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
