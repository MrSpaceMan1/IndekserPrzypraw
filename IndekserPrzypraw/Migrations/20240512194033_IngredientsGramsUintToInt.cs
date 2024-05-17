using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IndekserPrzypraw.Migrations
{
    /// <inheritdoc />
    public partial class IngredientsGramsUintToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpiceMixRecipes",
                columns: table => new
                {
                    SpiceMixRecipeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiceMixRecipes", x => x.SpiceMixRecipeId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpiceGroupId = table.Column<int>(type: "integer", nullable: false),
                    Grams = table.Column<int>(type: "integer", nullable: false),
                    SpiceMixRecipeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredient_SpiceGroups_SpiceGroupId",
                        column: x => x.SpiceGroupId,
                        principalTable: "SpiceGroups",
                        principalColumn: "SpiceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingredient_SpiceMixRecipes_SpiceMixRecipeId",
                        column: x => x.SpiceMixRecipeId,
                        principalTable: "SpiceMixRecipes",
                        principalColumn: "SpiceMixRecipeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_SpiceGroupId",
                table: "Ingredient",
                column: "SpiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_SpiceMixRecipeId",
                table: "Ingredient",
                column: "SpiceMixRecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "SpiceMixRecipes");
        }
    }
}
