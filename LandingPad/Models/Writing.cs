using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web;
using System.Web.Mvc;

namespace LandingPad.Models
{

    [Table("Writing")]
    public partial class Writing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Writing()
        {
            WritingPseudonyms = new HashSet<WritingPseudonym>();
            WritingFormats = new HashSet<WritingFormat>();
        }

        public int WritingID { get; set; }

        public int ProfileID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public byte[] Document { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? EditDate { get; set; }

        public bool LikesOn { get; set; }

        public bool CommentsOn { get; set; }

        public bool CritiqueOn { get; set; }

        [Required]
        public string DocType { get; set; }

        [Required]
        public string DescriptionText { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingPseudonym> WritingPseudonyms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingFormat> WritingFormats { get; set; }

        public virtual LPProfile LPProfile { get; set; }

        //public virtual AccessPermission AccessPermission { get; set; }

        [DisplayName("Select File")]
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}