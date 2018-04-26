using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using LandingPad.DAL;

namespace LandingPad.Models
{
    [Table("ProfileRole")]
    public partial class ProfileRole
    {
        [Key]
        [Required]
        public int ProfileRoleID { get; set; }

        [Required]
        public int ProfileID { get; set; }

        [Required]
        public int RoleID { get; set; }

        public bool UseSecondaryRoleName { get; set; }

        public virtual LPProfile LPProfile { get; set; }

        public virtual LPRole LPRole { get; set; }
    }
}