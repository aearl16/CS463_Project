using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{

    [Table("LPProfile")]
    public partial class LPProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LPProfile()
        {
            Pseudonyms = new HashSet<Pseudonym>();
            Writings = new HashSet<Writing>();
            ProfileRoles = new HashSet<ProfileRole>();
            Friends = new HashSet<Friendship>();
            FriendRequestsSent = new HashSet<FriendRequest>();
            FriendRequestsReceived = new HashSet<FriendRequest>();
        }

        [Key]
        public int ProfileID { get; set; }

        public int AccessPermissionID { get; set; }

        public int UserID { get; set; }

        public string LPDescription { get; set; }

        public byte[] ProfilePhoto { get; set; }

        public bool DisplayRealName { get; set; }

        public int? Followers { get; set; }

        public int? Writers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pseudonym> Pseudonyms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Writing> Writings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileRole> ProfileRoles { get; set; }

        [ForeignKey("FirstFriendID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friendship> Friends { get; set; }

        [ForeignKey("RequesterProfileID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendRequest> FriendRequestsSent { get; set; }

        [ForeignKey("RequesteeProfileID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendRequest> FriendRequestsReceived { get; set; }

        public virtual LPUser LPUser { get; set; }

        public virtual AccessPermission AccessPermission { get; set; }
    }
}
