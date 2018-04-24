using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{

    [Table("FormatCategory")]
    public partial class FormatCategory
    {
        [Key]
        [Required]
        public int FormatCategoryID { get; set; }

        [Required]
        public int FormatID { get; set; }

        [Required]
        public int ParentID { get; set; }

        public int? SecondaryParentID { get; set; }

        public virtual FormatTag FormatTag { get; set; }
    }
}