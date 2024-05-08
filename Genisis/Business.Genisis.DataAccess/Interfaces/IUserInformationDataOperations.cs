using Business.GalaxiaWordle.Data.Models;
using Business.Genisis.Data.Models;

namespace Business.Genisis.DataAccess.Interfaces
{
    public interface IUserInformationDataOperations
    {
        /// <summary>
        /// Retireves UserInformation from the users Username and Password.
        /// </summary>
        /// <param name="userNameOrEmail">Users username.</param>
        /// <param name="hashedPassword">Users Password.</param>
        /// <returns>Filled UserInformation.</returns>
        UserInformation RetrieveUserFromUserNameAndPassword(string userNameOrEmail, string hashedPassword);

        /// <summary>
        /// Validate if a user has a valid email in the UserInformation Table.
        /// </summary>
        /// <param name="userEmail">Users Email.</param>
        /// <returns>True or False: The email is in the UserInformation table.</returns>
        bool ValidateUserEmail(string userEmail);

        /// <summary>
        /// Creates new user in the Database.
        /// </summary>
        /// <param name="userInformation">UserInformation to create in the database.</param>
        void CreateNewUser(UserInformation userInformation);

        /// <summary>
        /// Retrieves the Users Infromation from the email Address.
        /// </summary>
        /// <param name="userEmail">Users Email Address</param>
        /// <returns>Users information.</returns>
        UserInformation RetrievesUsersInformationFromEmail(string userEmail);

        /// <summary>
        /// Updates the User Information where the Email Address matches.
        /// </summary>
        /// <param name="userInformation">Updated User Information.</param>
        /// <param name="userEmail">Users Email Address.</param>
        void UpdateUserInformation(UserInformation userInformation, string userEmail);

        /// <summary>
        /// Validates if the userName of email address is Unique.
        /// </summary>
        /// <param name="userName">User Name in question.</param>
        /// <param name="emailAddress">Email address in Question.</param>
        /// <returns>Return ValidateUserNameAndEmailAddress.</returns>
        ValidateUserNameAndEmailAddress IsUserNameAndEmailValid(string userName, string emailAddress);
    }
}