using Ci_Project.Entities.Data;
using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Repository
{
    public class StoryRepository:IStoryRepository
    {
        public readonly CiPlatformContext _db;
        public StoryRepository(CiPlatformContext db)
        {
            _db = db;
        }
        //Story Listing Page
        public StoryViewModel StoryUser(int pageIndex = 1, int pageSize = 3)
        {
            var story = _db.Stories.Where(x => x.Status != "Rejected").ToList();


            StoryViewModel svm = new StoryViewModel()
            {
                Missions = _db.Missions.ToList(),
                Users = _db.Users.ToList(),
                stories = story,
            };

            svm.totalrecord = svm.stories.Count();
            svm.currentPage = pageIndex;
            svm.stories = svm.stories.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return svm;
        }

       



        //below 2 method is for edit story
        public List<Mission> getMissions(long userid)
        {
            var missionApplication = _db.MissionApplications.Where(u => u.UserId == userid).Select(u => u.MissionId);
            return _db.Missions.Where(u => missionApplication.Contains(u.MissionId)).OrderBy(m => m.Title).ToList();
        }
        public StoryViewModel getDataForShareYourStory(string missionid, string uid)
        {

            if (missionid == null)
            {

                return new StoryViewModel();

            }
            else
            {
                var story = _db.Stories.SingleOrDefault(u => u.UserId == long.Parse(uid) && u.MissionId == long.Parse(missionid));

                if (story != null)
                {
                    var storyMedia = _db.StoryMedia.Where(u => u.StoryId == story.StoryId);
                    var images = storyMedia.Where(m => m.StoryType == "Image").Select(s => s.StoryPath).ToList();
                    var video = storyMedia.SingleOrDefault(m => m.StoryType == "video");
                    var missionTitle = _db.Missions.SingleOrDefault(m => m.MissionId == story.MissionId);
                    StoryViewModel model = new StoryViewModel()
                    {
                        missionName = missionTitle.Title,
                        storyId = story.StoryId,
                        missionId = story.MissionId,
                        storyTitle = story.Title,
                        storyDescription = story.Description,
                        dateAndTime = story.CreatedAt,
                        videoURL = video.StoryPath,
                        imagepaths = images,
                        storyStatus = story.Status
                    };
                    return model;
                }
                var missionTitle1 = _db.Missions.SingleOrDefault(m => m.MissionId == long.Parse(missionid));
                StoryViewModel model1 = new StoryViewModel()
                {
                    missionId = missionTitle1.MissionId,
                    missionName = missionTitle1.Title
                };
                return model1;
            }
        }

        public bool getDataForStoryTable(int mid, string sTitle, string sDateAndTime, string sDesc, int userId, string[] images, string videourl)
        {
            
            var model = new Story
            {
                MissionId = mid,
                Title = sTitle,
                Description = sDesc,
                Status = "DRAFT",
                UserId = userId
            };

            _db.Stories.Add(model);
            _db.SaveChanges();



            var story = _db.Stories.Where(s => s.MissionId == mid && s.UserId == userId).FirstOrDefault();

            foreach (var image in images)
            {
            var model1 = new StoryMedium
            {
                StoryId = story.StoryId,
                StoryType = "image",
                StoryPath = image

            };
                    _db.StoryMedia.Add(model1);

            }
                var model2 = new StoryMedium
                {
                    StoryId = story.StoryId,
                    StoryType = "video",
                    StoryPath = videourl

                };
                _db.StoryMedia.Add(model2);
                _db.SaveChanges();
                return true;
            }



            //press on submit button to change status in the databases;
            public void submit(long storyId)
            {
                var story = _db.Stories.SingleOrDefault(m => m.StoryId == storyId);
                story.Status = "Published";
                _db.Update(story);
                _db.SaveChanges();

            }


            public void editStory(int mid, string sTitle, string sDesc, int userId, string[] images, string videourl)
            {
                var entity = _db.Stories.SingleOrDefault(m => m.UserId == userId && m.MissionId == mid);
                entity.UserId = userId;
                entity.MissionId = mid;
                entity.Title = sTitle;
                entity.Description = sDesc;
                entity.Status = "Published";
                //entity.PublishedAt = sDateAndTime;
                // entity.PublishedAt = date ;
                var mediaentity = _db.StoryMedia.Where(u => u.StoryId == entity.StoryId);
                _db.StoryMedia.RemoveRange(mediaentity);
                foreach (var s in images)
                {
                    StoryMedium storyMedia = new StoryMedium()
                    {
                        StoryId = entity.StoryId,
                        StoryType = "Image",
                        StoryPath = s,
                    };
                    _db.StoryMedia.Add(storyMedia);
                }
                if (videourl != null)
                {
                    StoryMedium storyvideo = new StoryMedium()
                    {
                        StoryId = entity.StoryId,
                        StoryType = "Video",
                        StoryPath = videourl,
                    };
                    _db.StoryMedia.Add(storyvideo);
                }
                _db.SaveChanges();
            }

        //Story details page
        public List<User> RecommendedUser()
        {
            List<User> getUserList = _db.Users.ToList();
            return getUserList;
        }

       
    }
}
