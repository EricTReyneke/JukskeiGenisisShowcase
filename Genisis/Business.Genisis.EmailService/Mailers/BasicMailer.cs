using Business.Genisis.Data.Interfaces;
using Business.Genisis.EmailService.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Business.Genisis.EmailVerification.Mailers
{
    public class BasicMailer : IMailerSerivce
    {
        #region Fields
        IMailerContext _mailerContext;
        #endregion

        #region Constructors
        public BasicMailer(IMailerContext mailerContext) =>
            _mailerContext = mailerContext;
        #endregion

        #region Public Methods
        public void SendEmail(string subject, string body, string receiverAddress)
        {
            try
            {
                using SmtpClient smtpClient = new(_mailerContext.SmtpHost, _mailerContext.SmtpPort)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_mailerContext.EmailAddress, _mailerContext.EmailPassword)
                };

                using MailMessage mail = new()
                {
                    From = new MailAddress(_mailerContext.EmailAddress),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(receiverAddress));

                smtpClient.Send(mail);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}