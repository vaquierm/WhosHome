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

        public const string AddYourFriendsMessage = "Contribute to the database of mac addresses by adding your friends and family to be able to see if they are home";
        public const string PermissionMessage = "Ask your friends for permission to scan their homes and find out if you're left out from a party";
    }

    public enum InputType
    {
        Username, Password, FullName, MacAddress, PiID
    }
}
