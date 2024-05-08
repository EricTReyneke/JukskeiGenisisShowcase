using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.EmailService.Interfaces;
using Business.Genisis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Genisis.Pages
{
    public class ForgotPasswordModel : PageModel
    {

        //TODO: Add a success message when the email was sent successfuly.

        #region Fields
        IUserInformationDataOperations _userInformationDataOperations;
        IResetPasswordTokenDataOperations _resetPasswordTokenDataOperations;
        IMailerSerivce _mailerSerivce;
        IEncryption _encryption;
        #endregion

        #region Constructors
        public ForgotPasswordModel(IUserInformationDataOperations userInformationDataOperations, IMailerSerivce mailerSerivce, IEncryption encryption, IResetPasswordTokenDataOperations resetPasswordTokenDataOperations)
        {
            _userInformationDataOperations = userInformationDataOperations;
            _mailerSerivce = mailerSerivce;
            _encryption = encryption;
            _resetPasswordTokenDataOperations = resetPasswordTokenDataOperations;
        }
        #endregion

        #region Public Methods
        public void OnGet()
        {
        }

        public IActionResult OnPostResetPasswordEmail(string userEmail)
        {
            try
            {
                Guid token = Guid.NewGuid();
                ResetPasswordEmail resetPasswordEmail = new(EncryptUserEmail(userEmail), EncryptToken(token));

                if (_userInformationDataOperations.ValidateUserEmail(userEmail))
                    _mailerSerivce.SendEmail(resetPasswordEmail.Subject, resetPasswordEmail.EmailBodyString, userEmail);

                _resetPasswordTokenDataOperations.WriteTokenToTable(userEmail, token);

                return new JsonResult(new { success = "true" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = "false", error = $"Error: {ex}\n\nError Occurred sending email." });
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Encrypts the User Email.
        /// </summary>
        /// <param name="userEmail">User Email to Encrypt.</param>
        /// <returns>Encrypted user email.</returns>
        private string EncryptUserEmail(string userEmail) =>
            _encryption.Encrypt(userEmail);

        /// <summary>
        /// Encrypts the token which will be validated through the DB to check if the password is still in the state to be updated.
        /// </summary>
        /// <returns>Encrypted Token.</returns>
        private string EncryptToken(Guid token) =>
            _encryption.Encrypt(token.ToString());
        #endregion
    }
}