namespace LandingPad.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Twitter")]
    public partial class Twitter
    {
        public int TwitterID { get; set; }

        public int UserID { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(60)]
        public string TwName { get; set; }

        [StringLength(60)]
        public string TwTag { get; set; }

        public string TwOauth { get; set; }

        public string TwVOauth { get; set; }

        public virtual LPUser LPUser { get; set; }
    }
}
