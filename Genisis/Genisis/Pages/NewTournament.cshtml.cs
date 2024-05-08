using Business.DynamicModelReflector.Models;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace Genisis.Pages
{
    public class NewTournamentModel : PageModel
    {
        #region Fields
        ITournamentDataOperations _tournamentDataAccess;
        ICategoryDataOperations _categoryReflector;
        IEncryption _encryption;
        IEnumerable<PrimaryKeyInfo> _primaryKeyInfos;
        public static List<Category> categories = new();
        #endregion

        #region Properties
        public string ErrorLogging { get; set; }
        #endregion

        #region Constructors
        public NewTournamentModel(ITournamentDataOperations tournamentDataAccess, ICategoryDataOperations categoryReflector, IEncryption encryption)
        {
            _tournamentDataAccess = tournamentDataAccess;
            _categoryReflector = categoryReflector;
            _encryption = encryption;
        }
        #endregion

        #region Public Methods
        public void OnGet()
        {
        }

        public IActionResult OnPostCreateNewTournament(Tournament newTournament)
        {
            try
            {
                _primaryKeyInfos = _tournamentDataAccess.CreateNewTournament(newTournament);
                CreateNewCategory();

                return new RedirectResult($"/AddTeam?tGuid={HttpUtility.UrlEncode(_encryption.Encrypt(_primaryKeyInfos.FirstOrDefault().InsertedValue.ToString()))}");
            }
            catch (Exception ex)
            {
                if(_primaryKeyInfos != null && Guid.TryParse(_primaryKeyInfos.FirstOrDefault().InsertedValue.ToString(), out Guid tournamentId))
                    _tournamentDataAccess.DeleteTournament(tournamentId);

                ModelState.AddModelError(ErrorLogging, ex.Message);
                throw;
            }
        }

        public JsonResult OnPostAddChatagoryToList(string categoryName)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryName) || categoryName.Length > 30)
                    throw new Exception("Please enter a valid category name between 1 and 30 Characters.");

                categories.Add(new Category() { Name = categoryName });

                return new JsonResult(new { message = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message });
            }
        }

        public JsonResult OnPostRemoveCategoryFromList(string categoryName)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryName) || !categories.Contains(new Category() { Name = categoryName }))
                    throw new Exception("Please enter a valid category to remove.");

                categories.Remove(new Category() { Name = categoryName });

                return new JsonResult(new { message = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { message = ex.Message });
            }
        }
        #endregion

        #region Private Methods
        private void CreateNewCategory()
        {
            PrimaryKeyInfo? primaryKeyInfo = _primaryKeyInfos.FirstOrDefault();

            if (primaryKeyInfo.InsertedValue == null)
                throw new Exception("Failed to create Tournament.");

            foreach (Category category in categories)
                if (Guid.TryParse(primaryKeyInfo.InsertedValue.ToString(), out Guid tournamentId))
                    category.TournamentId = tournamentId;

            _categoryReflector.CreateNewCategory(categories);
        }
        #endregion
    }
}