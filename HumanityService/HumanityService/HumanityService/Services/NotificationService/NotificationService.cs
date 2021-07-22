using System.Net.Mail;
using System.Net;
using HumanityService.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace HumanityService.Services
{
    public class NotificationService : INotificationService
    {

        private readonly string Name;
        private readonly string Email;
        private readonly string Password;

        public NotificationService(IOptions<NotificationServiceSettings> options)
        {
            Name = options.Value.Name;
            Email = options.Value.Email;
            Password = options.Value.Password;
        }


        public void NotifyUser(string recipient, string subject, string body)
        {
            var fromAddress = new MailAddress(Email, Name);
            var toAddress = new MailAddress(recipient);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, Password),
                Timeout = 20000
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }
    }
}
