using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_SharedLibrary.Packet.Push
{
    public class StartTestPushPacket : PushPacket
    {

        // Inherited fields: CMD
        // Introduced fields: Message (type = chatmessage)

        public const string DefCmd = "PUSH-STARTTEST";

        public StartTestPushPacket() : base(DefCmd)
        {

        }

        public StartTestPushPacket(JObject json) : base(json)
        { 
            if (json == null)
                throw new ArgumentNullException("json", "StartTestPushPacket ctor: json is null!");
        }


        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }

    }
}
