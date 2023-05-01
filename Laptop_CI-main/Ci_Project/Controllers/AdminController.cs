using Microsoft.AspNetCore.Mvc;
using Ci_Project.Repository.Interface;
using Ci_Project.Entities.ViewModels;
using System.Security.Claims;
using Ci_Project.Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Ci_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
             _adminRepository = adminRepository;
        }

        //[Authorize]
        public IActionResult AdminUser()
        {

            var citydata = _adminRepository.getCityList();
            var countrydata = _adminRepository.getCountryList();
            var succ = _adminRepository.getUserList();
            AdminViewModel admin = new AdminViewModel()
            {
                Cities = citydata,
                Countries = countrydata,
                UserList = succ,
            };
            return View(admin);
        }


        /// USERS TABLE
        
        public IActionResult Users()
        {
            var userData = _adminRepository.getUserList();
            var citydata = _adminRepository.getCityList();
            var countrydata = _adminRepository.getCountryList();
            AdminViewModel admin = new AdminViewModel()
            {
                Cities = citydata,
                Countries = countrydata,
                UserList = userData,
            };
            return PartialView("_User", admin);
        }       
        //add data
        public IActionResult GetTimeData(AdminViewModel avm)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            _adminRepository.GetTime(avm, Convert.ToInt32(uid));
            return RedirectToAction("AdminUser");
        }
        //get edit data
        public JsonResult GetEditData(int id)
        { 
            var data1 = _adminRepository.getDataForEdit(id);
            return new JsonResult(data1);
        }
        //edit data
        public void EditData(int id,string fname, string lname, string email, string pass ,string Avatar ,int Eid, string Department,int Cityid,int CountryId,string Profiletxt, string status)
        {
             _adminRepository.getDetailsAndUpdate(id, fname, lname, email,pass, Avatar,Eid, Department, Cityid, CountryId, Profiletxt, status);
            //return View();
        }
        //delete data
        public void DeleteData(int id, string email)
        {
            _adminRepository.getDeleteData( id,  email);
        }


        /// MISSION TABLE
        
        public IActionResult MissionPage()
        {
            var Doc = _adminRepository.getDocList();
            var missionImage = _adminRepository.getMissionImage();
            var skill = _adminRepository.getMissionSkill();
            var theme = _adminRepository.getThemeList();
            var missionData = _adminRepository.getMissionList();
            AdminViewModel admin = new AdminViewModel()
            {
                MissionList = missionData,
                MissionThemeList = theme,
                MissionSkill = skill,
                MissionImages = missionImage,
                MissionDec = Doc,
            };
            return PartialView("_MissionPartialView", admin);
        }
        //get theme list
        public JsonResult getThemeList()
        {
            var themeList = _adminRepository.getThemeList();
            return Json(themeList);
        }
        //MISSION Add
        public IActionResult AddMission()
        {
            var skilldata = _adminRepository.getSkillList();
            var missionData = _adminRepository.getMissionList();
            AdminViewModel admin = new AdminViewModel()
            {
                SkillList = skilldata,
                MissionList = missionData,
            };
            return PartialView("_MissionAdd", admin);
        }
        public IActionResult GetmissionAdd(AdminViewModel avm, int [] listOfSkill)
        {
            var identity = User.Identity as ClaimsIdentity;
            var uid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            _adminRepository.MissionAdd(avm, Convert.ToInt32(uid), listOfSkill);
            return RedirectToAction("MissionPage","Admin");
        }
        //mission edit
        public IActionResult EditMissionPartial()
        {
            var missionimg = _adminRepository.getMissionImage();
            var missiondoc = _adminRepository.getDocList();
            var skilldata = _adminRepository.getSkillList();
            var missionData = _adminRepository.getMissionList();
            AdminViewModel admin = new AdminViewModel()
            {
                MissionList = missionData,
                SkillList = skilldata,
                MissionDec = missiondoc,
                MissionImages = missionimg
            };
            return PartialView("_MissionEdit", admin);
        }
        //public JsonResult EditMission(int id)
        //{
        //    var data1 = _adminRepository.getEditMission(id);  
        //    return new JsonResult(data1);
        //}
        public IActionResult EditMission(int id)
        {
            var data = _adminRepository.getEditMission(id);
            return PartialView("_MissionEdit", data);
        }
        //get images and videos
        public JsonResult MissionImages(int id)
        {
            var missionImg = _adminRepository.MissionImages(id);
            return new JsonResult(missionImg);
        }
        //get document
        public JsonResult MissionDocuments(int id)
        {
            var missionImg = _adminRepository.MissionDocuments(id);
            return new JsonResult(missionImg);
        }
        //edit data
        public void EditMissionData(int id, string mTitle, string Sdes,string des,int Cityid,int countryId,string OrgName,string OrdDetails, DateTime sDate , DateTime eDate, string mType,string seats,string mImages,string mVideo,string mDoc)
        {
             _adminRepository.getEditMissionData(id, mTitle, Sdes, des, Cityid, countryId, OrgName, OrdDetails, sDate, eDate, mType, seats,mImages,mVideo,mDoc);
            
        }
        //delete data
        public void DeleteMission(int id)
        {
            _adminRepository.getDeleteMission(id);
        }


        /// MISSION APPLICATION TABLE

        public IActionResult MissionApplicationPage()
        {
            var userdata = _adminRepository.getUserList();
            var missionData = _adminRepository.getMissionApplicationList();
            AdminViewModel admin = new AdminViewModel()
            {
                MissionApplicationList = missionData,
                UserList = userdata,
            };
            return PartialView("_MissionApplication", admin);
        }
        //MISSION APPLICATION
        public void Checked(int id , string email)
        {
            _adminRepository.getChecked(id, email);
        }
        public void Cancel(int id , string email)
        {
            _adminRepository.getCancel(id, email);
        }


        /// CMS TABLE 
        public IActionResult CMSPage()
        {
            var userData = _adminRepository.getCMSDataList();
            AdminViewModel admin = new AdminViewModel()
            {
                listOfCmsPage = userData,
            };
            return PartialView("_CMSPagePartialView", admin);
        }
        //Add Cms Page
        public IActionResult AddCMS()
        {
            var userData = _adminRepository.getCMSDataList();
            AdminViewModel admin = new AdminViewModel()
            {
                listOfCmsPage = userData,
            };
            return PartialView("_AddCMS", admin);
        }
        public IActionResult AddCMSPage(AdminViewModel avm)
        {
            _adminRepository.AddCMSPage(avm);
            return RedirectToAction("CMSPage", "Admin");
        }
        //Edit Cms Page
        public IActionResult EditCMSMethod()
        {
            var userData = _adminRepository.getCMSDataList();
            AdminViewModel admin = new AdminViewModel()
            {
                listOfCmsPage = userData,
            };
            return PartialView("_EditCMS", admin);
        }
        //get edit data
        
        public IActionResult GetEditCMS(int id)
        {
            var data1 = _adminRepository.GetEditCMS(id);
            
            return PartialView("_EditCMS", data1);
        }
        //edit and update data in cms
        public IActionResult EditCMSPage(int cmsPageId, AdminViewModel advm)
        {
            var x = _adminRepository.EditCMSPage(cmsPageId, advm);
            if (x)
            {
                return RedirectToAction("CMSPage", "Admin");
            }
            else
            {
                return RedirectToAction("CMSPage", "Admin");
            }
            
        }
        //delete cms data
        public void DeleteCMS(int id)
        {
            _adminRepository.getDeleteCMS(id);
        }
          

        ///STORY TABLE
        
        public IActionResult StoryPage()
        {
            var mission = _adminRepository.getMissionList();
            var user = _adminRepository.getUserList();
            var missionData = _adminRepository.getStoryList();
            AdminViewModel admin = new AdminViewModel()
            {
                StoryList = missionData,
                UserList = user,
                MissionList = mission
            };
            return PartialView("_StoryPartialView", admin);
        }
        //view
        public JsonResult ViewStory( int id)
        {
            var data1 = _adminRepository.getViewStory(id);
            return new JsonResult(data1);
        }
        public JsonResult UserName( int id)
        {
            var data1 = _adminRepository.UserName(id);
            return new JsonResult(data1);
        }
        public JsonResult GetStoryImage(int id)
        {
            var story = _adminRepository.getStoryMedia(id);
            return new JsonResult(story);
        }
        
        //delete story
        public void DeleteStory(int id)
        {
            _adminRepository.getDeleteStory(id);
        }
        public IActionResult CheckedStory(int id)
        {
            var acceptedStoryv = _adminRepository.CheckedStory(id);

            return RedirectToAction("StoryPage" , "Admin");
        }
        public void CancelStory(int id)
        {
            _adminRepository.CancelStory(id);
        }
        //public void ViewDetails(int id)
        //{
        //    _adminRepository.ViewDetails(id);
        //}

        ///SKILL TABLE

        public IActionResult SkillPage()
        {
            var missionData = _adminRepository.getSkillList();
            AdminViewModel admin = new AdminViewModel()
            {
                SkillList = missionData,
            };
            return PartialView("_Skill", admin);
        }
        //Add skill
        public IActionResult GetSkillAdd(AdminViewModel avm)
        {
            _adminRepository.GetSkillAdd(avm);
            return RedirectToAction("AdminUser");
        }
        //Skill Page
        public void CheckedSkill(int id)
        {
            _adminRepository.CheckedSkill(id);
        }
        public void CancelSkill(int id)
        {
            _adminRepository.CancelSkill(id);
        }



        ///THEME TABLE
        public IActionResult ThemePage()
        {
            var missionData = _adminRepository.getThemeList();
            AdminViewModel admin = new AdminViewModel()
            {
                MissionThemeList = missionData,
            };
            return PartialView("_MissionTheme", admin);
        }
        
        //theme add
        public IActionResult GetThemeAdd(AdminViewModel avm)
        {
            _adminRepository.GetThemeAdd(avm);
            return RedirectToAction("AdminUser");
        }
        //get edit Theme data 
        public JsonResult EditMissionTheme( int id)
        {
            var data1 = _adminRepository.GetEditTheme(id);
            return new JsonResult(data1);
        }
        //edit and update data in 
        public void EditMissionThemeData(int id, string tTitle, string tStatus)
        {
            _adminRepository.EditMissionThemeData(id, tTitle, tStatus);

        }
        //delete theme data
        public void DeleteTheme(int id)
        {
            _adminRepository.DeleteTheme(id);
        }


        ///Banner Table
        
        public IActionResult Banner()
        {
            var BannerData = _adminRepository.getBannerList();
            AdminViewModel admin = new AdminViewModel()
            {
                BannerList = BannerData,
            };
            return PartialView("_Banner", admin);
        }
        //theme add
        public IActionResult GetBannerAdd(AdminViewModel avm)
        {
            _adminRepository.GetBannerAdd(avm);
            return RedirectToAction("AdminUser");
        }
        //get edit Theme data 
        public JsonResult GetEditBanner(int id)
        {
            var data1 = _adminRepository.GetEditBanner(id);
            return new JsonResult(data1);
        }
        //edit and update data in 
        public void EditDataBanner(int id, string bImage,string bText, int SOrd)
        {
            _adminRepository.EditDataBanner(id, bImage, bText, SOrd);

        }
        //get image
        public JsonResult GetBannerImage(int id)
        {
            var story = _adminRepository.GetBannerImage(id);
            return new JsonResult(story);
        }

        ///Admin Login
        [Authorize]
        public IActionResult AdminLogin()
        {
            var banner = _adminRepository.getBannerList();
            AdminViewModel lvm = new AdminViewModel()
            {
                BannerList = banner,
            };
            return View(lvm);
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminViewModel adminVM)
        {
            List<Admin> getAdminList = _adminRepository.Login();
            List<Banner> getBannerList = _adminRepository.getBannerList();
            adminVM.BannerList = getBannerList;
            var validUser = getAdminList.Where(x => x.Email == adminVM.Email);

            if (validUser != null)
            {

                var validEmail = validUser.Where(u => u.Email == adminVM.Email && u.Password == adminVM.Password).FirstOrDefault();
                if (validEmail != null)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, validEmail.Email) },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, validEmail.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, validEmail.LastName));
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("EmailId", validEmail.Email);


                    TempData["success"] = "Admin Login is succesful.";
                    return RedirectToAction("Users", "Admin");
                }
                else
                {
                    TempData["Error"] = "Incorrect password ";
                }


            }
            else
            {
                TempData["Error"] = "Email is not register yet Register First";
            }

            return View(adminVM);
        }
    }
}
