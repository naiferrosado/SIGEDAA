using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class InvertirRelacionClubEntrenador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "NombreDirector",
                table: "Clubes");

            migrationBuilder.AddColumn<int>(
                name: "IdEntrenadorPrincipal",
                table: "Clubes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubes_IdEntrenadorPrincipal",
                table: "Clubes",
                column: "IdEntrenadorPrincipal");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubes_Entrenadores_IdEntrenadorPrincipal",
                table: "Clubes",
                column: "IdEntrenadorPrincipal",
                principalTable: "Entrenadores",
                principalColumn: "IdEntrenador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubes_Entrenadores_IdEntrenadorPrincipal",
                table: "Clubes");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores");

            migrationBuilder.DropIndex(
                name: "IX_Clubes_IdEntrenadorPrincipal",
                table: "Clubes");

            migrationBuilder.DropColumn(
                name: "IdEntrenadorPrincipal",
                table: "Clubes");

            migrationBuilder.AddColumn<string>(
                name: "NombreDirector",
                table: "Clubes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
