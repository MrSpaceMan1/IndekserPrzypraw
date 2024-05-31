using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndekserPrzypraw.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteBehaviorForDrawer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpiceGroups_Drawers_DrawerId",
                table: "SpiceGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_SpiceGroups_Drawers_DrawerId",
                table: "SpiceGroups",
                column: "DrawerId",
                principalTable: "Drawers",
                principalColumn: "DrawerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpiceGroups_Drawers_DrawerId",
                table: "SpiceGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_SpiceGroups_Drawers_DrawerId",
                table: "SpiceGroups",
                column: "DrawerId",
                principalTable: "Drawers",
                principalColumn: "DrawerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
