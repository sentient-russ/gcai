using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using gcai.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using gcai.Models;
using gcia.Controllers;

namespace magnadigi.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    [BindProperties(SupportsGet = true)]

    public class ContactController : Controller
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        [DisplayName("Full Name:")]
        [BindProperty(SupportsGet = true, Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255, MinimumLength = 1)]
        [DisplayName("Email Address:")]
        [BindProperty(SupportsGet = true, Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(255, MinimumLength = 1)]
        [DisplayName("Message:")]
        [BindProperty(SupportsGet = true, Name = "Message")]
        public string? Message { get; set; }

        [HttpPost]
        public IActionResult PostAsync([FromForm] ContactDataModel ComplexDataIn)
        {

            EmailSender emailService = new EmailSender();
            string response = emailService.SendContactMessage(ComplexDataIn);
            bool messageSent;
            if (response.Contains("Ok"))
            {
                messageSent = true;
            }
            else
            {
                messageSent = false;
            }
            ViewData["wasSent"] = messageSent;
            return RedirectToAction("Index", "Home");
        }
    }
}
