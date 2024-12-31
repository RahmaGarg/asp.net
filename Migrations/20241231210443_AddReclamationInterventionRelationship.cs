using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelier_2.Migrations
{
    /// <inheritdoc />
    public partial class AddReclamationInterventionRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer la contrainte de clé étrangère et l'index existant
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions");

            // Rendre la colonne ReclamationId nullable
            migrationBuilder.AlterColumn<int>(
                name: "ReclamationId",
                table: "Interventions",
                type: "int",
                nullable: true,  // La rendre nullable
                oldClrType: typeof(int),
                oldType: "int");

            // Ajouter la colonne InterventionId à Reclamations (si nécessaire)
            migrationBuilder.AddColumn<int>(
                name: "InterventionId",
                table: "Reclamations",
                type: "int",
                nullable: true);

            // Créer un nouvel index sur ReclamationId
            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions",
                column: "ReclamationId",
                unique: true);

            // Ajouter la contrainte de clé étrangère avec l'action ON DELETE SET NULL
            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions",
                column: "ReclamationId",
                principalTable: "Reclamations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer la contrainte de clé étrangère et l'index
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions");

            // Supprimer la colonne InterventionId de Reclamations
            migrationBuilder.DropColumn(
                name: "InterventionId",
                table: "Reclamations");

            // Rendre la colonne ReclamationId non nullable dans la méthode Down
            migrationBuilder.AlterColumn<int>(
                name: "ReclamationId",
                table: "Interventions",
                type: "int",
                nullable: false,  // La rendre non nullable
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Ajouter de nouveau l'index et la contrainte de clé étrangère avec ON DELETE CASCADE
            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions",
                column: "ReclamationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions",
                column: "ReclamationId",
                principalTable: "Reclamations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }

}
