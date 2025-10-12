using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API.Migrations
{
    /// <inheritdoc />
    public partial class addcolumninPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageDescription",
                table: "FoodPackages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageDescription",
                table: "FoodPackages");
        }
    }
}
