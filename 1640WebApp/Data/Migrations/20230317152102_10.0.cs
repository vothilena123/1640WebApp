﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1640WebApp.Data.Migrations
{
    public partial class _100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Catogorys_CatogoryId",
                table: "Ideas");

            migrationBuilder.DropIndex(
                name: "IX_Ideas_CatogoryId",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "CatogoryId",
                table: "Ideas");

            migrationBuilder.AddColumn<int>(
                name: "IdeaId",
                table: "Ideas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Catogorys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdeaId",
                table: "Catogorys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catogorys_IdeaId",
                table: "Catogorys",
                column: "IdeaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catogorys_Ideas_IdeaId",
                table: "Catogorys",
                column: "IdeaId",
                principalTable: "Ideas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catogorys_Ideas_IdeaId",
                table: "Catogorys");

            migrationBuilder.DropIndex(
                name: "IX_Catogorys_IdeaId",
                table: "Catogorys");

            migrationBuilder.DropColumn(
                name: "IdeaId",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Catogorys");

            migrationBuilder.DropColumn(
                name: "IdeaId",
                table: "Catogorys");

            migrationBuilder.AddColumn<int>(
                name: "CatogoryId",
                table: "Ideas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_CatogoryId",
                table: "Ideas",
                column: "CatogoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Catogorys_CatogoryId",
                table: "Ideas",
                column: "CatogoryId",
                principalTable: "Catogorys",
                principalColumn: "Id");
        }
    }
}
