using Business.Genisis.Data.Models;

namespace Business.Genisis.DataAccess.Interfaces
{
    public interface ICategoryDataOperations
    {
        /// <summary>
        /// Creates new Category in the database with the values provided in the data model.
        /// </summary>
        /// <param name="newCategory">List of Category DataModels with new Category data.</param>
        void CreateNewCategory(IEnumerable<Category> newCategory);

        /// <summary>
        /// Retrieves the Categories within a Tournament.
        /// </summary>
        /// <param name="tournamentId">Tournament Id.</param>
        /// <returns>Categories in a Tournament.</returns>
        IEnumerable<Category> RetrieveCategoriesOfTournament(Guid tournamentId);
    }
}