using Microsoft.AspNetCore.Identity;
namespace gcai.Models
{
    //this class was created to add feilds to the identity user table.
    public class AppUser : IdentityUser
    {
        public int Id {  get; set; } 
        public string? ScreenName { get; set; } = "";
        public string? SobrietyDate { get; set; }
        public bool? IsBanned { get; set; } = false;
        public int? Contributions { get; set; } = 0;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped] 
        public List<PostModel>? Posts { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<PostModel>? favorites { get; set; }

    }
}
