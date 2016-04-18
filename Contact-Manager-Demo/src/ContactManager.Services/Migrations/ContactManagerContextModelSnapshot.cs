using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ContactManager.Services.Infrastructure;

namespace ContactManager.Services.Migrations
{
    [DbContext(typeof(ContactManagerContext))]
    partial class ContactManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ContactManager.Services.Models.ContactNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("NoteDate")
                        .HasAnnotation("Relational:ColumnType", "date");

                    b.Property<string>("NoteText")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(500)");

                    b.Property<int>("UserContactId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ContactManager.Services.Models.UserContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlternativeEmail");

                    b.Property<string>("Avatar");

                    b.Property<string>("BioDetails");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ContactManager.Services.Models.ContactNote", b =>
                {
                    b.HasOne("ContactManager.Services.Models.UserContact")
                        .WithMany()
                        .HasForeignKey("UserContactId");
                });
        }
    }
}
