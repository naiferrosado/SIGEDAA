using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class AgregaRelacionCompetenciaClub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetenciasClubes_Clubes_IdClub",
                table: "CompetenciasClubes");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetenciasClubes_Clubes_IdClub",
                table: "CompetenciasClubes",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetenciasClubes_Clubes_IdClub",
                table: "CompetenciasClubes");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetenciasClubes_Clubes_IdClub",
                table: "CompetenciasClubes",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
