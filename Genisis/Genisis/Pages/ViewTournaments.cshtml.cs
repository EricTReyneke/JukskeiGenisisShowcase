using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Globalization;

namespace Genisis.Pages
{
    public class ViewTournamentsModel : PageModel
    {
        #region Fields
        ITournamentDataOperations _tournamentAccess;
        ICategoryDataOperations _categoryDataOperations;
        IScoresAllocationsDataOperations _scoresAllocationsDataOperations;
        #endregion

        #region Properties
        public List<Tournament> Tournaments { get; set; } = new();
        public List<List<Category>> Categories { get; set; } = new();
        public Dictionary<string, ScoresAllocations> ScoresAllocationsPerTournament { get; set; } = new();
        public int PageCounter { get; set; }
        #endregion

        #region Constructors
        public ViewTournamentsModel(ITournamentDataOperations tournamentDataAccess, ICategoryDataOperations categoryDataOperations, IScoresAllocationsDataOperations scoresAllocationsDataOperations)
        {
            _tournamentAccess = tournamentDataAccess;
            _categoryDataOperations = categoryDataOperations;
            _scoresAllocationsDataOperations = scoresAllocationsDataOperations;
            _tournamentAccess.RetrieveAllTournaments(Tournaments);
            SetCategories();
        }
        #endregion

        #region Public Methods
        public void OnGet()
        {

        }
        
        /// <summary>
        /// Retrieves the top 3 teams in all categories and sets them in the List<TotalScores> object.
        /// </summary>
        /// <param name="tournamentId">Tournament in Scope.</param>
        /// <param name="categoryId">Category in Scope.</param>
        /// <returns></returns>
        public List<TotalScores> RetrieveTopTeamsForCategory(Guid tournamentId, Guid categoryId)
        {

            //TODO: Lol moet kyk na die... Gan most prob die verkeerde data wys agv die teamScore checker.

            string jsonContent = _scoresAllocationsDataOperations.RetrievePointAllocationsForAChategory(tournamentId, categoryId)?.Scores;

            if (string.IsNullOrEmpty(jsonContent))
                return new List<TotalScores>();

            Dictionary<string, List<MatchScore>> teamScores = JsonConvert.DeserializeObject<Dictionary<string, List<MatchScore>>>(jsonContent);

            if (teamScores == null)
                return new List<TotalScores>();

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
                .Take(3)
                .ToList();
        }

        /// <summary>
        /// Retrieves a subset of tournaments based on the specified page number.
        /// </summary>
        /// <param name="pageCounter">The page number indicating which page of tournaments to retrieve.</param>
        /// <returns>A list of tournaments corresponding to the specified page.</returns>
        public List<Tournament> GetTournamentsByPage(int pageCounter) =>
            Tournaments.Skip((pageCounter - 1) * 5)
                           .Take(5)
                           .ToList();
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the Categories Property.
        /// </summary>
        private void SetCategories()
        {
            foreach(Tournament tournament in Tournaments)
                Categories.Add(_categoryDataOperations.RetrieveCategoriesOfTournament(tournament.Id).ToList());
        }
        #endregion
    }
}