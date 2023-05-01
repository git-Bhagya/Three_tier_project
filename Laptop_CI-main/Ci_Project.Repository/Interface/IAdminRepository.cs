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
    public interface IAdminRepository
    {
        public List<Admin> Login();
        public List<User> getUserList();
        public List<City> getCityList();
        public List<Country> getCountryList();
        public List<Banner> getBannerList();
        public List<MissionMedium> getMissionImage();
        public List<MissionDocument> getDocList();
        public List<MissionSkill> getMissionSkill();


        //public AdminViewModel getUser();
        public bool GetTime(AdminViewModel avm, int? uid);
        public void getDetailsAndUpdate(int id, string fname, string lname, string email, string pass, string Avatar, int Eid, string Department, int CityId, int CountryId, string Profiletxt, string status);

        public bool getDeleteData(int id, string email);
        public User getDataForEdit(int id);
        //Mission
        public List<Mission> getMissionList();
        public bool MissionAdd(AdminViewModel avm, int? uid, int[] listofSkills);
        public AdminViewModel getEditMission(int id);

        public string getEditMissionData(int id, string mTitle, string Sdes, string des, int Cityid, int countryId, string OrgName, string OrdDetails, DateTime sDate, DateTime eDate, string mType, string seats, string mImages, string mVideo, string mDoc);
        public bool getDeleteMission(int id);
        public List<MissionMedium> MissionImages(int id);
        public List<MissionDocument> MissionDocuments(int id);


        //Mission Application
        public List<MissionApplication> getMissionApplicationList();
        public bool getChecked(int id, string email);
        public bool getCancel(int id, string email);

        //Story
        public List<Story> getStoryList();
        public bool getDeleteStory(int id);
        public bool CheckedStory(int id);
        public bool CancelStory(int id);
        public Story getViewStory(int id);
        public User UserName(int id);
        public List<StoryMedium> getStoryMedia(int id);


        //theme
        public List<MissionTheme> getThemeList();
        public bool GetThemeAdd(AdminViewModel avm);
        public MissionTheme GetEditTheme(int id);
        public bool EditTheme(int themeId, AdminViewModel advm);
        public string EditMissionThemeData(int id, string tTitle, string tStatus);
        public bool DeleteTheme(int id);

        //skill
        public List<Skill> getSkillList();
        public bool GetSkillAdd(AdminViewModel avm);
        public bool CheckedSkill(int id);
        public bool CancelSkill(int id);


        //cms page
        public List<CmsPage> getCMSDataList();
        public bool AddCMSPage(AdminViewModel avm);
        //public CmsPage GetEditCMS(int id);
        public AdminViewModel GetEditCMS(int id);

        public bool EditCMSPage(int cmsId , AdminViewModel advm);
        public bool getDeleteCMS(int id);

        //banner
        public bool GetBannerAdd(AdminViewModel avm);
        public string EditDataBanner(int id, string bImage, string bText, int SOrd);
        public Banner GetEditBanner(int id);
        public List<Banner> GetBannerImage(int id);



    }
}
