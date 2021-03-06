using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LandingPad.DAL;

namespace LandingPad.Models
{
    [Table("FormatTag")]
    public partial class FormatTag
    {
        public FormatTag()
        {
            AltFormatNames = new HashSet<AltFormatName>();
            ChildFormats = new HashSet<FormatCategory>();
            ChildGenres = new HashSet<GenreFormat>();
            WritingFormats = new HashSet<WritingFormat>();
        }

        [Key]
        [Required]
        public int FormatID { get; set; }

        [Required]
        public string FormatName { get; set; }

        public string Explanation { get; set; }

        public bool IsFictionOnly { get; set; }

        public bool IsNonfictionOnly { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AltFormatName> AltFormatNames { get; set; }

        [ForeignKey("ParentID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormatCategory> ChildFormats { get; set; }

        [ForeignKey("ParentFormatID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GenreFormat> ChildGenres { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingFormat> WritingFormats { get; set; }
    }
}