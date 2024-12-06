using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelier_2.Migrations
{
    /// <inheritdoc />
    public partial class piecead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pieces_Interventions_InterventionId",
                table: "Pieces");

            migrationBuilder.DropIndex(
                name: "IX_Pieces_InterventionId",
                table: "Pieces");

            migrationBuilder.DropColumn(
                name: "InterventionId",
                table: "Pieces");

            migrationBuilder.CreateTable(
                name: "InterventionPiece",
                columns: table => new
                {
                    InterventionsId = table.Column<int>(type: "int", nullable: false),
                    PiecesUtiliseesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionPiece", x => new { x.InterventionsId, x.PiecesUtiliseesId });
                    table.ForeignKey(
                        name: "FK_InterventionPiece_Interventions_InterventionsId",
                        column: x => x.InterventionsId,
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterventionPiece_Pieces_PiecesUtiliseesId",
                        column: x => x.PiecesUtiliseesId,
                        principalTable: "Pieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterventionPiece_PiecesUtiliseesId",
                table: "InterventionPiece",
                column: "PiecesUtiliseesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterventionPiece");

            migrationBuilder.AddColumn<int>(
                name: "InterventionId",
                table: "Pieces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pieces_InterventionId",
                table: "Pieces",
                column: "InterventionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pieces_Interventions_InterventionId",
                table: "Pieces",
                column: "InterventionId",
                principalTable: "Interventions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
