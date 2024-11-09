using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.Migrations
{
    /// <inheritdoc />
    public partial class changedelmethod20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DeliveryMethods");

            migrationBuilder.DropColumn(
                name: "EstimatedDeliveryTime",
                table: "DeliveryMethods");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DeliveryMethods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DeliveryMethods",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "EstimatedDeliveryTime",
                table: "DeliveryMethods",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "DeliveryMethods",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
