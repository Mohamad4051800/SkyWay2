using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skyway2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFlightCountryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Countries_CountryId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_CountryId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Flights");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Countries_CountryId",
                table: "Flights",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
