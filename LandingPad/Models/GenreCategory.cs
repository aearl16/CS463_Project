using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{

    [Table("GenreCategory")]
    public partial class GenreCategory
    {
        [Key]
        [Required]
        public int GenreCategoryID { get; set; }

        [Required]
        public int GenreID { get; set; }

        [Required]
        public int ParentID { get; set; }

        public int? SecondaryParentID { get; set; }

        public int? TertiaryParentID { get; set; }

        [ForeignKey("GenreID")]
        public virtual GenreTag GenreTag { get; set; }

        [ForeignKey("ParentID")]
        public virtual GenreTag ParentGenre { get; set; }

        [ForeignKey("SecondaryParentID")]
        public virtual GenreTag SecondaryParentGenre { get; set; }

        [ForeignKey("TertiaryParentID")]
        public virtual GenreTag TertiaryParentGenre { get; set; }
    }
}