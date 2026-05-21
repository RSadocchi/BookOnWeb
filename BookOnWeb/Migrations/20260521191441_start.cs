using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookOnWeb.Migrations
{
    /// <inheritdoc />
    public partial class start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titolo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AutoreId = table.Column<int>(type: "int", nullable: false),
                    Trama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnoPubblicazione = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libri_Autori_AutoreId",
                        column: x => x.AutoreId,
                        principalTable: "Autori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prestiti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPrestito = table.Column<DateOnly>(type: "date", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    NomeUtente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cellulare = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DataRestituzione = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestiti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestiti_Libri_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libri_AutoreId",
                table: "Libri",
                column: "AutoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestiti_LibroId",
                table: "Prestiti",
                column: "LibroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestiti");

            migrationBuilder.DropTable(
                name: "Libri");

            migrationBuilder.DropTable(
                name: "Autori");
        }
    }
}
