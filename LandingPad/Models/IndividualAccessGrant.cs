using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandingPad.Models
{
    [Table("IndividualAccessGrant")]
    public partial class IndividualAccessGrant
    {
        [Key]
        [Required]
        public int IndividualAccessGrantID { get; set; }

        [Required]
        public int AccessPermissionID { get; set; }

        [Required]
        public int GranteeID { get; set; }
    }
}