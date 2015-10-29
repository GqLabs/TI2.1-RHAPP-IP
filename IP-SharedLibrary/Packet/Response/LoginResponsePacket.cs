using System;
using Newtonsoft.Json.Linq;
using IP_SharedLibrary.Packet.Response;

namespace IP_SharedLibrary.Packet.Response
{
    public class LoginResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-LOGIN";

        public bool isDoctor { get; private set; }
        public string username { get; set; }

        #region Constructors
        public LoginResponsePacket(Statuscode.Status status, string username, bool isDoctor)
            : base(status, DefCmd)
        {
            Initialize(username, isDoctor);
        }

        //public LoginResponsePacket(String status, String description, String authtoken) 
        //    : base(status, description, DefCmd)
        //{
        //    Initialize();
        //}

        public LoginResponsePacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "LoginResponsepacket ctor: json is null!");

            if (Status != "200") return;

            JToken username;
            if (!(json.TryGetValue("USERNAME", StringComparison.CurrentCultureIgnoreCase, out username)))
                throw new ArgumentException("USERNAME is not found in json \n" + json);
            JToken isDoctor;
            if (!(json.TryGetValue("ISDOCTOR", StringComparison.CurrentCultureIgnoreCase, out isDoctor)))
                throw new ArgumentException("ISDOCTOR is not found in json \n" + json);
            Initialize(username.ToString(), Boolean.Parse(isDoctor.ToString()));
        }
        #endregion

        #region Initializers
        private void Initialize(string username, bool isDoctor)
        {
            this.username = username;
            this.isDoctor = isDoctor;
        }
        #endregion

        #region Override Methods
        public override JObject ToJsonObject()
        {
            var returnJson = base.ToJsonObject();
            returnJson.Add("USERNAME", username);
            returnJson.Add("ISDOCTOR", isDoctor);

            return returnJson;

        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }

        #endregion

    }
}
