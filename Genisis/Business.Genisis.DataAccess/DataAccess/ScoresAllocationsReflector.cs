using Business.DynamicModelReflector.Interfaces;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;

namespace Business.Genisis.DataAccess.DataAccess
{
    public class ScoresAllocationsReflector : IScoresAllocationsDataOperations
    {
        #region Fields
        IModelReflector _reflect;
        #endregion

        #region Constructors
        public ScoresAllocationsReflector(IModelReflector reflect)
        {
            _reflect = reflect;
        }
        #endregion

        #region Public Methods
        public void InsertPointAllocations(ScoresAllocations pointAllocations) =>
            _reflect
                .Create(pointAllocations)
                .Execute();

        public void UpdatePointAllocationsForCategory(ScoresAllocations pointAllocations, Guid tournamentId, Guid categoryId) =>
            _reflect
                .Update(pointAllocations)
                .Where(pointAllocations => pointAllocations.TournamentId == tournamentId && pointAllocations.CategoryId == categoryId)
                .Execute();

        public ScoresAllocations RetrievePointAllocationsForAChategory(Guid tournamentId, Guid categoryId)
        {
            ScoresAllocations pointAllocations = new();

            _reflect.Load(pointAllocations)
                .Where(pointAllocations => pointAllocations.TournamentId == tournamentId && pointAllocations.CategoryId == categoryId)
                .Execute();

            return pointAllocations;
        }

        public ScoresAllocations RetrieveMatchesToPlay(Guid categoryId)
        {
            ScoresAllocations pointAllocations = new();

            _reflect.Load(pointAllocations)
                .Select(pointAllocations => pointAllocations.MatchToPlay, pointAllocations => pointAllocations)
                .Where(pointAllocations => pointAllocations.CategoryId == categoryId)
                .Execute();

            return pointAllocations;
        }
        #endregion
    }
}