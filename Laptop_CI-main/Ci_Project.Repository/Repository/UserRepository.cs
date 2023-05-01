using Ci_Project.Entities.Data;
using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly CiPlatformContext _db;
        public UserRepository(CiPlatformContext db)
        {
            _db = db;
        }
        public UserViewModel getprofile(string email)
        {
            var city = _db.Cities.ToList();
            var country = _db.Countries.ToList();
            var skill = _db.Skills.ToList();
            var UserSkill = _db.UserSkills.ToList();

            var user = _db.Users.Where(x=>x.Email == email).FirstOrDefault();
            UserViewModel uvm = new UserViewModel()
            {
                Users = _db.Users.ToList(),
                Cities = city,
                Countries = country,
                SkillList = skill,
                UserSkills = UserSkill,

                Name = user.FirstName,
                Surname = user.LastName,
                EmployeeId = user.EmployeeId,
                Title = user.Title,
                Department = user.Department,
                ProfileText = user.ProfileText,
                WhyIVolunteer = user.WhyIVolunteer,
                Manager = user.Manager,
                city = _db.Cities.Where(x => x.CityId == user.CityId).Select(x => x.Name).FirstOrDefault(),
                country = _db.Countries.Where(x => x.CountryId == user.CountryId).Select(x => x.Name).FirstOrDefault(),
                Status = user.Status,
                LinkedInURL = user.LinkedInUrl,
                Avatar = user.Avatar,
                //Skill = user.selectedSkill
            };

            return uvm;
        }

        //User Profile save details
        //public string SaveSkills(UserViewModel userView, string [] skills , int uid)
        //{
        //    foreach(var item in skills)
        //    {
        //        var skillid = _db.Skills.Where(x => x.SkillName == item).Select(x => x.SkillId).FirstOrDefault();
        //        var skillName = _db.Skills.Where(x => x.SkillId == skillid).Select(x => x.SkillName).FirstOrDefault();
        //        var skill = _db.UserSkills.Where(x => x.UserId == uid).Select(x => x.SkillId).ToList();

        //        if(skill != null)
        //        {

        //            if(!skill.Contains(skillid))
        //            {
        //                UserSkill userskill = new UserSkill()
        //                {
        //                    UserId = uid,
        //                    SkillId = skillid,
        //                };
        //                _db.Add(userskill);
        //                _db.SaveChanges();
        //            }
        //        }

        //        var Allskill = _db.UserSkills.Where(x => x.UserId == uid).Select(x => x.Skill.SkillName).ToList();
        //        foreach (var RemoveSkills in Allskill)
        //        {
        //            if (!skills.Contains(RemoveSkills))
        //            {
        //                var remove = _db.UserSkills.Where(x => x.Skill.SkillName == item && x.UserId == uid).FirstOrDefault();
        //                _db.UserSkills.Remove(remove);
        //                _db.SaveChanges();
        //            }
        //        }
        //    }
        //    return "Success";
        //}

        //final skills of users
        public int AddUserSkills(long[] SkillArray, int uid)
        {
            List<int> skillIds = new List<int>();

            var skills = _db.Skills.Where(skill => SkillArray.Contains(skill.SkillId)).ToList();

            foreach (var skill in skills)
            {
                skillIds.Add((int)skill.SkillId);
            }

            var existingUserSkills = _db.UserSkills.Where(user => user.UserId == uid);
            _db.UserSkills.RemoveRange(existingUserSkills);

            foreach (int skillid in skillIds)
            {
                var usrskill = new UserSkill()
                {
                    SkillId = skillid,
                    UserId = uid,
                    CreatedAt = DateTime.Now
                };
                _db.UserSkills.Add(usrskill);
            }
            _db.SaveChanges();

            return 1;
        }
        public void SaveUserDetails(UserViewModel userView, int uid)
        {

            var alreadyExitUser = _db.Users.Where(x => x.UserId == Convert.ToInt32(uid)).FirstOrDefault();

            if(userView.cityId == 0 && userView.countryId == 0)
            {
                alreadyExitUser.FirstName = userView.Name;
                alreadyExitUser.LastName = userView.Surname;
                alreadyExitUser.EmployeeId = userView.EmployeeId;
                alreadyExitUser.Title = userView.Title;
                alreadyExitUser.Department = userView.Department;
                alreadyExitUser.ProfileText = userView.ProfileText;
                alreadyExitUser.WhyIVolunteer = userView.WhyIVolunteer;
                alreadyExitUser.LinkedInUrl = userView.LinkedInURL;
                alreadyExitUser.Manager = userView.Manager;
                _db.SaveChanges();

            }
            else
            {
                alreadyExitUser.FirstName = userView.Name;
                alreadyExitUser.LastName = userView.Surname;
                alreadyExitUser.EmployeeId = userView.EmployeeId;
                alreadyExitUser.Title = userView.Title;
                alreadyExitUser.Department = userView.Department;
                alreadyExitUser.ProfileText = userView.ProfileText;
                alreadyExitUser.WhyIVolunteer = userView.WhyIVolunteer;
                alreadyExitUser.LinkedInUrl = userView.LinkedInURL;
                alreadyExitUser.Manager = userView.Manager;
                alreadyExitUser.CityId = userView.cityId;
                alreadyExitUser.CountryId = userView.countryId;
                _db.SaveChanges();
            }
        }


        //Change UserAvatar
        public string changeAvatar(string image, string email)
        {
            var data = _db.Users.Where(x => x.Email == email).FirstOrDefault();
            data.Avatar = image;
            _db.Update(data);
            _db.SaveChanges();
            return "Success";
        }

        //change pwd
        public bool changePass(UserViewModel userView, string email)
        {
            var oldP = userView.OldPwd;
            var user = _db.Users.Where(x => x.Email == email).FirstOrDefault();

            if (user.Password == oldP && user.Password != userView.NewPwd)
            {
                user.Password = userView.NewPwd;
                _db.Update(user);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //get city and country
        public List<Country> GetCountryList()
        {
            var countryList = _db.Countries.ToList();
            return countryList;
        }
        public List<City> GetCityList(int id)
        {
            var cityList = _db.Cities.Where(x => x.CountryId == id).ToList();
            return cityList;
        }

        //Policy page
        public UserViewModel Getpolicypage()
        {
            UserViewModel Uvm = new UserViewModel()
            {
                Users = _db.Users.ToList(),
            };

            return Uvm;
        }

        //contact us
        public bool getContactDetails(UserViewModel uvm,int uid)
        {
            ContactU cont = new ContactU()
            {
                UserId = uid,
                Subject = uvm.subjectForContactUsPage,
                Message = uvm.messageForContactUsPage
            };
            _db.Add(cont);
            _db.SaveChanges();
            return true;
        }

        //volunteer timesheet page
        public UserViewModel GetTimesheet(string missionid, int uid)
        {

            UserViewModel model = new UserViewModel()
            {
                Users = _db.Users.ToList(),
                Missions = _db.Missions.ToList(),
                Timesheets = _db.Timesheets.ToList(),
                mission_details = _db.Missions.FirstOrDefault(),
                timesheet = _db.Timesheets.FirstOrDefault(),
                missionApp = _db.MissionApplications.Where(x=>x.UserId == uid).ToList(),
            };
            return model;


        }

        //Add data to timsheet table
        public bool GetTime(UserViewModel uvm,int? uid)
        {
            if(uvm.hour != 0)
            {

                DateTime dateTime = DateTime.Now;
                var hours = uvm.hour;
                var minutes = uvm.minute;
                var notes = uvm.message;
                var missionID = uvm.missionId;

                var volunteerDate = _db.MissionApplications.Where(x => x.UserId == uid && x.MissionId == uvm.missionId).Select(x => x.AppliedAt).FirstOrDefault();

                var timesheet = _db.Timesheets.Where(x => x.UserId == Convert.ToInt32(uid) && x.MissionId == uvm.missionId).FirstOrDefault();
                
                    Timesheet model = new Timesheet()
                    {
                        UserId = uid,
                        MissionId = missionID,
                        Notes = uvm.message,
                        CreatedAt = DateTime.Now,
                        TimesheetTime =new TimeSpan(hours,minutes,0),
                        DateVolunteered = volunteerDate,
                        Status = "pending",

                    };
                    _db.Add(model);
                    _db.SaveChanges();
                    return true;                   
                
               
            }
            else
            {
                DateTime datetime = DateTime.Now;
                var volunteerDate = _db.MissionApplications.Where(x => x.UserId == uid && x.MissionId == uvm.missionId).Select(x => x.AppliedAt).FirstOrDefault();
                var timesheet = _db.Timesheets.Where(x => x.UserId == Convert.ToInt32(uid) && x.MissionId == uvm.missionId).FirstOrDefault();

               
                    Timesheet model = new Timesheet()
                    {
                        UserId = uid,
                        MissionId = uvm.missionId,
                        Action = uvm.action,
                        CreatedAt = DateTime.Now,
                        //TimesheetTime = (),
                        Notes = uvm.message,
                        DateVolunteered = volunteerDate,
                        Status = "pending",

                    };
                    _db.Add(model);
                    _db.SaveChanges();
                    return true;
                
                

            }
            return true;
        }

        
        public void getUserEdit(UserViewModel uvm,int uid,int id)
        {
            var data = _db.Timesheets.Where(x => x.TimesheetId == id).FirstOrDefault();
            var already = _db.Timesheets.Where(x => x.UserId == Convert.ToInt32(uid) && x.MissionId == uvm.missionId).FirstOrDefault();

            already.MissionId = uvm.missionId;
            //already.TimesheetTime = uvm.hour
            already.Notes = uvm.message;
            _db.SaveChanges();

        }

        //data for edit
        public Timesheet getDataForEdit(int id)
        {
            var data = _db.Timesheets.Where(x => x.TimesheetId == id).FirstOrDefault();
            return data;
        }

        public string getAllEditData(int Mid)
        {
            var data= _db.Missions.Where(x => x.MissionId == Mid).Select(x => x.Title).FirstOrDefault();
            return data;
        }

        public bool getDeleteData(int id)
        {
            var data = _db.Timesheets.Where(x => x.TimesheetId == id).FirstOrDefault();
            if (data != null)
            {

                _db.Remove(data);
                _db.SaveChanges();
            }
            return true;
        }

        //get and update data
        public string getDetailsAndUpdate(int id, string email, DateTime date, int hour, int mins, string desc, string descG, DateTime dateG, int action)
        {
            var userId = _db.Users.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefault();
            var x = _db.Timesheets.Where(y => y.TimesheetId == id && y.UserId == userId).FirstOrDefault();
            //var m = _db.Timesheets.Where(x => x.UserId == userId).Select(x => x.MissionId).FirstOrDefault();

            if (x != null && x.TimesheetTime != null)
            {
                var data = _db.Timesheets.Where(x => x.TimesheetId == id).FirstOrDefault();
                data.DateVolunteered = date;
                data.TimesheetTime = new TimeSpan(hour, mins, 0);
                data.Notes = desc;
                _db.Update(data);
                _db.SaveChanges();

            }
            else
            {
                var data = _db.Timesheets.Where(x => x.TimesheetId == id).FirstOrDefault();
                data.DateVolunteered = dateG;
                data.Action = action;
                data.Notes = descG;
                _db.Update(data);
                _db.SaveChanges();
            }


            return "Success";
        }
    }

}