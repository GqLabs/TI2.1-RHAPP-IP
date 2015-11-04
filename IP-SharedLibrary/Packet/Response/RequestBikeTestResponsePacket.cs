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

        public BikeTest Biketest { get; private set; }
        public string PatientUsername { get; private set; }

        #region Constructors
        public RequestBikeTestResponsePacket(BikeTest bikeTest, string patientUsername)
            : base(DefCmd)
        {
            Initialize(bikeTest, patientUsername);
        }


        public RequestBikeTestResponsePacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "LoginResponsepacket ctor: json is null!");

            JToken biketest;
            JToken patientusername;

            if (!(json.TryGetValue("PatientUsername", StringComparison.CurrentCultureIgnoreCase, out patientusername)))
                throw new ArgumentException("PatientUsername is not found in json \n" + json);

            if (!(json.TryGetValue("Biketest", StringComparison.CurrentCultureIgnoreCase, out biketest)))
                throw new ArgumentException("Biketest is not found in json \n" + json);

            var biketestObj = JsonConvert.DeserializeObject<BikeTest>(biketest.ToString());

            Initialize(biketestObj, (string)patientusername);
        }
        #endregion

        #region Initializers
        private void Initialize(BikeTest bikeTest, string patientUsername)
        {
            this.Biketest = bikeTest;
            this.PatientUsername = patientUsername;
        }
        #endregion

        #region Override Methods
        public override JObject ToJsonObject()
        {
            var returnJson = base.ToJsonObject();
            returnJson.Add("PatientUsername", PatientUsername);
            returnJson.Add("Biketest", JsonConvert.SerializeObject(Biketest));

            return returnJson;

        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }

        #endregion
    }
}
