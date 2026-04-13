using System.Net.Mail;
using System.Net;

namespace ShopComputer.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("nguyenminhhuyy.1518@gmail.com", "gpbppljczayyzqek")
            };

            return client.SendMailAsync(
                new MailMessage(from: "nguyenminhhuyy.1518@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
