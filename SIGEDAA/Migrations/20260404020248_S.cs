using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class S : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Atletas_IdAtleta",
                table: "ResultadosCompetencia");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Competencias_IdCompetencia",
                table: "ResultadosCompetencia");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Disciplinas_IdDisciplina",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_IdAtleta",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_IdCompetencia",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_IdDisciplina",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_Entrenadores_IdClub",
                table: "Entrenadores");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_IdAtleta",
                table: "ResultadosCompetencia",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_IdCompetencia",
                table: "ResultadosCompetencia",
                column: "IdCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_IdDisciplina",
                table: "ResultadosCompetencia",
                column: "IdDisciplina");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenadores_IdClub",
                table: "Entrenadores",
                column: "IdClub");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Atletas_IdAtleta",
                table: "ResultadosCompetencia",
                column: "IdAtleta",
                principalTable: "Atletas",
                principalColumn: "IdAtleta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Competencias_IdCompetencia",
                table: "ResultadosCompetencia",
                column: "IdCompetencia",
                principalTable: "Competencias",
                principalColumn: "IdCompetencia",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Disciplinas_IdDisciplina",
                table: "ResultadosCompetencia",
                column: "IdDisciplina",
                principalTable: "Disciplinas",
                principalColumn: "IdDisciplina",
                onDelete: ReferentialAction.Restrict);
        }
    }
}