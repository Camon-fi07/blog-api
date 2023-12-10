using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace weblog_API.Migrations
{
    /// <inheritdoc />
    public partial class fix_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("0a9cc0e7-4674-46a5-99d5-39ff974e9ad8"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("178ec05c-a831-4244-b55b-b68a135d3a34"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("5a0322be-2d5c-47b7-9fcf-f277145a01d3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("77c53869-ffab-42da-afe3-d7ea61ef3b17"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7d0aa767-ad15-4539-80a7-0444045d4cab"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8989e85a-1844-4de6-bf2c-e2027c72d351"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("b3325476-f7f5-4dcd-a5a0-2585f991aa03"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("cd85ec5e-5455-46dd-950b-fef25ddd9098"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("f2766be0-1d7c-4040-8ed9-131b1654251b"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("fe8b52d3-5789-4b74-8244-5dcd478d89a8"));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Users",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Communities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { new Guid("10304fd4-6392-4f3d-880c-0412c0c077ce"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7402), "история" },
                    { new Guid("1eb27543-bce6-4bb7-895a-88080bf91022"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7419), "теория_заговора" },
                    { new Guid("2b191385-6fb4-4e83-8792-0826fddb2e77"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7411), "приколы" },
                    { new Guid("2ec7f29e-c757-477f-b146-d35f5230a3d4"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7413), "it" },
                    { new Guid("571f043e-7cfe-4efa-96d7-b41c22954de7"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7421), "соцсети" },
                    { new Guid("60de4ae7-39be-42ee-9060-3ece4ad5ce0b"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7427), "косплей" },
                    { new Guid("8122feab-1fbc-4ddf-b116-06639840c3c3"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7409), "18+" },
                    { new Guid("8f82ec7b-38f9-4f97-98ae-dd4037ce3980"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7429), "преступление" },
                    { new Guid("a4564a27-5d19-48a3-ae7b-96f004a459fd"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7406), "еда" },
                    { new Guid("f85252d3-5dd4-4e20-874f-63d5ab3880e4"), new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7416), "интернет" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("10304fd4-6392-4f3d-880c-0412c0c077ce"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("1eb27543-bce6-4bb7-895a-88080bf91022"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2b191385-6fb4-4e83-8792-0826fddb2e77"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2ec7f29e-c757-477f-b146-d35f5230a3d4"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("571f043e-7cfe-4efa-96d7-b41c22954de7"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("60de4ae7-39be-42ee-9060-3ece4ad5ce0b"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8122feab-1fbc-4ddf-b116-06639840c3c3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8f82ec7b-38f9-4f97-98ae-dd4037ce3980"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a4564a27-5d19-48a3-ae7b-96f004a459fd"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("f85252d3-5dd4-4e20-874f-63d5ab3880e4"));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Communities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { new Guid("0a9cc0e7-4674-46a5-99d5-39ff974e9ad8"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(817), "еда" },
                    { new Guid("178ec05c-a831-4244-b55b-b68a135d3a34"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(881), "приколы" },
                    { new Guid("5a0322be-2d5c-47b7-9fcf-f277145a01d3"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(902), "теория_заговора" },
                    { new Guid("77c53869-ffab-42da-afe3-d7ea61ef3b17"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(907), "косплей" },
                    { new Guid("7d0aa767-ad15-4539-80a7-0444045d4cab"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(812), "история" },
                    { new Guid("8989e85a-1844-4de6-bf2c-e2027c72d351"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(905), "соцсети" },
                    { new Guid("b3325476-f7f5-4dcd-a5a0-2585f991aa03"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(900), "интернет" },
                    { new Guid("cd85ec5e-5455-46dd-950b-fef25ddd9098"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(884), "it" },
                    { new Guid("f2766be0-1d7c-4040-8ed9-131b1654251b"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(910), "преступление" },
                    { new Guid("fe8b52d3-5789-4b74-8244-5dcd478d89a8"), new DateTime(2023, 12, 3, 5, 44, 19, 884, DateTimeKind.Utc).AddTicks(820), "18+" }
                });
        }
    }
}
