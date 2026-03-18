using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAdminPorDefecto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ClaveAcceso", "Email", "EstadoActivo", "FechaRegistro", "Nombre", "Rol" },
                values: new object[] { 1, "Admin123", "admin@fdaa.com", true, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrador Principal", "Administrador" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
