using IP_SharedLibrary.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Request
{
    public class BikeTestPacket : RequestPacket
    {
        public const string DefCmd = "BIKETEST";

        public string PatientUsername { get; private set; }
        public BikeTest Biketest { get; private set; }

        public BikeTestPacket(JObject json)
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "StartTestpacket ctor: json is null!");

            JToken patientUsername;
            JToken biketest;

            if (!(json.TryGetValue("PatientUsername", StringComparison.CurrentCultureIgnoreCase, out patientUsername)
                && json.TryGetValue("Biketest", StringComparison.CurrentCultureIgnoreCase, out biketest)))
                throw new ArgumentException("Biketest is not found in json: \n" + json);

            var biketestObj = JsonConvert.DeserializeObject<BikeTest>(biketest.ToString());

            Initialize((string)patientUsername, biketestObj);
        }

        public BikeTestPacket(string patientUsername, BikeTest biketest)
            : base(DefCmd)
        {
            Initialize(patientUsername, biketest);
        }

        private void Initialize(string patientUsername, BikeTest biketest)
        {
            PatientUsername = patientUsername;
            Biketest = biketest;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("PatientUsername", PatientUsername);
            json.Add("Biketest", JsonConvert.SerializeObject(Biketest));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
