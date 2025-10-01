// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListUnions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BallotDocuments_Lists_ListId",
            table: "BallotDocuments");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions");

        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_ListUnions_ListUnionId",
            table: "ListUnions");

        migrationBuilder.DropIndex(
            name: "IX_ListUnions_ListUnionId",
            table: "ListUnions");

        migrationBuilder.DropIndex(
            name: "IX_ListUnionElements_ListUnionId",
            table: "ListUnionElements");

        migrationBuilder.DropIndex(
            name: "IX_BallotDocuments_ListId",
            table: "BallotDocuments");

        migrationBuilder.DropColumn(
            name: "ListUnionDescription",
            table: "ListUnions");

        migrationBuilder.DropColumn(
            name: "ListUnionId",
            table: "ListUnions");

        migrationBuilder.DropColumn(
            name: "ListId",
            table: "BallotDocuments");

        migrationBuilder.AlterColumn<Guid>(
            name: "ListId",
            table: "ListUnions",
            nullable: true,
            oldClrType: typeof(Guid));

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "ListUnions",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddUniqueConstraint(
            name: "AK_ListUnionElements_ListUnionId_ListId",
            table: "ListUnionElements",
            columns: new[] { "ListUnionId", "ListId" });

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions");

        migrationBuilder.DropUniqueConstraint(
            name: "AK_ListUnionElements_ListUnionId_ListId",
            table: "ListUnionElements");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "ListUnions");

        migrationBuilder.AlterColumn<Guid>(
            name: "ListId",
            table: "ListUnions",
            nullable: false,
            oldClrType: typeof(Guid),
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ListUnionDescription",
            table: "ListUnions",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ListUnionId",
            table: "ListUnions",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "ListId",
            table: "BallotDocuments",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ListUnions_ListUnionId",
            table: "ListUnions",
            column: "ListUnionId");

        migrationBuilder.CreateIndex(
            name: "IX_ListUnionElements_ListUnionId",
            table: "ListUnionElements",
            column: "ListUnionId");

        migrationBuilder.CreateIndex(
            name: "IX_BallotDocuments_ListId",
            table: "BallotDocuments",
            column: "ListId");

        migrationBuilder.AddForeignKey(
            name: "FK_BallotDocuments_Lists_ListId",
            table: "BallotDocuments",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_Lists_ListId",
            table: "ListUnions",
            column: "ListId",
            principalTable: "Lists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ListUnions_ListUnions_ListUnionId",
            table: "ListUnions",
            column: "ListUnionId",
            principalTable: "ListUnions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
