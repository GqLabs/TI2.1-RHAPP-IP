using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Request
{
    public class RequestBikeTestPacket : RequestPacket
    {
        public const string DefCmd = "REQBIKETEST";

        public string PatientUsername { get; private set; }

        public RequestBikeTestPacket(JObject json)
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "StartTestpacket ctor: json is null!");

            JToken patientusername;

            if (!(json.TryGetValue("PatientUsername", StringComparison.CurrentCultureIgnoreCase, out patientusername)))
                throw new ArgumentException("Username is not found in json: \n" + json);

            Initialize((string)patientusername);
        }

        public RequestBikeTestPacket(string patientusername)
            : base(DefCmd)
        {
            Initialize(patientusername);
        }

        private void Initialize(string patientusername)
        {
            PatientUsername = patientusername;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("PatientUsername", PatientUsername);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
