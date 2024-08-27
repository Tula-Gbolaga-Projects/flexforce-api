using agency_portal_api.DTOs;
using agency_portal_api.Entities;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace agency_portal_api.Services
{
    public interface IMailJetService
    {
        Task<bool> SendMail(string reciepientAddress, string message, string subject, CancellationToken token, bool isHtml = false, List<MailjetUserDetails> Bcc = null, List<MailjetUserDetails> Cc = null);
    }

    public class MailJetService : IMailJetService
    {
        private readonly IMailjetClient mailjetClient;
        private readonly string Name;
        private readonly string Email;

        public MailJetService(IMailjetClient mailjetClient, IConfiguration configuration)
        {
            this.mailjetClient = mailjetClient;
            this.Name = configuration.GetSection("MailJet:Name").Value;
            this.Email = configuration.GetSection("MailJet:Email").Value;
        }

        public async Task<bool> SendMail(string reciepientAddress, string message, string subject, CancellationToken token, bool isHtml = false, List<MailjetUserDetails> Bcc = null, List<MailjetUserDetails> Cc = null)
        {
            var mailjetSendClass = new List<MailJetCustomProp>();
            var reciepients = new List<MailjetUserDetails>();
            reciepients.Add(new MailjetUserDetails()
            {
                Email = reciepientAddress,
                Name = reciepientAddress
            });


            mailjetSendClass.Add(new MailJetCustomProp()
            {
                From = new MailjetUserDetails()
                {
                    Name = this.Name,
                    Email = this.Email
                },
                HTMLPart = isHtml ? message : null,
                Subject = subject,
                TextPart = message,
                To = reciepients,
                Bcc = Bcc,
                Cc = Cc
            });

            var result = JsonSerializer.Serialize<List<MailJetCustomProp>>(mailjetSendClass);

            var arr = JArray.Parse(result);

            MailjetRequest request = new MailjetRequest()
            {
                Resource = new ResourceInfo("send", null, ApiVersion.V3_1, ResourceType.Send)
            }.Property(Send.Messages, arr);

            var response = await mailjetClient.PostAsync(request);

            return response.IsSuccessStatusCode;
        }

    }
}
