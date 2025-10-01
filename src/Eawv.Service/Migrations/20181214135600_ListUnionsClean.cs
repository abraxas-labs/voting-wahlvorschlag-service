// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListUnionsClean : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions");

        migrationBuilder.DropTable(
            name: "ListUnionElements");

        migrationBuilder.DropIndex(
            name: "IX_ListUnions_ListId",
            table: "ListUnions");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "ListUnions");

        migrationBuilder.RenameColumn(
            name: "ListId",
            table: "ListUnions",
            newName: "RootListId");

        migrationBuilder.AddColumn<Guid>(
            name: "ListSubUnionId",
            table: "Lists",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ListUnionId",
            table: "Lists",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ListUnions_RootListId",
            table: "ListUnions",
            column: "RootListId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Lists_ListSubUnionId",
            table: "Lists",
            column: "ListSubUnionId");

        migrationBuilder.CreateIndex(
            name: "IX_Lists_ListUnionId",
            table: "Lists",
            column: "ListUnionId");

        migrationBuilder.AddForeignKey(
            name: "FK_Lists_ListUnions_ListSubUnionId",
            table: "Lists",
            column: "ListSubUnionId",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_Lists_ListUnions_ListUnionId",
            table: "Lists",
            column: "ListUnionId",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_Lists_RootListId",
            table: "ListUnions",
            column: "RootListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Lists_ListUnions_ListSubUnionId",
            table: "Lists");

        migrationBuilder.DropForeignKey(
            name: "FK_Lists_ListUnions_ListUnionId",
            table: "Lists");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_Lists_RootListId",
            table: "ListUnions");

        migrationBuilder.DropIndex(
            name: "IX_ListUnions_RootListId",
            table: "ListUnions");

        migrationBuilder.DropIndex(
            name: "IX_Lists_ListSubUnionId",
            table: "Lists");

        migrationBuilder.DropIndex(
            name: "IX_Lists_ListUnionId",
            table: "Lists");

        migrationBuilder.DropColumn(
            name: "ListSubUnionId",
            table: "Lists");

        migrationBuilder.DropColumn(
            name: "ListUnionId",
            table: "Lists");

        migrationBuilder.RenameColumn(
            name: "RootListId",
            table: "ListUnions",
            newName: "ListId");

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "ListUnions",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "ListUnionElements",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                ListId = table.Column<Guid>(nullable: false),
                ListUnionId = table.Column<Guid>(nullable: false),
                ModifiedBy = table.Column<string>(nullable: true),
                ModifiedDate = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ListUnionElements", x => x.Id);
                table.UniqueConstraint("AK_ListUnionElements_ListUnionId_ListId", x => new { x.ListUnionId, x.ListId });
                table.ForeignKey(
                    name: "FK_ListUnionElements_Lists_ListId",
                    column: x => x.ListId,
                    principalTable: "Lists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ListUnionElements_ListUnions_ListUnionId",
                    column: x => x.ListUnionId,
                    principalTable: "ListUnions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ListUnions_ListId",
            table: "ListUnions",
            column: "ListId");

        migrationBuilder.CreateIndex(
            name: "IX_ListUnionElements_ListId",
            table: "ListUnionElements",
            column: "ListId");

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
