using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ci_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userrepository;

        public UserController(IUserRepository userRepository)
        {
            _userrepository = userRepository;
        }

        [Authorize]
        public IActionResult UserProfile()
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;
            var x = _userrepository.getprofile(email);
            //_userrepository.getprofile(email);
            return View(x);
        }

        ////userDetails
        //public IActionResult SaveSkills(UserViewModel userView,string [] skills)
        //    {
        //    var identity = User.Identity as ClaimsIdentity;
        //    var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;

        //    var email = identity?.FindFirst(ClaimTypes.Email)?.Value;
        //    _userrepository.SaveSkills(userView, skills, Convert.ToInt32(uid));
        //    return RedirectToAction("UserProfile", "User");
        //}
        //userDetails
        public IActionResult SaveDetails(UserViewModel userView, long[] finalSkillList)
        {
            long[] skillsArray = finalSkillList;

            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            _userrepository.SaveUserDetails(userView, Convert.ToInt32(uid));
            _userrepository.AddUserSkills(skillsArray, Convert.ToInt32(uid));
            return RedirectToAction("UserProfile", "User");
        }

        //CHANGE LOGO
        public void changelogo(string image)
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;
             _userrepository.changeAvatar(image, email);
           
        }
        //change pwd
        public IActionResult changePassword(UserViewModel userView)
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;
            var x = _userrepository.changePass(userView, email);
            if (x == true)
            {
                TempData["success"] = "Password changed successfully";
            }
            else
            {
                TempData["Errors"] = "Incorrect old password";
            }
            return RedirectToAction("UserProfile", "User");
        }

        //get city
        public JsonResult getCityList(int id)
        {
            var cityList = _userrepository.GetCityList(id);
            return Json(cityList);
        }
        //get country
        public JsonResult getCountryList()
        {
            var countryList = _userrepository.GetCountryList();
            return Json(countryList);
        }

        //policy page
        public IActionResult PolicyPage()
        {
            var xyz = _userrepository.Getpolicypage();
            return View(xyz);
        }

        //contact us
        public IActionResult ContactUs(UserViewModel uvm)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
             _userrepository.getContactDetails(uvm, Convert.ToInt32(uid));
            return RedirectToAction("UserProfile");
        }

        //volunteer Timesheet
        public IActionResult VolunteerTimesheet(string missionid)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var succ = _userrepository.GetTimesheet(missionid, Convert.ToInt32(uid));
            return View(succ);
        }

        public IActionResult GetTimeData(UserViewModel uvm)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            _userrepository.GetTime(uvm, Convert.ToInt32(uid));
            return RedirectToAction("VolunteerTimesheet");
        }

        //public IActionResult EditData(UserViewModel uvm)
        //{
        //    var identity = User.Identity as ClaimsIdentity;
        //    var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
        //    _userrepository.EditUserData(uvm, Convert.ToInt32(uid));
        //    return RedirectToAction("VolunteerTimesheet");
        //}

        public JsonResult GetEditData(UserViewModel uvm ,int id,int mId)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var data1 = _userrepository.getDataForEdit(id);
            //_userrepository.getUserEdit(uvm, Convert.ToInt32(uid),id);
            //return Json(new {data = data1 });
            return new JsonResult(data1);
        }

        public JsonResult EditAllData(UserViewModel uvm, int Mid)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var allData = _userrepository.getAllEditData(Mid);
            //_userrepository.getUserEdit(uvm, Convert.ToInt32(uid),id);
            //return Json(new {data = data1 });
            return new JsonResult(allData);
        }

        public void DeleteData(int id)
        {
             _userrepository.getDeleteData(id);
        }

        //Edit and Update data for both
        public void getDetailsforhrs(int id, string email, DateTime date, int hour, int mins, string desc, string descG, DateTime dateG, int action)
        {
             _userrepository.getDetailsAndUpdate(id, email, date, hour, mins, desc, descG, dateG, action);
            
        }
    }
}
