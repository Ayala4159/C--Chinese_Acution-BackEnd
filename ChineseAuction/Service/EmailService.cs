
using System.Net;
using System.Net.Mail;

namespace Chinese_Auction.Services
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendLotteryWinnerEmailAsync(string toEmail, string recipientName, string giftName, string giftDescription, decimal giftValue, string donorName);
    }

    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
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
                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                throw;
            }
        }

        public async Task SendLotteryWinnerEmailAsync(string toEmail, string recipientName, string giftName, string giftDescription, decimal giftValue, string donorName)
        {
            try
            {
                string subject = "🎉 ברכות! אתה הזוכה בהגרלה שלנו! | Congratulations! You're a Winner!";
                
                string htmlBody = GenerateLotteryWinnerHtmlEmail(recipientName, giftName, giftDescription, giftValue, donorName);
                
                await SendEmailAsync(toEmail, subject, htmlBody);
                _logger.LogInformation("Lottery winner email sent successfully to {ToEmail} for gift {GiftName}", toEmail, giftName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send lottery winner email to {ToEmail}", toEmail);
                throw;
            }
        }

        private string GenerateLotteryWinnerHtmlEmail(string recipientName, string giftName, string giftDescription, decimal giftValue, string donorName)
        {
            return $@"
<!DOCTYPE html>
<html lang='he' dir='rtl'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: 'Arial', 'Segoe UI', sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-align: center;
            padding: 40px 20px;
        }}
        .header h1 {{
            font-size: 32px;
            margin: 0;
            font-weight: bold;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
        }}
        .header p {{
            margin: 10px 0 0 0;
            font-size: 16px;
            opacity: 0.9;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .greeting {{
            font-size: 18px;
            color: #333;
            margin-bottom: 20px;
            line-height: 1.6;
        }}
        .gift-card {{
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            border-left: 5px solid #667eea;
            padding: 25px;
            margin: 25px 0;
            border-radius: 10px;
        }}
        .gift-card-title {{
            font-size: 14px;
            color: #666;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-bottom: 10px;
        }}
        .gift-name {{
            font-size: 26px;
            color: #667eea;
            font-weight: bold;
            margin: 10px 0;
        }}
        .gift-description {{
            color: #555;
            font-size: 14px;
            line-height: 1.6;
            margin: 15px 0;
        }}
        .gift-details {{
            display: flex;
            justify-content: space-between;
            margin-top: 15px;
            border-top: 1px solid rgba(0, 0, 0, 0.1);
            padding-top: 15px;
        }}
        .detail {{
            text-align: center;
        }}
        .detail-label {{
            font-size: 12px;
            color: #999;
            text-transform: uppercase;
            margin-bottom: 5px;
        }}
        .detail-value {{
            font-size: 16px;
            color: #333;
            font-weight: bold;
        }}
        .footer-message {{
            background: #f0f4ff;
            padding: 20px;
            border-radius: 10px;
            margin-top: 25px;
            color: #555;
            font-size: 14px;
            line-height: 1.6;
        }}
        .cta-button {{
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 25px;
            margin-top: 20px;
            font-weight: bold;
            text-align: center;
        }}
        .footer {{
            background: #f9f9f9;
            padding: 20px;
            text-align: center;
            color: #999;
            font-size: 12px;
            border-top: 1px solid #eee;
        }}
        .celebration {{
            text-align: center;
            font-size: 40px;
            margin: 10px 0;
        }}
        @media (prefers-color-scheme: dark) {{
            body {{
                background: #1a1a1a;
            }}
            .container {{
                background: #2d2d2d;
            }}
            .content {{
                color: #e0e0e0;
            }}
            .greeting {{
                color: #e0e0e0;
            }}
            .gift-card {{
                background: #3a3a3a;
            }}
            .gift-description {{
                color: #b0b0b0;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='celebration'>🎉 🍾 🎁</div>
            <h1>!אתה הזוכה!</h1>
            <p>Congratulations! You've Won!</p>
        </div>
        
        <div class='content'>
            <p class='greeting'>
                שלום {recipientName},<br>
                <br>
                בברכה ובשמחה, אנחנו שמחים להדיע לך שאתה זכית בהגרלה שלנו! 🎊
                <br><br>
                Dear {recipientName},<br>
                We are delighted to announce that you have won our lottery!
            </p>
            
            <div class='gift-card'>
                <div class='gift-card-title'>🎁 הפרס שלך | Your Prize</div>
                <div class='gift-name'>{giftName}</div>
                <div class='gift-description'>
                    <strong>תיאור | Description:</strong><br>
                    {giftDescription}
                </div>
                <div class='gift-details'>
                    <div class='detail'>
                        <div class='detail-label'>שווי | Value</div>
                        <div class='detail-value'>₪{giftValue}</div>
                    </div>
                    <div class='detail'>
                        <div class='detail-label'>תורם | Donor</div>
                        <div class='detail-value'>{donorName}</div>
                    </div>
                </div>
            </div>
            
            <div class='footer-message'>
                <strong>💡 מה עכשיו? | Next Steps?</strong><br>
                אנא צור קשר עם הרכזת האירוע לתיאום קבלת הפרס שלך.
                <br>
                Please contact the event coordinator to arrange receiving your prize.
            </div>
        </div>
        
        <div class='footer'>
            <p>זה אירוע התרמה בחסות XYZ | This is a charitable event sponsored by XYZ</p>
            <p style='margin: 10px 0 0 0; font-size: 11px;'>© 2026 Chinese Auction | כל הזכויות שמורות</p>
        </div>
    </div>
</body>
</html>
";
        }
    }
}