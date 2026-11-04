using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skyway2.Migrations
{
    /// <inheritdoc />
    public partial class AddingPassengerIdColumntoticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentStatuses_PaymentStatuses_PaymentStatusId",
                table: "PaymentStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Passengers_PassengerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_PaymentStatuses_PaymentStatusId",
                table: "PaymentStatuses");

            migrationBuilder.DropColumn(
                name: "PaymentStatusId",
                table: "PaymentStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengerId1",
                table: "Passengers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_PassengerId1",
                table: "Passengers",
                column: "PassengerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Passengers_PassengerId1",
                table: "Passengers",
                column: "PassengerId1",
                principalTable: "Passengers",
                principalColumn: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Passengers_PassengerId",
                table: "Tickets",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Passengers_PassengerId1",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Passengers_PassengerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_PassengerId1",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "PassengerId1",
                table: "Passengers");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusId",
                table: "PaymentStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_PaymentStatusId",
                table: "PaymentStatuses",
                column: "PaymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentStatuses_PaymentStatuses_PaymentStatusId",
                table: "PaymentStatuses",
                column: "PaymentStatusId",
                principalTable: "PaymentStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Passengers_PassengerId",
                table: "Tickets",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId");
        }
    }
}
