using System;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace whoshomemobile.iOS
{
    public class SignInManager
    {
        private static DocumentClient _documentClient;
        private static Uri PublicUsersCollectionUri;
        private static Uri PrivateUsersCollectionUri;

        public static bool LogedIn {
            get
            {
                return userPublic != null && userPrivate != null;
            }
        }

        public static UserPublic userPublic = null;
        public static UserPrivate userPrivate = null;

        internal static void InitSignInManager()
        {
            //_documentClient = new DocumentClient(new Uri(Authentification.DBEndpointUri), Authentification.DBKey);
            PublicUsersCollectionUri = UriFactory.CreateDocumentCollectionUri("whoshome", "UsersPublic");
            PrivateUsersCollectionUri = UriFactory.CreateDocumentCollectionUri("whoshome", "UsersPrivate");
        }

        public static List<UserPublic> GetAllUsers()
        {
            IQueryable<UserPublic> users = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri);

            return users.ToList();
        }

        public static bool LogIn(string username, string password, out string statusMessage) {
            IQueryable<UserPrivate> userPrivateQueryable = _documentClient.CreateDocumentQuery<UserPrivate>(
                PublicUsersCollectionUri).Where(f => f.Id == username && f.Password == password);

            if (userPrivateQueryable.Count() != 1)
            {
                statusMessage = "Log in failed! Username or password is incorrect.";
                return false;
            }

            statusMessage = $"Logged in as '{username}'";

            userPrivate = userPrivateQueryable.First();

            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.Id == username);

            if (userPublicQueryable.Count() == 1)
                userPublic = userPublicQueryable.First();

            return true;
        }

        public static bool Register(string username, string password, string fullName, string macAddress, out string statusMessage, string piID)
        {
            UserPublic uPublic = new UserPublic(username, fullName, macAddress);
            UserPrivate uPrivate = new UserPrivate(username, password, piID);

            if (UsernameExists(username))
            {
                statusMessage = $"The username '{username}' is already taken.";
                return false;
            }
            else if (PiIDExists(piID))
            {
                statusMessage = $"The Pi ID '{piID}' is already taken.";
            }
            if (!InputValidation.ValidatePassword(password, out statusMessage))
                return false;

            _documentClient.CreateDocumentAsync(PublicUsersCollectionUri, uPublic);
            _documentClient.CreateDocumentAsync(PrivateUsersCollectionUri, uPrivate);

            userPublic = uPublic;
            userPrivate = uPrivate;

            statusMessage = $"User '{username}' has been registered successfully.";
            return true;
        }

        public static bool AddMacAddress(string fullName, string macAddress, out string ErrorMessage)
        {
            if (!InputValidation.ValidateMacAddress(macAddress, out ErrorMessage))
            {
                return false;
            }

            //TODO: generate an ID for this document
            UserPublic newUser = new UserPublic("", fullName, macAddress);
            _documentClient.CreateDocumentAsync(PublicUsersCollectionUri, newUser);

            ErrorMessage = $"The mac address for {fullName} was added to the database.";
            return true;
        }

        public static bool RequestPermission(string piID, out string ErrorMessage)
        {
            if (!PiIDExists(piID))
            {
                ErrorMessage = $"You cannot request the permission to scan '{piID}' because it does not exist...";
                return false;
            }

            //TODO: get the user with this pi ID and add yourself to the request list and push to database
            ErrorMessage = $"A request was sent to scan the pi '{piID}'!";
            return true;
        }

        public static string GetMacAddress()
        {
            string macAddress = string.Empty;
            try
            {
                NetworkInterface nic = (NetworkInterface.GetAllNetworkInterfaces()).First(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet && n.OperationalStatus == OperationalStatus.Up);
                string macAddressesString = nic.GetPhysicalAddress().ToString();

                for (int i = 0; i < macAddressesString.Length; i++)
                {
                    if (i > 0 && i % 2 == 0)
                        macAddress += ":";
                    macAddress += macAddressesString[i];
                }
            }
            finally
            {
            }
            return macAddress;
        }

        private static bool UsernameExists(string username)
        {
            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.Id == username);

            return (userPublicQueryable.Count() != 0);
        }

        private static bool PiIDExists(string piID)
        {
            IQueryable<UserPrivate> userPrivateQueryable = _documentClient.CreateDocumentQuery<UserPrivate>(
                PrivateUsersCollectionUri).Where(f => f.PiID == piID);

            return (userPrivateQueryable.Count() != 0);
        }
    }

    public class InputValidation
    {
        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <returns><c>true</c>, if password was validated, <c>false</c> otherwise.</returns>
        /// <param name="password">Password.</param>
        /// <param name="ErrorMessage">Error message.</param>
        public static bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Password should not be empty";
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (password.Length < 8)
            {
                ErrorMessage = "Password should not be less 8 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Validates the mac address.
        /// </summary>
        /// <returns><c>true</c>, if mac address was validated, <c>false</c> otherwise.</returns>
        /// <param name="address">Address.</param>
        /// <param name="ErrorMessage">Error message.</param>
        public static bool ValidateMacAddress(string address, out string ErrorMessage)
        {
            string input = address;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Mac Address should not be empty";
                return false;
            }

            input = input.Trim(' ', ':');

            Regex r = new Regex("^([:xdigit:]){12}$");


            if (r.IsMatch(input))
            {
                //TODO : check in database if it is unique
                ErrorMessage = "Mac Address is valid";
                return true;
            }
            else
            {
                ErrorMessage = "Mac Address format is invalid...";
                return false;
            }
        }
    }


    public class UserPublic
    {
        public UserPublic(string username, string fullName, string macAddress)
        {
            Id = username;
            FullName = fullName;
            MacAddress = macAddress;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "macAddress")]
        public string MacAddress { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserPrivate
    {
        public UserPrivate(string username, string password, string piID)
        {
            Id = username;
            Password = password;
            PiID = piID;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "piID")]
        public string PiID { get; set; }
        [JsonProperty(PropertyName = "authorizedPiList")]
        public List<string> AuthorizedPiList = new List<string>();
        [JsonProperty(PropertyName = "authorizedPiList")]
        public List<string> ScanRequestList = new List<string>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
