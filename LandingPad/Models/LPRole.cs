using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using LandingPad.DAL;

namespace LandingPad.Models
{
    [Table("LPRole")]
    public partial class LPRole
    {
        [Key]
        [Required]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string SecondaryRoleName { get; set; }
    }
}