// using System.Net;
// using System.Net.Mail;
// using CMS.Repositories.Interfaces;

// namespace CMS.Repositories.Services
// {
//     public class EmailService : IEmailService
//     {
//         public void SendEmail(string to, string subject, string body)
//         {
//             try
//             {
//                 var smtpClient = new SmtpClient("smtp.gmail.com")
//                 {
//                     Port = 587,
//                     Credentials = new NetworkCredential("ogunbanwofemi2000@gmail.com", "zdppgdttjfphwnbk"),
//                     EnableSsl = true,
//                 };

//                 var mailMessage = new MailMessage
//                 {
//                     From = new MailAddress("ogunbanwofemi2000@gmail.com", "CMS App"),
//                     Subject = subject,
//                     Body = body,
//                     IsBodyHtml = false,
//                 };

//                 mailMessage.To.Add(to);

//                 smtpClient.Send(mailMessage);
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception($"Email sending failed: {ex.Message}");
//             }
//         }
//     }
// }
using System;
using System.Threading.Tasks;
using CMS.Repositories.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CMS.Repositories.Services // ✅ must match the folder structure
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _apiKey;

      public SendGridEmailService(IConfiguration configuration)
{
    _apiKey = configuration["SendGrid:ApiKey"] 
              ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
              ?? throw new Exception("SendGrid API key not found in configuration or environment variables.");
}

        public async void SendEmail(string to, string subject, string body)
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
                throw new Exception($"Email sending failed: {ex.Message}");
            }
        }
    }
}
