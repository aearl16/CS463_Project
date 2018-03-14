using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    [Table("AltFormatName")]
    public partial class AltFormatName
    {
        [Key]
        [Required]
        public int AltFormatNameID { get; set; }

        [Required]
        public int FormatID { get; set; }

        [Required]
        public string AltName { get; set; }
    }
}