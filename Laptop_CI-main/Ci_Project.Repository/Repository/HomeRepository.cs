using Ci_Project.Entities.Data;
using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Repository
{
    public class HomeRepository:IHomeRepository
    {
        public readonly CiPlatformContext _db;
        public HomeRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public GeneralViewModel LandingPageList(int id,int pg=1)
        {
            GeneralViewModel gvm = new GeneralViewModel();

            List<Country> country = GetCountries();
            gvm.Countries = country;
            
            List<Admin> admin = _db.Admins.ToList();
            gvm.Admin = admin;

            List<MissionApplication> missionApp = GetMissionApp();
            gvm.missionApplications = missionApp;

            List<City> city = GetCities();
            gvm.Cities = city;

            List<GoalMission> goal = _db.GoalMissions.ToList();
            gvm.goal = goal;

            GoalMission goal_data = GetGoalData();
            gvm.GoalMissions = goal_data;

            List<Mission> mission =_db.Missions.ToList();
            gvm.Missions = mission;

            List<MissionTheme> missionTheme = GetMissionThemes();
            gvm.MissionThemes = missionTheme;

            List<Skill> skill = GetSkills();
            gvm.Skills = skill;

            Mission mission_data = GetMissionData(id);
            gvm.mission_details = mission_data;
            
            MissionMedium mission_media = _db.MissionMedia.FirstOrDefault();
            gvm.mediaMission = mission_media;

            List<MissionRating> missionRating = GetMissionRatings();
            gvm.missionRatings = missionRating;

            List<User> user = GetUser();
            gvm.Users = user;
            
            

            IEnumerable<FavoriteMission> fav = GetFav(id);
            gvm.FavoriteMissions = fav;

            return gvm;
        }
        //platform landing page
        public GeneralViewModel PlatformUser(int id,string[]? country, string[]? city, string[]? themes, string[]? skills, string? sortVal, string? search, int pg = 1)
        {
            GeneralViewModel gvm = new GeneralViewModel();

            List<Country> countries = GetCountries();
            gvm.Countries = countries;

            List<MissionApplication> missionApp = GetMissionApp();
            gvm.missionApplications = missionApp;

            List<City> cities = GetCities();
            gvm.Cities = cities;

            List<GoalMission> goal = _db.GoalMissions.ToList();
            gvm.goal = goal;

            GoalMission goal_data = GetGoalData();
            gvm.GoalMissions = goal_data;

            List<Mission> mission = _db.Missions.Where(x=>x.Status != "In-active").ToList();
            gvm.Missions = mission;

            List<MissionTheme> missionTheme = GetMissionThemes();
            gvm.MissionThemes = missionTheme;

            List<Skill> skill = GetSkills();
            gvm.Skills = skill;

            Mission mission_data = GetMissionData(id);
            gvm.mission_details = mission_data;

            List<MissionRating> missionRating = GetMissionRatings();
            gvm.missionRatings = missionRating;

            List<User> user = GetUser();
            gvm.Users = user;

            IEnumerable<FavoriteMission> fav = GetFav(id);
            gvm.FavoriteMissions = fav;

            List<MissionMedium> mission_media = GetMissionMedia();
            gvm.MissionMedia = mission_media;

            return gvm;
        }

        //fav in landing page
        public string favLanding(int mId, string uid)
        {
            var userId = Convert.ToInt32(uid);
            if (userId != null)
            {
                var data = _db.FavoriteMissions.Where(x => x.MissionId == mId && x.UserId == userId).FirstOrDefault();

                if (data != null)
                {
                    _db.FavoriteMissions.Remove(data);
                }
                else
                {
                    FavoriteMission fm = new FavoriteMission();
                    fm.UserId = userId;
                    fm.MissionId = mId;
                    _db.Add(fm);
                }

                _db.SaveChanges();
                return "Success";
            }
            return "Failure";
        }
        public List<MissionApplication> GetMissionApp()
        {
            List<MissionApplication> missions = _db.MissionApplications.ToList();
            return missions;
        }
        public List<GoalMission> GetGoalmission()
        {
            List<GoalMission> goal = _db.GoalMissions.ToList();
            return goal;
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
        public List<User> GetUser()
        {
            List<User> user = _db.Users.ToList();
            return user;
        }
        public User getUserDetails(int? id)
        {
            var user = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
            return user;
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

        public Mission? getMissionDetails(int? id)
        {
            var missionDetails = _db.Missions.Where(u => u.MissionId == id).FirstOrDefault();
            return missionDetails;
        }
        public List<MissionRating> GetMissionRatings()
        {
            List<MissionRating> MissionRatings = _db.MissionRatings.ToList();
            return MissionRatings;
        }
        public List<MissionMedium> GetMissionMedia()
        {
            List<MissionMedium> MissionMedium = _db.MissionMedia.ToList();
            return MissionMedium;
        }

        //Volunteer Page
        public GeneralViewModel VolunteerUser(int id,int uid, int pageIndex = 1, int pageSize = 3)
        {
            GeneralViewModel gvm = new GeneralViewModel();

            List<GoalMission> goal = _db.GoalMissions.ToList();
            gvm.goal = goal;

            Mission mission_data = GetMissionData(id);
            gvm.mission_details = mission_data;

            GoalMission goal_data = GetGoalData();
            gvm.GoalMissions = goal_data;

            City city_data = GetCityName(id);
            gvm.City = city_data;

            List<MissionMedium> mission_media = GetMissionMedia();
            gvm.MissionMedia = mission_media;
            //MissionTheme mission_theme_data = GetMissionThemeData();
            //gvm.MissionTheme = mission_theme_data;

            IEnumerable<Mission> relatedMissions = GetRelatedMission(id);
            gvm.RelatedMissions = relatedMissions;

            IEnumerable<MissionRating> mission_rating = GetMissionRatingData(id);
            gvm.missionRatings = mission_rating;

            List<User> users = GetUser();
            gvm.Users = users;

            List<MissionDocument> docs = GetDocumentData();
            gvm.Documents = docs;

            IEnumerable<FavoriteMission> fav = GetFav(id);
            gvm.FavoriteMissions = fav;

            IEnumerable<Comment> comment = GetComment();
            gvm.comments = comment;

            IEnumerable<MissionApplication> MissionsApp = GetMissionApplication();
            gvm.missionApplications = MissionsApp;

            var rate = _db.MissionRatings.Where(rate => rate.MissionId == id && rate.UserId == uid).Select(rate => rate.Rating).FirstOrDefault();

            if (rate != 0)
            {
                gvm.NumberOfVolunteers = mission_rating.Count();
                gvm.AvarageOfRating = (int)mission_rating.Average(x => x.Rating);

            }

            var rateOfUser = _db.MissionRatings.Where(rate => rate.MissionId == id && rate.UserId == uid).Select(rate => rate.Rating).FirstOrDefault();
            if(rateOfUser != null)
            {
                gvm.UserRate = rateOfUser;
            }
            else
            {
                gvm.UserRate = 0;
            }
            
            gvm.totalrecord = gvm.Users.Count();
            gvm.currentPage = pageIndex;
            gvm.Users = gvm.Users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();


            return gvm;
        }
        public MissionRating? GetRating()
        {
            var rate = _db.MissionRatings.FirstOrDefault();
            return rate;
        }
        public Mission? GetMissionData(int? id)
        {
            var missionDetails = _db.Missions.Where(u => u.MissionId == id).FirstOrDefault();
            return missionDetails;
        }
        public GoalMission? GetGoalData()
        {
            var goal = _db.GoalMissions.FirstOrDefault();
            return goal;
        }

        //public IEnumerable<MissionTheme> GetMissionThemeData(int id)
        //{
        //    var mission_data = _db.Missions.Where(x => x.MissionId == id).FirstOrDefault();
        //    List<MissionTheme> theme = _db.MissionThemes.Where(x => x.MissionThemeId == mission_data.ThemeId).FirstOrDefault();
        //    return theme;
        //}
        //public IEnumerable<City> GetCityData(int? id)
        //{
        //    List<City> cities = 
        //    return cities;
        //}
        public IEnumerable<MissionRating> GetMissionRatingData(int id)
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
        public List<MissionDocument> GetDocumentData()
        {
            List<MissionDocument> doc = _db.MissionDocuments.ToList();
            return doc;
        }
        public IEnumerable<FavoriteMission> GetFav(int id)
        {
            List<FavoriteMission> fav = _db.FavoriteMissions.Where(x => x.MissionId == id).ToList();
            return fav;
        }
        public List<Comment> GetComment()
        {
            List<Comment> comment = _db.Comments.ToList();
            return comment;
        }
        public List<MissionApplication> GetMissionApplication()
        {
            List<MissionApplication> doc = _db.MissionApplications.ToList();
            return doc;
        }
        //add to fav
        public bool favUser(int missionid, string email)
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
                return true;
            }
            else
            {
                var unfavourite = _db.FavoriteMissions.Where(u => u.UserId == userid).FirstOrDefault();
                _db.Remove(unfavourite);
                _db.SaveChanges();
                return true;
            }
            
        }
        //rating
        public bool RatingUser(int missionid, int rating, string Email)
        {
            GeneralViewModel gvm = new GeneralViewModel();

            MissionRating? rating_data = GetRating();
            gvm.rating = rating_data;

            var mission_rating = _db.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            var rate_update = mission_rating.SingleOrDefault(m => m.User.Email == Email && m.Mission.MissionId == missionid);

            if (rate_update != null)
            {

                rate_update.Rating = rating;
                _db.Update(rate_update);
                _db.SaveChanges();
                return true;

            }
            else
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
                return true;
            }
            
        }

        //comments
        public bool CommentUser(int missionid, string comment, string email)
        {
            var user = _db.Users.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefault();
            var post = new Comment
            {
                MissionId = missionid,
                UserId = user,
                Comments = comment,
            };
            if(comment != null)
            {
                _db.Add(post);
                _db.SaveChanges();
                
            }
            return true;
        }

        //recommended
        public List<User> RecommendedUser()
        {
            List<User> getUserList = _db.Users.ToList();
            return getUserList;
        }
        //Apply Button
        public bool ApplyUser(int id , long uid)
        {
            var alreadyApplied = _db.MissionApplications.Where(x => x.UserId == Convert.ToInt32(uid) && x.MissionId == id).FirstOrDefault();
            if (alreadyApplied == null)
            {
                MissionApplication missionApp = new MissionApplication()
                {
                    MissionId = id,
                    UserId = Convert.ToInt32(uid),
                    AppliedAt = DateTime.Now,
                };
                _db.Add(missionApp);
                _db.SaveChanges();
                return true;
            }
            else
            {
                var deleteData = _db.MissionApplications.Where(x => x.UserId == Convert.ToInt32(uid) && x.MissionId == id).FirstOrDefault();
                _db.Remove(deleteData);
                _db.SaveChanges();
                return true;
            }
        }

        



        //Story details page
        public GeneralViewModel StoryDetailsUser(string search,int id,long uid,int Mid, int count)
        {
           

            GeneralViewModel gvm = new GeneralViewModel();
            {

                List<Country> country = GetCountries();
                gvm.Countries = country;

                List<City> city = GetCities();
                gvm.Cities = city;

                List<Mission> mission =_db.Missions.ToList();
                gvm.Missions = mission;

                List<MissionTheme> missionTheme = GetMissionThemes();
                gvm.MissionThemes = missionTheme;

                List<Skill> skill = GetSkills();
                gvm.Skills = skill;

                List<User> user = GetUser();
                gvm.Users = user;

                Story mission_story = GetMissionStory(id);
                gvm.story_details = mission_story;

                Mission mission_data = _db.Missions.Where(x => x.MissionId == mission_story.MissionId).FirstOrDefault();
                gvm.mission_details = mission_data;

                User mission_user = GetMissionUser(uid);
                gvm.user_details = mission_user;

                gvm.firstName = _db.Users.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                gvm.lastName = _db.Users.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();
                gvm.Views = count;
                //GeneralViewModel obj = new GeneralViewModel()
                //{
                //    Views = count,

                //};
                //_db.SaveChanges();
                //return obj;

            }
            var storyid = _db.Stories.Where(x => x.UserId == id && x.MissionId == Mid).Select(x => x.StoryId).FirstOrDefault();
            var storyView = _db.Stories.Where(x => x.StoryId == storyid).FirstOrDefault();
            

            storyView.Views = count;

            _db.Stories.Update(storyView);
            _db.SaveChanges();
            
            return gvm;
        }
        public Story GetMissionStory(int? id)
        {
            var missionDetails = _db.Stories.Where(u => u.UserId == id).FirstOrDefault();
            return missionDetails;
        }
        public User GetMissionUser(long? uid)
        {
            var userDetails = _db.Users.Where(u => u.UserId == uid).FirstOrDefault();
            return userDetails;
        }
        public City GetCityName(int? id)
        {
            var userDetails = _db.Cities.Where(x=>x.CityId == (_db.Missions.Where(x=>x.MissionId==id).FirstOrDefault()).CityId).FirstOrDefault();
            return userDetails;
        }

      
    }
}
