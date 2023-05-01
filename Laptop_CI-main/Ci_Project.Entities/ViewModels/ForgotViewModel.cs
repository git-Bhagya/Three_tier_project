﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Entities.ViewModels
{
    public class ForgotViewModel
    {

        [Required(ErrorMessage="Email is Required")]
        public string Email { get; set; } = null!;
    }
}
