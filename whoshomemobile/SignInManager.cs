﻿using System;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace whoshomemobile
{
    public class SignInManager
    {
        private static DocumentClient _documentClient;
        private static Uri PublicUsersCollectionUri;
        private static Uri PrivateUsersCollectionUri;
        private static Uri PublicUserDocumentUri
        {
            get
            {
                if (userPublic == null)
                    return null;
                else
                    return UriFactory.CreateDocumentUri(Authentification.DatabaseName, Authentification.PublicUserCollectionName, userPublic.Id);
            }
        }
        private static Uri PrivateUserDocumentUri
        {
            get
            {
                if (userPrivate == null)
                    return null;
                else
                    return UriFactory.CreateDocumentUri(Authentification.DatabaseName, Authentification.PrivateUsersCollectionName, userPrivate.Id);
            }
        }

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
            _documentClient = new DocumentClient(new Uri(Authentification.DBEndpointUri), Authentification.DBKey);
            PublicUsersCollectionUri = UriFactory.CreateDocumentCollectionUri(Authentification.DatabaseName, Authentification.PublicUserCollectionName);
            PrivateUsersCollectionUri = UriFactory.CreateDocumentCollectionUri(Authentification.DatabaseName, Authentification.PrivateUsersCollectionName);
        }

        public static List<UserPublic> GetAllUsers()
        {
            IQueryable<UserPublic> users = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri);

            return users.ToList();
        }

        public static bool LogIn(string username, string password, out string statusMessage) {
            IQueryable<UserPrivate> userPrivateQueryable = _documentClient.CreateDocumentQuery<UserPrivate>(
                PrivateUsersCollectionUri).Where(f => f.Id == username && f.Password == password);
            
            if (userPrivateQueryable.Count() != 1)
            {
                statusMessage = "Log in failed! Username or password is incorrect.";
                return false;
            }

            foreach(UserPrivate user in userPrivateQueryable)
            {
                userPrivate = user;
            }

            statusMessage = $"Logged in as '{username}'";

            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.Id == username);
            
            foreach (UserPublic user in userPublicQueryable)
            {
                userPublic = user;
            }

            return true;
        }

        public static bool Register(string username, string password, string fullName, string macAddress, out string statusMessage)
        {
            UserPublic uPublic = new UserPublic(username, fullName, macAddress);
            UserPrivate uPrivate = new UserPrivate(username, password);

            if (!InputValidation.ValidateUsername(username, out statusMessage))
                return false;
           
            if (!InputValidation.ValidatePassword(password, out statusMessage))
                return false;

            string idToReplace = null;
            if (!InputValidation.ValidateMacAddress(macAddress, out statusMessage))
            {
                if (statusMessage != StringConstants.MacAddressAlreadyExistsMessage)
                {
                    return false;
                }
                if (IsActualUser(macAddress, out idToReplace))
                    return false;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                statusMessage = "Your full name cannot be empty";
                return false;
            }

            if (idToReplace == null)
                _documentClient.CreateDocumentAsync(PublicUsersCollectionUri, uPublic);
            else
                _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(Authentification.DatabaseName, Authentification.PublicUserCollectionName, idToReplace), uPublic);
            _documentClient.CreateDocumentAsync(PrivateUsersCollectionUri, uPrivate);

            userPublic = uPublic;
            userPrivate = uPrivate;

            statusMessage = $"User '{username}' has been registered successsfully.";
            return true;
        }

        public static bool UpdateUserPublic(InputType type, out string ErrorMessage) {
            Uri documentUri = PublicUserDocumentUri;
            if (documentUri == null)
            {
                ErrorMessage = StringConstants.UserDataNotLoadedErrorMessage;
                return false;
            }

            if (type == InputType.MacAddress && !InputValidation.ValidateMacAddress(userPublic.MacAddress, out ErrorMessage)) 
            {
                return false;
            }

            _documentClient.ReplaceDocumentAsync(documentUri, userPublic);

            ErrorMessage = $"{type} {StringConstants.UpdatedSuccessfullyMessage}";
            return true;
        }

        public static bool UpdateUserPrivate(InputType type, out string ErrorMessage)
        {
            Uri documentUri = PrivateUserDocumentUri;
            if (documentUri == null)
            {
                ErrorMessage = StringConstants.UserDataNotLoadedErrorMessage;
                return false;
            }

            if (type == InputType.Password && !InputValidation.ValidatePassword(userPrivate.Password, out ErrorMessage))
            {
                return false;
            }

            _documentClient.ReplaceDocumentAsync(documentUri, userPrivate);

            ErrorMessage = $"{type} {StringConstants.UpdatedSuccessfullyMessage}";
            return true;
        }

        public static bool AddMacAddress(string fullName, string macAddress, out string ErrorMessage)
        {
            if (!InputValidation.ValidateMacAddress(macAddress, out ErrorMessage))
            {
                return false;
            }

            UserPublicNoUserName newUser = new UserPublicNoUserName(fullName, macAddress);
            _documentClient.CreateDocumentAsync(PublicUsersCollectionUri, newUser);

            ErrorMessage = $"The mac address for {fullName} was added to the database.";
            return true;
        }

        public static bool RequestPermission(string userToRequest, out string ErrorMessage)
        {
            if (!UsernameExists(userToRequest))
            {
                ErrorMessage = $"You cannot request the permission to scan {userToRequest}'s house because this user does not exist...";
                return false;
            }

            //TODO: get the user with this pi ID and add yourself to the request list and push to database
            ErrorMessage = $"A request was sent to scan the user {userToRequest}!";
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

        public static bool UsernameExists(string username)
        {
            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.Id == username);

            return (userPublicQueryable.Count() != 0);
        }

        public static bool MacAddressExists(string macAddress)
        {
            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.MacAddress == macAddress);

            return (userPublicQueryable.Count() != 0);
        }

        public static bool IsActualUser(string macAddress, out string idToReplace)
        {
            idToReplace = null;

            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.MacAddress == macAddress);

            UserPublic user = null;

            foreach (UserPublic u in userPublicQueryable)
            {
                user = u;
            }

            if (user == null)
                return false;

            idToReplace = user.Id;

            return !InputValidation.IsGeneratedId(user.Id);
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

        public static bool ValidateUsername(string username, out string ErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ErrorMessage = "Your usernamecannotbe empty";
                return false;
            }
            if (IsGeneratedId(username))
            {
                ErrorMessage = "Your username cannot cannot be of this format";
                return false;
            }
            if (SignInManager.UsernameExists(username))
            {
                ErrorMessage = "This usernme is already taken";
                return false;
            }

            ErrorMessage = "Username Valid";
            return true;
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

            input = input.Trim(' ');

            Regex r = new Regex("^[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");


            if (r.IsMatch(input))
            {
                if (SignInManager.MacAddressExists(address))
                {
                    ErrorMessage = StringConstants.MacAddressAlreadyExistsMessage;
                    return false;
                }

                ErrorMessage = "Mac Address is valid";
                return true;
            }
            else
            {
                ErrorMessage = "Mac Address format is invalid...";
                return false;
            }
        }

        public static bool IsGeneratedId(string id)
        {
            Regex r = new Regex("^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$");

            return r.IsMatch(id);
        }
    }


    public class UserPublic : UserPublicNoUserName
    {
        public UserPublic(string username, string fullName, string macAddress) : base(fullName, macAddress)
        {
            Id = username;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserPublicNoUserName
    {
        public UserPublicNoUserName(string fullName, string macAddress)
        {
            FullName = fullName;
            MacAddress = macAddress;
        }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "macAddress")]
        public string MacAddress { get; set; }
        [JsonProperty(PropertyName = "authorizedPiList")]
        public List<AuthorizedPi> AuthorizedPiList = new List<AuthorizedPi>();
        [JsonProperty(PropertyName = "scanRequestList")]
        public List<ScanRequest> ScanRequestList = new List<ScanRequest>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserPrivate
    {
        public UserPrivate(string username, string password)
        {
            Id = username;
            Password = password;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "pwassord")]
        public string Password { get; set; }
        [JsonIgnore]
        public string PiConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_piConnectionString))
                    _piConnectionString = IoTClientManager.GetPiConnectionString(Id);

                return _piConnectionString;
            }
        }
        [JsonIgnore]
        private string _piConnectionString = null;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ScanRequest
    {
        public ScanRequest(string usernameRequester, string fullNameRequester)
        {
            UsernameRequester = usernameRequester;
            FullNameRequester = fullNameRequester;
        }

        [JsonProperty(PropertyName = "usernameRequester")]
        public string UsernameRequester { get; set; }
        [JsonProperty(PropertyName = "fullNameRequester")]
        public string FullNameRequester { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AuthorizedPi
    {
        public AuthorizedPi(string piID, string fullNameOwner, string preferedPiName = null)
        {
            PiID = piID;
            FullNameOwner = fullNameOwner;
            PreferedPiName = preferedPiName;
        }

        [JsonProperty(PropertyName = "piID")]
        public string PiID { get; set; }
        [JsonProperty(PropertyName = "fullNameOwner")]
        public string FullNameOwner { get; set; }
        [JsonProperty(PropertyName = "preferedPiName")]
        public string PreferedPiName
        {
            get
            {
                if (string.IsNullOrEmpty(_preferedPiName))
                    return $"{FullNameOwner}'s Home ({PiID})";
                else
                    return _preferedPiName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _preferedPiName = $"{value} ({PiID})";
                
            }
        }
        [JsonIgnore]
        private string _preferedPiName = null;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
