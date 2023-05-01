using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Interface
{
    public interface IforgotRepository
    {
        public List<User> Forgot(ForgotViewModel obj);
    }
}
