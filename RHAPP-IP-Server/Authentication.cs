using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RHAPP_IP_Server
{
    static class Authentication
    {
        //ConcurrentDictionary to enhance thread safety.
        private static readonly ConcurrentDictionary<User, ClientHandler> AuthUsers 
            = new ConcurrentDictionary<User, ClientHandler>();

        public static Boolean Authenticate(String username, String passhash, ClientHandler clientHandler)
        {
            //check that user and passhash are valid.  
            var user = Datastorage.Instance.GetUser(username);

            // if the user is null, there is no user found. 
            // => return false (authentication failed).
            if (user == null)  return false;
            // if the password is not equals to the passhash, 
            // the password is incorrect. => return false (auth failed).
            if (user.Password != passhash) return false;

            // Check if user was already in the list.
            var result = AuthUsers.Keys.FirstOrDefault(u => u.Username == user.Username);
            if (result != null)
            {
                return false;
            }
            AuthUsers.GetOrAdd(user, clientHandler);
            return true;
        }


        public static Boolean CheckLoggedIn(String username)
        {
            bool LoggedIn = false;
            foreach (User u in AuthUsers.Keys)
            {
                if (u.Username == username) LoggedIn = true;
            }
            return LoggedIn;
        }
        public static void DeAuthenticate(String username)
        {
            var users = AuthUsers.Keys.Where(user => user.Username == username);
            foreach (var user in users)
            {
                ClientHandler s;
                AuthUsers.TryRemove(user, out s);
            }
        }

        public static ClientHandler GetStream(String username)
        {
            return AuthUsers.First(x => x.Key.Username == username).Value;
        }

        public static User GetUser(String username)
        {
            return AuthUsers.First(x => x.Key.Username == username).Key;
        }

        public static List<User> GetAllUsers()
        {
            return AuthUsers.Keys.ToList();
        }
    }
}
