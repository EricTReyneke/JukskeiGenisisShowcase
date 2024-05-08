using Business.DynamicModelReflector.Interfaces;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;

namespace Business.Genisis.DataAccess.DataAccess
{
    public class CategoryReflector : ICategoryDataOperations
    {
        #region Fields
        IModelReflector _reflect;
        #endregion

        #region Constructors
        public CategoryReflector(IModelReflector reflect)
        {
            _reflect = reflect;
        }
        #endregion

        #region Public Methods
        public void CreateNewCategory(IEnumerable<Category> newCategory)
        {
            try
            {
                _reflect
                    .Create(newCategory)
                    .Execute();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Category> RetrieveCategoriesOfTournament(Guid tournamentId)
        {
            IEnumerable<Category> categories = new List<Category>();

            _reflect
                .Load(categories)
                .Where(category => category.TournamentId == tournamentId)
                .Execute();

            return categories;
        }
        #endregion
    }
}