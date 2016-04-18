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
