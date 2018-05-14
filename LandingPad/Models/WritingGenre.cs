namespace LandingPad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WritingGenre")]
    public partial class WritingGenre
    {
        [Key]
        [Required]
        public int WritingGenreID { get; set; }

        [Required]
        public int WritingID { get; set; }

        [Required]
        public int GenreID { get; set; }

        public virtual Writing Writing { get; set; }

        public virtual GenreTag GenreTag { get; set; }
    }
}