using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IndekserPrzypraw.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drawers",
                columns: table => new
                {
                    DrawerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drawers", x => x.DrawerId);
                });

            migrationBuilder.CreateTable(
                name: "SpiceGroups",
                columns: table => new
                {
                    SpiceGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MinimumCount = table.Column<long>(type: "bigint", nullable: false),
                    MinimumGrams = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiceGroups", x => x.SpiceGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Spices",
                columns: table => new
                {
                    SpiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpiceGroupId = table.Column<int>(type: "integer", nullable: false),
                    DrawerId = table.Column<int>(type: "integer", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Grams = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spices", x => x.SpiceId);
                    table.ForeignKey(
                        name: "FK_Spices_Drawers_DrawerId",
                        column: x => x.DrawerId,
                        principalTable: "Drawers",
                        principalColumn: "DrawerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spices_SpiceGroups_SpiceGroupId",
                        column: x => x.SpiceGroupId,
                        principalTable: "SpiceGroups",
                        principalColumn: "SpiceGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spices_DrawerId",
                table: "Spices",
                column: "DrawerId");

            migrationBuilder.CreateIndex(
                name: "IX_Spices_SpiceGroupId",
                table: "Spices",
                column: "SpiceGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spices");

            migrationBuilder.DropTable(
                name: "Drawers");

            migrationBuilder.DropTable(
                name: "SpiceGroups");
        }
    }
}
