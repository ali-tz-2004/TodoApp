using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Categories_CategoryId",
                schema: "Todo",
                table: "TodoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Users_UserId",
                schema: "Todo",
                table: "TodoItems");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Todo",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                schema: "Todo",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserId",
                schema: "Todo",
                table: "Categories",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Categories_CategoryId",
                schema: "Todo",
                table: "TodoItems",
                column: "CategoryId",
                principalSchema: "Todo",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Users_UserId",
                schema: "Todo",
                table: "TodoItems",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UserId",
                schema: "Todo",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Categories_CategoryId",
                schema: "Todo",
                table: "TodoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Users_UserId",
                schema: "Todo",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                schema: "Todo",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Todo",
                table: "Categories");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Categories_CategoryId",
                schema: "Todo",
                table: "TodoItems",
                column: "CategoryId",
                principalSchema: "Todo",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Users_UserId",
                schema: "Todo",
                table: "TodoItems",
                column: "UserId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
