// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Eawv.Service.Data.Countries;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Models.TemplateServiceModels;
using Ech0010_6_0;
using Ech0155_4_0;
using Ech0157_4_0;
using Voting.Lib.Common;
using Voting.Lib.Ech;
using Voting.Lib.Ech.Ech0157_4_0.Models;
using ElectionType = Eawv.Service.DataAccess.Entities.ElectionType;

namespace Eawv.Service.Services;

public class EchService
{
    private const string MatchOneOrMoreDigitsPattern = @"\d+";
    private const string UnknownValue = "-";
    private const string MajorityElectionDefaultCandidateReference = "0";
    private const string DefaultListOrderOfPrecedence = "99";

    private static readonly Dictionary<ElectionType, TypeOfElectionType> ElectionTypeMapping =
        new()
        {
            [ElectionType.Proporz] = TypeOfElectionType.Item1,
            [ElectionType.Majorz] = TypeOfElectionType.Item2,
        };

    private static readonly Dictionary<SexType, Ech0044_4_1.SexType> SexMapping =
        new()
        {
            [SexType.Male] = Ech0044_4_1.SexType.Item1,
            [SexType.Female] = Ech0044_4_1.SexType.Item2,
            [SexType.Undefined] = Ech0044_4_1.SexType.Item3,
        };

    private readonly DeliveryHeaderProvider _deliveryHeaderProvider;
    private readonly EchSerializer _echSerializer;

    public EchService(DeliveryHeaderProvider deliveryHeaderProvider, EchSerializer echSerializer)
    {
        _deliveryHeaderProvider = deliveryHeaderProvider;
        _echSerializer = echSerializer;
    }

    public void WriteXml(TemplateType type, TemplateBag bag, Stream stream)
    {
        if (type != TemplateType.ECH157)
        {
            throw new ArgumentException($"Cannot render {type} in xml", nameof(type));
        }

        var electionInfo = GetElection(bag, _echSerializer);
        var groupBallot = new EventInitialDeliveryElectionGroupBallot
        {
            DomainOfInfluenceIdentification = bag.Election.DomainsOfInfluence.First().DomainOfInfluence.OfficialId,
            ElectionInformation = { electionInfo },
        };

        var contest = new ContestType
        {
            ContestIdentification = bag.Election.Id.ToString(),
            ContestDate = bag.Election.ContestDate,
            ContestDescription = null,
        };
        var eventInitialDelivery = new EventInitialDelivery
        {
            Contest = contest,
            ElectionGroupBallot = { groupBallot },
        };
        var delivery = new Delivery
        {
            DeliveryHeader = _deliveryHeaderProvider.BuildHeader(),
            InitialDelivery = eventInitialDelivery,
        };

        _echSerializer.WriteXml(stream, delivery);
    }

    private static MrMrsType ToEchMrMrsType(SexType sex)
    {
        return sex == SexType.Male
            ? MrMrsType.Item2
            : MrMrsType.Item1;
    }

    /// <summary>
    /// Gets the eCH partyAffiliation element from the passed partyNameShort.
    /// Note: optional partyNameLong is not required from business perspective and therefore not set.
    /// </summary>
    /// <param name="partyNameShort">The party name short free text string (max. 12 characters).</param>
    /// <returns>The list of <see cref="PartyAffiliationformationTypePartyAffiliationInfo"/> eCH element or null if party name short is null or whitespace.</returns>
    private static List<PartyAffiliationformationTypePartyAffiliationInfo> GetPartyAffiliation(string partyNameShort)
    {
        if (string.IsNullOrWhiteSpace(partyNameShort))
        {
            return null;
        }

        return
        [
            new PartyAffiliationformationTypePartyAffiliationInfo
            {
                Language = Languages.German,
                PartyAffiliationShort = partyNameShort,
            }
        ];
    }

    /// <summary>
    /// Gets the listOrderOfPrecedence value from the listIndentureNumber or the <see cref="DefaultListOrderOfPrecedence"/> if it is unknown or not parsable.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The value for listOrderOfPrecedence.</returns>
    private static string GetListOrderOfPrecedence(List list)
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

        return int.TryParse(listIndentureDigits, out _) ? listIndentureDigits : DefaultListOrderOfPrecedence;
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

