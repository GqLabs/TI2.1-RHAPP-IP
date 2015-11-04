using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Request
{
    public class SendCommandPacket : RequestPacket
    {
        public const string DefCmd = "BIKECMD";

        public string Commmand { get; private set; }
        public string Username { get; private set; }

        public SendCommandPacket(JObject json)
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "StartTestpacket ctor: json is null!");

            JToken command;
            JToken username;

            if (!(json.TryGetValue("COMMAND", StringComparison.CurrentCultureIgnoreCase, out command)
                && json.TryGetValue("USERNAME", StringComparison.CurrentCultureIgnoreCase, out username)))
                throw new ArgumentException("Command or Usrname is not found in json: \n" + json);

            Initialize((string)command, (string)username);
        }

        public SendCommandPacket(string command, string username)
            : base(DefCmd)
        {
            Initialize(command, username);
        }

        private void Initialize(string command, string username)
        {
            this.Commmand = command;
            this.Username = username;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("COMMAND", Commmand);
            json.Add("USERNAME", Username);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
