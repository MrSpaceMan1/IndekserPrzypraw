using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IndekserPrzypraw.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                    Name = table.Column<string>(type: "text", nullable: false)
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
                    Barcode = table.Column<string>(type: "text", nullable: false),
                    Grams = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MinimumCount = table.Column<long>(type: "bigint", nullable: true),
                    MinimumGrams = table.Column<long>(type: "bigint", nullable: true),
                    DrawerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiceGroups", x => x.SpiceGroupId);
                    table.ForeignKey(
                        name: "FK_SpiceGroups_Drawers_DrawerId",
                        column: x => x.DrawerId,
                        principalTable: "Drawers",
                        principalColumn: "DrawerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spices",
                columns: table => new
                {
                    SpiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpiceGroupId = table.Column<int>(type: "integer", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spices", x => x.SpiceId);
                    table.ForeignKey(
                        name: "FK_Spices_SpiceGroups_SpiceGroupId",
                        column: x => x.SpiceGroupId,
                        principalTable: "SpiceGroups",
                        principalColumn: "SpiceGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpiceGroups_DrawerId",
                table: "SpiceGroups",
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
                name: "SpiceGroups");

            migrationBuilder.DropTable(
                name: "Drawers");
        }
    }
}
