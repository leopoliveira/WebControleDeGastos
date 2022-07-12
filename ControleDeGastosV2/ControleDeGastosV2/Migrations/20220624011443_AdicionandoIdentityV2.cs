using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeGastosV2.Migrations
{
    public partial class AdicionandoIdentityV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Categorias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Carteiras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UsuarioId",
                table: "Categorias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_UsuarioId",
                table: "Carteiras",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carteiras_AspNetUsers_UsuarioId",
                table: "Carteiras",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_AspNetUsers_UsuarioId",
                table: "Categorias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carteiras_AspNetUsers_UsuarioId",
                table: "Carteiras");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_AspNetUsers_UsuarioId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UsuarioId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Carteiras_UsuarioId",
                table: "Carteiras");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Carteiras");
        }
    }
}
