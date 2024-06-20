namespace Entities
{
    public static class Constants
    {
        public static class Keys
        {
            public const string SUCESS = "Sucess";
            public const string ADMIN = "Super Admin";
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
        }

        public static class VIEW_DATA
        {
            public const string LOGIN_MESSAGE = "LoginMessage";
        }
    }
}