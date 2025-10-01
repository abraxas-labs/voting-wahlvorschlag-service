// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eawv.Service.Models;

public class CreateElectionModel : UpdateElectionModel
{
    [MaxLength(50)]
    public IList<ModifyBallotDocumentModel> Documents { get; set; }

    [MaxLength(50)]
    public IList<CreateDomainOfInfluenceElectionModel> DomainsOfInfluence { get; set; }

    [MaxLength(100)]
    public IList<ModifyInfoTextModel> InfoTexts { get; set; }
}
