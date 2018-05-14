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

        public int? FirstPseudonymID { get; set; }

        public int? SecondPseudonymID { get; set; }

        public DateTime AcceptDate { get; set; }

        [ForeignKey("FirstFriendID")]
        public virtual LPProfile FirstFriend { get; set; }

        [ForeignKey("SecondFriendID")]
        public virtual LPProfile SecondFriend { get; set; }

        [ForeignKey("FirstPseudonymID")]
        public virtual Pseudonym FirstPseudonym { get; set; }

        [ForeignKey("SecondPseudonymID")]
        public virtual Pseudonym SecondPseudonym { get; set; }
    }
}