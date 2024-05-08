using Business.Genisis.Data.Models;

namespace Business.Genisis.DataAccess.Interfaces
{
    public interface IScoresAllocationsDataOperations
    {
        /// <summary>
        /// Inserts the PointAllocations row into the PointsAllocations table.
        /// </summary>
        /// <param name="pointAllocations">Point Allocation object which will be inserted.</param>
        void InsertPointAllocations(ScoresAllocations pointAllocations);

        /// <summary>
        /// Updates PointAllocations row with the Tournament Id and the Category Id in the Where claues.
        /// </summary>
        /// <param name="pointAllocations">Updates ScoresAllocations data.</param>
        /// <param name="tournamentId">The unique identifier of the tournament.</param>
        /// <param name="categoryId">The unique identifier of the category.</param>
        void UpdatePointAllocationsForCategory(ScoresAllocations pointAllocations, Guid tournamentId, Guid categoryId);

        /// <summary>
        /// Retrieves point allocation record for a specific category within a tournament.
        /// </summary>
        /// <param name="tournamentId">The unique identifier of the tournament.</param>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>PointAllocations associated with the given tournament and category IDs.</returns>
        ScoresAllocations RetrievePointAllocationsForAChategory(Guid tournamentId, Guid categoryId);

        /// <summary>
        /// Retrieve The match that needs to be played.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>Returns the Match that needs to be played in the specified category.</returns>
        ScoresAllocations RetrieveMatchesToPlay(Guid categoryId);
    }
}