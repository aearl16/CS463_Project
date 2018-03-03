using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    public class WritingFormat
    {
        [Key]
        [Required]
        public int WritingFormatID { get; set; }

        [Required]
        public int WritingID { get; set; }

        [Required]
        public int FormatID { get; set; }

        public virtual Writing Writing { get; set; }

        public virtual FormatTag FormatTag { get; set; }
    }
}