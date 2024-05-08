namespace Business.GalaxiaWordle.Interfaces
{
    public interface ILogin
    {
        /// <summary>
        /// Validate if the user credentials are valid.
        /// </summary>
        /// <param name="userNameOrPassword">Users User Name.</param>
        /// <param name="password">Users Password.</param>
        /// <returns>If the users data is in the database.</returns>
        bool ValidateUserCredentails(string userNameOrPassword, string password);
    }
}
