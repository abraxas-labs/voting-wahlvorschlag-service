// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class RemoveTimeFromElectionDates : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "SubmissionDeadlineEnd",
            table: "Elections",
            type: "date",
            nullable: false,
            oldClrType: typeof(DateTime));

        migrationBuilder.AlterColumn<DateTime>(
            name: "SubmissionDeadlineBegin",
            table: "Elections",
            type: "date",
            nullable: false,
            oldClrType: typeof(DateTime));

        migrationBuilder.AlterColumn<DateTime>(
            name: "ContestDate",
            table: "Elections",
            type: "date",
            nullable: false,
            oldClrType: typeof(DateTime));

        migrationBuilder.AlterColumn<DateTime>(
            name: "AvailableFrom",
            table: "Elections",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "SubmissionDeadlineEnd",
            table: "Elections",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "date");

        migrationBuilder.AlterColumn<DateTime>(
            name: "SubmissionDeadlineBegin",
            table: "Elections",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "date");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ContestDate",
            table: "Elections",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "date");

        migrationBuilder.AlterColumn<DateTime>(
            name: "AvailableFrom",
            table: "Elections",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "date",
            oldNullable: true);
    }
}
