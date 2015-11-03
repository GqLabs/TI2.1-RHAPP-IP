using System;

namespace IP_SharedLibrary.Entity
{
    public class User
    {
        public string Nickname { get; set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsDoctor { get; private set; }

        public bool OnlineStatus { get; set; }

        public User(string nickname, string username, string password, bool isDoctor)
        {
            Nickname = nickname;
            Username = username;
            Password = password;
            IsDoctor = isDoctor;
        }

        public User(string nickname, string username, string password)
        {
            Nickname = nickname;
            Username = username;
            Password = password;
        }

        public void ChangeNickname(string nickname)
        {
            Nickname = nickname;
        }

        public void ChangePassword()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Nickname, OnlineStatus ? "Online" : "Offline");
        }
    }
}