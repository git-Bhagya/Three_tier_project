using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CI.Controllers
{
    public class ShareStoryController : Controller
    {
        private readonly ILogger<ShareStoryController> _logger;
        private readonly CiPlatformContext _db;

        public ShareStoryController(ILogger<ShareStoryController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult ShareStory()
        {
            var mission_data = _db.Missions.FirstOrDefault();
            GeneralViewModel ss = new GeneralViewModel()
            {
                Missions = GetMissions(),
                Users = _db.Users.ToList(),
                mission_details = mission_data,
            };
            return View(ss);
        }
        public List<Mission> GetMissions()
        {
            List<Mission> missions = new List<Mission>();
            return missions;
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");

        }
      
    }
}
