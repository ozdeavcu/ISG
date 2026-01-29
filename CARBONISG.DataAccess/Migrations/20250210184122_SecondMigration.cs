using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARBONISG.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DateHired" },
                values: new object[] { new DateTime(2025, 2, 10, 21, 41, 22, 140, DateTimeKind.Local).AddTicks(5536), new DateTime(2025, 2, 10, 21, 41, 22, 140, DateTimeKind.Local).AddTicks(5515) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DateHired" },
                values: new object[] { new DateTime(2025, 2, 10, 19, 51, 41, 396, DateTimeKind.Local).AddTicks(4255), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
