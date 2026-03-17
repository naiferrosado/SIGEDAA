using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEDAA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atletas_Equipos_EquipoId",
                table: "Atletas");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Usuarios",
                newName: "ClaveAcceso");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Atletas",
                newName: "TipoSangre");

            migrationBuilder.RenameColumn(
                name: "EquipoId",
                table: "Atletas",
                newName: "IdClub");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "Atletas",
                newName: "Nombres");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Atletas",
                newName: "IdAtleta");

            migrationBuilder.RenameIndex(
                name: "IX_Atletas_EquipoId",
                table: "Atletas",
                newName: "IX_Atletas_IdClub");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "EstadoActivo",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Atletas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "EstaturaCm",
                table: "Atletas",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Atletas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PesoKg",
                table: "Atletas",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AsociacionesProvinciales",
                columns: table => new
                {
                    IdAsociacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreAsociacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombrePresidente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaFundacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CertificacionAlDia = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsociacionesProvinciales", x => x.IdAsociacion);
                });

            migrationBuilder.CreateTable(
                name: "Competencias",
                columns: table => new
                {
                    IdCompetencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEvento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sede = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencias", x => x.IdCompetencia);
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    IdDisciplina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreDisciplina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoMedicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneroPermitido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsRelevo = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaRecomendada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionReglas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.IdDisciplina);
                });

            migrationBuilder.CreateTable(
                name: "Clubes",
                columns: table => new
                {
                    IdClub = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAsociacion = table.Column<int>(type: "int", nullable: false),
                    NombreClub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionSede = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreDirector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubes", x => x.IdClub);
                    table.ForeignKey(
                        name: "FK_Clubes_AsociacionesProvinciales_IdAsociacion",
                        column: x => x.IdAsociacion,
                        principalTable: "AsociacionesProvinciales",
                        principalColumn: "IdAsociacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosCompetencia",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompetencia = table.Column<int>(type: "int", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false),
                    IdDisciplina = table.Column<int>(type: "int", nullable: false),
                    MarcaObtenida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosicionFinal = table.Column<int>(type: "int", nullable: false),
                    PuntosOtorgados = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EsRecordNacional = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosCompetencia", x => x.IdResultado);
                    table.ForeignKey(
                        name: "FK_ResultadosCompetencia_Atletas_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Atletas",
                        principalColumn: "IdAtleta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadosCompetencia_Competencias_IdCompetencia",
                        column: x => x.IdCompetencia,
                        principalTable: "Competencias",
                        principalColumn: "IdCompetencia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadosCompetencia_Disciplinas_IdDisciplina",
                        column: x => x.IdDisciplina,
                        principalTable: "Disciplinas",
                        principalColumn: "IdDisciplina",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entrenadores",
                columns: table => new
                {
                    IdEntrenador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClub = table.Column<int>(type: "int", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AniosExperiencia = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenadores", x => x.IdEntrenador);
                    table.ForeignKey(
                        name: "FK_Entrenadores_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clubes_IdAsociacion",
                table: "Clubes",
                column: "IdAsociacion");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenadores_IdClub",
                table: "Entrenadores",
                column: "IdClub");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Atletas_Clubes_IdClub",
                table: "Atletas",
                column: "IdClub",
                principalTable: "Clubes",
                principalColumn: "IdClub",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atletas_Clubes_IdClub",
                table: "Atletas");

            migrationBuilder.DropTable(
                name: "Entrenadores");

            migrationBuilder.DropTable(
                name: "ResultadosCompetencia");

            migrationBuilder.DropTable(
                name: "Clubes");

            migrationBuilder.DropTable(
                name: "Competencias");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "AsociacionesProvinciales");

            migrationBuilder.DropColumn(
                name: "EstadoActivo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Atletas");

            migrationBuilder.DropColumn(
                name: "EstaturaCm",
                table: "Atletas");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Atletas");

            migrationBuilder.DropColumn(
                name: "PesoKg",
                table: "Atletas");

            migrationBuilder.RenameColumn(
                name: "ClaveAcceso",
                table: "Usuarios",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "TipoSangre",
                table: "Atletas",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Nombres",
                table: "Atletas",
                newName: "Apellido");

            migrationBuilder.RenameColumn(
                name: "IdClub",
                table: "Atletas",
                newName: "EquipoId");

            migrationBuilder.RenameColumn(
                name: "IdAtleta",
                table: "Atletas",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Atletas_IdClub",
                table: "Atletas",
                newName: "IX_Atletas_EquipoId");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Atletas_Equipos_EquipoId",
                table: "Atletas",
                column: "EquipoId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
