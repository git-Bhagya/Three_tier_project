
using Ci_Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Entities.ViewModels
{
    public class GeneralViewModel
    {
        public Mission mission_details { get; set; }
        public string search { get; set; }
        public IEnumerable<Admin> Admin { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Mission> Missions { get; set; }
        public IEnumerable<MissionTheme> MissionThemes { get; set; }
        public IEnumerable<MissionMedium> MissionMedia { get; set; }
        public MissionMedium mediaMission { get; set; }

        public IEnumerable<Skill> Skills { get; set; }
        public int totalrecord { get; set; }
        public int currentPage { get; set; }


        //for volunteer page
        public City City { get; set; }
        public GoalMission GoalMissions { get; set; }
        public IEnumerable<GoalMission> goal { get; set; }

        public MissionTheme MissionTheme { get; set; }
        public IEnumerable<Mission> RelatedMissions { get; set; }
        public IEnumerable<MissionApplication> missionApplications { get; set; }

        //rating
        public IEnumerable<MissionRating>? missionRatings { get; set; }
        public MissionRating rating { get; set; }
        public int  UserRate { get; set; }
        public int NumberOfVolunteers { get; set; }
        public int AvarageOfRating { get; set; }
        public int rateUser { get; set; }

        //skill
        public Skill Skill { get; set; }

        //Comments
        public IEnumerable<Comment>? comments { get; set; }
        public IEnumerable<User> Users { get; set; }

        //Fav
        public IEnumerable<FavoriteMission> FavoriteMissions { get; set; }

        //document
        public IEnumerable<MissionDocument>? Documents { get; set; }

        //Story
        public IEnumerable<Story>? stories { get; set; }
        public Story story_details { get; set; }
        
        //user for story details
        public User user_details  { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int Views { get; set; }

        //hddfhfghgfh
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

        //contact us
        public string subjectForContactUsPage { get; set; }
        public string messageForContactUsPage { get; set; }

    }
}
