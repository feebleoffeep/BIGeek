using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _2.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cities_CityId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, 49.421599999999998, 26.996500000000001, "Хмельницький" },
                    { 2, 50.450099999999999, 30.523399999999999, "Київ" },
                    { 3, 49.839700000000001, 24.029699999999998, "Львів" },
                    { 4, 46.482500000000002, 30.723299999999998, "Одеса" },
                    { 5, 49.993499999999997, 36.230400000000003, "Харків" },
                    { 6, 48.464700000000001, 35.046199999999999, "Дніпро" },
                    { 7, 47.838799999999999, 35.139600000000002, "Запоріжжя" },
                    { 8, 49.232799999999997, 28.481000000000002, "Вінниця" },
                    { 9, 49.588299999999997, 34.551400000000001, "Полтава" },
                    { 10, 49.444400000000002, 32.059800000000003, "Черкаси" },
                    { 11, 48.290799999999997, 25.9346, "Чернівці" },
                    { 12, 51.498199999999997, 31.289300000000001, "Чернігів" },
                    { 13, 50.619900000000001, 26.2516, "Рівне" },
                    { 14, 48.922600000000003, 24.711099999999998, "Івано-Франківськ" },
                    { 15, 49.5535, 25.594799999999999, "Тернопіль" },
                    { 16, 50.2547, 28.6587, "Житомир" },
                    { 17, 48.620800000000003, 22.2879, "Ужгород" },
                    { 18, 50.907699999999998, 34.798099999999998, "Суми" },
                    { 19, 48.9482, 38.492400000000004, "Сєвєродонецьк" },
                    { 20, 48.722999999999999, 37.5563, "Краматорськ" },
                    { 21, 48.507899999999999, 32.262300000000003, "Кропивницький" },
                    { 22, 46.975000000000001, 31.994599999999998, "Миколаїв" },
                    { 23, 46.635399999999997, 32.616900000000001, "Херсон" },
                    { 24, 50.747199999999999, 25.325399999999998, "Луцьк" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CityId",
                table: "Orders",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cities_CityId",
                table: "Orders",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
