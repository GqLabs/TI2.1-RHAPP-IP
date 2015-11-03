using IP_SharedLibrary.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Push
{
    public class SerialDataPushPacket : PushPacket
    {
        // Inherited fields: CMD
        // Introduced fields: Data (type = serialdata)

        public const string DefCmd = "PUSH-SERDAT";

        public Measurement Measurement { get; private set; }

        public string Username { get; private set; }

        public SerialDataPushPacket(Measurement measurement, string username) : base(DefCmd)
        {
            Initialize(measurement, username);
        }

        public SerialDataPushPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "SerialDataPushPacket ctor: json is null!");

            JToken measurementToken;
            JToken username;

            if (!(json.TryGetValue("Measurement", StringComparison.CurrentCultureIgnoreCase, out measurementToken)
                && (json.TryGetValue("Username", StringComparison.CurrentCultureIgnoreCase, out username))))
                throw new ArgumentException("Measurement & Username is not found in json: \n" + json);

            var measurement = JsonConvert.DeserializeObject<Measurement>(measurementToken.ToString());

            Initialize(measurement, (string)username);
        }

        private void Initialize(Measurement measurement, string username)
        {
            Measurement = measurement;
            Username = username;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Measurement", JsonConvert.SerializeObject(Measurement));
            json.Add("Username", JsonConvert.SerializeObject(Username));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
