using Business.DynamicModelReflector.Interfaces;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;

namespace Business.Genisis.DataAccess.DataAccess
{
    public class PlayerTeamReflector : IPlayerTeamDataOperations
    {
        #region Fields
        IModelReflector _reflect;
        #endregion

        #region Constructors
        public PlayerTeamReflector(IModelReflector reflect)
        {
            _reflect = reflect;
        }
        #endregion

        #region Public Methods
        public void InsertNewPlayers(IEnumerable<PlayerTeam> playerTeams)
        {
            try
            {
                _reflect
                    .Create(playerTeams)
                    .Execute();
            }
            catch
            {
                throw;
            }
        }

        public Guid RetrieveIdForPlayerTeam(Guid tournamentId, string playerName, Guid categoryId)
        {
            PlayerTeam playerTeam = new();
            _reflect.Load(playerTeam)
                .Select(playerTeam => playerTeam.Id)
                .Where(playerTeam => playerTeam.TournamentId == tournamentId && playerTeam.PlayerFullName == playerName && playerTeam.CategoryId == categoryId)
                .Execute();

            return playerTeam.Id;
        }

        public IEnumerable<PlayerTeam> RetrieveTeamsInCategory(Guid tournamentId, Guid categoryId)
        {
            try
            {
                 IEnumerable<PlayerTeam> playerTeams = new List<PlayerTeam>();
                _reflect.Load(playerTeams)
                    .Select(playerTeam => playerTeam.TeamName)
                    .Where(playerTeam => playerTeam.TournamentId == tournamentId && playerTeam.CategoryId == categoryId)
                    .Execute();

                return playerTeams;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<PlayerTeam> RetrievePlayerNamesInTeam(Guid tournamentId, Guid categoryId, string teamName)
        {
            IEnumerable<PlayerTeam> playerTeams = new List<PlayerTeam>();

            _reflect.Load(playerTeams)
                .Select(playerTeams => playerTeams.PlayerFullName)
                .Where(playerTeams => playerTeams.TeamName == teamName && playerTeams.CategoryId == categoryId && playerTeams.TournamentId == tournamentId)
                .Execute();

            return playerTeams;
        }

        public IEnumerable<PlayerTeam> RetrieveTeamsInTournament(Guid tournamentId)
        {
            try
            {
                IEnumerable<PlayerTeam> playerTeams = new List<PlayerTeam>();
                _reflect.Load(playerTeams)
                    .Where(playerTeam => playerTeam.TournamentId == tournamentId)
                    .Execute();

                return playerTeams;
            }
            catch
            {
                throw;
            }
        }

        public void DeletePlayerTeamsInTournament(Guid tournamentId)
        {
            try
            {
                _reflect.Delete(new PlayerTeam())
                .Where(playerTeam => playerTeam.TournamentId == tournamentId)
                .Execute();
            }
            catch
            {
                throw;
            }
        }

        public void UpdatePlayerTeam(PlayerTeam newPlayerTeam, string oldPlayerName)
        {
            try
            {
                Guid whereGuidId = newPlayerTeam.Id;
                _reflect.Update(newPlayerTeam)
                    .Where(newPlayerTeam => newPlayerTeam.Id == whereGuidId)
                    .Execute();
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}