using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayerHub.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMapProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    VisibleStart = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    VisibleEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapProjects_Tenants_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapProjectLayers",
                columns: table => new
                {
                    MapProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    MapLayerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapProjectLayers", x => new { x.MapProjectId, x.MapLayerId });
                    table.ForeignKey(
                        name: "FK_MapProjectLayers_MapLayers_MapLayerId",
                        column: x => x.MapLayerId,
                        principalTable: "MapLayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapProjectLayers_MapProjects_MapProjectId",
                        column: x => x.MapProjectId,
                        principalTable: "MapProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapProjectLayers_MapLayerId",
                table: "MapProjectLayers",
                column: "MapLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MapProjects_OwnerId",
                table: "MapProjects",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapProjectLayers");

            migrationBuilder.DropTable(
                name: "MapProjects");
        }
    }
}
