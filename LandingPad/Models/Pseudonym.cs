using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace LandingPad.Models
{

    [Table("Pseudonym")]
    public partial class Pseudonym
    {
        public Pseudonym()
        {
            WritingPseudonyms = new HashSet<WritingPseudonym>();
        }

        public int PseudonymID { get; set; }

        public int ProfileID { get; set; }

        [Column("Pseudonym")]
        [Required]
        public string Pseudonym1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingPseudonym> WritingPseudonyms { get; set; }

        public virtual LPProfile LPProfile { get; set; }
    }
}