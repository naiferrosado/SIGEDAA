using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class InscripcionClubesCompetencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PuntosOtorgados",
                table: "ResultadosCompetencia",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "CompetenciasClubes",
                columns: table => new
                {
                    IdInscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompetencia = table.Column<int>(type: "int", nullable: false),
                    IdClub = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoInscripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciasClubes", x => x.IdInscripcion);
                    table.ForeignKey(
                        name: "FK_CompetenciasClubes_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetenciasClubes_Competencias_IdCompetencia",
                        column: x => x.IdCompetencia,
                        principalTable: "Competencias",
                        principalColumn: "IdCompetencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciasClubes_IdClub",
                table: "CompetenciasClubes",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciasClubes_IdCompetencia",
                table: "CompetenciasClubes",
                column: "IdCompetencia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenciasClubes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PuntosOtorgados",
                table: "ResultadosCompetencia",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }
    }
}
