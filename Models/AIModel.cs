using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace gcai.Models
{
    [ApiController]
    [Route("[Controller]")]
    [BindProperties(SupportsGet = true)]
    public class AIModel
    {
        [Key]
        [BindProperty(SupportsGet = true, Name = "idAIModel")]
        [DisplayName("Post Number")]
        public string? idAIModel { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "UserId")]
        [DisplayName("UserId")]
        public string? UserId { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Prompt")]
        [DisplayName("Prompt")]
        public string? Prompt { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Answer")]
        [DisplayName("Answer")]
        public string? Answer { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Context")]
        [DisplayName("Context")]
        public string? Context { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "PostDate")]
        [DisplayName("Post Date")]
        public string? PostDate { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "ScreenName")]
        [DisplayName("ScreenName")]
        public string? ScreenName { get; set; } = "";

    }

}



