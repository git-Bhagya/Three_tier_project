using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace CI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiPlatformContext _db;

        public HomeController(ILogger<HomeController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }

        public User getUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            var name = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var surname = identity?.FindFirst(ClaimTypes.Surname)?.Value;
            var Email = identity?.FindFirst(ClaimTypes.Email)?.Value;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var user = _db.Users.Where(x=>x.Email==Email).FirstOrDefault();
            return user;
        }

        //[Authorize]
        public IActionResult Platform(string search, int pageIndex = 1, int pageSize = 3, int id = 0)
        {
            //ViewBag.missons = _db.Missions.ToList();
            ViewBag.mission_theme = _db.MissionThemes.ToList();
            ViewBag.skill = _db.Skills.ToList();
            ViewBag.cities = _db.Cities.ToList();
            ViewBag.countries = _db.Countries.ToList();
            //List<Mission> generalViewModel = _db.Missions.ToList();

            GeneralViewModel generalViewModel = new()
            {

                Countries = GetCountries(),
                Cities = GetCities(),
                Missions = GetMissions(search),
                MissionThemes = GetMissionThemes(),
                Skills = GetSkills(),
                mission_details = getMissionDetails(id),
                missionRatings = GetMissionRatings(),
                Users = _db.Users.ToList(),
            };

            if (id == 1)
            {
                generalViewModel.Missions = generalViewModel.Missions.OrderByDescending(p => p.StartDate).ToList();
            }
            else if (id == 2)
            {
                generalViewModel.Missions = generalViewModel.Missions.OrderBy(p => p.StartDate).ToList();
            }
            else if (id == 3)
            {
                generalViewModel.Missions = generalViewModel.Missions.OrderByDescending(p => p.Availability).ToList();
            }
            else if (id == 4)
            {
                generalViewModel.Missions = generalViewModel.Missions.OrderBy(p => p.Availability).ToList();
            }
            else if (id == 5)
            {
                generalViewModel.Missions = generalViewModel.Missions.Where(p => p.MissionType == "Goal").ToList();
            }
            else if (id == 6)
            {
                generalViewModel.Missions = generalViewModel.Missions.Where(p => p.MissionType == "Time").ToList();
            }
            generalViewModel.totalrecord = generalViewModel.Missions.Count();
            generalViewModel.currentPage = pageIndex;
            generalViewModel.Missions = generalViewModel.Missions.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return View(generalViewModel);
        }

        public List<Country> GetCountries()
        {
            List<Country> countries = _db.Countries.ToList();
            return countries;
        }
        public List<City> GetCities()
        {
            List<City> cities = _db.Cities.ToList();
            return cities;
        }

        public List<Mission> GetMissions(string str)
        {
            //search
            List<Mission> missions = new List<Mission>();
            if (str == null)
            {
                missions = _db.Missions.ToList();

            }
            else
            {
                missions = _db.Missions.Where(m => m.Title.ToLower().Contains(str.ToLower())).ToList();
            }
            return missions;
        }
        public List<MissionTheme> GetMissionThemes()
        {
            List<MissionTheme> missionThemes = _db.MissionThemes.ToList();
            return missionThemes;
        }
        public List<Skill> GetSkills()
        {
            List<Skill> skills = _db.Skills.ToList();
            return skills;
        }

        public Mission getMissionDetails(int? id)
        {
            var missionDetails = _db.Missions.Where(u => u.MissionId == id).FirstOrDefault();
            return missionDetails;
        }
        public List<MissionRating> GetMissionRatings()
        {
            List<MissionRating> MissionRatings = _db.MissionRatings.ToList();
            return MissionRatings;
        }

        public IActionResult Volunteer(int id)
        {
            ViewBag.mission_theme = _db.MissionThemes.ToList();
            ViewBag.skill = _db.Skills.ToList();
            ViewBag.cities = _db.Cities.ToList();
            ViewBag.countries = _db.Countries.ToList();

            var mission_data = _db.Missions.Where(x => x.MissionId == id).FirstOrDefault();
            var goal_data = _db.GoalMissions.Where(x => x.MissionId == id).FirstOrDefault();
            var city_data = _db.Cities.Where(x => x.CityId == mission_data.CityId).FirstOrDefault();
            var mission_theme_data = _db.MissionThemes.Where(x => x.MissionThemeId == mission_data.ThemeId).FirstOrDefault();
            var relatedMissions = GetRelatedMission(id);
            var mission_rating = GetMissionRatings(id);
            var users = _db.Users.ToList();
            var docs = _db.MissionDocuments.ToList();
            var fav = _db.FavoriteMissions.Where(x=>x.MissionId==id).ToList();

            GeneralViewModel gvm = new GeneralViewModel()
            {
                mission_details = mission_data,
                GoalMission = goal_data,
                City = city_data,
                MissionTheme = mission_theme_data,
                RelatedMissions = relatedMissions,
                missionRatings = mission_rating,
                comments = _db.Comments.ToList(),
                Users = users,
                Documents = docs,
                FavoriteMissions = fav,
                missionApplications = _db.MissionApplications.ToList(),
            };
            gvm.NumberOfVolunteers = mission_rating.Count();
            gvm.AvarageOfRating = (int)mission_rating.Average(x => x.Rating);

            return View(gvm);
        }

        public IEnumerable<MissionRating> GetMissionRatings(int id)
        {
            var missionratings = new List<MissionRating>();
            if (id != 0 || id != null)
            {
                missionratings = _db.MissionRatings.Where(x => x.MissionId == id).ToList();
            }
            return missionratings;
        }

        public IEnumerable<Mission> GetRelatedMission(int id)
        {
            var relatedmissions = new List<Mission>();
            var thismission = _db.Missions.Where(m => m.MissionId.Equals(id)).FirstOrDefault();
            var relatedmissions_by_city = _db.Missions.Where(m => m.MissionId != thismission.MissionId && m.CityId == thismission.CityId).Take(3).ToList();
            var relatedmissions_by_country = _db.Missions.Where(m => m.MissionId != thismission.MissionId && m.CountryId == thismission.CountryId).Take(3).ToList();
            var relatedmissions_by_theme = _db.Missions.Where(m => m.MissionId != thismission.MissionId && m.MissionId == thismission.MissionId).Take(3).ToList();

            if (relatedmissions_by_city.Count() > 0)
            {
                relatedmissions = relatedmissions_by_city;
            }
            else if (relatedmissions_by_country.Count() > 0)
            {
                relatedmissions = relatedmissions_by_country;
            }
            else
            {
                relatedmissions = relatedmissions_by_theme;
            }

            foreach (var mission in relatedmissions)
            {
                _db.Entry(mission).Reference(c => c.City).Load();
                _db.Entry(mission).Reference(t => t.Theme).Load();
            }
            return relatedmissions;
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Add rating to database
        public string updateandaddrate(int missionid, int rating, string Email)
        {
            var mission_rating = _db.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            var rate_update = mission_rating.SingleOrDefault(m => m.User.Email == Email && m.Mission.MissionId == missionid);

            if (rate_update != null)
            {

                rate_update.Rating = rating;
                _db.Update(rate_update);
                _db.SaveChanges();

            }
            if (rate_update == null)
            {
                var userId = _db.Users
                .Where(u => u.Email == Email)
                .Select(u => u.UserId)
                .FirstOrDefault();
                var missionrating = new MissionRating
                {
                    MissionId = missionid,
                    UserId = userId,
                    Rating = rating,

                };

                _db.Add(missionrating);
                _db.SaveChanges();

            }
            return "successfull";
        }
        //Add to Favorite
        public string AddToFav(int missionid, string email)
        {
            var userid = _db.Users.Where(u => u.Email == email).Select(u => u.UserId).FirstOrDefault();
            var mission_fav = _db.FavoriteMissions.Include(m => m.Mission).Include(m => m.User).ToList();
            var fav_update = mission_fav.FirstOrDefault(m => m.User.UserId == userid && m.Mission.MissionId == missionid);

            if (fav_update == null)
            {
                var userId = _db.Users.Where(u => u.UserId == userid).Select(u => u.UserId).FirstOrDefault();
                var favorite = new FavoriteMission
                {
                    MissionId = missionid,
                    UserId = userId,
                };
                _db.Add(favorite);
                _db.SaveChanges();
            }
            else
            {
                var unfavourite = _db.FavoriteMissions.Where(u => u.UserId == userid).FirstOrDefault();
                _db.Remove(unfavourite);
                _db.SaveChanges();
            }
            return "Success";
        }

        //comments
        [HttpPost]
        public string PostComment(int missionid,string comment , string email)
        {
            var user = _db.Users.Where(x=>x.Email==email).Select(x => x.UserId).FirstOrDefault();
            var post = new Comment
            {
                MissionId=missionid,
                UserId=user,
                Comments = comment,
            };
            _db.Add(post);
            _db.SaveChanges();
            return "comments add successfully";
        }

        //recommended
        public string Recommend(int missionid, int[] to)
        {
            foreach (var id in to)
            {
                string url = Url.Action("Volunteer", "Home", new { id = missionid }, Request.Scheme);
                var user = _db.Users.SingleOrDefault(m => m.UserId == id);
                var resetLink = url;

                var from = new MailAddress("virupatel6048@gmail.com", "Mail From Bhagya Patel");

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
                    Credentials = new NetworkCredential("virupatel6048@gmail.com", "zwsgqrwabnvhmpfg"),
                    EnableSsl = true
                };
                smtpClient.Send(message);
            }
            //ViewData["result"] = "success";
            return "success";
        }

        //recommended for platfrom page
        public string RecommendPlatform(int missionid, int[] to)
        {
            foreach (var id in to)
            {
                string url = Url.Action("Platform", "Home", new { id = missionid }, Request.Scheme);
                var user = _db.Users.SingleOrDefault(m => m.UserId == id);
                var resetLink = url;

                var from = new MailAddress("virupatel6048@gmail.com", "Mail From Bhagya Patel");

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
                    Credentials = new NetworkCredential("virupatel6048@gmail.com", "zwsgqrwabnvhmpfg"),
                    EnableSsl = true
                };
                smtpClient.Send(message);
            }
            //ViewData["result"] = "success";
            return "success";
        }

        //applied mission
        public string applyForMission(int id)
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

            //var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var uid = _db.Users.Where(u => u.Email == email).Select(u => u.UserId).FirstOrDefault();


            var alreadyApplied = _db.MissionApplications.Where(x=>x.UserId==Convert.ToInt32(uid) && x.MissionId==id).FirstOrDefault();
            if(alreadyApplied == null)
            {
                MissionApplication missionApp = new MissionApplication()
                {
                    MissionId = id,
                    UserId = Convert.ToInt32(uid),
                    AppliedAt = DateTime.Now,
                };
                _db.Add(missionApp);
            }
            else
            {
                var deleteData = _db.MissionApplications.Where(x=>x.UserId==Convert.ToInt32(uid) && x.MissionId==id).FirstOrDefault();
                _db.Remove(deleteData);
            }
            _db.SaveChanges();
            return "successfully return";
        }
    }
}
