using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Entity
{
    public class BikeTest
    {
        public List<Measurement> Measurements { get; set; }
        public int Age { get; set; }
        public Double Weight { get; set; }
        public Double Heartbeat { get; set; }
        public String Username { get; set; }
        public double Vo2Max { get; set; }

        // true == man; false == woman
        public Boolean Gender { get; set; }

        public BikeTest()
        {

        }

        public BikeTest(string username, bool gender, double weight, int age)
        {
            Age = age;
            Weight = weight;
            Username = username;
            Gender = gender;
            Measurements = new List<Measurement>();
        }

        public BikeTest(string username, bool gender, double weight, int age, double heartbeat, List<Measurement> measurements,double vo2Max)
        {
            Age = age;
            Weight = weight;
            Username = username;
            Gender = gender;
            Heartbeat = heartbeat;
            Measurements = measurements;
            Vo2Max = vo2Max;
        }

        public void AddMeasurement(Measurement measurement)
        {
            Measurements.Add(measurement);
            Heartbeat = measurement.Pulse;
        }

        public JObject ToJsonObject()
        {
            var json = new JObject();
            json.Add("Measurements", JArray.FromObject(Measurements));
            json.Add("Age", Age);
            json.Add("Weight", Weight);
            json.Add("Heartbeat", Heartbeat);
            json.Add("Username", Username);
            json.Add("Vo2Max", Vo2Max);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
