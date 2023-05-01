using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Interface
{
    public interface IUserRepository
    {
        public UserViewModel getprofile(string email);
        public void SaveUserDetails(UserViewModel userView, int uid);

        //public string SaveSkills(UserViewModel userView, string [] skills,int uid);
        public int AddUserSkills(long[] SkillArray, int uid);

        public bool changePass(UserViewModel userView, string email);
        //public bool changeAva(IFormFile image, string email);
        public string changeAvatar(string image, string email);

        public List<Country> GetCountryList();
        public List<City> GetCityList(int id);
        public UserViewModel Getpolicypage();
        public bool getContactDetails(UserViewModel uvm,int uid);
        public UserViewModel GetTimesheet(string missionid, int uid);
        public bool GetTime(UserViewModel uvm, int? uid);
        //public bool EditUserData(UserViewModel uvm, int? uid);
        public void getUserEdit(UserViewModel uvm, int uid,int id);
        public Timesheet getDataForEdit(int id);
        public string getAllEditData(int Mid);

        public bool getDeleteData(int id);

        public string getDetailsAndUpdate(int id, string email, DateTime date, int hour, int mins, string desc, string descG, DateTime dateG, int action);









    }
}
