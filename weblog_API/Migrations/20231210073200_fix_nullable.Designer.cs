﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using weblog_API.Data;

#nullable disable

namespace weblog_API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231210073200_fix_nullable")]
    partial class fix_nullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Likes", b =>
                {
                    b.Property<Guid>("LikedPostsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersLikedId")
                        .HasColumnType("uuid");

                    b.HasKey("LikedPostsId", "UsersLikedId");

                    b.HasIndex("UsersLikedId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.Property<Guid>("PostsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid");

                    b.HasKey("PostsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("weblog_API.Models.Comment.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("weblog_API.Models.Community.Community", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Communities");
                });

            modelBuilder.Entity("weblog_API.Models.Community.UserCommunity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommunityId")
                        .HasColumnType("uuid");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CommunityId");

                    b.HasIndex("CommunityId");

                    b.ToTable("UserCommunities");
                });

            modelBuilder.Entity("weblog_API.Models.Post.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CommunityId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<int>("ReadingTime")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CommunityId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("weblog_API.Models.Tags.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = new Guid("10304fd4-6392-4f3d-880c-0412c0c077ce"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7402),
                            Name = "история"
                        },
                        new
                        {
                            Id = new Guid("a4564a27-5d19-48a3-ae7b-96f004a459fd"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7406),
                            Name = "еда"
                        },
                        new
                        {
                            Id = new Guid("8122feab-1fbc-4ddf-b116-06639840c3c3"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7409),
                            Name = "18+"
                        },
                        new
                        {
                            Id = new Guid("2b191385-6fb4-4e83-8792-0826fddb2e77"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7411),
                            Name = "приколы"
                        },
                        new
                        {
                            Id = new Guid("2ec7f29e-c757-477f-b146-d35f5230a3d4"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7413),
                            Name = "it"
                        },
                        new
                        {
                            Id = new Guid("f85252d3-5dd4-4e20-874f-63d5ab3880e4"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7416),
                            Name = "интернет"
                        },
                        new
                        {
                            Id = new Guid("1eb27543-bce6-4bb7-895a-88080bf91022"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7419),
                            Name = "теория_заговора"
                        },
                        new
                        {
                            Id = new Guid("571f043e-7cfe-4efa-96d7-b41c22954de7"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7421),
                            Name = "соцсети"
                        },
                        new
                        {
                            Id = new Guid("60de4ae7-39be-42ee-9060-3ece4ad5ce0b"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7427),
                            Name = "косплей"
                        },
                        new
                        {
                            Id = new Guid("8f82ec7b-38f9-4f97-98ae-dd4037ce3980"),
                            CreateTime = new DateTime(2023, 12, 10, 7, 32, 0, 6, DateTimeKind.Utc).AddTicks(7429),
                            Name = "преступление"
                        });
                });

            modelBuilder.Entity("weblog_API.Models.TokenModel", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Token");

                    b.ToTable("BannedTokens");
                });

            modelBuilder.Entity("weblog_API.Models.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Likes", b =>
                {
                    b.HasOne("weblog_API.Models.Post.Post", null)
                        .WithMany()
                        .HasForeignKey("LikedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("weblog_API.Models.User.User", null)
                        .WithMany()
                        .HasForeignKey("UsersLikedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostTag", b =>
                {
                    b.HasOne("weblog_API.Models.Post.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("weblog_API.Models.Tags.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("weblog_API.Models.Comment.Comment", b =>
                {
                    b.HasOne("weblog_API.Models.User.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("weblog_API.Models.Comment.Comment", "ParentComment")
                        .WithMany("SubComments")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("weblog_API.Models.Post.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("ParentComment");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("weblog_API.Models.Community.UserCommunity", b =>
                {
                    b.HasOne("weblog_API.Models.Community.Community", "Community")
                        .WithMany("Subscribers")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("weblog_API.Models.User.User", "User")
                        .WithMany("Communities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("User");
                });

            modelBuilder.Entity("weblog_API.Models.Post.Post", b =>
                {
                    b.HasOne("weblog_API.Models.User.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId");

                    b.HasOne("weblog_API.Models.Community.Community", "Community")
                        .WithMany("Posts")
                        .HasForeignKey("CommunityId");

                    b.Navigation("Author");

                    b.Navigation("Community");
                });

            modelBuilder.Entity("weblog_API.Models.Comment.Comment", b =>
                {
                    b.Navigation("SubComments");
                });

            modelBuilder.Entity("weblog_API.Models.Community.Community", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Subscribers");
                });

            modelBuilder.Entity("weblog_API.Models.Post.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("weblog_API.Models.User.User", b =>
                {
                    b.Navigation("Communities");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
