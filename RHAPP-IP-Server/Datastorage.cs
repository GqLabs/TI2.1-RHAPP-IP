using RHAPP_IP_Server.Properties;
using IP_SharedLibrary.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IP_SharedLibrary.Utilities;
using System;

namespace RHAPP_IP_Server
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
            //Debug code below: (already saved in JSON File)
            //_users.Add(new User("Henk", "patient", Crypto.CreateSHA256("1234"), false));
            //_users.Add(new User("Piet", "testuser", Crypto.CreateSHA256("5678"), false));
            //_users.Add(new User("Bart", "bart", Crypto.CreateSHA256("hoi"), true));
            //_users.Add(new User("Karel", "karel", Crypto.CreateSHA256("hoi"), true));

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

        public IEnumerable<BikeTest> GetBikeTestsOfUser(string username)
        {
            IEnumerable<BikeTest> x = null;
            try
            {
                x =
                from bikeTest in _bikeTests
                where bikeTest.Username == username
                //orderby bikeTest.Measurements.FirstOrDefault().Time ascending
                select bikeTest;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return x;
        }

        public bool AddUser(User user)
        {
            _users.Add(user);
            return true;
        }

        public void AddBikeTest(BikeTest bikeTest)
        {
            _bikeTests.Add(bikeTest);
        }

        private bool AddMeasurementToLastBikeTestByUser(Measurement measurement, string username)
        {
            BikeTest bikeTest = _bikeTests
                .Where(biketest => biketest.Username == username)
                .Select(biketest => biketest).LastOrDefault();
            if (bikeTest == null)
                return false;
            bikeTest.Measurements.Add(measurement);
            if (bikeTest.Measurements.LastOrDefault() == measurement)
                return true;
            else
                return false;
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
            var biketests = JsonConvert.DeserializeObject<List<BikeTest>>(File.ReadAllText(@location));
            foreach (var b in biketests)
                _bikeTests.Add(b);
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
