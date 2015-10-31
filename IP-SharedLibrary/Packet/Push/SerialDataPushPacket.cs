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

        public SerialDataPushPacket(Measurement measurement) : base(DefCmd)
        {
            Initialize(measurement);
        }

        public SerialDataPushPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "SerialDataPushPacket ctor: json is null!");

            JToken measurementToken;

            if (!(json.TryGetValue("Measurement", StringComparison.CurrentCultureIgnoreCase, out measurementToken)))
                throw new ArgumentException("Measurement is not found in json: \n" + json);

            var measurement = JsonConvert.DeserializeObject<Measurement>(measurementToken.ToString());

            Initialize(measurement);
        }

        private void Initialize(Measurement measurement)
        {
            Measurement = measurement;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Measurement", JsonConvert.SerializeObject(Measurement));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
