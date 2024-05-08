namespace Business.Genisis.Data.Interfaces
{
    public interface IMailerContext
    {
        /// <summary>
        /// Email address which will be sending the email.
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// Password to the email address which will be sending the mail.
        /// </summary>
        string EmailPassword { get; set; }

        /// <summary>
        /// SMTP service which the email will be sent through.
        /// </summary>
        string SmtpHost { get; set; }

        /// <summary>
        /// SMTP service port number.
        /// </summary>
        int SmtpPort { get; set; }
    }
}