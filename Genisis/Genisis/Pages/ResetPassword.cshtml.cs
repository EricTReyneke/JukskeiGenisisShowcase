using Business.GalaxiaWordle.Data.Models;
using Business.GalaxiaWordle.Interfaces;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Genisis.Pages
{
    public class ResetPasswordModel : PageModel
    {

        //TODO: Add the DB update login.
        //TODO: Add a success message when the users credentails where updated successfuly.

        #region Fields
        IResetPasswordTokenDataOperations _resetPasswordTokenDataOperations;
        IUserInformationDataOperations _userInformationDataOperations;
        IEncryption _encryption;
        string _userEmail;
        string _token;
        #endregion

        #region Constructors
        public ResetPasswordModel(IEncryption encryption, IResetPasswordTokenDataOperations resetPasswordTokenDataOperations, IUserInformationDataOperations userInformationDataOperations)
        {
            _encryption = encryption;
            _resetPasswordTokenDataOperations = resetPasswordTokenDataOperations;
            _userInformationDataOperations = userInformationDataOperations;

        }
        #endregion

        #region Public Methods
        public IActionResult OnGet(string userEmail, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail))
                    throw new ArgumentNullException($"{nameof(userEmail)} is empty when trying to reset password.");

                if (string.IsNullOrEmpty(token))
                    throw new ArgumentNullException($"{nameof(token)} is empty when trying to reset password.");

                DecryptUserEmail(userEmail);
                SerilizeUserEmail();
                DecryptToken(token);

                ValidateToken();

                return Page();
            }
            catch (Exception ex)
            {
                return new RedirectResult($"/Login?error=Error in Reset Password Page. Error: {ex}");
            }
        }

        public IActionResult OnPostUpdatePasswordDetails(string newPassword)
        {
            try
            {
                string userEmail = DeserializeUserEmail();

                if(string.IsNullOrEmpty(userEmail))
                {
                    UserInformation updatedUserInformation = _userInformationDataOperations.RetrievesUsersInformationFromEmail(userEmail);
                    updatedUserInformation.Password = _encryption.OneWayHashEncryption(newPassword);

                    _userInformationDataOperations.UpdateUserInformation(updatedUserInformation, userEmail);

                    return new JsonResult(new { valid = "true" });
                }

                return new JsonResult(new { valid = "false", error = $"Error: User email was Null." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { valid = "false", error = $"Error: {ex}\n\nError Occurred when updating new Password." });
            }
        }
        #endregion

        #region Private Methods
        private bool ValidateToken()
        {
            try
            {
                Guid guidToken = Guid.Empty;
                if (Guid.TryParse(_token, out guidToken))
                    return _resetPasswordTokenDataOperations.IsValidToken(_userEmail, guidToken);

                throw new Exception($"{_token} is not a valid Guid.");
            }
            catch (Exception exception)
            {
                throw new Exception($"Error: {exception}");
            }
        }

        /// <summary>
        /// Decrypts the User Email.
        /// </summary>
        private void DecryptUserEmail(string userEmail) =>
            _userEmail = _encryption.Decrypt(userEmail);

        /// <summary>
        /// Decrypts the token which will be validated through the DB to check if the password is still in the state to be updated.
        /// </summary>
        private string DecryptToken(string token) =>
            _token = _encryption.Decrypt(token);

        /// <summary>
        /// Sets the User Email in a Environment Variable.
        /// </summary>
        private void SerilizeUserEmail() =>
            Environment.SetEnvironmentVariable("UserEmail", JsonSerializer.Serialize(_userEmail), EnvironmentVariableTarget.Process);

        /// <summary>
        /// Retrieves User Email.
        /// </summary>
        private string DeserializeUserEmail() =>
            JsonSerializer.Deserialize<string>(Environment.GetEnvironmentVariable("UserEmail", EnvironmentVariableTarget.Process));
        #endregion
    }
}