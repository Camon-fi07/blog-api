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
                    Image = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    Content = table.Column<string>(type: "text", nullable: false),
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
                    { new Guid("09d1f148-7738-4b12-b3dc-50055c6c6bed"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7436), "it" },
                    { new Guid("1b1e7268-21fa-494a-9bd7-c3f4241172cc"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7440), "теория_заговора" },
                    { new Guid("36c03030-9d64-434f-b587-a1a5b4f91283"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7430), "еда" },
                    { new Guid("7bf8179f-f014-49ff-aa3c-e67d67bf4842"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7456), "косплей" },
                    { new Guid("7f275b82-4964-49ac-a988-42f5e955cd3a"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7454), "соцсети" },
                    { new Guid("98fde087-83b5-477b-94a2-673966aa3987"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7438), "интернет" },
                    { new Guid("9b38ee20-0c0c-4b78-8f05-e8be6a9dd5c2"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7434), "приколы" },
                    { new Guid("c4ecf99f-6255-4392-8b4c-f38598bfcde8"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7432), "18+" },
                    { new Guid("ca01652d-b566-4f3b-a792-3f821f8ed413"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7426), "история" },
                    { new Guid("f70859a7-8ba5-4be7-8315-d24279e3a50f"), new DateTime(2023, 12, 3, 3, 50, 23, 942, DateTimeKind.Utc).AddTicks(7458), "преступление" }
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
                keyValue: new Guid("09d1f148-7738-4b12-b3dc-50055c6c6bed"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("1b1e7268-21fa-494a-9bd7-c3f4241172cc"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("36c03030-9d64-434f-b587-a1a5b4f91283"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7bf8179f-f014-49ff-aa3c-e67d67bf4842"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7f275b82-4964-49ac-a988-42f5e955cd3a"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("98fde087-83b5-477b-94a2-673966aa3987"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("9b38ee20-0c0c-4b78-8f05-e8be6a9dd5c2"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c4ecf99f-6255-4392-8b4c-f38598bfcde8"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("ca01652d-b566-4f3b-a792-3f821f8ed413"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("f70859a7-8ba5-4be7-8315-d24279e3a50f"));

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
