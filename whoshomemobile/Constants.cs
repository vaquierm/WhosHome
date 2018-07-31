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
        public const string WouldLikeToScanYourHomeMessage = "{0} ({1}) would like to scan your home.";
        public const string RenamePiMessage = "Rename {0}";
        public const string NewPiNameCannotBeEmptyErrorMessage = "Name new name for {0} cannot be empty";
        public const string CannorRenameYourOwnHomeErrorMessage = "You cannot rename your own home";
        public static string FullNameCannotBeEmpty = "Full name cannot be empty";

        public const string ContinueToMainScreenMessage = "You have processed all your scan requests, continue to the main screen";

        public const string ScanAndFindOutWhoIsHomeMessage = "Scan and find out who's home!";
        public const string ScanSuccessfulMessage = "Scan {0} successfull!";
        public const string SomethingWentWrongWhileScanning = "Something went wrong while scanning {0}...";

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
        PiID,
        Unspecified
    }
}
