using Ci_Project.Entities.Data;
using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public readonly CiPlatformContext _db;
        public AdminRepository(CiPlatformContext db)
        {
            _db = db;
        }
        //user list
        public List<User> getUserList()
        {
            var data = _db.Users.ToList();
            return data;
        }
        public List<Admin> Login()
        {
            List<Admin> getAdminList = _db.Admins.ToList();
            return getAdminList;
        }
        public List<MissionMedium> getMissionImage()
        {
            var data = _db.MissionMedia.ToList();
            return data;
        }
        public List<MissionDocument> getDocList()
        {
            var data = _db.MissionDocuments.ToList();
            return data;
        }
        public List<MissionSkill> getMissionSkill()
        {
            var data = _db.MissionSkills.ToList();
            return data;
        }
        public List<City> getCityList()
        {
            var data = _db.Cities.ToList();
            return data;
        }
        public List<Country> getCountryList()
        {
            var data = _db.Countries.ToList();
            return data;
        }
        public List<Banner> getBannerList()
        {
            var data = _db.Banners.ToList();
            return data;
        }
        
        
        /// USER TABLE
     
        //Add USER data
        public bool GetTime(AdminViewModel avm, int? uid)
        {
                User model = new User()
                {
                    CityId=avm.city,
                    CountryId=avm.country,
                    //UserId = Convert.ToInt32(uid),
                    FirstName = avm.FirstName,
                    LastName = avm.LastName,
                    EmployeeId = avm.EmployeeId,
                    Department = avm.Department,
                    Email = avm.Email,
                    Status = avm.status,
                    Password = avm.Password,
                    Avatar = "/Assets/" + avm.Avatar,
                    ProfileText = avm.ProfileText
                    
                };
                _db.Add(model);
                _db.SaveChanges();
                return true;
            
        }
        //USER edit data GET
        public User getDataForEdit(int id)
        {
            var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
            return data;
        }
        //edit data
        public void getDetailsAndUpdate(int id, string fname,string lname,string email,string pass,string Avatar,int Eid , string Department,int CityId,int CountryId,string Profiletxt, string status)
        {
            var userId = _db.Users.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefault();
            //var x = _db.Timesheets.Where(y => y.TimesheetId == id && y.UserId == userId).FirstOrDefault();
            //var m = _db.Timesheets.Where(x => x.UserId == userId).Select(x => x.MissionId).FirstOrDefault();

            if (userId != null)
            {
                var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
                data.FirstName = fname;
                data.LastName = lname;
                data.Email = email;
                data.EmployeeId = Eid;
                data.Department = Department;
                data.Status = status;
                //data.Password = pass;
                //data.CityId = CityId;
                //data.CountryId = CountryId;
                data.ProfileText = Profiletxt;
                _db.Update(data);
                _db.SaveChanges();
            }
        }
        //delete USER data 
        public bool getDeleteData(int id,string email)
        {
            //var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
            var userId = _db.Users.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefault();

            if (userId != null)
            {
                var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
                
                data.Status = "Deactive";
                _db.Update(data);
                _db.SaveChanges();
                
            }
            return true;
        }


        /// MISSION TABLE
        
        //MISSION LIST
        public List<Mission> getMissionList()
        {
            return _db.Missions.ToList();
        }       
        //MISSION ADD
        public bool MissionAdd(AdminViewModel avm, int? uid,int [] listofSkills)
        {
            Mission model = new Mission()
            {

                Title = avm.MissionTitle,
                ShortDescription = avm.SDescription,
                Description = avm.Description,
                CountryId = avm.country,
                CityId = avm.city,
                OrganizationName = avm.OrgName,
                OrganizationDetail = avm.OrgDetails,
                StartDate = avm.StartDate,
                EndDate = avm.EndDate,
                MissionType = avm.MissionType,
                Availability = avm.Seats,
                ThemeId = 1

            };
            _db.Missions.Add(model);
            _db.SaveChanges();

            //foreach (var image in missionVM.images)
            //{
            //    var fileName = image.FileName;
            //    var fileType = image.ContentType;
            //    byte[] imageData;

            //    using (var fileStream = image.OpenReadStream())
            //    {
            //        using (var memoryStream = new MemoryStream())
            //        {
            //            fileStream.CopyTo(memoryStream);
            //            imageData = memoryStream.ToArray();
            //        }
            //    }
            //    var base64String = Convert.ToBase64String(imageData);
            //    var missionMedia = new MissionMedium
            //    {
            //        MissionId = mission.MissionId,
            //        MediaPath = $"data:{fileType};base64,{base64String}",
            //        MediaName = fileName,
            //        MediaType = fileType
            //    };

            //    _ciPlatformContext.MissionMedia.Add(missionMedia);
            //}
            foreach (var img in avm.Mimages)
            {
                var fileName = img.FileName;
                var fileType = img.ContentType;
                byte[] imageData;

                using (var fileStream = img.OpenReadStream())
                {
                    using (var memorystream = new MemoryStream())
                    {
                        fileStream.CopyTo(memorystream);
                        imageData = memorystream.ToArray();
                    }
                    var base64String = Convert.ToBase64String(imageData);
                    var missionMedia = new MissionMedium
                    {
                        MissionId = model.MissionId,
                        MediaPath = $"data:{fileType};base64,{base64String}",
                        MediaName = fileName,
                        MediaType = fileType
                    };
                    _db.MissionMedia.Add(missionMedia);
                    _db.SaveChanges();

                }
            }
            //foreach (var img in avm.Mimages)
            //{
            //    var fileName = img.FileName;
            //    var fileType = img.ContentType;

            //    using (var fileStream = img.OpenReadStream())
            //    {
            //        var filePath = Path.Combine("/UploadImage/", fileName);
            //        using (var fStream = new FileStream(Path.Combine("wwwroot", "UploadImage", fileName), FileMode.Create))
            //        {

            //            MissionMedium model1 = new MissionMedium()
            //            {
            //                MediaType = fileType,
            //                MediaPath = filePath ,
            //                //MediaPath = filePath,
            //                MissionId = model.MissionId,
            //                MediaName = fileName
            //            };
            //            _db.MissionMedia.Add(model1);
            //            _db.SaveChanges();
            //        }

            //    }
            //}

            MissionMedium model2 = new MissionMedium()
            {
                MediaType = "video",
                MediaPath = avm.Video,
                MissionId = model.MissionId,
                MediaName = model.Title


            };
            _db.MissionMedia.Add(model2);
            _db.SaveChanges();
           
            foreach(var skill in listofSkills)
            {
               
                MissionSkill model3 = new MissionSkill()
                {
                    SkillId = skill,
                    MissionId = model.MissionId,

                };
                _db.MissionSkills.Add(model3);
                _db.SaveChanges();

            }
            foreach (var doc in avm.Document)
            {
                var docName = doc.FileName;
                var docType = doc.ContentType;
                byte[] docData;

                using (var docStream = doc.OpenReadStream())
                {
                    using (var memorystream = new MemoryStream())
                    {
                        docStream.CopyTo(memorystream);
                        docData = memorystream.ToArray();
                    }
                    var base64String = Convert.ToBase64String(docData);
                    var missionDoc = new MissionDocument
                    {
                        MissionId = model.MissionId,
                        DocumentPath = $"data:{docType};base64,{base64String}",
                        DocumentName = docName,
                        DocumentType = docType
                    };
                    _db.MissionDocuments.Add(missionDoc);
                    _db.SaveChanges();

                }
                //using (var docStream = doc.OpenReadStream())
                //{
                //    var docPath = Path.Combine("/Document/", docName);
                //    using (var fStream = new FileStream(Path.Combine("wwwroot", "Document", docName), FileMode.Create))
                //    {

                //        MissionDocument docs = new MissionDocument()
                //        {
                //            DocumentType = docType,
                //            DocumentPath = docPath,
                //            //MediaPath = filePath,
                //            MissionId = model.MissionId,
                //            DocumentName = docName
                //        };
                //        _db.MissionDocuments.Add(docs);
                //        _db.SaveChanges();
                //    }

                //}
            }
            
            return true;
        }
        //MISSION get edit data
        //public Mission getEditMission(int id)
        //{
        //    var data = _db.Missions.Where(x => x.MissionId == id).FirstOrDefault();
        //    return data;
        //}
        
        public AdminViewModel getEditMission(int id)
        {
            var missionData = _db.Missions.FirstOrDefault(mission => mission.MissionId == id);
            var images = _db.MissionMedia.Where(image => image.MissionId == id).Select(image => image.MediaPath).ToList();
            var docs = _db.MissionDocuments.Where(doc => doc.MissionId == id).Select(doc => doc.DocumentPath).ToList();
            var missionSkill = _db.MissionSkills.Where(skill => skill.MissionId == id).ToList();
            AdminViewModel avm = new AdminViewModel()
            {
                MissionTitle = missionData.Title,
                Description = missionData.Description,
                SDescription = missionData.ShortDescription,
                OrgName = missionData.OrganizationName,
                OrgDetails = missionData.OrganizationDetail,
                StartDate = missionData.StartDate,
                EndDate = missionData.EndDate,
                MissionType = missionData.MissionType,
                Seats = missionData.Availability,
                city = missionData.CityId,
                country = missionData.CountryId,
                Theme = missionData.ThemeId,
                missionImages = images.ToList(),
                missiondocs = docs.ToList(),
                //missionSkill = 
                
                //Mimages = images.MediaPath,
                //Document = docs.DocumentPath
            };
            return avm;
        }
        //MISSION edit data
        public string getEditMissionData(int id, string mTitle, string Sdes, string des, int Cityid, int countryId, string OrgName, string OrdDetails, DateTime sDate, DateTime eDate, string mType, string seats, string mImages, string mVideo, string mDoc)
        {
            var missionId = _db.Missions.Where(x => x.MissionId == id).Select(x => x.MissionId).FirstOrDefault();
            if (missionId != null)
            {
                var data = _db.Missions.Where(x => x.MissionId == id).FirstOrDefault();
                var missionMedia = _db.MissionMedia.Where(x => x.MissionId == id).FirstOrDefault();
                var missionDoc = _db.MissionDocuments.Where(x => x.MissionId == id).FirstOrDefault();

                //mission table
                data.Title = mTitle;
                data.ShortDescription = Sdes;
                data.Description = des;
                data.CityId = Cityid;
                data.CountryId = countryId;
                data.OrganizationName = OrgName;
                data.OrganizationDetail = OrdDetails;
                data.Availability = seats;
                data.MissionType = mType;
                data.StartDate = sDate;
                data.EndDate = eDate;

                _db.Missions.Update(data);
                _db.SaveChanges();
                //media table
                if (missionMedia != null)
                {
                    missionMedia.MissionId = data.MissionId;
                    missionMedia.MediaName = data.Title;
                    if (missionMedia.MediaType == "image")
                    {
                        missionMedia.MediaPath = mImages;

                    }
                    else
                    {
                        missionMedia.MediaPath = mVideo;
                    }
                    _db.Update(data);
                    _db.SaveChanges();
                }
               

                //mission doc
                if(missionDoc != null)
                {

                    missionDoc.MissionId = data.MissionId;
                    missionDoc.DocumentPath = mDoc;
                    _db.Update(data);
                    _db.SaveChanges();
                }

                
            }
            return "Success";
        }
        //get images and video
        public List<MissionMedium> MissionImages(int id)
        {
            var data = _db.MissionMedia.Where(media => media.MissionId == id).ToList();
            return data;
        }
        //get document
        public List<MissionDocument> MissionDocuments(int id)
        {
            var data = _db.MissionDocuments.Where(media => media.MissionId == id).ToList();
            return data;
        }
        //delete mission 
        public bool getDeleteMission(int id)
        {
            var data = _db.Missions.Where(x => x.MissionId == id).FirstOrDefault();

            if (data != null)
            {
                data.Status = "In-active";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
           
        }


        /// MISSION APPLICATION TABLE 
        
        //mission application
        public List<MissionApplication> getMissionApplicationList()
        {
            
            return _db.MissionApplications.ToList();
        }
        //checked mission application
        public bool getChecked(int id , string email)
        {
            var missionApp = _db.MissionApplications.Where(x => x.MissionApplicationId == id).FirstOrDefault();

            if (missionApp != null)
            {
                //var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

                missionApp.ApprovalStatus = "Approved";
                _db.Update(missionApp);
                _db.SaveChanges();

            }
            return true;
        }
        public bool getCancel(int id , string email)
        {
            var missionApp = _db.MissionApplications.Where(x => x.MissionApplicationId == id).FirstOrDefault();

            if (missionApp != null)
            {
                //var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

                missionApp.ApprovalStatus = "Rejected";
                _db.Update(missionApp);
                _db.SaveChanges();

            }
            return true;
        }


        /// STORY TABLE 

        //Story Page
        public List<Story> getStoryList()
        {
            return _db.Stories.ToList();
        }
        //View data get
        public Story getViewStory(int id)
        {
            var data = _db.Stories.Where(x => x.StoryId == id).FirstOrDefault();
            return data;
        }
        public User UserName(int id)
        {
            var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
            return data;
        }
        public List<StoryMedium> getStoryMedia(int id)
        {
            var data = _db.StoryMedia.Where(media => media.StoryId == id).ToList();
            return data;
        }
       
        //checked skill
        public bool CheckedStory(int id)
        {
            var data = _db.Stories.Where(x => x.StoryId == id).FirstOrDefault();

            if (data != null)
            {
                data.Status = "Published";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
        }
        //cancel skill
        public bool CancelStory(int id)
        {
            var data = _db.Stories.Where(x => x.StoryId == id).FirstOrDefault();

            if (data != null)
            {
                //var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

                data.Status = "Rejected";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
        }
        
        //delete story
        public bool getDeleteStory(int id)
        {
            var data = _db.Stories.Where(x => x.StoryId == id).FirstOrDefault();

            if (data != null)
            {
                data.Status = "Delete";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
        }


        /// THEME TABLE 

        //Theme page
        public List<MissionTheme> getThemeList()
        {
            return _db.MissionThemes.ToList();
        }
        //theme add
        public bool GetThemeAdd(AdminViewModel avm)
        {
            MissionTheme model = new MissionTheme()
            {
                Title = avm.ThemeTitle,
                Status = avm.ThemeStatus,

            };
            _db.Add(model);
            _db.SaveChanges();
            return true;
        }
        //get edit Theme data
        public MissionTheme GetEditTheme(int id)
        {
            var data = _db.MissionThemes.Where(x => x.MissionThemeId == id).FirstOrDefault();
            return data;
        }
        // GET edit data Theme
        public bool EditTheme(int themeId, AdminViewModel advm)
        {
            var data = _db.MissionThemes.FirstOrDefault(theme => theme.MissionThemeId == themeId);
            if (data != null)
            {
                data.Title = advm.ThemeTitle;
                data.Status = advm.ThemeStatus;
                _db.Update(data);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        //edit data theme
        public string EditMissionThemeData(int id, string tTitle, string tStatus)
        {
            var data = _db.MissionThemes.FirstOrDefault(theme => theme.MissionThemeId == id);
            if (data != null)
            {
                data.Title = tTitle;
                data.Status = tStatus;
                _db.Update(data);
                _db.SaveChanges();
            }
            return "Success";
        }
        //delete theme 
        public bool DeleteTheme(int id)
        {
            var data = _db.MissionThemes.Where(x => x.MissionThemeId == id).FirstOrDefault();

            if (data != null)
            {
                data.Status = "In-active";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;

        }


        /// SKILL TABLE 

        //Skill page
        public List<Skill> getSkillList()
        {
            return _db.Skills.ToList();
        }
        //Add skill
        public bool GetSkillAdd(AdminViewModel avm)
        {
            Skill model = new Skill()
            {

                SkillName = avm.SkillName,
                Status = avm.status,
            };
            _db.Add(model);
            _db.SaveChanges();
            return true;

        }
        //checked skill
        public bool CheckedSkill(int id)
        {
            var data = _db.Skills.Where(x => x.SkillId == id).FirstOrDefault();

            if (data != null)
            {
                data.Status = "Approved";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
        }
        //cancel skill
        public bool CancelSkill(int id)
        {
            var data = _db.Skills.Where(x => x.SkillId == id).FirstOrDefault();

            if (data != null)
            {
                //var data = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

                data.Status = "Rejected";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
        }


        /// CMS TABLE 

        //cms page
        public List<CmsPage> getCMSDataList()
        {
            return _db.CmsPages.ToList();
        }
        //Add Cms
        public bool AddCMSPage(AdminViewModel avm)
        {
            CmsPage model = new CmsPage()
            {
               
                Title = avm.CMStitle,
                Description = avm.CMSdescription,
                Slug = avm.CMSslug,
                Status = avm.CMSStatus,

            };
            _db.Add(model);
            _db.SaveChanges();
            return true;

        }
        //GET Edit Cms
        public AdminViewModel GetEditCMS(int id)
        {
            var CMS = _db.CmsPages.Where(x => x.CmsPageId == id).FirstOrDefault();
            AdminViewModel avm = new AdminViewModel()
            {
                CMStitle = CMS.Title,
                CMSdescription = CMS.Description,
                CMSslug = CMS.Slug,
            };
            return avm;
        }
        //edit data Cms
        public bool EditCMSPage(int cmsId, AdminViewModel advm)
        {
            var data = _db.CmsPages.FirstOrDefault(cms => cms.CmsPageId == cmsId);
            if (data != null)
            {
                data.Title = advm.CMStitle;
                data.Description = advm.CMSdescription;
                data.Slug = advm.CMSslug;
                data.Status = advm.CMSStatus;
                _db.CmsPages.Update(data);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        //delete data 
        public bool getDeleteCMS(int id)
        {
            var data = _db.CmsPages.FirstOrDefault(cms => cms.CmsPageId == id);

            if (data != null)
            {
                data.Status = "In-active";
                _db.Update(data);
                _db.SaveChanges();

            }
            return true;
        }

        ///Banner page
        //MISSION ADD
        public bool GetBannerAdd(AdminViewModel avm)
        {
            Banner model = new Banner()
            {

                Image = "/Assets/" + avm.BImages,
                Text = avm.BText,
                SortOrder = avm.SortOrder,

            };
            _db.Add(model);
            _db.SaveChanges();
            return true;
        }
        //GET Edit banner
        public Banner GetEditBanner(int id)
        {
            var data = _db.Banners.Where(x => x.BannerId == id).FirstOrDefault();
            return data;
        }
        //edit data banner
        public string EditDataBanner(int id, string bImage,string bText, int SOrd)
        {
            var data = _db.Banners.FirstOrDefault(theme => theme.BannerId == id);
            if (data != null)
            {
                var imageName = Path.GetFileName(bImage);
                if(imageName != null)
                {
                    data.Image = "/Assets/" + imageName;

                }
                data.Text = bText;
                data.SortOrder = SOrd;
                _db.Update(data);
                _db.SaveChanges();
            }
            return "Success";
        }
        //get image
        public List<Banner> GetBannerImage(int id)
        {
            var data = _db.Banners.Where(media => media.BannerId == id).ToList();
            return data;
        }
    }
}
