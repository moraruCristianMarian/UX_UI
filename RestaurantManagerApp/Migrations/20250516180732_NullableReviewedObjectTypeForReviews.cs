using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagerApp.Migrations
{
    /// <inheritdoc />
    public partial class NullableReviewedObjectTypeForReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Products_ReviewedProductId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Restaurants_ReviewedRestaurantId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_ReviewedObjectType_ReviewedObjectTypeId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ReviewedRestaurantId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewedRestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ReviewedProductId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewedProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ReviewedObjectTypeId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewedObjectTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ReviewedProductId",
                table: "Reviews",
                column: "ReviewedProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_ReviewedRestaurantId",
                table: "Reviews",
                column: "ReviewedRestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewedObjectType_ReviewedObjectTypeId",
                table: "Reviews",
                column: "ReviewedObjectTypeId",
                principalTable: "ReviewedObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ReviewedProductId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_ReviewedRestaurantId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewedObjectType_ReviewedObjectTypeId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewedRestaurantId",
                table: "Review",
                newName: "IX_Review_ReviewedRestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewedProductId",
                table: "Review",
                newName: "IX_Review_ReviewedProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewedObjectTypeId",
                table: "Review",
                newName: "IX_Review_ReviewedObjectTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Products_ReviewedProductId",
                table: "Review",
                column: "ReviewedProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Restaurants_ReviewedRestaurantId",
                table: "Review",
                column: "ReviewedRestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ReviewedObjectType_ReviewedObjectTypeId",
                table: "Review",
                column: "ReviewedObjectTypeId",
                principalTable: "ReviewedObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
