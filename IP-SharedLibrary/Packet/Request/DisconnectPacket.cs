using System;
using Newtonsoft.Json.Linq;

namespace IP_SharedLibrary.Packet.Request
{
    public class DisconnectPacket : RequestPacket
    {
        public const string DefCmd = "DC";
        public string Username { private set; get; }

        public DisconnectPacket(string username) :base(DefCmd)
        {
            Initialize(username);
        }

        public DisconnectPacket(JObject json) :base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Authenticatedpacket ctor: json is null!");

            JToken username;

            if (!(json.TryGetValue("USERNAME", StringComparison.CurrentCultureIgnoreCase, out username)))
                throw new ArgumentException("Username is not found in json: \n" + json);

            Initialize((string)username);
        }

        private void Initialize(string username)
        {
            Username = username;
        }

        public override JObject ToJsonObject()
        {
            var x = base.ToJsonObject();
            x.Add("USERNAME", Username);
            return x;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
