using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace weblog_API.Migrations
{
    /// <inheritdoc />
    public partial class Add_posts_and_comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("02b7fa9e-8ec1-4e8a-a9fe-c7acb650b818"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("08d8de3c-4fe6-44d0-96d9-553053b0bf40"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("32f1aff6-0940-4c9c-939d-ee7571fc67cd"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("36d37a9b-e25b-4261-b556-a06ed2ac81f0"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("611f5b6e-8c1b-45a8-904b-13a58f0a97a7"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8ecd3b0d-7a60-4d12-8e0e-3953c7a0adda"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a9f82a79-8501-4dea-96de-300775ea54aa"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("b17ae7cc-a0b4-4161-be69-c7f4ceee3a05"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e76bd965-6159-42cf-b521-e4bf2356f327"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("fca8f8d9-f386-41c8-ac14-be2660378054"));

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReadingTime = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikedPostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersLikedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.LikedPostsId, x.UsersLikedId });
                    table.ForeignKey(
                        name: "FK_Likes_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UsersLikedId",
                        column: x => x.UsersLikedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { new Guid("1551b909-15f8-4b5e-9511-470ccd8cf0a9"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5858), "еда" },
                    { new Guid("17a6107b-1b2f-45a3-92ca-9586d204751e"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5864), "приколы" },
                    { new Guid("37ae3bac-f424-4f13-8581-ec59e40ada74"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5896), "соцсети" },
                    { new Guid("410a67b1-1a76-4bf8-a096-4c059aa4bd4e"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5850), "история" },
                    { new Guid("7651dbbf-4954-4754-8c34-aa7774127106"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5902), "преступление" },
                    { new Guid("765ab790-3098-46ac-9808-38040e53400f"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5893), "теория_заговора" },
                    { new Guid("7996188f-9417-45a3-8b7f-5f6a1c36652e"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5861), "18+" },
                    { new Guid("8272d8a3-d2a4-458e-8977-35c10fb2a9e8"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5899), "косплей" },
                    { new Guid("9678b4ed-b8c9-45fd-87eb-5c322db929fc"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5886), "it" },
                    { new Guid("b4d3c107-7740-4070-86b4-4845a20d28b3"), new DateTime(2023, 12, 2, 17, 25, 12, 437, DateTimeKind.Utc).AddTicks(5890), "интернет" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UsersLikedId",
                table: "Likes",
                column: "UsersLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CommunityId",
                table: "Posts",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("1551b909-15f8-4b5e-9511-470ccd8cf0a9"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("17a6107b-1b2f-45a3-92ca-9586d204751e"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("37ae3bac-f424-4f13-8581-ec59e40ada74"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("410a67b1-1a76-4bf8-a096-4c059aa4bd4e"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7651dbbf-4954-4754-8c34-aa7774127106"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("765ab790-3098-46ac-9808-38040e53400f"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7996188f-9417-45a3-8b7f-5f6a1c36652e"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8272d8a3-d2a4-458e-8977-35c10fb2a9e8"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("9678b4ed-b8c9-45fd-87eb-5c322db929fc"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("b4d3c107-7740-4070-86b4-4845a20d28b3"));

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
    }
}
