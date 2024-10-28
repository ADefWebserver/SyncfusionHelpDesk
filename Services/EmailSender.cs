using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SyncfusionHelpDesk
{
    public class EmailSender
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmailSender(
            IConfiguration Configuration,
            IHttpContextAccessor HttpContextAccessor)
        {
            configuration = Configuration;
            httpContextAccessor = HttpContextAccessor;
        }

        public async Task SendEmail(
            string EmailType,
            string EmailAddress,
            string TicketGuid)
        {
            try
            {
                // Email settings
                SendGridMessage msg = new SendGridMessage();
                var apiKey = configuration["SENDGRID_APIKEY"];
                var senderEmail = configuration["SenderEmail"];
                var client = new SendGridClient(apiKey);
                var FromEmail = new EmailAddress(
                    senderEmail,
                    senderEmail
                    );

                // Format Email contents
                string strPlainTextContent =
                    $"{EmailType}: {GetHelpDeskTicketUrl(TicketGuid)}";
                string strHtmlContent =
                    $"<b>{EmailType}:</b> ";
                strHtmlContent = strHtmlContent +
                    $"<a href='{ GetHelpDeskTicketUrl(TicketGuid) }'>";
                strHtmlContent = strHtmlContent +
                    $"{GetHelpDeskTicketUrl(TicketGuid)}</a>";

                if (EmailType == "Help Desk Ticket Created")
                {
                    msg = new SendGridMessage()
                    {
                        From = FromEmail,
                        Subject = EmailType,
                        PlainTextContent = strPlainTextContent,
                        HtmlContent = strHtmlContent
                    };

                    // Created Email always goes to Administrator
                    // Send to senderEmail configured in appsettings.json
                    msg.AddTo(new EmailAddress(senderEmail, EmailType));
                }

                if (EmailType == "Help Desk Ticket Updated")
                {
                    msg = new SendGridMessage()
                    {
                        From = FromEmail,
                        Subject = EmailType,
                        PlainTextContent = strPlainTextContent,
                        HtmlContent = strHtmlContent
                    };

                    // Updated emails go to Administrator or Ticket creator
                    // Send to EmailAddress passed to method
                    msg.AddTo(new EmailAddress(EmailAddress, EmailType));
                }

                var response = await client.SendEmailAsync(msg);
            }
            catch
            {
                // Could not send email
                // Perhaps SENDGRID_APIKEY not set in 
                // appsettings.json
            }
        }

        // Utility

        #region public string GetHelpDeskTicketUrl(string TicketGuid)
        public string GetHelpDeskTicketUrl(string TicketGuid)
        {
            var request = httpContextAccessor.HttpContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $@"{request.Scheme}://{host}{pathBase}/emailticketedit/{TicketGuid}";
        }
        #endregion
    }
}
