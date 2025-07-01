using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

#nullable disable

namespace TrailFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddZDimensionToGeometries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:difficulty_level", "easy,moderate,hard,extreme,unknown")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "trails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    distance = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    elevation_gain = table.Column<double>(type: "double precision", nullable: false),
                    difficulty_level = table.Column<DifficultyLevel>(type: "difficulty_level", nullable: true),
                    start_point = table.Column<Point>(type: "geometry(PointZ, 4326)", nullable: true),
                    end_point = table.Column<Point>(type: "geometry(PointZ, 4326)", nullable: true),
                    route_geom = table.Column<LineString>(type: "geometry(LineStringZ, 4326)", nullable: true),
                    web_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    has_gpx = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trails", x => x.id);
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_trails_route_geom",
                table: "trails",
                column: "route_geom")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_trails_slug",
                table: "trails",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trails_start_point",
                table: "trails",
                column: "start_point")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_trails_user_id",
                table: "trails",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trails");
        }
    }
}
