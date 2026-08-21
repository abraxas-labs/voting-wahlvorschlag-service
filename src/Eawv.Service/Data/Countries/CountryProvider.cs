// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Eawv.Service.Models;

namespace Eawv.Service.Data.Countries;

public static class CountryProvider
{
    public const int SwissCountryId = 8100;
    public const string SwissCountryIso = "CH";
    public const string SwissCountryNameShort = "Schweiz";

    private const string BfsCountryListFile = "Eawv.Service.Data.Countries.BFSCountryList.xml";
    private static readonly List<CountryXmlModel> Countries = GetCountryList();

    public static List<CountryXmlModel> GetAll()
    {
        return Countries;
    }

    public static CountryXmlModel GetCountryFromIsoId(string isoId)
    {
        return Countries.Find(x => x.IsoId.Equals(isoId, StringComparison.InvariantCultureIgnoreCase));
    }

    public static bool IsSwissCountry(string isoId)
    {
        return SwissCountryIso.Equals(isoId, StringComparison.InvariantCultureIgnoreCase);
    }

    private static List<CountryXmlModel> GetCountryList()
    {
        var serializer = new XmlSerializer(typeof(CountryXmlRootModel));
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(BfsCountryListFile)
            ?? throw new FileNotFoundException(BfsCountryListFile);

        using var reader = new StreamReader(stream);
        var rootModel = serializer.Deserialize(reader) as CountryXmlRootModel;

        ArgumentNullException.ThrowIfNull(rootModel?.Country);

        return rootModel.Country
            .Where(x => x.EntryValid && (x.RecognizedCh || x.IsoId.Equals(SwissCountryIso, StringComparison.InvariantCultureIgnoreCase)))
            .OrderBy(x => x.Description)
            .ToList();
    }
}
