using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace gcai.Models
{
    /*
     * This object stores tallies for a specific post.
     * Four fields are marked bellow that are tallied for the specific user calling the method
     */
    public class VoteTallyModel
    {
        [Key]
        public string? idVoteModel { get; set; } = "0";
        public string? PostRefNum { get; set; } = "0";

        public string? UpVotedTotal { get; set; } = "0";

        public string? DownVotedTotal { get; set; } = "0";

        public string? StarVotedTotal { get; set; } = "0";

        public string? FlaggedTotal { get; set; } = "0";
    }

}
