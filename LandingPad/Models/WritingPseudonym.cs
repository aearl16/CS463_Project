using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    [Table("WritingPseudonym")]
    public partial class WritingPseudonym
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