// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Models;

public class ListCommentModel : BaseEntityModel
{
    public string Content { get; set; }

    public string CreatorFirstName { get; set; } = string.Empty;

    public string CreatorLastName { get; set; } = string.Empty;
}
