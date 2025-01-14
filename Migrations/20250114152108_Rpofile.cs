using Microsoft.EntityFrameworkCore.Migrations;

public partial class Rpofile : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Supprimer les colonnes ClientFirstName et ClientId
        migrationBuilder.DropColumn(
            name: "ClientFirstName",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "ClientId",
            table: "AspNetUsers");

        // Renommer la colonne ClientLastName en Adresse
        migrationBuilder.RenameColumn(
            name: "ClientLastName",
            table: "AspNetUsers",
            newName: "Adresse");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Renommer la colonne Adresse en ClientLastName lors du rollback
        migrationBuilder.RenameColumn(
            name: "Adresse",
            table: "AspNetUsers",
            newName: "ClientLastName");

        // Reajouter les colonnes ClientFirstName et ClientId
        migrationBuilder.AddColumn<string>(
            name: "ClientFirstName",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "ClientId",
            table: "AspNetUsers",
            type: "int",
            nullable: true);
    }
}
