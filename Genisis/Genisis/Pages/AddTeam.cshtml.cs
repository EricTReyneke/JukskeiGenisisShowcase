using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json;

namespace Genisis.Pages
{
    public class AddTeamModel : PageModel
    {
        #region Fields
        IEncryption _encryption;
        ICategoryDataOperations _categoryDataOperations;
        IPlayerTeamDataOperations _playerTeamDataOperations;
        Guid _tournamentId;
        #endregion

        #region Properties
        public List<Category> Categories { get; set; }
        public List<PlayerTeam> PlayerTeams { get; set; }
        public Dictionary<string, List<PlayerTeam>> TeamsInCategories { get; set; } = new();
        public Dictionary<string, List<PlayerTeam>> TeamsInCategoriesUniqueTeamNames { get; set; } = new();
        #endregion

        #region Constructors
        public AddTeamModel(IEncryption encryption, ICategoryDataOperations categoryDataOperations, IPlayerTeamDataOperations playerTeamDataOperations)
        {
            _encryption = encryption;
            _categoryDataOperations = categoryDataOperations;
            _playerTeamDataOperations = playerTeamDataOperations;
        }
        #endregion

        #region Public Methods
        public void OnGet(string tGuid)
        {
            try
            {
                SetEnvironmentVariables(tGuid);
                SetTeamsInCategories(tGuid);
            }
            catch
            {
                throw;
            }

            //TODO Change code for prod.
            //TODO Add Code to delete the team in the Accordion.
            //TODO Add Validations for the AddTeams Page.
            //Prod.
            //Environment.SetEnvironmentVariable("Decoded_Data", _encryption.DecryptString(_encryption.RetrieveEncryptionKey(), HttpUtility.UrlDecode(tGuid)), EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// Addes Players to the Category and Team selected.
        /// </summary>
        /// <param name="playerNames">Player Names.</param>
        /// <param name="categoryName">Category Names.</param>
        /// <param name="teamName">Team Name.</param>
        /// <returns>Jason Result.</returns>
        public IActionResult OnPostAddPlayersToList(string playerNames, string categoryName, string teamName, int isCaptain)
        {
            try
            {
                SetPlayerTeamObject(playerNames.Split(',').ToList(), categoryName, teamName,isCaptain);
                InsertPlayerTeams();
                return new StatusCodeResult(204);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Updates the PlayerTeam Specified.
        /// </summary>
        /// <param name="newPlayerTeam">New PlayerTeam to be updated.</param>
        /// <param name="oldPlayerName">Old Player Name.</param>
        /// <param name="categoryName">Category name the player Plays in.</param>
        /// <returns></returns>
        public IActionResult OnPostUpdatePlayerName([FromBody] PlayerTeam newPlayerTeam, string oldPlayerName, string categoryName)
        {
            try
            {
                newPlayerTeam.TournamentId = RetrieveTournamentId();
                newPlayerTeam.CategoryId = DeserializeCategories(categoryName);
                newPlayerTeam.Id = _playerTeamDataOperations.RetrieveIdForPlayerTeam(newPlayerTeam.TournamentId, oldPlayerName, newPlayerTeam.CategoryId);
                UpdatePlayerTeam(newPlayerTeam, oldPlayerName);
                return new StatusCodeResult(204);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public IActionResult OnPostIsThereNoTeamsAdded()
        {
            try
            {
                if(_playerTeamDataOperations.RetrieveTeamsInTournament(RetrieveTournamentId()).Count() != 0)
                    return new JsonResult(new { success = true, teams = true });

                return new JsonResult(new { success = true, teams = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public IActionResult OnPostDeletePlayerTeamsInTournament()
        {
            try
            {
                _playerTeamDataOperations.DeletePlayerTeamsInTournament(RetrieveTournamentId());
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public List<PlayerTeam> RetrievePlayersInTeams(Guid categoryId, string teamName) =>
            _playerTeamDataOperations.RetrievePlayerNamesInTeam(RetrieveTournamentId(), categoryId, teamName).ToList();
        #endregion

        #region Private Methods
        /// <summary>
        /// Inserts new PlayerTeams into the Database.
        /// </summary>
        private void InsertPlayerTeams() =>
            _playerTeamDataOperations.InsertNewPlayers(PlayerTeams);

        /// <summary>
        /// Initializes the PlayerTeams property with a new list of PlayerTeam objects based on provided player names, category name, and team name. It assigns the TournamentId from an environment variable, 
        /// finds the CategoryId by deserializing Categories from an environment variable, and sets specific players as captain or reserve based on their position in the list.
        /// </summary>
        /// <param name="playerNames">List of player names to be added to PlayerTeams.</param>
        /// <param name="categoryName">Name of the category to match and retrieve the CategoryId for each player.</param>
        /// <param name="teamName">Name of the team to be assigned to each player.</param>
        /// <param name="captainIndex">What Index the player is Captain.</param>
        private void SetPlayerTeamObject(List<string> playerNames, string categoryName, string teamName, int captainIndex)
        {
            PlayerTeams = new List<PlayerTeam>();

            for (int i = 0; i < playerNames.Count; i++)
            {
                string player = playerNames[i];

                Guid categoryId = DeserializeCategories(categoryName);

                PlayerTeams.Add(new PlayerTeam()
                {
                    PlayerFullName = player,
                    TournamentId = RetrieveTournamentId(),
                    CategoryId = categoryId,
                    TeamName = teamName,
                    Captain = i == captainIndex,
                    Reserve = i == 4
                });

                if (captainIndex == 100)
                    break;
            }
        }

        /// <summary>
        /// Updates the PlayerTeam with Corrected/New values.
        /// </summary>
        /// <param name="newPlayerTeam">New Updated PlayerTeam.</param>
        /// <param name="oldPlayerName">Old Player Name to specify in the where statment.</param>
        private void UpdatePlayerTeam(PlayerTeam newPlayerTeam, string oldPlayerName) =>
            _playerTeamDataOperations.UpdatePlayerTeam(newPlayerTeam, oldPlayerName);

        /// <summary>
        /// Deserializes the Category EnvironmentVariable to get the Guid of the Gategory.
        /// </summary>
        /// <returns>Category Id.</returns>
        private Guid DeserializeCategories(string categoryName) =>
            JsonSerializer.Deserialize<List<Category>>(Environment.GetEnvironmentVariable("Categories", EnvironmentVariableTarget.Process)).FirstOrDefault(category => category.Name == categoryName).Id;

        /// <summary>
        /// Retrieves the TournamentId from the EnvironmentVariables.
        /// </summary>
        /// <returns>Tournament Id.</returns>
        private Guid RetrieveTournamentId() =>
            Guid.Parse(Environment.GetEnvironmentVariable("TournamentGuid", EnvironmentVariableTarget.Process));


        /// <summary>
        /// Sets Environment Variables to reduce Database calls.
        /// </summary>
        /// <param name="tournamentGuid">Tournament Id which is being used.</param>
        private void SetEnvironmentVariables(string tournamentGuid)
        {
            //Prod
            //Environment.SetEnvironmentVariable("TournamentGuid", tournamentGuid, EnvironmentVariableTarget.Process);
            //Categories = _categoryDataOperations.RetrieveCategoriesOfTournament(Guid.Parse(tournamentGuid)).ToList();
            Categories = _categoryDataOperations.RetrieveCategoriesOfTournament(Guid.Parse("0B33D5D2-AE8B-4E8B-BC3D-2E9E9B5B6C67")).ToList();
            Categories.Sort(Comparer<Category>.Create((x, y) => x.Name.CompareTo(y.Name)));

            Environment.SetEnvironmentVariable("TournamentGuid", "0B33D5D2-AE8B-4E8B-BC3D-2E9E9B5B6C67", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("Categories", JsonSerializer.Serialize(Categories), EnvironmentVariableTarget.Process);
        }

        private void SetTeamsInCategories(string tournamentId)
        {

            //TODO: Fix For prod.

            Guid tournamentIdGuid = Guid.Empty;
            if (Guid.TryParse("0B33D5D2-AE8B-4E8B-BC3D-2E9E9B5B6C67", out tournamentIdGuid))
                foreach (Category category in Categories)
                {
                    List<PlayerTeam> playerTeams = _playerTeamDataOperations.RetrieveTeamsInCategory(tournamentIdGuid, category.Id).ToList();

                    playerTeams.Sort(Comparer<PlayerTeam>.Create((x, y) => x.TeamName.CompareTo(y.TeamName)));

                    TeamsInCategories.Add(category.Name, playerTeams);

                    List<PlayerTeam> playerTeamsUnique = playerTeams
                                                            .GroupBy(pt => pt.TeamName)
                                                            .Select(g => g.First())
                                                            .ToList();

                    TeamsInCategoriesUniqueTeamNames.Add(category.Name, playerTeamsUnique);
                }
            else
                throw new Exception($"Tournament Id: {tournamentId}, is not a valid Guid.");
        }
        #endregion
    }
}