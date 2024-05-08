using Business.DynamicModelReflector.Models;
using Business.Genisis.Data.Models;

namespace Business.Genisis.DataAccess.Interfaces
{
    public interface ITournamentDataOperations
    {
        /// <summary>
        /// Retieves all Tournaments from database.
        /// </summary>
        /// <param name="tournaments">IEnumerable to fill.</param>
        /// <returns>IEnumerable of all Tournaments.</returns>
        IEnumerable<Tournament> RetrieveAllTournaments(IEnumerable<Tournament> tournaments);

        /// <summary>
        /// Creates new tournament in the database with the values provided in the data model.
        /// </summary>
        /// <param name="newTournament">Tournament DataModel with new tournament data.</param>
        /// <returns>New tournament primary key info.</returns>
        IEnumerable<PrimaryKeyInfo> CreateNewTournament(Tournament newTournament);

        /// <summary>
        /// Retrieves the Tournament from a sepcified Tournament Id.
        /// </summary>
        /// <param name="tournamentId">Sepcified Tournament Id.</param>
        /// <returns>Tournament object with the Sepcified Tournament Id.</returns>
        Tournament RetrieveTournamentFromId(Guid tournamentId);

        /// <summary>
        /// Deletes tournament from database using the Id provided.
        /// </summary>
        /// <param name="tournamentId">Specified tournament Id.</param>
        void DeleteTournament(Guid tournamentId);
    }
}