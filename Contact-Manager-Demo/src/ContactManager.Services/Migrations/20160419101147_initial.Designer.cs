using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ContactManager.Services.Infrastructure;

namespace ContactManager.Services.Migrations
{
    [DbContext(typeof(ContactManagerContext))]
    [Migration("20160419101147_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<bool>("Active")
                        .HasAnnotation("Relational:DefaultValue", "True")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<string>("AlternativeEmail")
                        .HasAnnotation("MaxLength", 200)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("Avatar")
                        .HasAnnotation("MaxLength", 10)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(10)");

                    b.Property<string>("BioDetails")
                        .HasAnnotation("MaxLength", 500)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(500)");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 200)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("MobileNumber")
                        .HasAnnotation("MaxLength", 20)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(20)");

                    b.Property<string>("PhoneNumber")
                        .HasAnnotation("MaxLength", 20)
                        .HasAnnotation("Relational:ColumnType", "nvarchar(20)");

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
