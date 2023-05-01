using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Interface
{
    public interface ILoginRepository
    {
        public List<User> Login();
        public List<Banner> BannerList();

    }
}
