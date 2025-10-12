using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_API.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    FoodCategoriesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.FoodCategoriesId);
                });

            migrationBuilder.CreateTable(
                name: "FoodPackages",
                columns: table => new
                {
                    FoodPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodPackages", x => x.FoodPackageId);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    FoodsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodCategoriesId = table.Column<int>(type: "int", nullable: false),
                    FoodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.FoodsId);
                    table.ForeignKey(
                        name: "FK_Foods_FoodCategories_FoodCategoriesId",
                        column: x => x.FoodCategoriesId,
                        principalTable: "FoodCategories",
                        principalColumn: "FoodCategoriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodMenus",
                columns: table => new
                {
                    FoodMenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodPackageId = table.Column<int>(type: "int", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    PackageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    FoodsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodMenus", x => x.FoodMenuId);
                    table.ForeignKey(
                        name: "FK_FoodMenus_FoodPackages_FoodPackageId",
                        column: x => x.FoodPackageId,
                        principalTable: "FoodPackages",
                        principalColumn: "FoodPackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodMenus_Foods_FoodsId",
                        column: x => x.FoodsId,
                        principalTable: "Foods",
                        principalColumn: "FoodsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodMenus_FoodPackageId",
                table: "FoodMenus",
                column: "FoodPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodMenus_FoodsId",
                table: "FoodMenus",
                column: "FoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_FoodCategoriesId",
                table: "Foods",
                column: "FoodCategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodMenus");

            migrationBuilder.DropTable(
                name: "FoodPackages");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "FoodCategories");
        }
    }
}
