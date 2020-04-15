using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepositoryId = table.Column<int>(nullable: false),
                    CustomExperience = table.Column<string>(nullable: true),
                    TextFormat = table.Column<int>(nullable: false),
                    LastEdit = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomExperienceImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(nullable: true),
                    AdditionalRepositoryDataId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomExperienceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomExperienceImages_AdditionalData_AdditionalRepositoryDataId",
                        column: x => x.AdditionalRepositoryDataId,
                        principalTable: "AdditionalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    RepositoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: true),
                    Readme = table.Column<string>(nullable: true),
                    AdditionalRepositoryDataId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.RepositoryId);
                    table.ForeignKey(
                        name: "FK_Repositories_AdditionalData_AdditionalRepositoryDataId",
                        column: x => x.AdditionalRepositoryDataId,
                        principalTable: "AdditionalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomExperienceImages_AdditionalRepositoryDataId",
                table: "CustomExperienceImages",
                column: "AdditionalRepositoryDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_AdditionalRepositoryDataId",
                table: "Repositories",
                column: "AdditionalRepositoryDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomExperienceImages");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "AdditionalData");
        }
    }
}
