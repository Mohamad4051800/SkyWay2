using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skyway2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlightCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_City_CityId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Countries_CountryId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_CityId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DepartureCityId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DestinationCityId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureCityId",
                table: "Flights",
                column: "DepartureCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationCityId",
                table: "Flights",
                column: "DestinationCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_City_DepartureCityId",
                table: "Flights",
                column: "DepartureCityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_City_DestinationCityId",
                table: "Flights",
                column: "DestinationCityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Countries_CountryId",
                table: "Flights",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_City_DepartureCityId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_City_DestinationCityId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Countries_CountryId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DepartureCityId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DestinationCityId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DepartureCityId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DestinationCityId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CityId",
                table: "Flights",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_City_CityId",
                table: "Flights",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

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
