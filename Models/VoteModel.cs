using System.ComponentModel.DataAnnotations;

namespace gcai.Models
{
    public class VoteModel
    {
        [Key]
        public string? idVoteModel { get; set; } = "";
        public string? PostRefNum { get; set; } = "";
        public string? UserId { get; set; } = "";
        public string? UpVoted { get; set; } = "";
        public string? DownVoted { get; set; } = "";
        public string? StarVoted { get; set; } = "";
        public string? Flagged { get; set; } = "";
        public string? DateVoted { get; set; } = "";
        public string? ScreenName { get; set; } = "";
    }
}
