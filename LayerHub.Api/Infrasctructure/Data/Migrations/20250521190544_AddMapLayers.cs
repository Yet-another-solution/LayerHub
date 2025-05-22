using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayerHub.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMapLayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapLayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Layer = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapLayers_Tenants_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapFeatureLayers",
                columns: table => new
                {
                    MapFeatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    MapLayerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapFeatureLayers", x => new { x.MapFeatureId, x.MapLayerId });
                    table.ForeignKey(
                        name: "FK_MapFeatureLayers_MapFeatures_MapFeatureId",
                        column: x => x.MapFeatureId,
                        principalTable: "MapFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapFeatureLayers_MapLayers_MapLayerId",
                        column: x => x.MapLayerId,
                        principalTable: "MapLayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapFeatureLayers_MapLayerId",
                table: "MapFeatureLayers",
                column: "MapLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayers_OwnerId",
                table: "MapLayers",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapFeatureLayers");

            migrationBuilder.DropTable(
                name: "MapLayers");
        }
    }
}
