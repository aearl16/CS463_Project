namespace LandingPad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
