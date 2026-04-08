using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class UnirTorneoConDisciplina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetenciasDisciplinas",
                columns: table => new
                {
                    IdCompetenciaDisciplina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompetencia = table.Column<int>(type: "int", nullable: false),
                    IdDisciplina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciasDisciplinas", x => x.IdCompetenciaDisciplina);
                    table.ForeignKey(
                        name: "FK_CompetenciasDisciplinas_Competencias_IdCompetencia",
                        column: x => x.IdCompetencia,
                        principalTable: "Competencias",
                        principalColumn: "IdCompetencia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetenciasDisciplinas_Disciplinas_IdDisciplina",
                        column: x => x.IdDisciplina,
                        principalTable: "Disciplinas",
                        principalColumn: "IdDisciplina",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciasDisciplinas_IdCompetencia",
                table: "CompetenciasDisciplinas",
                column: "IdCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciasDisciplinas_IdDisciplina",
                table: "CompetenciasDisciplinas",
                column: "IdDisciplina");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenciasDisciplinas");
        }
    }
}
