using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Business.Genisis.Data.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Tournament")]
        public Guid TournamentId { get; set; }

        [IgnoreDataMember]
        public Tournament Tournament { get; set; }
    }
}