using Ci_Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Entities.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        public string Password { get; set; }

        //public IEnumerable<Banner> banner { get; set; }
        public List<Banner> banner_list { get; set; }
    }
}
