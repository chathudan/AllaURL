﻿// <auto-generated />
using System;
using AllaURL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AllaURL.Data.Migrations
{
    [DbContext(typeof(AllaUrlDbContext))]
    [Migration("20250118151529_Updated_TokenEntity")]
    partial class Updated_TokenEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AllaURL.Data.Entities.TokenDataEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RedirectUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("TokenId")
                        .HasColumnType("integer");

                    b.Property<int>("TokenType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TokenId")
                        .IsUnique();

                    b.ToTable("TokenData");
                });

            modelBuilder.Entity("AllaURL.Data.Entities.TokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAllocated")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TokenType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("AllaURL.Data.Entities.TokenDataEntity", b =>
                {
                    b.HasOne("AllaURL.Data.Entities.TokenEntity", "TokenEntity")
                        .WithOne("TokenDataEntity")
                        .HasForeignKey("AllaURL.Data.Entities.TokenDataEntity", "TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TokenEntity");
                });

            modelBuilder.Entity("AllaURL.Data.Entities.TokenEntity", b =>
                {
                    b.Navigation("TokenDataEntity")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
