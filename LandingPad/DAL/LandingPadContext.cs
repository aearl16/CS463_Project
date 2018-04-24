using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LandingPad.Models;

namespace LandingPad.DAL
{

    public partial class LandingPadContext : DbContext
    {
        public LandingPadContext()
            : base("name=LandingPadContext")
        {
        }

        public virtual DbSet<LPProfile> LPProfiles { get; set; }
        public virtual DbSet<LPUser> LPUsers { get; set; }
        public virtual DbSet<LPRole> LPRoles { get; set; }
        public virtual DbSet<ProfileRole> ProfileRoles { get; set; }
        public virtual DbSet<Pseudonym> Pseudonyms { get; set; }
        public virtual DbSet<Writing> Writings { get; set; }
        public virtual DbSet<WritingPseudonym> WritingPseudonyms { get; set; }
        public virtual DbSet<FormatTag> FormatTags { get; set; }
        public virtual DbSet<AltFormatName> AltFormatNames { get; set; }
        public virtual DbSet<FormatCategory> FormatCategories { get; set; }
        public virtual DbSet<WritingFormat> WritingFormats { get; set; }
        public virtual DbSet<AccessPermission> AccessPermissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LPProfile>()
                .HasMany(e => e.Pseudonyms)
                .WithRequired(e => e.LPProfile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LPUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<LPUser>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<LPUser>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<LPUser>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<LPUser>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<LPUser>()
                .HasMany(e => e.LPProfiles)
                .WithRequired(e => e.LPUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pseudonym>()
                .Property(e => e.Pseudonym1)
                .IsUnicode(false);

            modelBuilder.Entity<Writing>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Writing>()
                .Property(e => e.DocType)
                .IsUnicode(false);

            modelBuilder.Entity<Writing>()
                .Property(e => e.DescriptionText)
                .IsUnicode(false);
        }
    }
}
