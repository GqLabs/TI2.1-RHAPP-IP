using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Push
{
    public class CommandPushPacket : PushPacket
    {
        // Inherited fields: CMD
        // Introduced fields: Message (type = chatmessage)

        public const string DefCmd = "PUSH-BIKECMD";

        public string Command { get; private set; }

        public CommandPushPacket(string command) : base(DefCmd)
        {
            Command = command;
        }

        public CommandPushPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "StartTestPushPacket ctor: json is null!");

            JToken command;

            if (!(json.TryGetValue("COMMAND", StringComparison.CurrentCultureIgnoreCase, out command)))
                throw new ArgumentException("Command is not found in json: \n" + json);

            // Intitialize
            Command = (string)command;
        }


        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("COMMAND", Command);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
