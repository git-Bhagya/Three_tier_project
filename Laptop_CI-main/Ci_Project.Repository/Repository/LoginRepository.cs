using Ci_Project.Entities.Data;
using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Project.Repository.Repository
{
    public class LoginRepository : ILoginRepository
    {
        public readonly CiPlatformContext _db;

        public LoginRepository(CiPlatformContext CiPlatformContext)
        {
            _db = CiPlatformContext;
        }
        public List<User> Login()
        {
            List<User> getUserList = _db.Users.ToList();
            return getUserList;
        }
        public List<Banner> BannerList()
        {
            List<Banner> getBannerList = _db.Banners.ToList();
            return getBannerList;
        }
       

        
    }
}
