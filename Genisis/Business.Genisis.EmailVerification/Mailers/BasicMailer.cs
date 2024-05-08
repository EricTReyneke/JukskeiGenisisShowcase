using System.Net.Mail;

namespace Business.Genisis.EmailVerification.Mailers
{
    public class BasicMailer
    {
        #region Fields
        IMailerContext _mailerContext;
        #endregion

        #region Constructors
        public LoggerMailerService(IMailerContext mailerContext) =>
            _mailerContext = mailerContext;
        #endregion

        #region Public Methods
        public void SendEmail(string subject, string body)
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

                mail.To.Add(new MailAddress(_mailerContext.ReceiverAddress));

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}