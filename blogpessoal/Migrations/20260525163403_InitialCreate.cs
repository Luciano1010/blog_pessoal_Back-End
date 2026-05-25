using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace blogpessoal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_temas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_temas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "Varchar", maxLength: 255, nullable: false),
                    Usuario = table.Column<string>(type: "Varchar", maxLength: 255, nullable: false),
                    Senha = table.Column<string>(type: "Varchar", maxLength: 255, nullable: false),
                    Foto = table.Column<string>(type: "Varchar", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_postagens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "Varchar", maxLength: 100, nullable: false),
                    Texto = table.Column<string>(type: "Varchar", maxLength: 1000, nullable: false),
                    TemaId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: true),
                    Data = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_postagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_postagens_tb_temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "tb_temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_postagens_tb_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "tb_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_postagens_TemaId",
                table: "tb_postagens",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_postagens_UsuarioId",
                table: "tb_postagens",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_postagens");

            migrationBuilder.DropTable(
                name: "tb_temas");

            migrationBuilder.DropTable(
                name: "tb_usuarios");
        }
    }
}
