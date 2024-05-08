namespace Business.Genisis.Data.Models
{
    public class ResetPasswordEmail
    {
        #region Fields
        string userEmail;
        string token;
        #endregion

        #region Constructors
        public ResetPasswordEmail(string userEmail, string token)
        {
            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentNullException($"{nameof(userEmail)} is empty when trying to reset password.");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException($"{nameof(token)} is empty when trying to reset password.");

            this.userEmail = userEmail.Trim();
            this.token = token.Trim();
        }
        #endregion

        #region Properties
        public string Subject { get; set; } = "Reset Password for GNNS";

        public string EmailBodyString
        {
            get
            {
                return $@"<html>
<head>
    <style>
        body {{
            font-family: 'Arial', sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }}
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        .email-header {{
            background-color: #eff6ff;
            color: #333333;
            padding: 20px;
            text-align: center;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }}
        .email-body {{
            padding: 20px;
            color: #333333;
        }}
        .email-footer {{
            background-color: #eff6ff;
            color: #333333;
            padding: 20px;
            text-align: center;
            border-bottom-left-radius: 8px;
            border-bottom-right-radius: 8px;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            margin: 10px 0;
            border-radius: 20px;
            background-color: #2979ff;
            color: #ffffff;
            text-decoration: none;
            font-weight: bold;
        }}
        .small {{
            font-size: 0.9em;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""email-header"">
            <h1>Forgot Your Password?</h1>
        </div>
        <div class=""email-body"">
            <p>Hi there,</p>
            <p>You recently requested to reset your password for your account. Click the button below to reset it.</p>
            <a href=""https://localhost:7010/ResetPassword?userEmail={userEmail}&token={token}"" class=""button"">Reset Your Password</a>
            <p class=""small"">If you did not request a password reset, please ignore this email or reply to let us know. This password reset is only valid for the next 10 minutes.</p>
        </div>
        <div class=""email-footer"">
            <p>Thanks,<br>GNNS</p>
        </div>
    </div>
</body>
</html>";
            }
        }
        #endregion

        #region Private Methods
        private string UserEmail
        {
            get => userEmail;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException($"{nameof(UserEmail)} cannot be null or empty when trying to reset password.");
                userEmail = value.Trim();
            }
        }

        private string Token
        {
            get => token;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException($"{nameof(Token)} cannot be null or empty when trying to reset password.");
                token = value.Trim();
            }
        }
        #endregion
    }
}