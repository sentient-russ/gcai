using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace gcai.Models
{
    public class SourceModel
    {
        [Key]
        [DisplayName("Source Name")]
        public string? SourceName { get; set; } = "";

        [DisplayName("excerpt")]
        public string? Excerpt { get; set; } = "";
    }

}