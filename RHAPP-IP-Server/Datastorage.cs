﻿using RHAPP_IP_Server.Properties;
using IP_SharedLibrary.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chatserver.FileController
{
    internal class Datastorage
    {
        private static Datastorage _instance;
        public static Datastorage Instance
        {
            get { return _instance ?? (_instance = new Datastorage()); }
        }

        private readonly List<User> _users = new List<User>();
        private readonly List<BikeTest> _bikeTests = new List<BikeTest>(); 

        private Datastorage()
        {
            //Debug code below:
            //_users.Add(new User("Henk", "testuser01", Crypto.CreateSHA256("1234")));
            //_users.Add(new User("Bart", "bart", Crypto.CreateSHA256("hoi")));
            //_users.Add(new User("Jordy", "jordy", Crypto.CreateSHA256("hoi")));
            
            OpenFromFile();
        } 

        public User GetUser(string username)
        {
            return _users.FirstOrDefault(user => user.Username == username);
        }

        public IEnumerable<User> GetUsers()
        {
            return _users;
        } 

        public IEnumerable<BikeTest> GetBikeTests(string username)
        {
            var x =
                from message in _bikeTests
                where message.Recipient == username || message.Sender == username
                orderby message.Timestamp ascending
                select message;

            return x;
        }

        public bool AddUser(User user)
        {
            _users.Add(user);
            return true;
        }

        public void AddMessage(BikeTest bikeTest)
        {
            _bikeTests.Add(bikeTest);
        }

        private void OpenFromFile()
        {
            var location = Settings.Default.UsersFileLocation;
            if (File.Exists(@location))
            {
                var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@location));
                foreach (var u in users)
                    _users.Add(u);
            }

            location = Settings.Default.BikeTestsFileLocation;
            if (!File.Exists(@location)) return;
            var messages = JsonConvert.DeserializeObject<List<BikeTest>>(File.ReadAllText(@location));
            foreach (var m in messages)
                _bikeTests.Add(m);
        }

        public void SaveToFile()
        {
            SaveToFile(_users, Settings.Default.UsersFileLocation);
            SaveToFile(_bikeTests, Settings.Default.BikeTestsFileLocation);
        }
        public void SaveToFile(object list, string location)
        {
            if (File.Exists(@location))
                File.Delete(@location);
            var fs = File.Open(@location, FileMode.Create);
            var sw = new StreamWriter(fs);
            JsonWriter jw = new JsonTextWriter(sw);
            {
                jw.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer();
                serializer.Serialize(jw, list);
            }
            jw.Close();
            sw.Close();
            fs.Close();
        }
    }
}
