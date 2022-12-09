using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfyBlog.API.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bc6695-86bd-42e3-88a4-1d77edf17de2",
                column: "ConcurrencyStamp",
                value: "f5d13411-d09e-4bd6-ae13-481ae751c6e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6",
                column: "ConcurrencyStamp",
                value: "263b94b2-f9af-403c-832f-f015a9f4e736");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Articles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bc6695-86bd-42e3-88a4-1d77edf17de2",
                column: "ConcurrencyStamp",
                value: "1b6dafd5-5165-4048-9dcd-970004b6cbd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6",
                column: "ConcurrencyStamp",
                value: "77ff09fe-b9dd-4368-ba9a-d5af7e0e88ec");
        }
    }
}
