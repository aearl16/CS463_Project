using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

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
            WritingGenres = new HashSet<WritingGenre>();
        }

        public int WritingID { get; set; }

        public int ProfileID { get; set; }

        public int AccessPermissionID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public byte[] Document { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy h:mm tt}")]
        public DateTime AddDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy h:mm tt}")]
        public DateTime? EditDate { get; set; }

        public bool LikesOn { get; set; }

        public bool CommentsOn { get; set; }

        public bool CritiqueOn { get; set; }

        public bool UsePseudonymsInAdditionToUsername { get; set; }

        [Required]
        public string DocType { get; set; }

        [Required]
        public string DescriptionText { get; set; }

        [Required]
        public string WritingFileName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingPseudonym> WritingPseudonyms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingFormat> WritingFormats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingGenre> WritingGenres { get; set; }

        public virtual LPProfile LPProfile { get; set; }
        
        public virtual AccessPermission AccessPermission { get; set; }

        [DisplayName("Select File")]
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}
