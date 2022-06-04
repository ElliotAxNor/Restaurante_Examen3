using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Data.Migrations
{
    public partial class ModificacionPlatillosCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImagen",
                table: "Platillos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlImagen",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImagen",
                table: "Platillos");

            migrationBuilder.DropColumn(
                name: "UrlImagen",
                table: "Categorias");
        }
    }
}
