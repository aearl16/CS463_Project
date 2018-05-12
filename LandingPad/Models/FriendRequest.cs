using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{
    [Table("FriendRequest")]
    public partial class FriendRequest
    {
        [Key]
        [Required]
        public int FriendRequestID { get; set; }

        [Required]
        public int RequesterProfileID { get; set; }

        [Required]
        public int RequesteeProfileID { get; set; }

        public int? RequesterPseudonymID { get; set; }

        public int? RequesteePseudonymID { get; set; }

        [ForeignKey("RequesterProfileID")]
        public LPProfile RequesterProfile;

        [ForeignKey("RequesteeProfileID")]
        public LPProfile RequesteeProfile;

        [ForeignKey("RequesterPseudonymID")]
        public LPProfile RequesterPseudonym;

        [ForeignKey("RequesteePseudonymID")]
        public LPProfile RequesteePseudonym;
    }
}