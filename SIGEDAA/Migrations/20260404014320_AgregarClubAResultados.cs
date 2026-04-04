using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarClubAResultados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdClub",
                table: "ResultadosCompetencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCompetencia_IdClub",
                table: "ResultadosCompetencia",
                column: "IdClub");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCompetencia_Clubes_IdClub",
                table: "ResultadosCompetencia");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCompetencia_IdClub",
                table: "ResultadosCompetencia");

            migrationBuilder.DropColumn(
                name: "IdClub",
                table: "ResultadosCompetencia");
        }
    }
}
