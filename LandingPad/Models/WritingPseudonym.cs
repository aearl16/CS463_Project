using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    public class WritingPseudonym
    {
        [Key]
        [Required]
        public int WritingPseudonymID { get; set; }

        [Required]
        public int WritingID { get; set; }

        [Required]
        public int PseudonymID { get; set; }
    }
}