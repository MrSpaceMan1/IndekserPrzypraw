using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndekserPrzypraw.Migrations
{
    /// <inheritdoc />
    public partial class IngredientNoTiesWithSpiceGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_SpiceGroups_SpiceGroupId",
                table: "Ingredient");

            migrationBuilder.DropIndex(
                name: "IX_Ingredient_SpiceGroupId",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "SpiceGroupId",
                table: "Ingredient");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Ingredient",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Ingredient");

            migrationBuilder.AddColumn<int>(
                name: "SpiceGroupId",
                table: "Ingredient",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_SpiceGroupId",
                table: "Ingredient",
                column: "SpiceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_SpiceGroups_SpiceGroupId",
                table: "Ingredient",
                column: "SpiceGroupId",
                principalTable: "SpiceGroups",
                principalColumn: "SpiceGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
