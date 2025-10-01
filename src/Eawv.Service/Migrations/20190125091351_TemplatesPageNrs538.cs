// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class TemplatesPageNrs538 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("3464ef11-14e5-9cbb-fcc5-fd31fc1704fd"),
            column: "Content",
            value: @"<html>
  <head>
    <meta charset=""UTF-8"" />
    <style>
      body {
        margin: 0;
        padding: 0;
        position: relative;
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
      }

      @(""@"")media only screen {
      body {
        background-color: gray;
        padding: 1cm;
      }

      .doc {
        background-color: white;
      }
      }

      h1,
      h2,
      h3,
      h4,
      h5 {
        margin: 0;
      }

      h1 {
        margin-bottom: 1mm;
      }

      .input {
        height: 1rem;
        position: relative;
        display: inline-block;
        padding: 0 1mm;
        box-sizing: border-box;
      }
      .input .border {
        border: 1px solid #999;
        border-top: none;
        height: 1mm;
        margin-top: 3mm;
        margin: 3mm 0.5mm 0.5mm 0.5mm;
        bottom: 0;
        position: absolute;
        left: 0;
        right: 0;
      }
      input {
        border: none;
        width: 100%;
        position: relative;
        bottom: 1mm;
      }
      .inline-input {
        bottom: -.5mm;
      }

      .w-2 {
        width: 2rem;
      }
      .w-4 {
        width: 4rem;
      }
      .w-6 {
        width: 6rem;
      }
      .w-12 {
        width: 12rem;
      }
      .w-16 {
        width: 16rem;
      }
      .w-24 {
        width: 24rem;
      }

      .col-main {
        width: 19cm;
      }

      .col-right {
        width: 8.5cm;
      }

      .label {
        font-size: 1.1rem;
        font-weight: 700;
        color: #999;
        margin-right: 0.2rem;
        position: relative;
        top: -1mm;
      }

      .size-A4.landscape {
        width: 297mm;
      }

      .size-A4 {
        width: 210mm;
      }
      
      .page {
        page-break-after: always;
      }

      .hint {
        font-size: 8pt;
        color: #555;
      }

      .filled {
        background-color: #dadada;
      }

      .small {
        font-size: 7pt;
      }
      .bold {
        font-weight: 700;
      }
      .super {
        vertical-align: super;
      }

      .row {
        display: flex;
        flex-direction: row;
        margin-bottom: 0.15rem;
      }
      .col {
        display: flex;
        flex-direction: column;
      }
      .stretch {
        justify-content: stretch;
      }
      .space {
        justify-content: space-between;
      }
      .grow {
        flex-grow: 1;
      }
      .no-shrink {
        flex-shrink: 0;
      }
      .end {
        justify-content: flex-end;
      }
      .items-end {
        align-items: flex-end;
      }
      .center {
        justify-content: center;
      }
      .right {
        text-align: right;
      }
      .inline {
        display: inline-block;
        margin: 0;
      }
      .fullwidth {
        width: 100%;
      }
      .spacer {
        display: inline-block;
        width: 0.5rem;
      }
      .space-bottom {
        margin-bottom: 1.5mm;
      }
      .space-right {
        margin-right: 2mm;
      }

      .upright {
        writing-mode: vertical-rl;
        transform: rotate(-180deg);
      }

      table {
        margin-bottom: 1.5mm;
      }

      table,
      th,
      td {
        font-size: 11px;
        border-collapse: collapse;
        border: 1px solid #000;
      }

      th {
        background-color: #dadada;
        vertical-align: top;
      }

      th,
      td {
        padding: 1mm;
      }

      thead tr > tr th {
        font-weight: 400;
      }

      thead {
        height: 200px;
      }
      tbody {
        background-color: #fff;
      }
      tbody td {
        border-top: 1px dashed #000;
        border-bottom: 1px dashed #000;
      }

      tbody tr {
        height: 1cm;
      }
      
      tbody tr > td.right {
        text-align: right;
      }
      
      tbody tr > td.center {
        text-align: center;
      }

      .top-no-border th,
      .top-no-border tr {
        border-top: none;
      }
      .bot-no-border {
        border-bottom: none;
      }

      .center-placeholder::placeholder {
        text-align: center;
        color: #000;
      }

      .version {
        align-items: flex-end;
        justify-content: flex-end;
        display: flex;
      }
      
      .checkbox-checked::after {
        content: '☑';
        font-size: 22px;
      }
      
      .checkbox-unchecked::after {
        content: '☐';
        font-size: 22px;
      }

      .preserve-whitespace {
        white-space: pre-wrap;
      }
      
      footer.pdf {
        width: 100%;
        padding-right: 0.75cm;
      }

      footer.pdf div {
        width: 100%;
        font-size: 8pt;
        text-align: right;
      }
      
      @RenderSection(""Styles"", false)
      </style>
