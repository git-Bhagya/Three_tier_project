using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Interface
{
    public interface IHomeRepository
    {
        public GeneralViewModel LandingPageList(int id,int pg=1);
        public string favLanding(int mId, string uid);

        public GeneralViewModel PlatformUser(int id,string[]? country, string[]? city, string[]? themes, string[]? skills, string? sortVal, string? search, int pg = 1);
        public GeneralViewModel VolunteerUser(int id,int uid, int pageIndex = 1, int pageSize = 3);
        public bool favUser(int missionid, string email);
        public bool RatingUser(int missionid, int rating, string Email);
        public bool CommentUser(int missionid, string comment, string email);
        public List<User> RecommendedUser();
        public bool ApplyUser(int id ,long uid);
        //public GeneralViewModel StoryUser(int pageIndex = 1, int pageSize = 3);
        //public GeneralViewModel ShareStoryUser();
        //public bool AddStoryUser(int missionid, string title, string email, string description);
        //public bool getDataForStoryTable(int mid, string sTitle, DateTime sDateAndTime, string sDesc, int userId, string[] images, string videourl);
        public GeneralViewModel StoryDetailsUser(string search,int id,long uid,int Mid,int count);
        //public Story GetMissionStory(int? id);
        public User getUserDetails(int? id);

        //public GeneralViewModel getDataForShareYourStory(string missionid, string uid);
        //public void submit(long storyId);
        //public void editStory(int mid, string sTitle, string sDesc, int userId, string[] images, string videourl);
        //public List<Mission> getMissions(long uid);

    }
}
