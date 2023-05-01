using Ci_Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Entities.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public User user_details { get; set; }
        [StringLength(10, MinimumLength = 6)]
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? EmployeeId { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string ProfileText { get; set; }
        public string WhyIVolunteer { get; set; }
        public string Manager { get; set; }
        
        public string city { get; set; }
        public string country { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public string Status { get; set; }
        public string LinkedInURL { get; set; }
        public IEnumerable<Mission> Missions { get; set; }
        
        public string Avatar { get; set; }
        public Skill Skill { get; set; }
        public IEnumerable<Skill> SkillList { get; set; }
        public IEnumerable<UserSkill> UserSkills { get; set; }


        //change pwd
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6)]
        [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Invalid password format")]
        public string OldPwd { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6)]
        [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Invalid password format")]
        public string NewPwd { get; set; }

        [Required(ErrorMessage = "Password is not matched")]
        [Compare("NewPwd")]
        [NotMapped]
        public string ComPwd { get; set; }

        public long cityId { get; set; }
        public long countryId { get; set; }

        //contact us
        public string subjectForContactUsPage { get; set; }
        public string messageForContactUsPage { get; set; }

        //timesheets
        public IEnumerable<Timesheet> Timesheets { get; set; }
        public IEnumerable<MissionApplication> missionApp { get; set; }

        public Timesheet timesheet { get; set; }
        public Mission mission_details { get; set; }
        public long? missionId { get; set; }

        public TimeOnly dateAndTime { get; set; }

        public string? message { get; set; }

        public int hour { get; set; }

        public int minute { get; set; }

        public long? userid { get; set; }
        public int action { get; set; }
        public string mission { get; set; }
        public string goalMessage { get; set; }
        public string timeMessage { get; set; }
    }
}
