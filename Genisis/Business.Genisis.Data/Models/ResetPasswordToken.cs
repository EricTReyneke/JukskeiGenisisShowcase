namespace Business.Genisis.Data.Models
{
    public class ResetPasswordToken
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public Guid Token { get; set; }

        public DateTime GeneratedDate { get; set; }
    }
}