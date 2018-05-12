using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LandingPad.DAL;

namespace LandingPad.Models
{
    [Table("GenreTag")]
    public partial class GenreTag
    {
        public GenreTag()
        {
            AltGenreNames = new HashSet<AltGenreName>();
            ChildGenres = new HashSet<GenreCategory>();
            ParentGenres = new HashSet<GenreCategory>();
            GenreFormats = new HashSet<GenreFormat>();
            WritingGenres = new HashSet<WritingGenre>();
        }

        [Key]
        [Required]
        public int GenreID { get; set; }

        [Required]
        public string GenreName { get; set; }

        public string Explanation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AltGenreName> AltGenreNames { get; set; }

        [ForeignKey("ParentID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GenreCategory> ChildGenres { get; set; }

        [ForeignKey("GenreID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GenreCategory> ParentGenres { get; set; }

        [ForeignKey("GenreID")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GenreFormat> GenreFormats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritingGenre> WritingGenres { get; set; }
    }
}