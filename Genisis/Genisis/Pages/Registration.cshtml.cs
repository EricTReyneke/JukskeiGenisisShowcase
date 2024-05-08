using Business.GalaxiaWordle.Data.Models;
using Business.GalaxiaWordle.Interfaces;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Genisis.Pages
{
    public class RegistrationModel : PageModel
    {

        //TODO: Add validation to check for no duplicate Emails or UserNames before creating a new User. If this is not added there will be moeilikheid when reseting passwords.
        //TODO: Add a success message when the user is created with a delay before going back to the Login page.

        #region Fields
        IRegistration _registration;
        IUserInformationDataOperations _userInformationDataOperations;
        #endregion

        #region Constructors
        public RegistrationModel(IRegistration registration, IUserInformationDataOperations userInformationDataOperations)
        {
            _registration = registration;
            _userInformationDataOperations = userInformationDataOperations;
        }
        #endregion

        #region Public Methods
        public void OnGet()
        {
        }

        public IActionResult OnPostCreateNewUser([FromBody] UserInformation userInformation)
        {
            try
            {
                ValidateUserNameAndEmailAddress validateUserNameAndEmailAddress = _userInformationDataOperations.IsUserNameAndEmailValid(userInformation.UserName, userInformation.Email);

                if (validateUserNameAndEmailAddress == ValidateUserNameAndEmailAddress.UserName)
                    return new JsonResult(new { success = "false", error = $"{ValidateUserNameAndEmailAddress.UserName}" });
                else if (validateUserNameAndEmailAddress == ValidateUserNameAndEmailAddress.Email)
                    return new JsonResult(new { success = "false", error = $"{ValidateUserNameAndEmailAddress.Email}" });

                _registration.CreateNewUser(userInformation);

                return new JsonResult(new { success = "true" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = "false", error = $"Error Message: {ex}" });
            }
        }
        #endregion
    }
}