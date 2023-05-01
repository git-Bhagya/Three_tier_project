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
    public class ResetRepository : IResetRepository
    {
        public readonly CiPlatformContext _db;
        public ResetRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public List<PasswordReset> ResetUser()
        {
            List<PasswordReset> passReset = _db.PasswordResets.ToList();
            return passReset;
        }
        public List<User> User(TempResetPasswordModel tempResetPasswordModel)
        {
            List<User> getUserList = _db.Users.ToList();
            var validEmail = _db.Users.FirstOrDefault(m => m.Email == tempResetPasswordModel.Email);
            if (validEmail != null)
            {
                validEmail.Password = tempResetPasswordModel?.NewPassword;
                _db.SaveChanges();  
            }
            return getUserList;
        }
    }
}
