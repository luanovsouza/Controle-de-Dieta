using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDietaApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumed_Users_UserId",
                table: "Consumed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumed",
                table: "Consumed");

            migrationBuilder.RenameTable(
                name: "Consumed",
                newName: "MeatGoals");

            migrationBuilder.RenameIndex(
                name: "IX_Consumed_UserId",
                table: "MeatGoals",
                newName: "IX_MeatGoals_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeatGoals",
                table: "MeatGoals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeatGoals_Users_UserId",
                table: "MeatGoals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeatGoals_Users_UserId",
                table: "MeatGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeatGoals",
                table: "MeatGoals");

            migrationBuilder.RenameTable(
                name: "MeatGoals",
                newName: "Consumed");

            migrationBuilder.RenameIndex(
                name: "IX_MeatGoals_UserId",
                table: "Consumed",
                newName: "IX_Consumed_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumed",
                table: "Consumed",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumed_Users_UserId",
                table: "Consumed",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
