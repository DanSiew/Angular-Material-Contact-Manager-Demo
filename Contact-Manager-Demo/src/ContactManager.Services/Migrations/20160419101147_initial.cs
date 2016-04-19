using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace ContactManager.Services.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserContact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    AlternativeEmail = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    BioDetails = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContact", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "ContactNote",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteDate = table.Column<DateTime>(type: "date", nullable: false),
                    NoteText = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    UserContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNote_UserContact_UserContactId",
                        column: x => x.UserContactId,
                        principalTable: "UserContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ContactNote");
            migrationBuilder.DropTable("UserContact");
        }
    }
}
