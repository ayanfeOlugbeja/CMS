using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetExpiry",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetOtp",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "PasswordResetExpiry",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "PasswordResetOtp",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "ApplicationUsers");
        }
    }
}
