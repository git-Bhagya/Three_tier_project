using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace CI.Controllers
{
    public class ForgotController : Controller
    {

        private readonly ILogger<ForgotController> _logger;
        private readonly CiPlatformContext _db;

       
        public ForgotController(ILogger<ForgotController> logger, CiPlatformContext db)
        {
            _logger = logger;
            _db = db;
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
                return RedirectToAction("Login", "Login");
            }

            var token = Guid.NewGuid().ToString();

            PasswordReset passwordReset = new PasswordReset
            {
                Email = obj.Email,
                Token = token,
            };
            _db.Add(passwordReset);
            _db.SaveChanges();

            var resetLink = Url.Action("Reset", "Reset", new { email = obj.Email, token }, Request.Scheme);

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
            return RedirectToAction("Reset", "Reset");
        }
    }
}
