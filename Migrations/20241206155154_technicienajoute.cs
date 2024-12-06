using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelier_2.Migrations
{
    /// <inheritdoc />
    public partial class technicienajoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TechnicienId",
                table: "Interventions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_TechnicienId",
                table: "Interventions",
                column: "TechnicienId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_AspNetUsers_TechnicienId",
                table: "Interventions",
                column: "TechnicienId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
    onDelete: ReferentialAction.Restrict); // Utilisez "Restrict" au lieu de "Cascade"
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_AspNetUsers_TechnicienId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_TechnicienId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "TechnicienId",
                table: "Interventions");
        }
    }
}
