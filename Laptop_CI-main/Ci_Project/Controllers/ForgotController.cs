using Ci_Project.Entities.Models;
using Ci_Project.Entities.ViewModels;
using Ci_Project.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Ci_Project.Controllers
{
    public class ForgotController : Controller
    {
        public readonly IforgotRepository _forgotRepository;
        public ForgotController(IforgotRepository ForgotRepository)
        {
            _forgotRepository = ForgotRepository;
        }
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgot(ForgotViewModel obj)
        {

            List<User> getUserList = _forgotRepository.Forgot(obj);
            var status = getUserList.FirstOrDefault(m => m.Email == obj.Email);
            if (status == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var token = Guid.NewGuid().ToString();

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
