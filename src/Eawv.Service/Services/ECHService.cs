// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Ech.Ech0157.Models;
using Eawv.Service.Ech.Providers;
using Eawv.Service.Models.TemplateServiceModels;
using eCH_0010_6_0;
using eCH_0155_4_0;
using eCH_0157_4_0;
using Voting.Lib.Common;
using ElectionType = Eawv.Service.DataAccess.Entities.ElectionType;

namespace Eawv.Service.Services;

public class EchService
{
    private const string MatchOneOrMoreDigitsPattern = @"\d+";
    private const string UnknownValue = "-";
    private const string SwissCountryIso = "CH";
    private const string SwissCountryNameShort = "Schweiz";
    private const string MajorityElectionDefaultCandidateReference = "0";
    private const int SwissCountryId = 8100;
    private const int DefaultListOrderOfPrecedence = 99;

    private static readonly Dictionary<ElectionType, TypeOfElectionType> ElectionTypeMapping =
        new()
        {
            [ElectionType.Majorz] = TypeOfElectionType.Majorz,
            [ElectionType.Proporz] = TypeOfElectionType.Proporz,
        };

    private static readonly Dictionary<SexType, eCH_0044_4_1.SexType> SexMapping =
        new()
        {
            [SexType.Male] = eCH_0044_4_1.SexType.Männlich,
            [SexType.Female] = eCH_0044_4_1.SexType.Weiblich,
            [SexType.Undefined] = eCH_0044_4_1.SexType.Unbestimmt,
        };

    private readonly DeliveryHeaderProvider _deliveryHeaderProvider;

    public EchService(DeliveryHeaderProvider deliveryHeaderProvider)
    {
        _deliveryHeaderProvider = deliveryHeaderProvider;
    }

    public void WriteXml(TemplateType type, TemplateBag bag, Stream stream)
    {
        if (type != TemplateType.ECH157)
        {
            throw new ArgumentException($"Cannot render {type} in xml", nameof(type));
        }

        var electionInfo = GetElection(bag);
        var electionInfos = new[] { electionInfo };

        var groupBallot = ElectionGroupBallotType.Create(bag.Election.DomainsOfInfluence.First().DomainOfInfluence.OfficialId, electionInfos);
        var groupBallots = new[] { groupBallot };

        var contest = ContestType.Create(bag.Election.Id.ToString(), bag.Election.ContestDate);
        var eventInitialDelivery = EventInitialDeliveryType.Create(contest, groupBallots);
        var delivery = DeliveryType.Create(_deliveryHeaderProvider.BuildHeader(), eventInitialDelivery);

        ToXml(delivery, stream);
    }

    private static MrMrsType ToEchMrMrsType(SexType sex)
    {
        return sex == SexType.Male
            ? MrMrsType.Herr
            : MrMrsType.Frau;
    }

    /// <summary>
    /// Gets the eCH partyAffiliation element from the passed partyNameShort.
    /// Note: optional partyNameLong is not required from business perspective and therefore not set.
    /// </summary>
    /// <param name="partyNameShort">The party name short free text string (max. 12 characters).</param>
    /// <returns>The <see cref="PartyAffiliationInformation"/> eCH element or null if party name short is null or whitespace.</returns>
    private static PartyAffiliationInformation GetPartyAffiliation(string partyNameShort)
    {
        if (string.IsNullOrWhiteSpace(partyNameShort))
        {
            return null;
        }

        var partyAffiliationInfo =
            new List<PartyAffiliationInfo>
            {
                    PartyAffiliationInfo.Create(Languages.German, partyNameShort),
            };

        return PartyAffiliationInformation.Create(partyAffiliationInfo);
    }

    /// <summary>
    /// Gets the listOrderOfPrecedence value from the listIndentureNumber or the <see cref="DefaultListOrderOfPrecedence"/> if it is unknown or not parsable.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The value for listOrderOfPrecedence.</returns>
    private static int? GetListOrderOfPrecedence(List list)
    {
        var listIndenture = GetListIndentureNumber(list);

        if (listIndenture.Equals(UnknownValue, StringComparison.InvariantCulture))
        {
            return DefaultListOrderOfPrecedence;
        }

        var listIndentureDigitsMatches = Regex.Matches(listIndenture, MatchOneOrMoreDigitsPattern);

        if (listIndentureDigitsMatches.Count == 0)
        {
            return DefaultListOrderOfPrecedence;
        }

        var listIndentureDigits = string.Concat(listIndentureDigitsMatches.Select(m => m.Value));

        if (int.TryParse(listIndentureDigits, out var listOrderOfPrecedence))
        {
            return listOrderOfPrecedence;
        }

        return DefaultListOrderOfPrecedence;
    }

