using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class EntrenadorClubOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores");

            migrationBuilder.AlterColumn<int>(
                name: "IdClub",
                table: "Entrenadores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores");

            migrationBuilder.AlterColumn<int>(
                name: "IdClub",
                table: "Entrenadores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenadores_Clubes_IdClub",
                table: "Entrenadores",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
