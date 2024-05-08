using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;

namespace Business.Genisis.DataAccess.DataTransactions
{
    public class TournamentReflector : ITournamentDataOperations
    {
        #region Fields
        IModelReflector _reflect;
        #endregion

        #region Constructors
        public TournamentReflector(IModelReflector reflect)
        {
            _reflect = reflect;
        }
        #endregion

        #region Public Methods
        public IEnumerable<Tournament> RetrieveAllTournaments(IEnumerable<Tournament> tournaments)
        {
            _reflect
                .Load(tournaments)
                .Execute();

            return tournaments;
        }

        public IEnumerable<PrimaryKeyInfo> CreateNewTournament(Tournament newTournament)
        {
            newTournament.Name += $" {DateTime.Now.Year}";

            return _reflect
                .Create(newTournament)
                .Execute();
        }

        public void DeleteTournament(Guid tournamentId)
        {
            try
            {
                _reflect
                    .Delete(new Tournament())
                    .Where(tournament => tournament.Id == tournamentId)
                    .Execute();
            }
            catch
            {
                throw;
            }
        }

        public Tournament RetrieveTournamentFromId(Guid tournamentId)
        {
            try
            {
                Tournament tournament = new();
                _reflect
                    .Load(tournament)
                    .Where(tournament => tournament.Id == tournamentId)
                    .Execute();

                return tournament;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}