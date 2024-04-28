using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace gcai.Models
{
    [ApiController]
    [Route("[Controller]")]
    [BindProperties(SupportsGet = true)]
    public class FavoritesModel
    {
        [Key]
        [BindProperty(SupportsGet = true, Name = "idPostModel")]
        [DisplayName("Post Number")]
        public string? idPostModel { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "UserId")]
        [DisplayName("UserId")]
        public string? UserId { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "ScreenName")]
        [DisplayName("ScreenName")]
        public string? ScreenName { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "PostType")]
        [DisplayName("Post Type")]
        public string? PostType { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Humor")]
        [DisplayName("Humor")]
        public string? Humor { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Problem")]
        [DisplayName("Problem")]
        public string? Problem { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Solution")]
        [DisplayName("Solution")]
        public string? Solution { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "Truth")]
        [DisplayName("Truth")]
        public string? Truth { get; set; } = "";

        [BindProperty(SupportsGet = true, Name = "PostDate")]
        [DisplayName("Post Date")]
        public string? PostDate { get; set; } = "";

        //The following feilds are used for vote totals stored in the gcai.postModel table 
        [BindProperty(SupportsGet = true, Name = "Promotions")]
        [DisplayName("Promotions")]
        public int? NumPromotions { get; set; } = 0;

        [BindProperty(SupportsGet = true, Name = "Demotions")]
        [DisplayName("Demotions")]
        public int? NumDemotions { get; set; } = 0;

        [BindProperty(SupportsGet = true, Name = "Flags")]
        [DisplayName("Flags")]
        public int? NumFlags { get; set; } = 0;

    }

}



