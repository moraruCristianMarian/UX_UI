using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RestaurantManagerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "ReviewedObjectType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewedObjectType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ReviewedObjectTypeId = table.Column<int>(type: "integer", nullable: false),
                    ReviewedRestaurantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.CheckConstraint("CK_Poly_RestaurantOrProduct", "(\"ReviewedObjectTypeId\" = 1 AND \"ReviewedRestaurantId\" IS NOT NULL AND \"ReviewedProductId\" IS NULL) OR(\"ReviewedObjectTypeId\" = 2 AND \"ReviewedRestaurantId\" IS NULL AND \"ReviewedProductId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Review_Products_ReviewedProductId",
                        column: x => x.ReviewedProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_Restaurants_ReviewedRestaurantId",
                        column: x => x.ReviewedRestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_ReviewedObjectType_ReviewedObjectTypeId",
                        column: x => x.ReviewedObjectTypeId,
                        principalTable: "ReviewedObjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReviewedObjectTypeId",
                table: "Review",
                column: "ReviewedObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReviewedProductId",
                table: "Review",
                column: "ReviewedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReviewedRestaurantId",
                table: "Review",
                column: "ReviewedRestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "ReviewedObjectType");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers");
        }
    }
}
