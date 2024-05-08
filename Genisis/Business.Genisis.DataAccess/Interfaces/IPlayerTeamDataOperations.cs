using Business.Genisis.Data.Models;

namespace Business.Genisis.DataAccess.Interfaces
{
    public interface IPlayerTeamDataOperations
    {
        /// <summary>
        /// Inserts PlayerTeams into the PlayerTeam table.
        /// </summary>
        /// <param name="playerTeams">PlayerTeams to be inserted.</param>
        void InsertNewPlayers(IEnumerable<PlayerTeam> playerTeams);

        /// <summary>
        /// Retrieve the PlayerTeam Id from the TournamentId and the Name specified.
        /// </summary>
        /// <param name="tournamentId">Specified Tournament Id.</param>
        /// <param name="playerName">Specified Player Name.</param>
        /// <param name="playerName">Specified Category Id.</param>
        /// <returns>PlayerTeam Id.</returns>
        Guid RetrieveIdForPlayerTeam(Guid tournamentId, string playerName, Guid categoryId);

        /// <summary>
        /// Retrieves all the Teams in the Specified category.
        /// </summary>
        /// <param name="tournamentId">Specified Tournament Id.</param>
        /// <param name="categoryId">Specified Category Id.</param>
        /// <returns></returns>
        IEnumerable<PlayerTeam> RetrieveTeamsInCategory(Guid tournamentId, Guid categoryId);

        /// <summary>
        /// Retrieves all the Teams in a Tournament.
        /// </summary>
        /// <param name="tournamentId">Specified Tournament Id.</param>
        /// <returns>List of PlayerTeams in the Tournament.</returns>
        IEnumerable<PlayerTeam> RetrieveTeamsInTournament(Guid tournamentId);

        /// <summary>
        /// Retrieves the Player names in a Team.
        /// </summary>
        /// <param name="tournamentId">Tournament in Scope.</param>
        /// <param name="categoryId">Category in Scope.</param>
        /// <param name="teamName">Team Name in Scope.</param>
        /// <returns>A List of players names in a team.</returns>
        IEnumerable<PlayerTeam> RetrievePlayerNamesInTeam(Guid tournamentId, Guid categoryId, string teamName);

        /// <summary>
        /// Deletes all PlayerTeams with the Specified TournamentId.
        /// </summary>
        /// <param name="tournamentId">Specified TournamentId.</param>
        void DeletePlayerTeamsInTournament(Guid tournamentId);

        /// <summary>
        /// Updates the PlayerTeam with Corrected/New values.
        /// </summary>
        /// <param name="newPlayerTeam">New Updated PlayerTeam.</param>
        /// <param name="oldPlayerName">Old Player Name to specify in the where statment.</param>
        void UpdatePlayerTeam(PlayerTeam newPlayerTeam, string oldPlayerName);
    }
}