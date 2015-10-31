using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Entity
{
    public class BikeTest
    {
        public List<Measurement> Measurements { get; private set; }
        public int Age { get; private set; }
        public String Weight { get; private set; }
        public Double Heartbeat { get; private set; }
        public String Username { get; private set; }
        public DateTime TimeStampStarted { get; private set; }
        public DateTime TimeStampStopped { get; private set; }

        // true == man; false == woman
        public Boolean Gender { get; private set; }

        public BikeTest(string username, bool gender, string weight, int age)
        {
            Age = age;
            Weight = weight;
            Username = username;
            Gender = gender;
        }

        public BikeTest(string username, bool gender, string weight, int age, double heartbeat, List<Measurement> measurements, DateTime timeStampStart, DateTime timeStampStop)
        {
            Age = age;
            Weight = weight;
            Username = username;
            Gender = gender;
            Heartbeat = heartbeat;
            Measurements = measurements;
            TimeStampStarted = timeStampStart;
            TimeStampStopped = timeStampStop;
        }
    }
}