    /// <summary>
    /// Gets the list indenture number if defined otherwise the <see cref="UnknownValue"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The value for listIndentureNumber.</returns>
    private static string GetListIndentureNumber(List list)
    {
        return list.Indenture ?? UnknownValue;
    }

    /// <summary>
    /// Gets the list description from list name concatenated with list description if set.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The concatenated list description.</returns>
    private static string GetListDescription(List list)
    {
        if (string.IsNullOrEmpty(list.Description))
        {
            return list.Name;
        }

        return string.Join(", ", list.Name, list.Description);
    }

    private static ElectionInformationType GetElection(TemplateBag bag)
    {
        var desc = ElectionDescriptionInfoType.Create(Languages.German, bag.Election.Name);
        var election = new eCH_0155_4_0.ElectionType
        {
            ElectionPosition = 1,
            ElectionIdentification = bag.Election.Id.ToString(),
            TypeOfElection = ElectionTypeMapping[bag.Election.ElectionType],
            NumberOfMandates = bag.ElectionNumberOfMandates,
            ElectionDescription = ElectionDescriptionInformationType.Create([desc]),
        };

        var candidates = GetCandidates(bag).ToArray();
        var lists = GetLists(bag).ToArray();
        var listUnions = GetListUnions(bag).ToArray();
        var electionInformationExtension = GetElectionInformationExtension(bag);
        ExtensionType electionInformationExtensionType = null;
        if (electionInformationExtension.Candidates.Count > 0)
        {
            electionInformationExtensionType = new ExtensionType() { Extension = electionInformationExtension };
        }

        return ElectionInformationType.Create(election, candidates, lists, listUnions, electionInformationExtensionType);
    }

    private static ElectionInformationExtension GetElectionInformationExtension(TemplateBag bag)
    {
        var extension = new ElectionInformationExtension
        {
            Candidates = [],
        };

        foreach (var candidate in bag.Election.Lists.SelectMany(l => l.Candidates).Where(l => !string.IsNullOrEmpty(l.BallotOccupationalTitle)))
        {
            extension.Candidates.Add(new()
            {
                CandidateIdentification = candidate.Id.ToString(),
                TitleAndOccupation = candidate.BallotOccupationalTitle,
            });
        }

        return extension;
    }

    private static IEnumerable<CandidateType> GetCandidates(TemplateBag bag)
    {
        foreach (var candidate in bag.Election.Lists.SelectMany(l => l.Candidates))
        {
            var candidateReference = bag.Election.ElectionType == ElectionType.Majorz
                    ? MajorityElectionDefaultCandidateReference
                    : candidate.Index.ToString(CultureInfo.InvariantCulture);

            var occupationInfo = new List<OccupationalTitleInfo>
            {
                OccupationalTitleInfo.Create(Languages.German, candidate.OccupationalTitle),
            };

            yield return CandidateType.Create(
                null,
                candidate.Id.ToString(),
                string.IsNullOrEmpty(candidate.BallotFamilyName) ? candidate.FamilyName : candidate.BallotFamilyName,
                candidate.FirstName,
                string.IsNullOrEmpty(candidate.BallotFirstName) ? candidate.FirstName : candidate.BallotFirstName,
                candidate.Title,
                candidateReference,
                null,
                null,
                candidate.DateOfBirth,
                SexMapping[candidate.Sex],
                OccupationalTitleInformation.Create(occupationInfo),
                null,
                null,
                GetCandidateDwellingAddress(candidate),
                Swiss.Create(string.IsNullOrEmpty(candidate.Origin) ? UnknownValue : candidate.Origin),
                ToEchMrMrsType(candidate.Sex),
                Languages.German,
                candidate.Incumbent,
                null,
                GetPartyAffiliation(candidate.Party));
        }
    }

