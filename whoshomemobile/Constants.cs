using System;
using System.ComponentModel;

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
        public const string EditInformationMessage = "Edit your user profile information";

        public const string SomethingWentWrongScanPermissionErrorMessage = "Something went wrong while requesting scan permission";

        public const string UpdatedSuccessfullyMessage = "updated successfully!";
        public const string MacAddressAlreadyExistsMessage = "This Mac Address already exists in our databases";

        public const string AddYourFriendsMessage = "Contribute to the database of mac addresses by adding your friends and family to be able to see if they are home";
        public const string PermissionMessage = "Ask your friends for permission to scan their homes and find out if you're left out from a party";

        public const string UserDataNotLoadedErrorMessage = "Your user data is not loaded. Try logging off and back in.";
    }

    public enum InputType
    {
        Username,
        Password,
        [Description("Full name")]
        FullName,
        [Description("Mac address")]
        MacAddress,
        [Description("Pi ID")]
        PiID
    }
}
