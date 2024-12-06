using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelier_2.Migrations
{
    /// <inheritdoc />
    public partial class technicien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_AspNetUsers_TechnicienId",
                table: "Interventions");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_AspNetUsers_TechnicienId",
                table: "Interventions",
                column: "TechnicienId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_AspNetUsers_TechnicienId",
                table: "Interventions");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_AspNetUsers_TechnicienId",
                table: "Interventions",
                column: "TechnicienId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
