
using AspNetCoreIdentityApp.Core.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentity.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailsettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailsettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string ToEmail)
        {
            var smtpClient = new SmtpClient();
            
            smtpClient.Host =_emailsettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailsettings.Email,_emailsettings.Password);
            smtpClient.EnableSsl = true;    

            var mailMessage =new MailMessage();
            mailMessage.From =new MailAddress(_emailsettings.Email);
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "LocalHost | Şifre Sıfırlama Linki ";
            mailMessage.Body = @$"<h4>Şifrenizi Yenilemek için aşağıdaki linke tıklayınız.</h4>
            <p>
            <a href='{resetEmailLink}'>Şifre Yenileme Linki</a>
            </p>" ;

            mailMessage.IsBodyHtml= true;

            await smtpClient.SendMailAsync(mailMessage);



        }
    }
}
