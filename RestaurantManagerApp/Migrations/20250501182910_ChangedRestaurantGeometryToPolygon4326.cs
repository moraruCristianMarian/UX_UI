using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace RestaurantManagerApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRestaurantGeometryToPolygon4326 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Polygon>(
                name: "Geom",
                table: "Restaurants",
                type: "geometry(Polygon, 4326)",
                nullable: false,
                oldClrType: typeof(Geometry),
                oldType: "geometry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Geometry>(
                name: "Geom",
                table: "Restaurants",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon, 4326)");
        }
    }
}
