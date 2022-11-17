using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WolfyBlog.API.Database.Migrations
{
    /// <inheritdoc />
    public partial class ArticleUserDataSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61bc6695-86bd-42e3-88a4-1d77edf17de2", "00d2a329-3733-4494-b5b1-d63425133975", "Guest", "GUEST" },
                    { "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6", "eae34bfa-aef5-46f8-88fb-e367421d525e", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bc6695-86bd-42e3-88a4-1d77edf17de2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6");
        }
    }
}
