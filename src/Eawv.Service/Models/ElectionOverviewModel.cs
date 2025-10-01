// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Eawv.Service.DataAccess.Entities;

namespace Eawv.Service.Models;

public class ElectionOverviewModel : BaseEntityModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime SubmissionDeadlineBegin { get; set; }

    public DateTime SubmissionDeadlineEnd { get; set; }

    public DateTime ContestDate { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public ElectionType ElectionType { get; set; }

    public int State { get; set; }

    public bool IsArchived { get; set; }
}
