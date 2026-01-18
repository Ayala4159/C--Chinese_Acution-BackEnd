
using System.Net;
using System.Net.Mail;

namespace Chinese_Auction.Services
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var portString = _configuration["EmailSettings:Port"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:SenderPassword"];
            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Email settings are missing in appsettings.json");
            }

            int port = int.TryParse(portString, out var p) ? p : 587;

            var client = new SmtpClient(smtpServer, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}