using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagerApp.Migrations
{
    /// <inheritdoc />
    public partial class LimitedMenuProductDiscountTo0Through1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"MenuProducts\" ADD CONSTRAINT \"CK_MenuProducts_DiscountRange\" CHECK (\"Discount\" >= 0.0 AND \"Discount\" <= 1.0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"MenuProducts\" DROP CONSTRAINT \"CK_MenuProducts_DiscountRange\"");
        }
    }
}