    /// <summary>
    /// Gets the candidate's dwelling address,
    /// where the dwelling address town and zip code is depending on the candidate's political address town <see cref="Candidate.BallotLocality"/>.
    /// If the political address town is defined and is different than the dwelling address town,
    /// then the political address town is mapped to the political address town
    /// and the zip code is cleared, since it is not relevant and matching in this case.
    /// </summary>
    /// <param name="candidate">The candidate model to map to a <see cref="AddressInformationType"/>.</param>
    /// <returns>A mapped <see cref="AddressInformationType"/>.</returns>
    private static AddressInformationType GetCandidateDwellingAddress(Candidate candidate)
    {
        var zipCodeIsSwiss = int.TryParse(candidate.ZipCode, out var zipCode) && zipCode is > 1000 and <= 9999;
        var isDifferentPoliticalAddressTownSet = false;
        var town = string.IsNullOrEmpty(candidate.Locality) ? UnknownValue : candidate.Locality;

        if (!string.IsNullOrEmpty(candidate.BallotLocality) &&
            !town.Equals(candidate.BallotLocality, StringComparison.Ordinal))
        {
            town = candidate.BallotLocality;
            isDifferentPoliticalAddressTownSet = true;
        }

        return new AddressInformationType
        {
            SwissZipCode = zipCodeIsSwiss && !isDifferentPoliticalAddressTownSet ? zipCode : null,
            ForeignZipCode = zipCodeIsSwiss || isDifferentPoliticalAddressTownSet ? null : candidate.ZipCode,
            Town = town,
            Country = CountryType.Create(SwissCountryId, SwissCountryIso, SwissCountryNameShort),
        };
    }

    private static IEnumerable<ListType> GetLists(TemplateBag bag)
    {
        foreach (var list in bag.Election.Lists)
        {
            var listDescriptions = new List<ListDescriptionInfo>
            {
                ListDescriptionInfo.Create(Languages.German, GetListDescription(list), UnknownValue),
            };

            var candidatePositions = new List<CandidatePositionInformation>();

            int i = 0;
            foreach (var candidate in bag.GetClonedAndOrderedCandidates(list))
            {
                var candidateTextInfo = CandidateTextInfo.Create(Languages.German, $"{candidate.BallotFamilyName} {candidate.BallotFirstName}");
                var candidateTextInfos = CandidateTextInformation.Create([candidateTextInfo]);
                var candidatePosition = CandidatePositionInformation.Create(++i, candidate.Index.ToString("D2", CultureInfo.InvariantCulture), candidate.Id.ToString(), candidateTextInfos);
                candidatePositions.Add(candidatePosition);
            }

            yield return new ListType
            {
                ListIdentification = list.Id.ToString(),
                ListIndentureNumber = GetListIndentureNumber(list),
                ListDescription = ListDescriptionInformation.Create(listDescriptions),
                IsEmptyList = list.Candidates.Count == 0,
                ListOrderOfPrecedence = GetListOrderOfPrecedence(list),
                TotalPositionsOnList = candidatePositions.Count,
                CandidatePosition = candidatePositions,
            };
        }
    }

    private static IEnumerable<ListUnionTypeType> GetListUnions(TemplateBag bag)
    {
        var listUnions = bag.Election.Lists
            .Select(x => x.ListUnion)
            .Concat(bag.Election.Lists.Select(l => l.ListSubUnion))
            .Where(lu => lu != null)
            .GroupBy(lu => lu.Id)
            .Select(g => g.First());

        foreach (var listUnion in listUnions)
        {
            var lists = listUnion.Lists.Select(l => l.Id.ToString()).ToList();
            var desc = ListUnionDescriptionInfoType.Create(Languages.German, listUnion.Id.ToString());
            var description = ListUnionDescriptionType.Create([desc]);
            var type = listUnion.IsSubUnion ? ListRelationType.SubListUnion : ListRelationType.ListUnion;
            yield return ListUnionTypeType.Create(listUnion.Id.ToString(), description, type, lists);
        }
    }

    private static void ToXml(object o, Stream stream)
    {
        var serializer = new XmlSerializer(o.GetType(), [typeof(ElectionInformationExtension)]);
        using var writer = new StreamWriter(stream);
        serializer.Serialize(writer, o);
    }
}
