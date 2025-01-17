﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelier_2.Migrations
{
    /// <inheritdoc />
    public partial class EtatReclam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Etat",
                table: "Reclamations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Etat",
                table: "Reclamations");
        }
    }
}
