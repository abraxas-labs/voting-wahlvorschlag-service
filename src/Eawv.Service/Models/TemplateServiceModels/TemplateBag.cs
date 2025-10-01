// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Eawv.Service.Configuration;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Utils;
using Microsoft.AspNetCore.Html;
using IdentityClient = Voting.Lib.Iam.Services.ApiClient.Identity;
using PermissionClient = Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Models.TemplateServiceModels;

public class TemplateBag
{
    public TemplateEntity Template { get; set; }

    public Election Election { get; set; }

    public List List { get; set; }

    public Setting Settings { get; set; }

    public AppConfig Configuration { get; set; }

    public DateTime Now => DateTime.Now;

    // the pdf serivce injects according values (only in header.pdf / footer.pdf)
    public IHtmlContent CurrentPage => new HtmlString(@"<span class=""pageNumber""></span>");

    public IHtmlContent TotalPages => new HtmlString(@"<span class=""totalPages""></span>");

    public IHtmlContent ElectionTenantLogoDataURI => new HtmlString("data:image/png;base64," + Convert.ToBase64String(Election.TenantLogo));

    public Func<string, Task<PermissionClient.V1Tenant>> GetTenantAsync { get; set; }

    public Func<string, Task<IdentityClient.V1User>> GetUserAsync { get; set; }

    public bool IsMajorz => Election.ElectionType == ElectionType.Majorz;

    public bool IsProporz => Election.ElectionType == ElectionType.Proporz;

    public PermissionClient.V1Tenant ElectionTenant => GetTenant(Election.TenantId);

    public int ElectionNumberOfMandates => Election.DomainsOfInfluence.Sum(x => x.NumberOfMandates);

    public PermissionClient.V1Tenant ListPartyTenant => GetTenant(List.ResponsiblePartyTenantId);

    public IdentityClient.V1User ListRepresentative => GetUser(List.Representative);

    public IdentityClient.V1User ListDeputy => GetUser(List.DeputyUsers?.FirstOrDefault());

    public bool ListIsDraft => List.State == ListState.Draft;

    public string Theme { get; set; }

    public string GetInfoText(string key)
    {
        var txt = Election.InfoTexts.FirstOrDefault(it => it.Key == key);
        return txt?.Visible != true ? string.Empty : txt.Value;
    }

    /// <summary>
    /// Encodes the text as HTML.
    /// Also makes sure that possible links and emails are "encoded" and not recognized by email parsers.
    /// </summary>
    /// <param name="text">The text to encode.</param>
    /// <returns>The encoded text.</returns>
    public IHtmlContent EncodeHtmlAndPossibleLinks(string text)
    {
        var encodedText = HtmlEncoder.Default.Encode(text ?? string.Empty);

        // By replacing these characters, we fool email parses such as Gmail to not parse the highlight the texts as
        // links, to prevent phishing.
        encodedText = encodedText
            .Replace(".", "<span>.</span>")
            .Replace("@", "<span>@</span>")
            .Replace(":", "<span>:</span>");
        return new HtmlString(encodedText);
    }

    public IEnumerable<Candidate> GetClonedAndOrderedCandidates() => GetClonedAndOrderedCandidates(List);

    /// <summary>
    /// Retrieves a list of candidates from the provided list, ordered by their original
    /// index and then by their clone order index. Cloned candidates are inserted back
    /// into the list at their designated clone index, or at the end if their index exceeds
    /// the current candidate count.
    /// </summary>
    /// <param name="list">The list containing candidates to be ordered and processed.</param>
    /// <returns>
    /// A collection of candidates, including cloned candidates, ordered by their original
    /// and clone order indices.
    /// </returns>
    public IEnumerable<Candidate> GetClonedAndOrderedCandidates(List list)
    {
        var candidatesList = list.Candidates
            .OrderBy(c => c.Index)
            .ThenBy(c => c.CloneOrderIndex)
            .ToList();

        foreach (var candidate in list.Candidates.Where(c => c.Cloned).OrderBy(c => c.CloneOrderIndex))
        {
            var cloneIndex = (int)candidate.CloneOrderIndex - 1;
            if (cloneIndex < candidatesList.Count)
            {
                candidatesList.Insert(cloneIndex, candidate);
            }
            else
            {
                candidatesList.Add(candidate);
            }
        }

        return candidatesList;
    }

    private PermissionClient.V1Tenant GetTenant(string id)
    {
        return id == null ? null : AsyncUtils.RunSync(() => GetTenantAsync(id));
    }

    private IdentityClient.V1User GetUser(string id)
    {
        return id == null ? null : AsyncUtils.RunSync(() => GetUserAsync(id));
    }
}
