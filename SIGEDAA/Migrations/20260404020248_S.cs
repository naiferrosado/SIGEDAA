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
            //migrationBuilder.DropForeignKey(
                //name: "FK_Atletas_Clubes_ClubIdClub",
                //table: "Atletas");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Atletas_AtletaIdAtleta",
                table: "ResultadosCompetencia");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Competencias_CompetenciaIdCompetencia",
                table: "ResultadosCompetencia");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Disciplinas_DisciplinaIdDisciplina",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_AtletaIdAtleta",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_CompetenciaIdCompetencia",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_DisciplinaIdDisciplina",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_Entrenadores_IdClub",
                table: "Entrenadores");

          //  migrationBuilder.DropIndex(
            //    name: "IX_Atletas_ClubIdClub",
              //  table: "Atletas");

            migrationBuilder.DropColumn(
                name: "AtletaIdAtleta",
                table: "ResultadosCompetencia");

            migrationBuilder.DropColumn(
                name: "CompetenciaIdCompetencia",
                table: "ResultadosCompetencia");

            migrationBuilder.DropColumn(
                name: "DisciplinaIdDisciplina",
                table: "ResultadosCompetencia");

           // migrationBuilder.DropColumn(
             //   name: "ClubIdClub",
               // table: "Atletas");

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

            migrationBuilder.AddColumn<int>(
                name: "AtletaIdAtleta",
                table: "ResultadosCompetencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompetenciaIdCompetencia",
                table: "ResultadosCompetencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisciplinaIdDisciplina",
                table: "ResultadosCompetencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClubIdClub",
                table: "Atletas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_AtletaIdAtleta",
                table: "ResultadosCompetencia",
                column: "AtletaIdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_CompetenciaIdCompetencia",
                table: "ResultadosCompetencia",
                column: "CompetenciaIdCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_DisciplinaIdDisciplina",
                table: "ResultadosCompetencia",
                column: "DisciplinaIdDisciplina");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenadores_IdClub",
                table: "Entrenadores",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_Atletas_ClubIdClub",
                table: "Atletas",
                column: "ClubIdClub");

            migrationBuilder.AddForeignKey(
                name: "FK_Atletas_Clubes_ClubIdClub",
                table: "Atletas",
                column: "ClubIdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Atletas_AtletaIdAtleta",
                table: "ResultadosCompetencia",
                column: "AtletaIdAtleta",
                principalTable: "Atletas",
                principalColumn: "IdAtleta",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Competencias_CompetenciaIdCompetencia",
                table: "ResultadosCompetencia",
                column: "CompetenciaIdCompetencia",
                principalTable: "Competencias",
                principalColumn: "IdCompetencia",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Disciplinas_DisciplinaIdDisciplina",
                table: "ResultadosCompetencia",
                column: "DisciplinaIdDisciplina",
                principalTable: "Disciplinas",
                principalColumn: "IdDisciplina",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
