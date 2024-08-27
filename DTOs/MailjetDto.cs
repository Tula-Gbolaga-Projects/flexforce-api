using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace agency_portal_api.DTOs
{
    public class SendTemplateEmail
    {
        public string Subject { get; set; }
        public string ReciepeintAddress { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; } = "";
        public string SenderName { get; set; }
        public List<MailjetUserDetails> Cc { get; set; }
        public List<MailjetUserDetails> Bcc { get; set; }
    }

    public class MailjetUserDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class MailJetAttachment
    {
        public string ContentType { get; set; }
        public string Filename { get; set; }
        public string Base64Content { get; set; }
    }

    public class MailJetCustomProp
    {
        public MailjetUserDetails From { get; set; }
        public string Subject { get; set; }
        public string TextPart { get; set; }
        public string HTMLPart { get; set; }
        public List<MailjetUserDetails> To { get; set; }
        public List<MailjetUserDetails> Cc { get; set; }
        public List<MailjetUserDetails> Bcc { get; set; }
        public List<MailJetAttachment> Attachments { get; set; }

        public MailJetCustomProp()
        {
            To = new List<MailjetUserDetails>();
        }
    }

    public class BaseMailjetVariable
    {
        public string UserName { get; set; } = "User";
        public string CustomMessage { get; set; } = "";
    }

}