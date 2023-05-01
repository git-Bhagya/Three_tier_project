using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Interface
{
    public interface IStoryRepository
    {
        public StoryViewModel StoryUser(int pageIndex = 1, int pageSize = 3);
        //public StoryViewModel getStoryDetail(int pageIndex, int pageSize);

        /*public bool getDataForStoryTable(int MissionId, string StoryTitle, string StoryText, DateTime StoryDate, int userId, string[] images, string videourl);*/

        //public StoryViewModel GetStoryDetail(long userId, int missionId);

        //public string userWithId(int[] ids, int missionid, string url);
        //public List<User> getUsersForRecomandateToCoWorker(string uid);

        public StoryViewModel getDataForShareYourStory(string missionid, string uid);
        public List<Mission> getMissions(long uid);

        public bool getDataForStoryTable(int mid, string sTitle, string sDateAndTime, string sDesc, int userId, string[] images, string videourl);

        /* public bool saveStory(shareYourStoryVM storyVM, long userId);*/

        public void submit(long storyId);

        public void editStory(int mid, string sTitle, string sDesc, int userId, string[] images, string videourl);

        //story details
        public List<User> RecommendedUser();

    }
}
