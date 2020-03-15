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

        public async Task SendEmail()
        {
            var apiKey = configuration["SENDGRID_APIKEY"];
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Webmaster@adefwebserver.com", "Sample Email"),
                Subject = "Hello World from the SendGrid CSharp SDK!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress("Webmaster@adefwebserver.com", "Test User"));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
