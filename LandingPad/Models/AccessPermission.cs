using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    [Table("AccessPermission")]
    public partial class AccessPermission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccessPermission()
        {
            IndividualAccessGrants = new HashSet<IndividualAccessGrant>();
            IndividualAccessRevokes = new HashSet<IndividualAccessRevoke>();
        }

        [Key]
        [Required]
        public int AccessPermissionID { get; set; }

        public int? WritingID { get; set; }

        public int? ProfileID { get; set; }

        public int? PseudonymID { get; set; }

        [Required]
        public bool PublicAccess { get; set; }

        [Required]
        public bool FriendAccess { get; set; }

        [Required]
        public bool PublisherAccess { get; set; }

        [Required]
        public bool MinorAccess { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IndividualAccessGrant> IndividualAccessGrants { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IndividualAccessRevoke> IndividualAccessRevokes { get; set; }
    }
}