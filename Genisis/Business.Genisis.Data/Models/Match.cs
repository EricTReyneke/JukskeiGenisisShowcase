namespace Business.Genisis.Data.Models
{
    public class Match
    {
        public string FirstTeam { get; set; }
        public string SecondTeam { get; set; }
        public DateTime MatchStartTime { get; set; }
        public DateTime MatchEndTime { get; set; }
        public string Lane { get; set; }
        public string Category { get; set; }
    }
}
