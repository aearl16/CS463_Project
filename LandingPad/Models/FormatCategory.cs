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

        [ForeignKey("FormatID")]
        public virtual FormatTag FormatTag { get; set; }

        [ForeignKey("ParentID")]
        public virtual FormatTag ParentFormat { get; set; }

        [ForeignKey("SecondaryParentID")]
        public virtual FormatTag SecondaryParentFormat { get; set; }
    }
}