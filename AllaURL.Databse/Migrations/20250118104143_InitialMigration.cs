using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AllaURL.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RedirectUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VCardEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VcardType = table.Column<int>(type: "integer", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    CompanyNumber = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    JobTitle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VCardEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenDataEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenId = table.Column<int>(type: "integer", nullable: false),
                    TokenType = table.Column<int>(type: "integer", nullable: false),
                    TokenDataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenDataEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenDataEntity_UrlEntity_TokenDataId",
                        column: x => x.TokenDataId,
                        principalTable: "UrlEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TokenDataEntity_VCardEntity_TokenDataId",
                        column: x => x.TokenDataId,
                        principalTable: "VCardEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokenEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsAllocated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TokenDataEntityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenEntity_TokenDataEntity_Id",
                        column: x => x.Id,
                        principalTable: "TokenDataEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TokenEntity_TokenDataEntity_TokenDataEntityId",
                        column: x => x.TokenDataEntityId,
                        principalTable: "TokenDataEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenDataEntity_Id_TokenDataId",
                table: "TokenDataEntity",
                columns: new[] { "Id", "TokenDataId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TokenDataEntity_TokenDataId",
                table: "TokenDataEntity",
                column: "TokenDataId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenEntity_Identifier",
                table: "TokenEntity",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TokenEntity_TokenDataEntityId",
                table: "TokenEntity",
                column: "TokenDataEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UrlEntity_RedirectUrl",
                table: "UrlEntity",
                column: "RedirectUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VCardEntity_Email",
                table: "VCardEntity",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenEntity");

            migrationBuilder.DropTable(
                name: "TokenDataEntity");

            migrationBuilder.DropTable(
                name: "UrlEntity");

            migrationBuilder.DropTable(
                name: "VCardEntity");
        }
    }
}
