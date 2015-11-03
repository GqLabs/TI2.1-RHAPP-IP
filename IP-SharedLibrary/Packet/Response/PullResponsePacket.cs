using System;
using System.Collections.Generic;
using System.Linq;
using IP_SharedLibrary.Entity;
using Newtonsoft.Json.Linq;

namespace IP_SharedLibrary.Packet.Response
{
    public class PullResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-PULL";

        public IEnumerable<User> Data { get; private set; }

        public PullResponsePacket(List<User> users)
            : base(DefCmd)
        {

            Initialize(users);
        }

        public PullResponsePacket(string status,List<User> users)
            : base(status, null, DefCmd)
        {
            Initialize(users);
        }

        public PullResponsePacket(JObject json)
            : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "PullResponsePacket ctor: json is null!");

            if (Status != "200") return;

            JToken dataToken;
            if (!(json.TryGetValue("DATA", StringComparison.CurrentCultureIgnoreCase, out dataToken)))
                throw new ArgumentException("Data is not found in json \n" + json);
            
            var array = (JArray)dataToken;
            var data = array.Select(value => value.ToObject<User>()).ToList();
            Initialize(data);
        }

        private void Initialize(List<User> users)
        {
            Data = users;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("DATA", JArray.FromObject(Data));
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
