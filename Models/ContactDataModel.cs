using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace gcia.Controllers
{
    public class ContactDataModel

    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [DisplayName("Full Name:")]
        [BindProperty(SupportsGet = true, Name = "Name")]
        public string? Name { get; set; }

/*        [Required]
        [StringLength(50, MinimumLength = 4)]
        [DisplayName("Business Name:")]
        [BindProperty(SupportsGet = true, Name = "Business")]
        public string? Business { get; set; }*/

/*        [Required]
        [StringLength(50, MinimumLength = 6)]
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
        [StringLength(1000, MinimumLength = 1)]
        [DisplayName("Message:")]
        [BindProperty(SupportsGet = true, Name = "Message")]
        public string? Message { get; set; }

/*        [DataType(DataType.Date)]
        [DisplayName("Projected Start Date:")]
        [BindProperty(SupportsGet = true, Name = "startDate")]
        public DateTime startDate { get; set; } = DateTime.Now;*/

/*        [Required]
        [DisplayName("Priority Level")]
        [BindProperty(SupportsGet = true, Name = "priorityLevel")]
        public string priorityLevel { get; set; }*/

    }

}
