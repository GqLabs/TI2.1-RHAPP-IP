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

        public string DestinationUsername { get; private set; }
        public BikeTest Biketest { get; private set; }

        public BikeTestPacket(JObject json)
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "StartTestpacket ctor: json is null!");

            JToken destUsernameToken;
            JToken biketest;

            if (!(json.TryGetValue("DestinationUsername", StringComparison.CurrentCultureIgnoreCase, out destUsernameToken)
                && json.TryGetValue("Biketest", StringComparison.CurrentCultureIgnoreCase, out biketest)))
                throw new ArgumentException("Biketest is not found in json: \n" + json);

            var measurementObj = JsonConvert.DeserializeObject<BikeTest>(biketest.ToString());

            Initialize((string)destUsernameToken, measurementObj);
        }

        public BikeTestPacket(string destUsername, BikeTest biketest)
            : base(DefCmd)
        {
            Initialize(destUsername, biketest);
        }

        private void Initialize(string destUsername, BikeTest biketest)
        {
            DestinationUsername = destUsername;
            Biketest = biketest;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("DestinationUsername", DestinationUsername);
            json.Add("Biketest", JsonConvert.SerializeObject(Biketest));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
