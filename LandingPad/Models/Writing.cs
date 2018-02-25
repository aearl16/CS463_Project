namespace LandingPad.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Writing")]
    public partial class Writing
    {
        public int WritingID { get; set; }

        public int ProfileID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public byte[] Document { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? EditDate { get; set; }

        public bool LikesOn { get; set; }

        public bool CommentsOn { get; set; }

        public bool CritiqueOn { get; set; }

        [Required]
        public string DocType { get; set; }

        [Required]
        public string DescriptionText { get; set; }
    }
}
