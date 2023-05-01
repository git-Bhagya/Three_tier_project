using CI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class GeneralViewModel
    {
        public Mission mission_details { get; set; }
        public string search { get; set; }
        public IEnumerable<Country>? Countries { get; set; }
        public IEnumerable<City>? Cities { get; set; }
        public IEnumerable<Mission>? Missions { get; set; }
        public IEnumerable<MissionTheme>? MissionThemes { get; set; }
        public IEnumerable<Skill>? Skills { get; set; }
        public int totalrecord { get; set; }
        public int currentPage { get; set; }


        //for volunteer page
        public City City { get; set; }
        public GoalMission GoalMission { get; set; }
        public MissionTheme MissionTheme { get; set; }
        public IEnumerable<Mission> RelatedMissions { get; set; }
        public IEnumerable<MissionApplication> missionApplications { get; set; }

        //rating
        public IEnumerable<MissionRating>? missionRatings { get; set; }
        public int Rating { get; set; }
        public int NumberOfVolunteers { get; set; }
        public int AvarageOfRating { get; set; }

        //skill
        public Skill Skill { get; set; }

        //Comments
        public IEnumerable<Comment>? comments { get; set; }
        public IEnumerable<User> Users { get; set; }

        //Fav
        public IEnumerable<FavoriteMission>? FavoriteMissions { get; set; }

        //document
        public IEnumerable<MissionDocument>? Documents { get; set; }

        //Story
        public IEnumerable<Story>? stories { get; set; }
        public Story story_details { get; set; }

    }
}
