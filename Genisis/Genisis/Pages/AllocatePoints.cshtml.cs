using Business.Genesis.Scheduler.Interfaces;
using Business.Genesis.Scheduler.Stratagies;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Genisis.Pages
{
    public class AllocatePointsModel : PageModel
    {
        //TODO: Fix the Success message that displays when all matches has been completed.

        #region Fields
        IScoresAllocationsDataOperations _pointAllocationsDataOperations;
        ICategoryDataOperations _categoryDataOperations;
        ITournamentDataOperations _turnamentDataOperations;
        IPlayerTeamDataOperations _playerTeamDataOperations;
        IMatchmakingStrategy _matchmakingStrategy;
        Dictionary<string, List<MatchScore>> _matchScores;
        #endregion

        #region Properties
        public List<Category> Categories { get; set; }

        public Tournament Tournament { get; set; }

        public Dictionary<string, IEnumerable<(string First, string Second)>> CategoryRosters { get; set; }

        public Dictionary<string, int> NumberOfTeamsInCategory { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> CategoryCurrentMatch { get; set; } = new Dictionary<string, int>();
        #endregion

        #region Constructor
        public AllocatePointsModel(IScoresAllocationsDataOperations pointAllocationsDataOperations, ICategoryDataOperations categoryDataOperations, 
            ITournamentDataOperations tournamentDataOperations, IPlayerTeamDataOperations playerTeamDataOperations, IMatchmakingStrategy matchmakingStrategy)
        {
            _pointAllocationsDataOperations = pointAllocationsDataOperations;
            _categoryDataOperations = categoryDataOperations;
            _turnamentDataOperations = tournamentDataOperations;
            _playerTeamDataOperations = playerTeamDataOperations;
            _matchmakingStrategy = matchmakingStrategy;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes data and retrieves tournament information for the specified tournament GUID.
        /// </summary>
        /// <param name="tournamentGuid">The unique identifier for the tournament.</param>
        public void OnGet(string tournamentGuid)
        {
            SetEnvironmentVariables(tournamentGuid);
            //RetrieveTournament(Guid.Parse(tournamentGuid));
            RetrieveTournament(Guid.Parse("727437D4-FDDA-4F81-B314-4F39959AA834"));
            RetrieveCategoryRosters();
        }

        /// <summary>
        /// Allocates points for matches within a specific category.
        /// </summary>
        /// <param name="categoryName">The name of the category for which points are being allocated.</param>
        /// <param name="matchScores">A list of match scores for the matches within the specified category.</param>
        /// <returns>A JsonResult indicating the success or failure of the operation along with an optional message.</returns>
        public IActionResult OnPostAllocatePointsForMatches(string categoryName, [FromBody] List<MatchScore> matchScores)
        {
            try
            {
                InsertMatchValues(categoryName, matchScores);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the current match number for a specified category.
        /// </summary>
        /// <param name="categoryName">The name of the category for which the current match number is being requested.</param>
        /// <returns>A JsonResult containing the success status and the current match number or a failure message.</returns>
        public IActionResult OnPostRetrieveCategoryCurrentMatch(string categoryName)
        {
            try
            {
                DeserializeCategoryCurrentMatch();
                return new JsonResult(new { success = true, matchNumber = CategoryCurrentMatch[categoryName] });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Inserts or updates match scores for a specific category.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="matchScores">A list of match scores to be inserted or updated.</param>
        private void InsertMatchValues(string categoryName, List<MatchScore> matchScores)
        {
            try
            {
                if (_matchScores == null)
                    _matchScores = new Dictionary<string, List<MatchScore>>();

                if (_pointAllocationsDataOperations.RetrievePointAllocationsForAChategory(RetrieveTournamentId(), DeserializeCategories(categoryName)).Scores == null)
                {
                    _matchScores.Add(categoryName, matchScores);
                    InsertNewMatchScores(categoryName);
                    IncrementMatchCounterForCategory(categoryName);
                    return;
                }

                _matchScores.Add(categoryName, matchScores);
                UpdateMatchScores(categoryName, matchScores);
                IncrementMatchCounterForCategory(categoryName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Increments the match counter for a given category.
        /// </summary>
        /// <param name="categoryName">The name of the category to increment the match counter for.</param>
        private void IncrementMatchCounterForCategory(string categoryName)
        {
            DeserializeCategoryCurrentMatch();
            CategoryCurrentMatch[categoryName] += 1;
            SerializeCategoryCurrentMatch();
        }

        /// <summary>
        /// Inserts new match scores for a specific category.
        /// </summary>
        /// <param name="categoryName">The name of the category for which to insert new match scores.</param>
        private void InsertNewMatchScores(string categoryName) =>
            _pointAllocationsDataOperations.InsertPointAllocations(new()
            {
                Scores = JsonSerializer.Serialize(_matchScores),
                MatchToPlay = 2,
                CategoryId = DeserializeCategories(categoryName),
                TournamentId = RetrieveTournamentId()
            });

        /// <summary>
        /// Updates match scores for a specific category.
        /// </summary>
        /// <param name="categoryName">The name of the category to update match scores for.</param>
        /// <param name="matchScores">The new match scores to be added.</param>
        private void UpdateMatchScores(string categoryName, List<MatchScore> matchScores)
        {
            ScoresAllocations scoresAllocations = _pointAllocationsDataOperations.RetrievePointAllocationsForAChategory(RetrieveTournamentId(), DeserializeCategories(categoryName));
            Dictionary<string, List<MatchScore>> currentMatchScores = JsonSerializer.Deserialize<Dictionary<string, List<MatchScore>>>(scoresAllocations.Scores);
            currentMatchScores[categoryName].AddRange(matchScores);

            scoresAllocations.Scores = JsonSerializer.Serialize(currentMatchScores);

            _pointAllocationsDataOperations.UpdatePointAllocationsForCategory(scoresAllocations, RetrieveTournamentId(), DeserializeCategories(categoryName));
        }

        /// <summary>
        /// Deserializes the current match counter for categories from an environment variable.
        /// </summary>
        private void DeserializeCategoryCurrentMatch() =>
            CategoryCurrentMatch = JsonSerializer.Deserialize<Dictionary<string, int>>(Environment.GetEnvironmentVariable("CategoryCurrentMatch", EnvironmentVariableTarget.Process));

        /// <summary>
        /// Serializes the current match counter for categories to an environment variable.
        /// </summary>
        private void SerializeCategoryCurrentMatch() =>
            Environment.SetEnvironmentVariable("CategoryCurrentMatch", JsonSerializer.Serialize(CategoryCurrentMatch), EnvironmentVariableTarget.Process);

        /// <summary>
        /// Retrieves and organizes roster data for all categories, based on available player teams.
        /// </summary>
        private void RetrieveCategoryRosters()
        {
            Dictionary<string, IEnumerable<(string First, string Second)>> categoryRosters = new();

            foreach (Category category in Categories)
            {
                ScoresAllocations matchesToPlay = _pointAllocationsDataOperations.RetrieveMatchesToPlay(DeserializeCategories(category.Name));
                CategoryCurrentMatch[category.Name] = matchesToPlay.MatchToPlay == 0 ? 1 : matchesToPlay.MatchToPlay;

                IEnumerable<PlayerTeam> playerTeams = _playerTeamDataOperations.RetrieveTeamsInCategory(RetrieveTournamentId(), DeserializeCategories(category.Name)).ToList();
                NumberOfTeamsInCategory[category.Name] = playerTeams.Count() % 2 != 0 ? playerTeams.Count() + 1: playerTeams.Count();

                categoryRosters[category.Name] = _matchmakingStrategy.RoundRobin(playerTeams.Select(pt => pt.TeamName).ToList());
            }

            SerializeCategoryCurrentMatch();
            CategoryRosters = categoryRosters;
        }

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
        /// Retrieves the Tournament from the Primary key Guid.
        /// </summary>
        /// <param name="tournamentGuid">Tournament Primary Key.</param>
        private void RetrieveTournament(Guid tournamentGuid) =>
            Tournament = _turnamentDataOperations.RetrieveTournamentFromId(tournamentGuid);

        /// <summary>
        /// Sets Environment Variables to reduce Database calls.
        /// </summary>
        /// <param name="tournamentGuid">Tournament Id which is being used.</param>
        private void SetEnvironmentVariables(string tournamentGuid)
        {
            //Prod
            //Environment.SetEnvironmentVariable("TournamentGuid", tournamentGuid, EnvironmentVariableTarget.Process);
            //Categories = _categoryDataOperations.RetrieveCategoriesOfTournament(Guid.Parse(tournamentGuid)).ToList();
            Categories = _categoryDataOperations.RetrieveCategoriesOfTournament(Guid.Parse("727437D4-FDDA-4F81-B314-4F39959AA834")).ToList();
            Categories.Sort(Comparer<Category>.Create((x, y) => x.Name.CompareTo(y.Name)));

            Environment.SetEnvironmentVariable("TournamentGuid", "727437D4-FDDA-4F81-B314-4F39959AA834", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("Categories", JsonSerializer.Serialize(Categories), EnvironmentVariableTarget.Process);
        }
        #endregion
    }
}