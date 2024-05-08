namespace Business.Genisis.Data.Models
{
    public class MatchScore
    {
        public int MatchNumber { get; set; }
        public TeamScore Team1 { get; set; }
        public TeamScore Team2 { get; set; }
    }
}