using Business.Genisis.Data.Interfaces;

namespace Business.Genisis.Data.Contexts
{
    public class GenisisMailerContext : IMailerContext
    {
        public string EmailAddress { get; set; } = "erictestreyneke@gmail.com";

        public string EmailPassword { get; set; } = "owyqajctbszbigef";

        public string SmtpHost { get; set; } = "smtp.gmail.com";

        public int SmtpPort { get; set; } = 587;
    }
}