using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Ci_Project.Controllers
{
    public class ResetController : Controller
    {
        private readonly IResetRepository _resetRepository;
        public ResetController(IResetRepository resetRepository)
        {
            _resetRepository = resetRepository;
        }
        [HttpGet]
        public IActionResult Reset(string Email, string Token)
        {
            var passreset = _resetRepository.ResetUser();
            var validEmail = passreset.FirstOrDefault(m => m.Email == Email && m.Token == Token);
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
            var user = _resetRepository.User(tempResetPasswordModel);
            var validEmail = user.FirstOrDefault(m => m.Email == tempResetPasswordModel.Email);
            if (validEmail != null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