    private static EventInitialDeliveryElectionGroupBallotElectionInformation GetElection(TemplateBag bag, EchSerializer echSerializer)
    {
        var desc = new ElectionDescriptionInformationTypeElectionDescriptionInfo
        {
            Language = Languages.German,
            ElectionDescription = bag.Election.Name,
        };
        var election = new Ech0155_4_0.ElectionType
        {
            ElectionPosition = "1",
            ElectionIdentification = bag.Election.Id.ToString(),
            TypeOfElection = ElectionTypeMapping[bag.Election.ElectionType],
            NumberOfMandates = bag.ElectionNumberOfMandates.ToString(),
            ElectionDescription = { desc },
        };

        var candidates = GetCandidates(bag).ToList();
        var lists = GetLists(bag).ToList();
        var listUnions = GetListUnions(bag).ToList();
        var electionInformationExtension = GetElectionInformationExtension(bag);
        ExtensionType electionInformationExtensionType = null;
        if (electionInformationExtension.Candidates?.Count > 0)
        {
            electionInformationExtensionType = new ExtensionType();
            electionInformationExtensionType.Any.Add(echSerializer.Serialize(electionInformationExtension));
        }

        return new EventInitialDeliveryElectionGroupBallotElectionInformation
        {
            Election = election,
            Candidate = candidates,
            List = lists,
            ListUnion = listUnions,
            Extension = electionInformationExtensionType,
        };
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

            var occupationInfo = new List<OccupationalTitleInformationTypeOccupationalTitleInfo>
            {
                new()
                {
                    Language = Languages.German,
                    OccupationalTitle = candidate.OccupationalTitle,
                },
            };

            yield return new CandidateType
            {
                CandidateIdentification = candidate.Id.ToString(),
                FamilyName = string.IsNullOrEmpty(candidate.BallotFamilyName)
                    ? candidate.FamilyName
                    : candidate.BallotFamilyName,
                FirstName = candidate.FirstName,
                CallName = string.IsNullOrEmpty(candidate.BallotFirstName)
                    ? candidate.FirstName
                    : candidate.BallotFirstName,
                Title = candidate.Title,
                CandidateReference = candidateReference,
                CandidateText = null,
                DateOfBirth = candidate.DateOfBirth,
                Sex = SexMapping[candidate.Sex],
                OccupationalTitle = occupationInfo,
                DwellingAddress = GetCandidateDwellingAddress(candidate),
                Swiss = { string.IsNullOrEmpty(candidate.Origin) ? UnknownValue : candidate.Origin },
                MrMrs = ToEchMrMrsType(candidate.Sex),
                LanguageOfCorrespondence = Languages.German,
                IncumbentYesNo = candidate.Incumbent,
                Role = null,
                PartyAffiliation = GetPartyAffiliation(candidate.Party),
            };
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
        var country = CountryProvider.GetCountryFromIsoId(candidate.Country);
        var countryIso = country?.IsoId ?? CountryProvider.SwissCountryIso;
        var isSwiss = CountryProvider.IsSwissCountry(countryIso);
        var zipCodeIsSwiss = int.TryParse(candidate.ZipCode, out var zipCode) && zipCode is >= 1000 and <= 9999;
        if (!zipCodeIsSwiss && isSwiss)
        {
            // Fallback for invalid stored data. Without this, the export would not be valid.
            zipCode = 1000;
        }

        var town = string.IsNullOrEmpty(candidate.Locality) ? UnknownValue : candidate.Locality;
        if (!string.IsNullOrEmpty(candidate.BallotLocality) &&
            !town.Equals(candidate.BallotLocality, StringComparison.Ordinal))
        {
            town = candidate.BallotLocality;
        }

        return new AddressInformationType
        {
            SwissZipCode = isSwiss ? (uint?)zipCode : null,
            ForeignZipCode = !isSwiss ? candidate.ZipCode : null,
            Town = town,
            Street = candidate.Street,
            HouseNumber = string.IsNullOrEmpty(candidate.HouseNumber) ? null : candidate.HouseNumber,
            Country = new CountryType
            {
                CountryId = (ushort?)(country?.Id ?? CountryProvider.SwissCountryId),
                CountryIdIso2 = countryIso,
                CountryNameShort = country?.Description ?? CountryProvider.SwissCountryNameShort,
            },
        };
    }

    private static IEnumerable<ListType> GetLists(TemplateBag bag)
    {
        foreach (var list in bag.Election.Lists)
        {
            var listDescription = GetListDescription(list);
            var listDescriptions = new List<ListDescriptionInformationTypeListDescriptionInfo>
            {
                new()
                {
                    Language = Languages.German,
                    ListDescription = listDescription,
                    ListDescriptionShort = Truncate(listDescription, 20),
                },
            };

            var candidatePositions = new List<CandidatePositionInformationType>();

            var i = 0;
            foreach (var candidate in bag.GetClonedAndOrderedCandidates(list))
            {
                var candidateTextInfo = new CandidateTextInformationTypeCandidateTextInfo()
                {
                    Language = Languages.German,
                    CandidateText = $"{candidate.BallotFamilyName} {candidate.BallotFirstName}",
                };
                var candidatePosition = new CandidatePositionInformationType
                {
                    PositionOnList = $"{++i}",
                    CandidateReferenceOnPosition = candidate.Index.ToString("D2", CultureInfo.InvariantCulture),
                    CandidateIdentification = candidate.Id.ToString(),
                    CandidateTextOnPosition = { candidateTextInfo },
                };
                candidatePositions.Add(candidatePosition);
            }

            yield return new ListType
            {
                ListIdentification = list.Id.ToString(),
                ListIndentureNumber = GetListIndentureNumber(list),
                ListDescription = listDescriptions,
                IsEmptyList = list.Candidates.Count == 0,
                ListOrderOfPrecedence = GetListOrderOfPrecedence(list),
                TotalPositionsOnList = candidatePositions.Count.ToString(),
                CandidatePosition = candidatePositions,
                ListUnionBallotText = null,
            };
        }
    }

    private static IEnumerable<ListUnionType> GetListUnions(TemplateBag bag)
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
            var desc = new ListUnionDescriptionTypeListUnionDescriptionInfo
            {
                Language = Languages.German,
                ListUnionDescription = listUnion.Id.ToString(),
            };
            var type = listUnion.IsSubUnion ? ListRelationType.Item2 : ListRelationType.Item1;
            yield return new ListUnionType
            {
                ListUnionIdentification = listUnion.Id.ToString(),
                ListUnionDescription = { desc },
                ListUnionTypeProperty = type,
                ReferencedList = lists,
                ReferencedListUnion = listUnion.RootList?.ListUnionId?.ToString(),
            };
        }
    }

    private static string Truncate(string s, int maxLength)
        => s.Length > maxLength ? s[..maxLength] : s;
}
