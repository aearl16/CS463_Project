namespace LandingPad.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pseudonym")]
    public partial class Pseudonym
    {
        public int PseudonymID { get; set; }

        public int ProfileID { get; set; }

        [Column("Pseudonym")]
        [Required]
        public string Pseudonym1 { get; set; }

        public virtual LPProfile LPProfile { get; set; }
    }
}
