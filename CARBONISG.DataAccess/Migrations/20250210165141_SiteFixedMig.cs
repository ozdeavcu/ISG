using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARBONISG.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SiteFixedMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SiteFixedInformation",
                columns: new[] { "ID", "Address", "ContactNumber", "Email", "Facebook", "FacebookIcon", "Instagram", "InstagramIcon", "Linkedln", "LinkedlnIcon", "LogoUrl", "Twitter", "TwitterIcon", "UpdateDate", "WhatsappIcon", "WhatsappPhone", "Youtube", "YoutubeIcon" },
                values: new object[] { 1, "Varsayılan Adres", "111111", "info@example.com", "https://facebook.com/example", null, "https://instagram.com/example", null, "https://linkedin.com/company/example", null, "/images/default-logo.png", "https://twitter.com/example", null, null, null, null, "https://youtube.com/example", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 10, 19, 51, 41, 396, DateTimeKind.Local).AddTicks(4255));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SiteFixedInformation",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 10, 19, 46, 34, 115, DateTimeKind.Local).AddTicks(8183));
        }
    }
}
