using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{
    [Table("Friendship")]
    public partial class Friendship
    {
        [Key]
        [Required]
        public int FriendshipID { get; set; }

        [Required]
        public int FirstFriendID { get; set; }

        [Required]
        public int SecondFriendID { get; set; }
    }
}