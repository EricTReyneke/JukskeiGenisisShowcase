namespace Business.Genisis.DataAccess.Interfaces
{
    public interface IResetPasswordTokenDataOperations
    {
        /// <summary>
        /// Writes the newly generated token to the ResetPasswordToken table.
        /// </summary>
        /// <param name="userEmail">Specified user email.</param>
        /// <param name="token">Generated Token.</param>
        void WriteTokenToTable(string userEmail, Guid token);

        /// <summary>
        /// Validates if a Token is valid and still in the ResetPasswordToken Table. 
        /// </summary>
        /// <param name="userEmail">Specified user email.</param>
        /// <param name="token">Token in Question.</param>
        /// <returns>True or False: If the token in question is valid.</returns>
        bool IsValidToken(string userEmail, Guid token);
    }
}