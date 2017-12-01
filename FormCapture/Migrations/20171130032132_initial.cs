using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FormCapture.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicantId = table.Column<int>(nullable: false),
                    Attitude = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Preparation = table.Column<int>(nullable: false),
                    Presentation = table.Column<int>(nullable: false),
                    Recommend = table.Column<bool>(nullable: false),
                    RecommendedPosition = table.Column<string>(nullable: true),
                    Spirit = table.Column<int>(nullable: false),
                    Team = table.Column<string>(nullable: true),
                    Understanding = table.Column<int>(nullable: false),
                    Uniform = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "Interviews");
        }
    }
}
