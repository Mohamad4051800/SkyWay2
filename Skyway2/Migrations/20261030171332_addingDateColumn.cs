using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skyway2.Migrations
{
    /// <inheritdoc />
    public partial class addingDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Tickets",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusId",
                table: "PaymentStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PaymentId",
                table: "Tickets",
                column: "PaymentId");

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
                name: "FK_Tickets_PaymentStatuses_PaymentId",
                table: "Tickets",
                column: "PaymentId",
                principalTable: "PaymentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentStatuses_PaymentStatuses_PaymentStatusId",
                table: "PaymentStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_PaymentStatuses_PaymentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PaymentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_PaymentStatuses_PaymentStatusId",
                table: "PaymentStatuses");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PaymentStatusId",
                table: "PaymentStatuses");
        }
    }
}
