using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ClothIdAllowNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Cloths_ClothId",
                table: "Shelves");

            migrationBuilder.DropIndex(
                name: "IX_Shelves_ClothId",
                table: "Shelves");

            migrationBuilder.AlterColumn<int>(
                name: "ClothId",
                table: "Shelves",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_ClothId",
                table: "Shelves",
                column: "ClothId",
                unique: true,
                filter: "[ClothId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Cloths_ClothId",
                table: "Shelves",
                column: "ClothId",
                principalTable: "Cloths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Cloths_ClothId",
                table: "Shelves");

            migrationBuilder.DropIndex(
                name: "IX_Shelves_ClothId",
                table: "Shelves");

            migrationBuilder.AlterColumn<int>(
                name: "ClothId",
                table: "Shelves",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_ClothId",
                table: "Shelves",
                column: "ClothId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Cloths_ClothId",
                table: "Shelves",
                column: "ClothId",
                principalTable: "Cloths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
