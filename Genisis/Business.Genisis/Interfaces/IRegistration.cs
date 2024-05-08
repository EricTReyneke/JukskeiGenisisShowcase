using Business.GalaxiaWordle.Data.Models;

namespace Business.GalaxiaWordle.Interfaces
{
    public interface IRegistration
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userInformation">New User data.</param>
        void CreateNewUser(UserInformation userInformation);
    }
}