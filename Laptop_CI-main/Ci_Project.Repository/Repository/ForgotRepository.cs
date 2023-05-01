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
    public class ForgotRepository : IforgotRepository
    {
        public readonly CiPlatformContext _db;
        public ForgotRepository(CiPlatformContext db)
        {
            _db = db;
        }
        public List<User> Forgot(ForgotViewModel obj)
        {
            List<User> getUserList = _db.Users.ToList();

            var token = Guid.NewGuid().ToString();
            PasswordReset passwordReset = new PasswordReset
            {
                Email = obj.Email,
                Token = token,
            };
            _db.Add(passwordReset);
            _db.SaveChanges();
            return getUserList;
        }


    }
}
