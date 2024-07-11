namespace Entities
{
    public static class Constants
    {
        public static class Keys
        {
            public const string SUCESS = "Sucess";
            public const string ADMIN = "Super Admin";
            public const string REQUESTOR_DOES_NOT_EXISTS = "Requestor does not exists";
            public const string FORBIDDEN = "Forbidden";
            public const string INVALID_PAGINATION = "Invalid pagination";
            public const string USERS = "Users";
            public const string USER_DOES_NOT_EXISTS = "Users does not exists";
            public const string DOES_NOT_EXISTS = "{0} does not exists";
        }
        public static class Account
        {
            public const string EMAIL_EXISTS = "Email already registered";
            public const string USER_NAME_ALREADY_EXISTS = "Username already in use";
            public const string INVALID_DATE_OF_BIRTH = "Invalid date of birth";
            public const string USER_REGISTERED_SUCCESSFULLY = "User registered sucessfully";
            public const string INVALID_CREDENTIAL = "Invalid credential";
            public const string FAILED_LOGIN_ATTEMPT_LIMIT = "You have entered wrong credential 5 times, please try again after 24hours";
            public const string LOGGED_IN_SUCESSFULLY = "Logged in sucessfully";
            public const string USER_NOT_FOUND = "User not found";
            public const string ACCOUNT_VERIFICATION_SUCESSFULLY = "Account verification done sucessfully";
            public const string ACCOUNT_ALREADY_VERIFIED = "Your account is already verified";
            public const string INVALID_ACCOUNT_VERIFICATION_LINK = "Invalid link Please contact admin";
            public const string RESENT_EMAIL_VERIFICATION_LIMIT = "You have reched resend email verification, please contact support";
            public const string EMAIL_VERIFICATION_SENT_SUCESSFULLY = "Email verification sent sucessfully";
            public const string PASSWORD_RESET_EMAIL_LIMIT = "You have reached the maximum number of password reset requests for today";
            public const string EMAIL_SENT_TO_RESET_PASSWORD = "A password reset link has been sent to your registered email address. Please check your email to reset your password.";
            public const string PASSWORD_RESET_LINK_DOES_NOT_EXISTS = "Password reset link does not exits, please contact support or generate new link";
            public const string PASSWORD_UPDATED_SUCESSFULLY = "Password updated sucessfully";
            public const string VERIFY_EMAIL_ID = "Your email is not verified, Please verify your email id";
        }

        public static class SELLER
        {
            public const string REQUEST_ALREADY_EXISTS = "You have already submitted a request to become a seller. Your application is currently under review. We will notify you once a decision has been made. Thank you for your patience and interest in becoming a seller on our platform.";
            public const string REQEUST_SUBMITTED = "Your application to become a seller has been submitted successfully.";
            public const string SELLET_REQUEST = "Seller Reqeust";
        }

        public static class VIEW_DATA
        {
            public const string LOGIN_MESSAGE = "LoginMessage";
        }
    }
}