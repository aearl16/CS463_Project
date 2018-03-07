using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    [Table("WritingFormat")]
    public partial class WritingFormat
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