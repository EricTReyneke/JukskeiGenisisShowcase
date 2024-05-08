using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Business.Genisis.Data.Models
{
    public class ScoresAllocations
    {
        [Key]
        public Guid Id { get; set; }

        public string Scores { get; set; }

        public int MatchToPlay { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        [ForeignKey("Tournament")]
        public Guid TournamentId { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Category> Category { get; set; }

        [IgnoreDataMember]
        public IEnumerable<Tournament> Tournament { get; set; }
    }
}

//Create table ScoresAllocations(
//Id UniqueIdentifier Primary Key,
//Scores VarChar(Max) Not Null,
//MatchToPlay int Not Null,
//CategoryId UniqueIdentifier Not Null,
//TournamentId UniqueIdentifier Not Null,
//FOREIGN KEY (CategoryId) REFERENCES Category(Id),
//FOREIGN KEY(TournamentId) REFERENCES Tournament(Id))