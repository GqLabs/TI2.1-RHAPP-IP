using IP_SharedLibrary.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Response
{
    public class RequestBikeTestResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-BIKETEST";

        public string PatientUsername { get; private set; }
        public List<BikeTest> Biketest { get; private set; }

        public RequestBikeTestResponsePacket(JObject json)
            :base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "StartTestpacket ctor: json is null!");

            JToken patientUsername;
            JToken biketest;

            if (!(json.TryGetValue("PatientUsername", StringComparison.CurrentCultureIgnoreCase, out patientUsername)
                && json.TryGetValue("Biketest", StringComparison.CurrentCultureIgnoreCase, out biketest)))
                throw new ArgumentException("Biketest is not found in json: \n" + json);

            var biketestObj = JsonConvert.DeserializeObject<List<BikeTest>>(biketest.ToString());

            Initialize((string)patientUsername, biketestObj);
        }

        public RequestBikeTestResponsePacket(string patientUsername, List<BikeTest> biketest)
            : base(Statuscode.Status.Ok, DefCmd)
        {
            Initialize(patientUsername, biketest);
        }

        private void Initialize(string patientUsername, List<BikeTest> biketest)
        {
            PatientUsername = patientUsername;
            Biketest = biketest;
            foreach(BikeTest test in biketest)
            {
                test.Measurements = null;
            }
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
