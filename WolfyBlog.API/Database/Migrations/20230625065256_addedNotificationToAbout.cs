using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfyBlog.API.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedNotificationToAbout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotification",
                table: "AboutPage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bc6695-86bd-42e3-88a4-1d77edf17de2",
                column: "ConcurrencyStamp",
                value: "6cd8db51-4cc6-4706-9f23-141ac716f8b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6",
                column: "ConcurrencyStamp",
                value: "6d7a0c1a-638b-49c3-9901-ea4eb068c75d");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotification",
                table: "AboutPage");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bc6695-86bd-42e3-88a4-1d77edf17de2",
                column: "ConcurrencyStamp",
                value: "17c35940-abdf-4d7a-95d7-7385e0379b22");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6",
                column: "ConcurrencyStamp",
                value: "9b3ecbcd-ae21-486d-86d0-05419b78bbc4");
        }
    }
}
