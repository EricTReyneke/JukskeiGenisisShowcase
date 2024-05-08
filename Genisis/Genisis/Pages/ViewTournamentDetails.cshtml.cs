using Business.Genesis.Scheduler.Interfaces;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;

namespace Genisis.Pages
{
    public class ViewTournamentDetailsModel : PageModel
    {
        #region Fields
        ITournamentDataOperations _tournamentReflector;

        ICategoryDataOperations _categoryDataOperations;

        IScoresAllocationsDataOperations _scoresAllocationsDataOperations;

        IMatchmakingStrategy _matchmakingStrategy;

        IPlayerTeamDataOperations _playerTeamDataOperations;
        #endregion

        #region Properties
        public Tournament Tournament { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public List<TotalScores> TotalScores { get; set; } = new();
        #endregion

        #region Constructors
        public ViewTournamentDetailsModel(ITournamentDataOperations tournamentReflector, ICategoryDataOperations categoryDataOperations, IScoresAllocationsDataOperations scoresAllocationsDataOperations,
            IMatchmakingStrategy matchmakingStrategy, IPlayerTeamDataOperations playerTeamDataOperations)
        {
            _tournamentReflector = tournamentReflector;
            _categoryDataOperations = categoryDataOperations;
            _scoresAllocationsDataOperations = scoresAllocationsDataOperations;
            _matchmakingStrategy = matchmakingStrategy;
            _playerTeamDataOperations = playerTeamDataOperations;
        }
        #endregion

        #region Public Methods
        public void OnGet(Guid tournamentId)
        {
            SetTournament(tournamentId);
            SetCategories();
            SetEnvironmentVariables(tournamentId);
        }

        public IEnumerable<(string First, string Second)> RetrieveCategoryRoster(Guid tournamentGuid, Guid categoryId) =>
            _matchmakingStrategy.RoundRobin(_playerTeamDataOperations.RetrieveTeamsInCategory(tournamentGuid, categoryId).Select(teamNames => teamNames.TeamName).ToList());
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the tournament in scope.
        /// </summary>
        /// <param name="tournamentId">Specified Tournament Id.</param>
        private void SetTournament(Guid tournamentId) =>
            Tournament = _tournamentReflector.RetrieveTournamentFromId(tournamentId);

        /// <summary>
        /// Sets the Categories Property.
        /// </summary>
        private void SetCategories()
        {
            Categories = _categoryDataOperations.RetrieveCategoriesOfTournament(Tournament.Id).ToList();
            Categories.Sort(Comparer<Category>.Create((x, y) => x.Name.CompareTo(y.Name)));
        }

        /// <summary>
        /// Retrieves the top 3 teams in all categories and sets them in the List<TotalScores> object.
        /// </summary>
        /// <param name="tournamentId">Tournament in Scope.</param>
        /// <param name="categoryId">Category in Scope.</param>
        /// <returns></returns>
        public List<TotalScores> RetrieveTopTeamsForCategory(Guid tournamentId, Guid categoryId)
        {
            string jsonContent = _scoresAllocationsDataOperations.RetrievePointAllocationsForAChategory(tournamentId, categoryId)?.Scores;

            if (string.IsNullOrEmpty(jsonContent))
                return new();

            Dictionary<string, List<MatchScore>> teamScores = JsonConvert.DeserializeObject<Dictionary<string, List<MatchScore>>>(jsonContent);

            if (teamScores == null)
                return new();

            return teamScores
                .SelectMany(kv => kv.Value)
                .SelectMany(match => new[] { match.Team1, match.Team2 })
                .GroupBy(team => team.Name)
                .Select(group =>
                {
                    double totalScore = group.Sum(teamScore =>
                    {
                        if (double.TryParse(teamScore.Score, NumberStyles.Any, CultureInfo.InvariantCulture, out double score))
                            return score;
                        else
                            return 0;
                    });

                    return new TotalScores
                    {
                        TeamName = group.Key,
                        TotalScore = totalScore
                    };
                })
                .OrderByDescending(team => team.TotalScore)
                .ToList();
        }

        /// <summary>
        /// Sets Environment Variables to reduce Database calls.
        /// </summary>
        /// <param name="tournamentGuid">Tournament Id which is being used.</param>
        private void SetEnvironmentVariables(Guid tournamentGuid)
        {
            Environment.SetEnvironmentVariable("TournamentGuid", tournamentGuid.ToString(), EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("Categories", System.Text.Json.JsonSerializer.Serialize(Categories), EnvironmentVariableTarget.Process);
        }
        #endregion
    }
}