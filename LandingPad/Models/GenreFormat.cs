using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{

    [Table("GenreFormat")]
    public partial class GenreFormat
    {
        [Key]
        [Required]
        public int GenreFormatID { get; set; }

        [Required]
        public int GenreID { get; set; }

        [Required]
        public int ParentFormatID { get; set; }

        public int? ParentGenreID { get; set; }

        [ForeignKey("GenreID")]
        public virtual GenreTag GenreTag { get; set; }

        [ForeignKey("ParentFormatID")]
        public virtual FormatTag ParentFormatTag { get; set; }
    }
}