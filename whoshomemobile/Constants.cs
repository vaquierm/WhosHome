using System;
namespace whoshomemobile
{
    public class StringConstants
    {
        public const string ConfirmString = "Confirm";
        public const string EditString = "Edit";
        public const string SignInString = "Sign In";
        public const string RegisterString = "Register";

        public const string SigningInMessage = "Sign in to Who's Home";
        public const string RegisterMessage = "Register to Who's Home";

        public const string PasswordChangedMessage = "Password changed successfully";
    }

    public enum InputType
    {
        Username, Password, FullName, MacAddress, PiID
    }
}
