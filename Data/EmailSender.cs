using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncfusionHelpDesk
{
    public class EmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration Configuration)
        {
            configuration = Configuration;
        }

        public async Task SendEmail(string EmailAddress, string EmailSubject)
        {
            var apiKey = configuration["SENDGRID_APIKEY"];
            var senderEmail = configuration["SenderEmail"];
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(senderEmail, senderEmail),
                Subject = EmailSubject,
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress(EmailAddress, EmailSubject));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
