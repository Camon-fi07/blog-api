using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace weblog_API.Migrations
{
    /// <inheritdoc />
    public partial class addtags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { new Guid("02b7fa9e-8ec1-4e8a-a9fe-c7acb650b818"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1214), "косплей" },
                    { new Guid("08d8de3c-4fe6-44d0-96d9-553053b0bf40"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1210), "теория_заговора" },
                    { new Guid("32f1aff6-0940-4c9c-939d-ee7571fc67cd"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1193), "приколы" },
                    { new Guid("36d37a9b-e25b-4261-b556-a06ed2ac81f0"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1195), "it" },
                    { new Guid("611f5b6e-8c1b-45a8-904b-13a58f0a97a7"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1212), "соцсети" },
                    { new Guid("8ecd3b0d-7a60-4d12-8e0e-3953c7a0adda"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1216), "преступление" },
                    { new Guid("a9f82a79-8501-4dea-96de-300775ea54aa"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1185), "история" },
                    { new Guid("b17ae7cc-a0b4-4161-be69-c7f4ceee3a05"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1208), "интернет" },
                    { new Guid("e76bd965-6159-42cf-b521-e4bf2356f327"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1191), "18+" },
                    { new Guid("fca8f8d9-f386-41c8-ac14-be2660378054"), new DateTime(2023, 12, 2, 14, 11, 12, 62, DateTimeKind.Utc).AddTicks(1189), "еда" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
