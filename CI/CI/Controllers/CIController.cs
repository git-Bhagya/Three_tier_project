using CI.Entities.Data;
using CI.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using CI.Entities.ViewModels;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace CI.Controllers
{
    public class CIController : Controller
    {


        private readonly ILogger<CIController> _logger;
        private readonly CiPlatformContext _db;

        public string Email { get; private set; }
        public string Password { get; private set; }

        public CIController(ILogger<CIController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {

                var status = _db.Users.Where(u => u.Email == lvm.Email && u.Password == lvm.Password).FirstOrDefault();
                if (status != null)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, status.Email) },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, status.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Name, status.LastName));
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("EmailId", status.Email);
                    return RedirectToAction("Platform", "CI");
                }
                else
                {
                    TempData["Error"] = "Enter Correct details";
                    return RedirectToAction("Login", "CI");
                }
            }

            return View();
        }


        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Forgot(ForgotViewModel obj)
        {

            var status = _db.Users.FirstOrDefault(m => m.Email == obj.Email);
            if (status == null)
            {
                return RedirectToAction("login", "CI");
            }

            var token = Guid.NewGuid().ToString();

            PasswordReset passwordReset = new PasswordReset
            {
                Email = obj.Email,
                Token = token,
            };
            _db.Add(passwordReset);
            _db.SaveChanges();

            var resetLink = Url.Action("Reset", "CI", new { email = obj.Email, token }, Request.Scheme);

            var fromAddress = new MailAddress("virupatel6048@gmail.com", "Sender Name");
            var toAddress = new MailAddress(obj.Email);
            var subject = "Password reset request";
            var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("virupatel6048@gmail.com", "zwsgqrwabnvhmpfg"),
                EnableSsl = true
            };
            smtpClient.Send(message);
            return RedirectToAction("Reset", "CI");
        }
        [HttpGet]
        public IActionResult Registration()
        {

            return View();
        }

        [HttpPost]
        [Route("/CI/Registration", Name = "Register")]
        public IActionResult Registration(RegistrationViewModel obj)
        {
            var User_data = new User()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Password = obj.Password,
                CityId = 5,
                CountryId = 1
            };
            _db.Users.Add(User_data);
            _db.SaveChanges();
            return RedirectToAction("Login");

        }

        [HttpGet]
        public IActionResult Reset(string Email, string Token)
        {
            var validEmail = _db.PasswordResets.FirstOrDefault(m => m.Email == Email && m.Token == Token);
            if (validEmail != null)
            {
                var tempResetPassword = new TempResetPasswordModel()
                {
                    Email = Email,
                    Token = Token,
                };
                return View(tempResetPassword);
            }
           
            return View();
        }

        [HttpPost]
        public IActionResult Reset(TempResetPasswordModel tempResetPasswordModel)
        {
            var validEmail = _db.Users.FirstOrDefault(m => m.Email == tempResetPasswordModel.Email);
            if (validEmail != null)
            {
                validEmail.Password = tempResetPasswordModel.NewPassword;
                _db.SaveChanges();
                return RedirectToAction("Login", "CI");
            }
            return View();
        }

        public IActionResult Platform()
        {
            ViewBag.cities = _db.Cities.ToList();
            ViewBag.countries = _db.Countries.ToList();
            List<Mission> missions = _db.Missions.ToList();
            
            return View(missions);
        }

        [Authorize]
        public IActionResult StoryListingPage()
        {

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "CI");
         
        }

    }
}
