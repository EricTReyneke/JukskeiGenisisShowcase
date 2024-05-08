namespace Business.Genisis.EmailService.Interfaces
{
    public interface IMailerSerivce
    {
        /// <summary>
        /// Sends an email to the specified address.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email. Can be HTML.</param>
        /// /// <param name="body">Email which would recieve the Mail.</param>
        /// <remarks>
        /// This method uses SMTP to send an email. It requires a configured <c>_mailerContext</c> with SMTP host, port, and credentials.
        /// </remarks>
        /// <exception cref="Exception">Thrown when an error occurs during sending the email.</exception>
        void SendEmail(string subject, string body, string receiverAddress);
    }
}