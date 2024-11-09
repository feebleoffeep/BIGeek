using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.Migrations
{
    /// <inheritdoc />
    public partial class changeOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DeliveryPrice",
                table: "Orders",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryTime",
                table: "Orders",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "Orders");
        }
    }
}
