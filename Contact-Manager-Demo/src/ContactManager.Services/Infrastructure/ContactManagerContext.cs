using ContactManager.Services.Models;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Services.Infrastructure
{
    public class ContactManagerContext : DbContext
    {
        public DbSet<UserContact> UserContact { get; set; }
        public DbSet<ContactNote> ContactNote { get; set; }

        public ContactManagerContext(DbContextOptions options) : base(options)
        {
        }

        public ContactManagerContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserContact>(entity =>
            {
                entity.Property(u => u.FirstName)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnType("nvarchar(200)");

                entity.Property(u => u.LastName)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnType("nvarchar(200)");

                entity.Property(u => u.Avatar)
               .HasMaxLength(10)
               .HasColumnType("nvarchar(10)");

                entity.Property(u => u.AlternativeEmail)
               .HasMaxLength(200)
               .HasColumnType("nvarchar(200)");

                entity.Property(u => u.Email)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

                entity.Property(u => u.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

                entity.Property(u => u.MobileNumber)
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

                entity.Property(u => u.BioDetails)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

                entity.Property(u => u.Active)
                .HasDefaultValue(true);
            });


            modelBuilder.Entity<ContactNote>(entity =>
            {
                entity.Property(c => c.NoteText)
                .HasMaxLength(500)
                .IsRequired()
                .HasColumnType("nvarchar(500)");
                entity.Property(c => c.NoteDate)
                .IsRequired()
                .HasColumnType("date");
                entity.HasOne(d => d.UserContact)
                .WithMany(p => p.ContactNotes).HasForeignKey(d => d.UserContactId);
            });
        }
    }
}
