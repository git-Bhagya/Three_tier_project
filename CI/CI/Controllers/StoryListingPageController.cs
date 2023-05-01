using CI.Entities.Data;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class StoryListingPageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiPlatformContext _db;

        public StoryListingPageController(ILogger<HomeController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult StoryListingPage()
        {
            ViewBag.mission_theme = _db.MissionThemes.ToList();
            ViewBag.skill = _db.Skills.ToList();
            ViewBag.cities = _db.Cities.ToList();
            ViewBag.countries = _db.Countries.ToList();

            var story = _db.Stories.ToList();
           
            GeneralViewModel svm = new GeneralViewModel()
            {
                Users = _db.Users.ToList(),
                stories = story,
            };

            return View(svm);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");

        }
    }
}
