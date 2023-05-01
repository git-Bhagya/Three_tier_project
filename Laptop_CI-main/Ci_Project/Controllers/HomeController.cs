using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;

using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace Ci_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homerepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homerepository = homeRepository;
        }
        //get the user
        public User GetThisUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

            List<User> getUserList = _homerepository.RecommendedUser();
            User? user = getUserList.Where(m => m.Email == email).FirstOrDefault();
            return user;
        }

        //logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Platform", "Home");

        }


        public IActionResult Platform(int id,int pg=1)
        {
            var navBarData = _homerepository.LandingPageList(id,pg);
            
            return View(navBarData);
        }

        [HttpPost]
        public IActionResult Platform(int id,string[]? country, string[]? city, string[]? themes, string[]? skills, string? sortVal, string? search, int pg = 1)
        {

            var landingPageData = _homerepository.PlatformUser(id,country, city, themes, skills, sortVal, search, pg);

            GeneralViewModel landVM = new()
            {
                
                MissionThemes = landingPageData.MissionThemes,
                missionRatings = landingPageData.missionRatings,
                Skills = landingPageData.Skills,
                GoalMissions = landingPageData.GoalMissions,
                goal = landingPageData.goal,
                FavoriteMissions = landingPageData.FavoriteMissions,
                Users = landingPageData.Users,
                //Missions = landingPageData.Missions,
                Skill = landingPageData.Skill,
                Countries = landingPageData.Countries,
                Cities = landingPageData.Cities,
                MissionMedia = landingPageData.MissionMedia,
                missionApplications = landingPageData.missionApplications,


            };


            List<Mission> missions = (List<Mission>)landingPageData.Missions;

            if (search != null)
            {
                missions = missions.Where(m => m.Title.ToLower().Contains(search.ToLower())).ToList();
            }

            if (country.Length > 0 || city.Length > 0 || themes.Length > 0 || skills.Length > 0)
            {
                missions = FilterMission(missions, country, city, themes, skills);
            }

            //pagination
            int Count = missions.Count();
            const int pageSize = 3;
            if (pg < 1)
                pg = 1;
            var pager = new Pager(Count, pg, pageSize);

            int Skip = (pg - 1) * pageSize;

            missions = missions.Skip(Skip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            ViewBag.missionCount = Count;

            missions = SortingData(sortVal, missions);
            landVM.Missions = missions;
           
            return PartialView("_cardsPartialView", landVM);

        }

        public List<Mission> SortingData(string sortVal, List<Mission> missions)
        {
            switch (sortVal)
            {
                case "Newest":
                    return missions.OrderByDescending(p => p.StartDate).ToList();
                case "Oldest":
                    return missions.OrderBy(p => p.StartDate).ToList();
                case "Lowest":
                    return missions.OrderBy(p => p.Availability).ToList();
                case "Higgest":
                    return missions.OrderByDescending(p => p.Availability).ToList();
                case "Goal":
                    return missions.Where(m => m.MissionType.Equals("Goal")).ToList();
                case "Time":
                    return missions.Where(m => m.MissionType.Equals("Time")).ToList();
                default:
                    return missions.ToList();
            }
        }


        public List<Mission> FilterMission(List<Mission> missions, string[]? country, string[]? city, string[]? themes, string[]? skills)
        {
            if (country?.Length > 0)
            {
                missions = missions.Where(C => country.Contains(C.Country.Name)).ToList();
            }
            if (city?.Length > 0)
            {
                missions = missions.Where(c => city.Contains(c.City.Name)).ToList();
            }
            if (themes?.Length > 0)
            {
                missions = missions.Where(t => themes.Contains(t.Theme.Title)).ToList();
            }
            /*	if (skills.Length > 0)
				{
					missions = missions.Where(s => skills.Contains(s.MissionSkills.)).ToList();
				}*/
            return missions;
        }

        //VOLUNTEER PAGE
        [Authorize]
        public IActionResult Volunteer(int id, int pageIndex = 1, int pageSize = 6)
        {
            var identity = User.Identity as ClaimsIdentity; 
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            
            var success = _homerepository.VolunteerUser(id,Convert.ToInt32(uid), pageIndex, pageSize);
            return View(success);
        }
        //Add to Favorite
        [HttpPost]
        public IActionResult FavouriteMission(int mId)
        {

            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;

            var missionRating = _homerepository.favLanding(mId, uid);
            return RedirectToAction("Volunteer", new { id = mId });
        }

        public IActionResult AddToFav(int missionid,string email)
        {
            //var user = GetThisUser();
            _homerepository.favUser(missionid, email);
            return RedirectToAction("Volunteer","Home",new {id=missionid});  
        }
       
        //Add rating to database
        public IActionResult updateandaddrate(int missionid, int rating, string Email)
        {
            //var rate = _homerepository.RatingUser(missionid, rating, Email);
            //return View(rate);

            _homerepository.RatingUser(missionid, rating, Email);
            return RedirectToAction("Volunteer", new { id = missionid });
        }
        //comments 
        public IActionResult PostComment(int missionid, string comment ,string email)
        {
            _homerepository.CommentUser(missionid, comment , email);
            return RedirectToAction("Volunteer", new { id = missionid });
        }
        //Recommended
        public void Recommend(int missionid, int[] to)
        {
            foreach (var id in to)
            {
                List<User> getUserList = _homerepository.RecommendedUser();
                string url = Url.Action("Volunteer", "Home", new { id = missionid }, Request.Scheme);
                var user = getUserList.SingleOrDefault(m => m.UserId == id);
                var resetLink = url;

                var from = new MailAddress("patelviral7180@gmail.com", "Mail From Bhagya Patel");

                var To = new MailAddress(user.Email);
                var subject = "Recommendation for volunteering a mission";
                var body = $"Hi,<br /><br />Please click on the following link to apply for mission:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                var message = new MailMessage(from, To)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("patelviral7180@gmail.com", "zztpvcsykhzpxari"),
                    EnableSsl = true
                };
                smtpClient.Send(message);

            }
            //ViewData["result"] = "success";
            //return View();
        }

        //Applied Mission
        public void applyForMission(int id)
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

            List<User> getUserList = _homerepository.RecommendedUser();

            var uid = getUserList.Where(u => u.Email == email).Select(u => u.UserId).FirstOrDefault();
            var applybtn = _homerepository.ApplyUser(id, uid);
            //return View(applybtn);
        }


        //story details page
        [Authorize]
        public IActionResult StoryDetails(string search,int id,int Mid,int view)
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

            List<User> getUserList = _homerepository.RecommendedUser();

            var uid = getUserList.Where(u => u.Email == email).Select(u => u.UserId).FirstOrDefault();
   
            var details = _homerepository.StoryDetailsUser(search,id,uid, Mid, view);
            return View(details);
        }

       
    }
}