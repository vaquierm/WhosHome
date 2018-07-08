using System;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections.Generic;

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

        public static bool Register(string username, string password, string fullName, string macAddress, out string statusMessage, string piID = "")
        {
            UserPublic uPublic = new UserPublic(username, fullName, macAddress);
            UserPrivate uPrivate = new UserPrivate(username, password, piID);

            if (!IsUsernameValid(username))
            {
                statusMessage = $"The username '{username}' is already taken.";
                return false;
            }
            else if (!IsPiIDValid(piID))
            {
                statusMessage = $"The Pi ID '{piID}' is already taken.";
            }

            _documentClient.CreateDocumentAsync(PublicUsersCollectionUri, uPublic);
            _documentClient.CreateDocumentAsync(PrivateUsersCollectionUri, uPrivate);

            statusMessage = $"User '{username}' has been registered successfully.";
            return true;
        }

        private static bool IsUsernameValid(string username)
        {
            IQueryable<UserPublic> userPublicQueryable = _documentClient.CreateDocumentQuery<UserPublic>(
                PublicUsersCollectionUri).Where(f => f.Id == username);

            return (userPublicQueryable.Count() == 0);
        }

        private static bool IsPiIDValid(string piID)
        {
            IQueryable<UserPrivate> userPrivateQueryable = _documentClient.CreateDocumentQuery<UserPrivate>(
                PrivateUsersCollectionUri).Where(f => f.PiID == piID);

            return (userPrivateQueryable.Count() == 0);
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
        public string FullName { get; set; }
        public string MacAddress { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserPrivate
    {
        public UserPrivate(string username, string password, string piID = "")
        {
            Id = username;
            Password = password;
            PiID = PiID;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Password { get; set; }
        public string PiID { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
