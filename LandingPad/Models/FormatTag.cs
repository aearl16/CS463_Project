using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    public class FormatTag
    {
        public FormatTag()
        {
            WritingFormats = new HashSet<WritingFormat>();
        }

        [Key]
        [Required]
        public int FormatID { get; set; }

        [Required]
        public string FormatName { get; set; }

        [Required]
        public string CategoryType { get; set; }

        [Required]
        public string FormatType { get; set; }

        public string Explanation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingFormat> WritingFormats { get; set; }
    }
}