using Business.DynamicModelReflector.Interfaces;
using Business.GalaxiaWordle.Data.Models;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;

namespace Business.Genisis.DataAccess.DataAccess
{
    public class UserInformationReflector : IUserInformationDataOperations
    {
        #region Fields
        IModelReflector _reflect;
        #endregion

        #region Constructors
        public UserInformationReflector(IModelReflector reflect)
        {
            _reflect = reflect;
        }
        #endregion

        #region Public Methods
        public UserInformation RetrieveUserFromUserNameAndPassword(string userNameOrEmail, string hashedPassword)
        {
            UserInformation userInformation = new();

            _reflect
                .Load(userInformation)
                .Where(userLogin => userLogin.Password == hashedPassword && (userLogin.UserName == userNameOrEmail || userLogin.Email == userNameOrEmail))
                .Execute();

            return userInformation;
        }

        public bool ValidateUserEmail(string userEmail)
        {
            UserInformation userInformation = new();

            _reflect
                .Load(userInformation)
                .Where(userLogin => userLogin.Email == userEmail)
                .Execute();

            return userInformation.Email != null;
        }

        public void CreateNewUser(UserInformation userInformation)
        {
            try
            {
                _reflect
                    .Create(userInformation)
                    .Execute();
            }
            catch
            {
                throw;
            }
        }

        public UserInformation RetrievesUsersInformationFromEmail(string userEmail)
        {
            UserInformation userInformation = new();

            _reflect
                .Load(userInformation)
                .Where(userLogin => userLogin.UserName == userEmail)
                .Execute();

            return userInformation;
        }

        public void UpdateUserInformation(UserInformation userInformation, string userEmail)
        {
            _reflect
                .Update(userInformation)
                .Where(userInformation => userInformation.Email == userEmail)
                .Execute();
        }

        public ValidateUserNameAndEmailAddress IsUserNameAndEmailValid(string userName, string emailAddress)
        {
            UserInformation userInformation = new();

            _reflect
                .Load(userInformation)
                .Where(userLogin => userLogin.UserName == userName)
                .Execute();

            if (!string.IsNullOrEmpty(userInformation.UserName))
                return ValidateUserNameAndEmailAddress.UserName;

            _reflect
                .Load(userInformation)
                .Where(userLogin => userLogin.Email == emailAddress)
                .Execute();

            if (!string.IsNullOrEmpty(userInformation.Email))
                return ValidateUserNameAndEmailAddress.Email;

            return ValidateUserNameAndEmailAddress.Valid;
        }
        #endregion
    }
}