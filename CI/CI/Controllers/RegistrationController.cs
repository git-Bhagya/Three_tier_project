using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly CiPlatformContext _db;

        public RegistrationController(ILogger<RegistrationController> logger,CiPlatformContext db)
        {
            _logger = logger;
                _db = db;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("/Registration/Registration", Name = "Register")]
        public IActionResult Registration(RegistrationViewModel obj)
        {
            var User_data = new User()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Password = obj.Password,
                CityId = 5,
                CountryId = 1
            };
            _db.Users.Add(User_data);
            _db.SaveChanges();
            return RedirectToAction("Login","Login");

        }
    }
}
