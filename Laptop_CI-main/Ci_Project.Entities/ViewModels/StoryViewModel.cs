using Ci_Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Entities.ViewModels
{
    public class StoryViewModel
    {
        public IEnumerable<Mission> Missions { get; set; }
        public Mission mission_details { get; set; }

        public IEnumerable<Story>? stories { get; set; }
        public Story story_details { get; set; }

        //user for story details
        public User user_details { get; set; }
        public IEnumerable<User> Users { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public int totalrecord { get; set; }
        public int currentPage { get; set; }

        //share story
        public long userId { get; set; }
        public string missionName { get; set; }
        public long storyId { get; set; }
        public long missionId { get; set; }
        public string storyTitle { get; set; }
        public string storyDescription { get; set; }
        public DateTime dateAndTime { get; set; }
        public string videoURL { get; set; }

        public string storyStatus { get; set; }
        public List<string> imagepaths { get; set; }
    }
}