</head>
<body>
<div class=""doc size-@ViewBag.Model.Template.Format @(ViewBag.Model.Template.Landscape ? ""landscape"" : """")"">
  <header class=""pdf""><span></span></header><!-- enforce empty pdf header -->
  @RenderBody()
  <footer class=""pdf"">
    <div>Seite <span class=""pageNumber""></span> / <span class=""totalPages""></span></div>
  </footer>
</div>
</body>
</html>
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Templates",
            keyColumn: "Id",
            keyValue: new Guid("3464ef11-14e5-9cbb-fcc5-fd31fc1704fd"),
            column: "Content",
            value: @"<html>
  <head>
    <meta charset=""UTF-8"" />
    <style>
      body {
        margin: 0;
        padding: 0;
        position: relative;
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
      }

      @(""@"")media only screen {
        body {
          background-color: gray;
          padding: 1cm;
        }

        .doc {
          background-color: white;
        }
      }

      h1,
      h2,
      h3,
      h4,
      h5 {
        margin: 0;
      }

      h1 {
        margin-bottom: 1mm;
      }

      .input {
        height: 1rem;
        position: relative;
        display: inline-block;
        padding: 0 1mm;
        box-sizing: border-box;
      }
      .input .border {
        border: 1px solid #999;
        border-top: none;
        height: 1mm;
        margin-top: 3mm;
        margin: 3mm 0.5mm 0.5mm 0.5mm;
        bottom: 0;
        position: absolute;
        left: 0;
        right: 0;
      }
      input {
        border: none;
        width: 100%;
        position: relative;
        bottom: 1mm;
      }
      .inline-input {
        bottom: -.5mm;
      }

      .w-2 {
        width: 2rem;
      }
      .w-4 {
        width: 4rem;
      }
      .w-6 {
        width: 6rem;
      }
      .w-12 {
        width: 12rem;
      }
      .w-16 {
        width: 16rem;
      }
      .w-24 {
        width: 24rem;
      }

      .col-main {
        width: 19cm;
      }

      .col-right {
        width: 8.5cm;
      }

      .label {
        font-size: 1.1rem;
        font-weight: 700;
        color: #999;
        margin-right: 0.2rem;
        position: relative;
        top: -1mm;
      }

      .size-A4.landscape {
        width: 297mm;
      }

      .size-A4 {
        width: 210mm;
      }
      
      .page {
        page-break-after: always;
      }

      .hint {
        font-size: 8pt;
        color: #555;
      }

      .filled {
        background-color: #dadada;
      }

      .small {
        font-size: 7pt;
      }
      .bold {
        font-weight: 700;
      }
      .super {
        vertical-align: super;
      }

      .row {
        display: flex;
        flex-direction: row;
        margin-bottom: 0.15rem;
      }
      .col {
        display: flex;
        flex-direction: column;
      }
      .stretch {
        justify-content: stretch;
      }
      .space {
        justify-content: space-between;
      }
      .grow {
        flex-grow: 1;
      }
      .no-shrink {
        flex-shrink: 0;
      }
      .end {
        justify-content: flex-end;
      }
      .items-end {
        align-items: flex-end;
      }
      .center {
        justify-content: center;
      }
      .right {
        text-align: right;
      }
      .inline {
        display: inline-block;
        margin: 0;
      }
      .fullwidth {
        width: 100%;
      }
      .spacer {
        display: inline-block;
        width: 0.5rem;
      }
      .space-bottom {
        margin-bottom: 1.5mm;
      }
      .space-right {
        margin-right: 2mm;
      }

      .upright {
        writing-mode: vertical-rl;
        transform: rotate(-180deg);
      }

      table {
        margin-bottom: 1.5mm;
      }

      table,
      th,
      td {
        font-size: 11px;
        border-collapse: collapse;
        border: 1px solid #000;
      }

      th {
        background-color: #dadada;
        vertical-align: top;
      }

      th,
      td {
        padding: 1mm;
      }

      thead tr > tr th {
        font-weight: 400;
      }

      thead {
        height: 200px;
      }
      tbody {
        background-color: #fff;
      }
      tbody td {
        border-top: 1px dashed #000;
        border-bottom: 1px dashed #000;
      }

      tbody tr {
        height: 1cm;
      }
      
      tbody tr > td.right {
        text-align: right;
      }
      
      tbody tr > td.center {
        text-align: center;
      }

      .top-no-border th,
      .top-no-border tr {
        border-top: none;
      }
      .bot-no-border {
        border-bottom: none;
      }

      .center-placeholder::placeholder {
        text-align: center;
        color: #000;
      }

      .version {
        align-items: flex-end;
        justify-content: flex-end;
        display: flex;
      }
      
      .checkbox-checked::after {
        content: '☑';
        font-size: 22px;
      }
      
      .checkbox-unchecked::after {
        content: '☐';
        font-size: 22px;
      }

      .preserve-whitespace {
        white-space: pre-wrap;
      }
      
      @RenderSection(""Styles"", false)
    </style>
  </head>
<body>
<div class=""doc size-@ViewBag.Model.Template.Format @(ViewBag.Model.Template.Landscape ? ""landscape"" : """")"">
  @RenderBody()
</div>
</body>
</html>
");
    }
}
