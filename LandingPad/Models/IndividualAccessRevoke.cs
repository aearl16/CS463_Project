using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    [Table("IndividualAccessRevoke")]
    public partial class IndividualAccessRevoke
    {
        [Key]
        [Required]
        public int IndividualAccessRevokeID { get; set; }

        [Required]
        public int AccessPermissionID { get; set; }

        [Required]
        public int RevokeeID { get; set; }
    }
}