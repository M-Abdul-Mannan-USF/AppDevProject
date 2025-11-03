using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsBiasChecker.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsStories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Authors = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsStories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsStories_Url",
                table: "NewsStories",
                column: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsStories");
        }
    }
}
