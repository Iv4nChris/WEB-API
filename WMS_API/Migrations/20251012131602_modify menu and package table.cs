using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API.Migrations
{
    /// <inheritdoc />
    public partial class modifymenuandpackagetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "FoodMenus");

            migrationBuilder.DropColumn(
                name: "PackageDescription",
                table: "FoodMenus");

            migrationBuilder.DropColumn(
                name: "isAvailable",
                table: "FoodMenus");

            migrationBuilder.AddColumn<bool>(
                name: "isAvailable",
                table: "FoodPackages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAvailable",
                table: "FoodPackages");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "FoodMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PackageDescription",
                table: "FoodMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isAvailable",
                table: "FoodMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
