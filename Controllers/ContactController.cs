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

/*        [Required]
        [StringLength(50, MinimumLength = 4)]
        [DisplayName("Business Name:")]
        [BindProperty(SupportsGet = true, Name = "Business")]
        public string? Business { get; set; }*/

/*        [Required]
        [StringLength(14, MinimumLength = 10)]
        [DisplayName("Phone Number:")]
        [BindProperty(SupportsGet = true, Name = "Phone")]
        public string? Phone { get; set; }*/

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

/*        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Projected Start Date:")]
        [BindProperty(SupportsGet = true, Name = "startDate")]
        public DateTime startDate { get; set; }*/

/*        [Required]
        [DefaultValue(1)]
        [DisplayName("Priority Level")]
        [BindProperty(SupportsGet = true, Name = "priorityLevel")]
        public string priorityLevel { get; set; } = "High";*/

        [HttpPost]
        public IActionResult PostAsync([FromForm] ContactDataModel ComplexDataIn)
        {

            EmailService emailService = new EmailService();
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
