// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class ListComments : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ListComments",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreationDate = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                ModifiedBy = table.Column<string>(nullable: true),
                Content = table.Column<string>(nullable: false),
                ListId = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ListComments", x => x.Id);
                table.ForeignKey(
                    name: "FK_ListComments_Lists_ListId",
                    column: x => x.ListId,
                    principalTable: "Lists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ListComments_ListId",
            table: "ListComments",
            column: "ListId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ListComments");
    }
}
