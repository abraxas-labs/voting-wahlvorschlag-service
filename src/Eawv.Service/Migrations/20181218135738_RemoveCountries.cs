// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eawv.Service.Migrations;

public partial class RemoveCountries : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Candidates_Countries_CountryId",
            table: "Candidates");

        migrationBuilder.DropTable(
            name: "Countries");

        migrationBuilder.DropIndex(
            name: "IX_Candidates_CountryId",
            table: "Candidates");

        migrationBuilder.DropColumn(
            name: "CountryId",
            table: "Candidates");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "CountryId",
            table: "Candidates",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IsoName = table.Column<string>(nullable: false),
                ShortName = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Countries", x => x.Id);
                table.UniqueConstraint("AK_Countries_IsoName", x => x.IsoName);
            });

        migrationBuilder.InsertData(
            table: "Countries",
            columns: new[] { "Id", "IsoName", "ShortName" },
            values: new object[,]
            {
                { new Guid("00004641-0000-0000-0000-000000000000"), "AF", "Afghanistan" },
                { new Guid("0000454e-0000-0000-0000-000000000000"), "NE", "Niger" },
                { new Guid("0000474e-0000-0000-0000-000000000000"), "NG", "Nigeria" },
                { new Guid("00004f4e-0000-0000-0000-000000000000"), "NO", "Norwegen" },
                { new Guid("00004d4f-0000-0000-0000-000000000000"), "OM", "Oman" },
                { new Guid("00005441-0000-0000-0000-000000000000"), "AT", "Österreich" },
                { new Guid("00004c54-0000-0000-0000-000000000000"), "TL", "Osttimor" },
                { new Guid("00004b50-0000-0000-0000-000000000000"), "PK", "Pakistan" },
                { new Guid("00005750-0000-0000-0000-000000000000"), "PW", "Palau" },
                { new Guid("00004150-0000-0000-0000-000000000000"), "PA", "Panama" },
                { new Guid("00004c4e-0000-0000-0000-000000000000"), "NL", "Niederlande" },
                { new Guid("00004750-0000-0000-0000-000000000000"), "PG", "Papua-Neuguinea" },
                { new Guid("00004550-0000-0000-0000-000000000000"), "PE", "Peru" },
                { new Guid("00004850-0000-0000-0000-000000000000"), "PH", "Philippinen" },
                { new Guid("00004c50-0000-0000-0000-000000000000"), "PL", "Polen" },
                { new Guid("00005450-0000-0000-0000-000000000000"), "PT", "Portugal" },
                { new Guid("00005752-0000-0000-0000-000000000000"), "RW", "Ruanda" },
                { new Guid("00004f52-0000-0000-0000-000000000000"), "RO", "Rumänien" },
                { new Guid("00005552-0000-0000-0000-000000000000"), "RU", "Russland" },
                { new Guid("00004253-0000-0000-0000-000000000000"), "SB", "Salomonen" },
                { new Guid("00004d5a-0000-0000-0000-000000000000"), "ZM", "Sambia" },
                { new Guid("00005950-0000-0000-0000-000000000000"), "PY", "Paraguay" },
                { new Guid("00005357-0000-0000-0000-000000000000"), "WS", "Samoa" },
                { new Guid("0000494e-0000-0000-0000-000000000000"), "NI", "Nicaragua" },
                { new Guid("0000504e-0000-0000-0000-000000000000"), "NP", "Nepal" },
                { new Guid("0000574d-0000-0000-0000-000000000000"), "MW", "Malawi" },
                { new Guid("0000594d-0000-0000-0000-000000000000"), "MY", "Malaysia" },
                { new Guid("0000564d-0000-0000-0000-000000000000"), "MV", "Malediven" },
                { new Guid("00004c4d-0000-0000-0000-000000000000"), "ML", "Mali" },
                { new Guid("0000544d-0000-0000-0000-000000000000"), "MT", "Malta" },
                { new Guid("0000414d-0000-0000-0000-000000000000"), "MA", "Marokko" },
                { new Guid("0000484d-0000-0000-0000-000000000000"), "MH", "Marshallinseln" },
                { new Guid("0000524d-0000-0000-0000-000000000000"), "MR", "Mauretanien" },
                { new Guid("0000554d-0000-0000-0000-000000000000"), "MU", "Mauritius" },
                { new Guid("00005a4e-0000-0000-0000-000000000000"), "NZ", "Neuseeland" },
                { new Guid("00004b4d-0000-0000-0000-000000000000"), "MK", "Mazedonien" },
                { new Guid("00004d46-0000-0000-0000-000000000000"), "FM", "Mikronesien" },
                { new Guid("0000444d-0000-0000-0000-000000000000"), "MD", "Moldau" },
                { new Guid("0000434d-0000-0000-0000-000000000000"), "MC", "Monaco" },
                { new Guid("00004e4d-0000-0000-0000-000000000000"), "MN", "Mongolei" },
                { new Guid("0000454d-0000-0000-0000-000000000000"), "ME", "Montenegro" },
                { new Guid("00005a4d-0000-0000-0000-000000000000"), "MZ", "Mosambik" },
                { new Guid("00004d4d-0000-0000-0000-000000000000"), "MM", "Myanmar" },
                { new Guid("0000414e-0000-0000-0000-000000000000"), "NA", "Namibia" },
                { new Guid("0000524e-0000-0000-0000-000000000000"), "NR", "Nauru" },
                { new Guid("0000584d-0000-0000-0000-000000000000"), "MX", "Mexiko" },
                { new Guid("00004d53-0000-0000-0000-000000000000"), "SM", "San Marino" },
                { new Guid("00005453-0000-0000-0000-000000000000"), "ST", "São Tomé und Príncipe" },
                { new Guid("00004153-0000-0000-0000-000000000000"), "SA", "Saudi-Arabien" },
                { new Guid("00004754-0000-0000-0000-000000000000"), "TG", "Togo" },
                { new Guid("00004f54-0000-0000-0000-000000000000"), "TO", "Tonga" },
                { new Guid("00005454-0000-0000-0000-000000000000"), "TT", "Trinidad und Tobago" },
                { new Guid("00004454-0000-0000-0000-000000000000"), "TD", "Tschad" },
                { new Guid("00005a43-0000-0000-0000-000000000000"), "CZ", "Tschechien" },
                { new Guid("00004e54-0000-0000-0000-000000000000"), "TN", "Tunesien" },
                { new Guid("00005254-0000-0000-0000-000000000000"), "TR", "Türkei" },
                { new Guid("00004d54-0000-0000-0000-000000000000"), "TM", "Turkmenistan" },
                { new Guid("00005654-0000-0000-0000-000000000000"), "TV", "Tuvalu" },
                { new Guid("00004854-0000-0000-0000-000000000000"), "TH", "Thailand" },
                { new Guid("00004755-0000-0000-0000-000000000000"), "UG", "Uganda" },
                { new Guid("00005548-0000-0000-0000-000000000000"), "HU", "Ungarn" },
                { new Guid("00005955-0000-0000-0000-000000000000"), "UY", "Uruguay" },
                { new Guid("00005a55-0000-0000-0000-000000000000"), "UZ", "Usbekistan" },
                { new Guid("00005556-0000-0000-0000-000000000000"), "VU", "Vanuatu" },
                { new Guid("00004556-0000-0000-0000-000000000000"), "VE", "Venezuela" },
                { new Guid("00004541-0000-0000-0000-000000000000"), "AE", "Vereinigte Arabische Emirate" },
                { new Guid("00005355-0000-0000-0000-000000000000"), "US", "Vereinigte Staaten" },
                { new Guid("00004247-0000-0000-0000-000000000000"), "GB", "Vereinigtes Königreich" },
                { new Guid("00004e56-0000-0000-0000-000000000000"), "VN", "Vietnam" },
                { new Guid("00004155-0000-0000-0000-000000000000"), "UA", "Ukraine" },
                { new Guid("00005a54-0000-0000-0000-000000000000"), "TZ", "Tansania" },
                { new Guid("00004a54-0000-0000-0000-000000000000"), "TJ", "Tadschikistan" },
                { new Guid("00005953-0000-0000-0000-000000000000"), "SY", "Syrien" },
                { new Guid("00004553-0000-0000-0000-000000000000"), "SE", "Schweden" },
                { new Guid("00004843-0000-0000-0000-000000000000"), "CH", "Schweiz" },
                { new Guid("00004e53-0000-0000-0000-000000000000"), "SN", "Senegal" },
                { new Guid("00005352-0000-0000-0000-000000000000"), "RS", "Serbien" },
                { new Guid("00004353-0000-0000-0000-000000000000"), "SC", "Seychellen" },
                { new Guid("00004c53-0000-0000-0000-000000000000"), "SL", "Sierra Leone" },
                { new Guid("0000575a-0000-0000-0000-000000000000"), "ZW", "Simbabwe" },
                { new Guid("00004753-0000-0000-0000-000000000000"), "SG", "Singapur" },
                { new Guid("00004b53-0000-0000-0000-000000000000"), "SK", "Slowakei" },
                { new Guid("00004953-0000-0000-0000-000000000000"), "SI", "Slowenien" },
                { new Guid("00004f53-0000-0000-0000-000000000000"), "SO", "Somalia" },
                { new Guid("00005345-0000-0000-0000-000000000000"), "ES", "Spanien" },
                { new Guid("00004b4c-0000-0000-0000-000000000000"), "LK", "Sri Lanka" },
                { new Guid("00004e4b-0000-0000-0000-000000000000"), "KN", "St. Kitts und Nevis" },
                { new Guid("0000434c-0000-0000-0000-000000000000"), "LC", "St. Lucia" },
                { new Guid("00004356-0000-0000-0000-000000000000"), "VC", "St. Vincent und die Grenadinen" },
                { new Guid("0000415a-0000-0000-0000-000000000000"), "ZA", "Südafrika" },
                { new Guid("00004453-0000-0000-0000-000000000000"), "SD", "Sudan" },
                { new Guid("00005353-0000-0000-0000-000000000000"), "SS", "Südsudan" },
                { new Guid("00005253-0000-0000-0000-000000000000"), "SR", "Suriname" },
                { new Guid("00005a53-0000-0000-0000-000000000000"), "SZ", "Swasiland" },
                { new Guid("0000474d-0000-0000-0000-000000000000"), "MG", "Madagaskar" },
                { new Guid("00004643-0000-0000-0000-000000000000"), "CF", "Zentral­afrikanische Republik" },
                { new Guid("0000554c-0000-0000-0000-000000000000"), "LU", "Luxemburg" },
                { new Guid("0000494c-0000-0000-0000-000000000000"), "LI", "Liechtenstein" },
                { new Guid("00005242-0000-0000-0000-000000000000"), "BR", "Brasilien" },
                { new Guid("00004e42-0000-0000-0000-000000000000"), "BN", "Brunei" },
                { new Guid("00004742-0000-0000-0000-000000000000"), "BG", "Bulgarien" },
                { new Guid("00004642-0000-0000-0000-000000000000"), "BF", "Burkina Faso" },
                { new Guid("00004942-0000-0000-0000-000000000000"), "BI", "Burundi" },
                { new Guid("00004c43-0000-0000-0000-000000000000"), "CL", "Chile" },
                { new Guid("00004e43-0000-0000-0000-000000000000"), "CN", "Volksrepublik China" },
                { new Guid("00005243-0000-0000-0000-000000000000"), "CR", "Costa Rica" },
                { new Guid("00004943-0000-0000-0000-000000000000"), "CI", "Elfenbeinküste" },
                { new Guid("00005742-0000-0000-0000-000000000000"), "BW", "Botswana" },
                { new Guid("00004b44-0000-0000-0000-000000000000"), "DK", "Dänemark" },
                { new Guid("00004d44-0000-0000-0000-000000000000"), "DM", "Dominica" },
                { new Guid("00004f44-0000-0000-0000-000000000000"), "DO", "Dominikanische Republik" },
                { new Guid("00004a44-0000-0000-0000-000000000000"), "DJ", "Dschibuti" },
                { new Guid("00004345-0000-0000-0000-000000000000"), "EC", "Ecuador" },
                { new Guid("00005653-0000-0000-0000-000000000000"), "SV", "El Salvador" },
                { new Guid("00005245-0000-0000-0000-000000000000"), "ER", "Eritrea" },
                { new Guid("00004545-0000-0000-0000-000000000000"), "EE", "Estland" },
                { new Guid("00004a46-0000-0000-0000-000000000000"), "FJ", "Fidschi" },
                { new Guid("00004946-0000-0000-0000-000000000000"), "FI", "Finnland" },
                { new Guid("00004544-0000-0000-0000-000000000000"), "DE", "Deutschland" },
                { new Guid("00005246-0000-0000-0000-000000000000"), "FR", "Frankreich" },
                { new Guid("00004142-0000-0000-0000-000000000000"), "BA", "Bosnien und Herzegowina" },
                { new Guid("00005442-0000-0000-0000-000000000000"), "BT", "Bhutan" },
                { new Guid("00004745-0000-0000-0000-000000000000"), "EG", "Ägypten" },
                { new Guid("00004c41-0000-0000-0000-000000000000"), "AL", "Albanien" },
                { new Guid("00005a44-0000-0000-0000-000000000000"), "DZ", "Algerien" },
                { new Guid("00004441-0000-0000-0000-000000000000"), "AD", "Andorra" },
                { new Guid("00004f41-0000-0000-0000-000000000000"), "AO", "Angola" },
                { new Guid("00004741-0000-0000-0000-000000000000"), "AG", "Antigua und Barbuda" },
                { new Guid("00005147-0000-0000-0000-000000000000"), "GQ", "Äquatorialguinea" },
                { new Guid("00005241-0000-0000-0000-000000000000"), "AR", "Argentinien" },
                { new Guid("00004d41-0000-0000-0000-000000000000"), "AM", "Armenien" },
                { new Guid("00004f42-0000-0000-0000-000000000000"), "BO", "Bolivien" },
                { new Guid("00005a41-0000-0000-0000-000000000000"), "AZ", "Aserbaidschan" },
                { new Guid("00005541-0000-0000-0000-000000000000"), "AU", "Australien" },
                { new Guid("00005342-0000-0000-0000-000000000000"), "BS", "Bahamas" },
                { new Guid("00004842-0000-0000-0000-000000000000"), "BH", "Bahrain" },
                { new Guid("00004442-0000-0000-0000-000000000000"), "BD", "Bangladesch" },
                { new Guid("00004242-0000-0000-0000-000000000000"), "BB", "Barbados" },
                { new Guid("00005942-0000-0000-0000-000000000000"), "BY", "Weißrussland" },
                { new Guid("00004542-0000-0000-0000-000000000000"), "BE", "Belgien" },
                { new Guid("00005a42-0000-0000-0000-000000000000"), "BZ", "Belize" },
                { new Guid("00004a42-0000-0000-0000-000000000000"), "BJ", "Benin" },
                { new Guid("00005445-0000-0000-0000-000000000000"), "ET", "Äthiopien" },
                { new Guid("00004147-0000-0000-0000-000000000000"), "GA", "Gabun" },
                { new Guid("00004d47-0000-0000-0000-000000000000"), "GM", "Gambia" },
                { new Guid("00004547-0000-0000-0000-000000000000"), "GE", "Georgien" },
                { new Guid("00005a4b-0000-0000-0000-000000000000"), "KZ", "Kasachstan" },
                { new Guid("00004151-0000-0000-0000-000000000000"), "QA", "Katar" },
                { new Guid("0000454b-0000-0000-0000-000000000000"), "KE", "Kenia" },
                { new Guid("0000474b-0000-0000-0000-000000000000"), "KG", "Kirgisistan" },
                { new Guid("0000494b-0000-0000-0000-000000000000"), "KI", "Kiribati" },
                { new Guid("00004f43-0000-0000-0000-000000000000"), "CO", "Kolumbien" },
                { new Guid("00004d4b-0000-0000-0000-000000000000"), "KM", "Komoren" },
                { new Guid("00004443-0000-0000-0000-000000000000"), "CD", "Kongo, Demokratische Republik" },
                { new Guid("00004743-0000-0000-0000-000000000000"), "CG", "Kongo, Republik" },
                { new Guid("00005643-0000-0000-0000-000000000000"), "CV", "Kap Verde" },
                { new Guid("0000504b-0000-0000-0000-000000000000"), "KP", "Korea, Nord" },
                { new Guid("00005248-0000-0000-0000-000000000000"), "HR", "Kroatien" },
                { new Guid("00005543-0000-0000-0000-000000000000"), "CU", "Kuba" },
                { new Guid("0000574b-0000-0000-0000-000000000000"), "KW", "Kuwait" },
                { new Guid("0000414c-0000-0000-0000-000000000000"), "LA", "Laos" },
                { new Guid("0000534c-0000-0000-0000-000000000000"), "LS", "Lesotho" },
                { new Guid("0000564c-0000-0000-0000-000000000000"), "LV", "Lettland" },
                { new Guid("0000424c-0000-0000-0000-000000000000"), "LB", "Libanon" },
                { new Guid("0000524c-0000-0000-0000-000000000000"), "LR", "Liberia" },
                { new Guid("0000594c-0000-0000-0000-000000000000"), "LY", "Libyen" },
                { new Guid("0000524b-0000-0000-0000-000000000000"), "KR", "Korea, Süd" },
                { new Guid("00004143-0000-0000-0000-000000000000"), "CA", "Kanada" },
                { new Guid("00004d43-0000-0000-0000-000000000000"), "CM", "Kamerun" },
                { new Guid("0000484b-0000-0000-0000-000000000000"), "KH", "Kambodscha" },
                { new Guid("00004847-0000-0000-0000-000000000000"), "GH", "Ghana" },
                { new Guid("00004447-0000-0000-0000-000000000000"), "GD", "Grenada" },
                { new Guid("00005247-0000-0000-0000-000000000000"), "GR", "Griechenland" },
                { new Guid("00005447-0000-0000-0000-000000000000"), "GT", "Guatemala" },
                { new Guid("00004e47-0000-0000-0000-000000000000"), "GN", "Guinea" },
                { new Guid("00005747-0000-0000-0000-000000000000"), "GW", "Guinea-Bissau" },
                { new Guid("00005947-0000-0000-0000-000000000000"), "GY", "Guyana" },
                { new Guid("00005448-0000-0000-0000-000000000000"), "HT", "Haiti" },
                { new Guid("00004e48-0000-0000-0000-000000000000"), "HN", "Honduras" },
                { new Guid("00004e49-0000-0000-0000-000000000000"), "IN", "Indien" },
                { new Guid("00004449-0000-0000-0000-000000000000"), "ID", "Indonesien" },
                { new Guid("00005149-0000-0000-0000-000000000000"), "IQ", "Irak" },
                { new Guid("00005249-0000-0000-0000-000000000000"), "IR", "Iran" },
                { new Guid("00004549-0000-0000-0000-000000000000"), "IE", "Irland" },
                { new Guid("00005349-0000-0000-0000-000000000000"), "IS", "Island" },
                { new Guid("00004c49-0000-0000-0000-000000000000"), "IL", "Israel" },
                { new Guid("00005449-0000-0000-0000-000000000000"), "IT", "Italien" },
                { new Guid("00004d4a-0000-0000-0000-000000000000"), "JM", "Jamaika" },
                { new Guid("0000504a-0000-0000-0000-000000000000"), "JP", "Japan" },
                { new Guid("00004559-0000-0000-0000-000000000000"), "YE", "Jemen" },
                { new Guid("00004f4a-0000-0000-0000-000000000000"), "JO", "Jordanien" },
                { new Guid("0000544c-0000-0000-0000-000000000000"), "LT", "Litauen" },
                { new Guid("00005943-0000-0000-0000-000000000000"), "CY", "Zypern" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Candidates_CountryId",
            table: "Candidates",
            column: "CountryId");

        migrationBuilder.AddForeignKey(
            name: "FK_Candidates_Countries_CountryId",
            table: "Candidates",
            column: "CountryId",
            principalTable: "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
