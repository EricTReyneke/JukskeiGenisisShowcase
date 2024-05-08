using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Business.Genisis.Data.Models
{
    public class PlayerTeam
    {
        [Key]
        public Guid Id { get; set; }

        public string PlayerFullName { get; set; }

        public string TeamName { get; set; }

        public bool Captain { get; set; }

        public bool Reserve { get; set; }

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