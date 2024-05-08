using Business.DynamicModelReflector.Interfaces;
using Business.Genisis.Data.Models;
using Business.Genisis.DataAccess.Interfaces;

namespace Business.Genisis.DataAccess.DataAccess
{
    public class ResetPasswordTokenReflector : IResetPasswordTokenDataOperations
    {
        #region Fields
        IModelReflector _reflect;
        #endregion

        #region Constructors
        public ResetPasswordTokenReflector(IModelReflector reflect)
        {
            _reflect = reflect;
        }
        #endregion

        #region Public Methods
        public void WriteTokenToTable(string userEmail, Guid token)
        {
            try
            {
                ResetPasswordToken resetPasswordToken = new()
                {
                    Email = userEmail,
                    Token = token,
                    GeneratedDate = DateTime.Now,
                };

                _reflect
                    .Create(resetPasswordToken)
                    .Execute();
            }
            catch
            {
                throw;
            }
        }

        public bool IsValidToken(string userEmail, Guid token)
        {
            try
            {
                ResetPasswordToken resetPasswordToken = new();

                _reflect.Load(resetPasswordToken)
                    .Where(resetPasswordToken => resetPasswordToken.Email == userEmail && resetPasswordToken.Token == token)
                    .Execute();

                return resetPasswordToken.Token != Guid.Empty;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}