using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace csiro_mvc.Migrations
{
    /// <inheritdoc />
    public partial class SeedResearchPrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ResearchPrograms",
                columns: new[] { "Id", "Description", "Name", "OpenPositions" },
                values: new object[,]
                {
                    { 1, "Digital and data innovation for Australia's digital future", "Data61", 5 },
                    { 2, "Unlocking the secrets of the universe and supporting Australia's space industry", "Space and Astronomy", 3 },
                    { 3, "Developing sustainable energy solutions for a cleaner future", "Energy", 4 },
                    { 4, "Advanced manufacturing technologies and processes", "Manufacturing", 2 },
                    { 5, "Improving health outcomes and protecting Australia's biosecurity", "Health and Biosecurity", 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ResearchPrograms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ResearchPrograms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ResearchPrograms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ResearchPrograms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ResearchPrograms",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
