using Business.DynamicModelReflector.Interfaces;
using Business.GalaxiaWordle.Data.Models;
using Business.GalaxiaWordle.Interfaces;
using Business.Genisis.DataAccess.Interfaces;
using Business.Genisis.Interfaces;

namespace Business.GalaxiaWordle.Registrations
{
    public class HashedRegistration : IRegistration
    {
        #region Fields
        IEncryption _encryption;
        IUserInformationDataOperations _userInformationDataAccess;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the BasicRegistration class.
        /// </summary>
        public HashedRegistration(IEncryption encryption, IUserInformationDataOperations userInformationDataAccess)
        {
            _encryption = encryption;
            _userInformationDataAccess = userInformationDataAccess;
        }
        #endregion

        #region Public Methods
        public void CreateNewUser(UserInformation userInformation)
        {
            try
            {
                if (userInformation == null) throw new ArgumentNullException(nameof(userInformation));

                userInformation.Password = _encryption.OneWayHashEncryption(userInformation.Password);

                _userInformationDataAccess.CreateNewUser(userInformation);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}