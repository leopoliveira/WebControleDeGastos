using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeGastosV2.Migrations
{
    public partial class PrimeiraMigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carteiras",
                columns: table => new
                {
                    CarteiraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CorHexaDecimal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CarteiraPrincipal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carteiras", x => x.CarteiraId);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CorHexaDecimal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsReceita = table.Column<bool>(type: "bit", nullable: false),
                    IsDepesa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Depesas",
                columns: table => new
                {
                    DespesaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DataDespesa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    CarteiraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depesas", x => x.DespesaId);
                    table.ForeignKey(
                        name: "FK_Depesas_Carteiras_CarteiraId",
                        column: x => x.CarteiraId,
                        principalTable: "Carteiras",
                        principalColumn: "CarteiraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Depesas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receitas",
                columns: table => new
                {
                    ReceitaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DataReceita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    CarteiraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receitas", x => x.ReceitaId);
                    table.ForeignKey(
                        name: "FK_Receitas_Carteiras_CarteiraId",
                        column: x => x.CarteiraId,
                        principalTable: "Carteiras",
                        principalColumn: "CarteiraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Receitas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depesas_CarteiraId",
                table: "Depesas",
                column: "CarteiraId");

            migrationBuilder.CreateIndex(
                name: "IX_Depesas_CategoriaId",
                table: "Depesas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Receitas_CarteiraId",
                table: "Receitas",
                column: "CarteiraId");

            migrationBuilder.CreateIndex(
                name: "IX_Receitas_CategoriaId",
                table: "Receitas",
                column: "CategoriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depesas");

            migrationBuilder.DropTable(
                name: "Receitas");

            migrationBuilder.DropTable(
                name: "Carteiras");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
