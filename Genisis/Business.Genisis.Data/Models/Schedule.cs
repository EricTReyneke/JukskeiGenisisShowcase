namespace Business.Genisis.Data.Models
{
    public class Schedule
    {
        public string Category { get; set; }
        public List<MatchBracket> MatchBrackets { get; set; }
    }
}