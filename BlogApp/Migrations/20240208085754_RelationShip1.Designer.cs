﻿// <auto-generated />
using System;
using BlogApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240208085754_RelationShip1")]
    partial class RelationShip1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogApp.Models.Domain.Blog", b =>
                {
                    b.Property<Guid>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverPhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BlogId");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogComment", b =>
                {
                    b.Property<Guid>("BlogCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BlogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CommentDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BlogCommentId");

                    b.HasIndex("BlogId");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("BlogComments");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogCommentLike", b =>
                {
                    b.Property<Guid>("BlogCommentLikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BlogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CommentBlogCommentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BlogCommentLikeId");

                    b.HasIndex("BlogId");

                    b.HasIndex("CommentBlogCommentId");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("BlogCommentLikes");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogLike", b =>
                {
                    b.Property<Guid>("BlogLikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BlogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BlogLikeId");

                    b.HasIndex("BlogId");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("BlogLikes");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bio")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.Blog", b =>
                {
                    b.HasOne("BlogApp.Models.Domain.User", "CreatedBy")
                        .WithMany("Blogs")
                        .HasForeignKey("CreatedByUserId");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogComment", b =>
                {
                    b.HasOne("BlogApp.Models.Domain.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId");

                    b.HasOne("BlogApp.Models.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.Navigation("Blog");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogCommentLike", b =>
                {
                    b.HasOne("BlogApp.Models.Domain.Blog", "Blog")
                        .WithMany()
                        .HasForeignKey("BlogId");

                    b.HasOne("BlogApp.Models.Domain.BlogComment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentBlogCommentId");

                    b.HasOne("BlogApp.Models.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.Navigation("Blog");

                    b.Navigation("Comment");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogLike", b =>
                {
                    b.HasOne("BlogApp.Models.Domain.Blog", "Blog")
                        .WithMany("Likes")
                        .HasForeignKey("BlogId");

                    b.HasOne("BlogApp.Models.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.Navigation("Blog");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.Blog", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.BlogComment", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("BlogApp.Models.Domain.User", b =>
                {
                    b.Navigation("Blogs");
                });
#pragma warning restore 612, 618
        }
    }
}
