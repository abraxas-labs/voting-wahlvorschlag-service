// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Eawv.Service.Models.NotificationServiceModels;

public class SendEmailRequestModel
{
    public List<RecipientModel> Recipients { get; set; }

    public List<RecipientModel> Bcc { get; set; }

    public MessageModel Message { get; set; }

    public EmailSenderModel Sender { get; set; }
}
