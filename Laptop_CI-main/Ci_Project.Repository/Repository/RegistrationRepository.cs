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
    public class RegistrationRepository : IRegistrationRepository
    {
        public readonly CiPlatformContext _db;
        public RegistrationRepository(CiPlatformContext db)
        {
            _db = db;
        }
        public bool Registration(RegistrationViewModel obj)
        {
            var user = _db.Users.Where(x=>x.Email == obj.Email).FirstOrDefault();
            if(user == null)
            {
                var User_data = new User()
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    Email = obj.Email,
                    PhoneNumber = obj.PhoneNumber,
                    Password = obj.Password,
                    CityId = 5,
                    CountryId = 1,
                    Status = "Active"
                };
                _db.Users.Add(User_data);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

    }
}
