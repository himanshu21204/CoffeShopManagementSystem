using System.Net.Mail;
using System.Net;
using System.Text;
using ConsumeCoffeeShopWebAPI.Models.Email;

namespace ConsumeCoffeeShopWebAPI
{
	public class EmailSender : IEmailSender
	{
        private IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<bool> SendEmail(string email, string subject, string message)
        {
            bool status = false;
            try
            {
                EmailSetting setting = new EmailSetting()
                {
                    SecretKey = _configuration.GetValue<string>("AppSettings:SecretKey"),
                    From = _configuration.GetValue<string>("AppSettings:EmailSettings:From"),
                    SmtpServer = _configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer"),
                    Port = _configuration.GetValue<int>("AppSettings:EmailSettings:Port"),
                    EnableSSL = _configuration.GetValue<bool>("AppSettings:EmailSettings:EnablSSL"),
                };

                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(setting.From),
                    Subject = subject,
                    Body = message
                };
                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(setting.SmtpServer)
                {
                    Port = setting.Port,
                    Credentials = new NetworkCredential(setting.From, setting.SecretKey),
                    EnableSsl = setting.EnableSSL
                };
                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
    }
}
