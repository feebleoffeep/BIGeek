using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BatteryCapacity",
                table: "Products",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Products",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraphicsType",
                table: "Products",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "Products",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RamSize",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ScreenDiagonal",
                table: "Products",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenResolution",
                table: "Products",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StorageSize",
                table: "Products",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatteryCapacity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GraphicsType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Processor",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RamSize",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ScreenDiagonal",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ScreenResolution",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StorageSize",
                table: "Products");
        }
    }
}
