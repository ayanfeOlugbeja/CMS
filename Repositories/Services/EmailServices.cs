using System;
using System.Threading.Tasks;
using CMS.Repositories.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace CMS.Repositories.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _apiKey;

        public SendGridEmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"]
                      ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                      ?? throw new Exception("SendGrid API key not found.");
        }

        // Implements IEmailService.SendEmail (sync)
        public void SendEmail(string to, string subject, string body)
        {
            // Call the async method but block (not ideal, but satisfies interface)
            SendEmailAsync(to, subject, body).GetAwaiter().GetResult();
        }

        // Implements IEmailService.SendEmailAsync (async)
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress("ogunbanwofemi2000@gmail.com", "CMS App");
                var toEmail = new EmailAddress(to);

                var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, body, body);
                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid email failed. Status: {response.StatusCode}, Body: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Email sending failed: {ex.Message}", ex);
            }
        }
    }
}
