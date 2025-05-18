using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace RestaurantManagerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedOpenTimesAndRatingRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Restaurants");

            migrationBuilder.AlterColumn<Polygon>(
                name: "Geom",
                table: "Restaurants",
                type: "geometry(Polygon, 4326)",
                nullable: true,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon, 4326)");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingTime",
                table: "Restaurants",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningTime",
                table: "Restaurants",
                type: "interval",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PromotionEndDate",
                table: "MenuProducts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Restaurants");

            migrationBuilder.AlterColumn<Polygon>(
                name: "Geom",
                table: "Restaurants",
                type: "geometry(Polygon, 4326)",
                nullable: false,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon, 4326)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Restaurants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PromotionEndDate",
                table: "MenuProducts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
