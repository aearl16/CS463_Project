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
        [Key]
        [Required]
        public int AccessPermissionID { get; set; }

        public int ProfileID { get; set; }

        public int WritingID { get; set; }

        [Required]
        public bool PublicAccess { get; set; }

        [Required]
        public bool FriendAccess { get; set; }

        [Required]
        public bool PublisherAccess { get; set; }

        [Required]
        public bool MinorAccess { get; set; }
    }
}