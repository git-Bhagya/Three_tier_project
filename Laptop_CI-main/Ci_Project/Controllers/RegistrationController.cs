using Microsoft.AspNetCore.Mvc;
using Ci_Project.Repository.Interface;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Entities.Models;

namespace Ci_Project.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegistrationRepository _registrationRepository;
        public RegistrationController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("/Registration/Registration", Name = "Register")]
        public IActionResult Registration(RegistrationViewModel obj)
        {
           var loginSucc = _registrationRepository.Registration(obj);
            if(loginSucc)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                TempData["Error"] = "Please Enter Valid Data ";
                return RedirectToAction("Registration","Registration");
            }

        }

    }
}
