namespace LandingPad.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LPProfile")]
    public partial class LPProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LPProfile()
        {
            Pseudonyms = new HashSet<Pseudonym>();
        }

        [Key]
        public int ProfileID { get; set; }

        public int UserID { get; set; }

        public byte[] ProfilePhoto { get; set; }

        public bool DisplayRealName { get; set; }

        public int? Friends { get; set; }

        public int? Followers { get; set; }

        public int? Writers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pseudonym> Pseudonyms { get; set; }

        public virtual LPUser LPUser { get; set; }
    }
}
