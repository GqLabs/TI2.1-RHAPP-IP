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
    public class SerialDataPacket : RequestPacket
    {
        // Inherited fields: CMD
        // Introduced fields: Data (type = serialdata)

        public const string DefCmd = "SERIAL";

        //public Measurement Measurement { get; private set; }
        
        public string PatientUsername { get; private set; }

        public SerialDataPacket(Measurement measurement, string patientUsername) : base(DefCmd)
        {
            Initialize(measurement, patientUsername);
        }

        public SerialDataPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "SerialDataPacket ctor: json is null!");

            JToken measurement;
            JToken patientUsername;

            if (!(json.TryGetValue("Measurement", StringComparison.CurrentCultureIgnoreCase, out measurement)
                && json.TryGetValue("PatientUsername", StringComparison.CurrentCultureIgnoreCase, out patientUsername)))
                throw new ArgumentException("Measurement is not found in json: \n" + json);

            var measurementObj = JsonConvert.DeserializeObject<Measurement>(measurement.ToString());

            Initialize(measurementObj, (string)patientUsername);
        }

        private void Initialize(Measurement measurement, string patientUsername)
        {
            Measurement = measurement;
            PatientUsername = patientUsername;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("Measurement", JsonConvert.SerializeObject(Measurement));
            json.Add("PatientUsername", PatientUsername);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
