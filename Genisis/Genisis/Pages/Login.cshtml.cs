using Business.GalaxiaWordle.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Genisis.Pages
{
    public class LoginModel : PageModel
    {
        #region Fields
        ILogin _login;
        #endregion

        #region Properties
        public string ErrorMessage { get; set; }
        #endregion

        #region Constructors
        public LoginModel(ILogin login)
        {
            _login = login;
        }
        #endregion

        #region Public Methods
        public void OnGet(string error = null)
        {
            ErrorMessage = error;
        }

        public IActionResult OnPostValidateUserInfo(string userNameOrEmail, string password)
        {
            try
            {
                if (_login.ValidateUserCredentails(userNameOrEmail, password))
                    return new JsonResult(new { success = "true" });

                return new JsonResult(new { success = "false", error = "The Credentials entered does not exist.\nPlease try again." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = "false", error = $"Error: {ex}" });
            }
        }
        #endregion
    }
}