using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolfyBlog.API.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedCommentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_CommentId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentId",
                table: "Comments",
                newName: "IX_Comments_ParentCommentId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                newName: "IX_Comments_CommentId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bc6695-86bd-42e3-88a4-1d77edf17de2",
                column: "ConcurrencyStamp",
                value: "3f8a0d9e-1710-4baf-8001-bff5bf730bdb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6",
                column: "ConcurrencyStamp",
                value: "0fc5b0ce-19ce-4d9c-8014-8d1d3d6984cb");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_CommentId",
                table: "Comments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
