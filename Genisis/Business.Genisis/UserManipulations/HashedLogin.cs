using Business.DynamicModelReflector.Interfaces;
using Business.GalaxiaWordle.Data.Models;
using Business.GalaxiaWordle.Interfaces;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.Interfaces;

namespace Business.GalaxiaWordle.Login.Logins
{
    public class HashedLogin : ILogin
    {
        #region Fields
        IEncryption _encryption;
        IUserInformationDataOperations _userInformationDataAccess;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the BasicLogin class.
        /// </summary>
        public HashedLogin(IEncryption encryption, IUserInformationDataOperations userInformationDataAccess)
        {
            _encryption = encryption;
            _userInformationDataAccess = userInformationDataAccess;
        }
        #endregion

        #region Public Methods
        public bool ValidateUserCredentails(string userNameOrEmail, string password)
        {
            string hashedPassword = _encryption.OneWayHashEncryption(password);

            UserInformation userLogin = _userInformationDataAccess.RetrieveUserFromUserNameAndPassword(userNameOrEmail, hashedPassword);

            return !string.IsNullOrEmpty(userLogin.UserName);
        }
        #endregion
    }
}