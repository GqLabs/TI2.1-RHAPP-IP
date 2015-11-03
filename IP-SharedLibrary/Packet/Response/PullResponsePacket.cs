using System;
using System.Collections.Generic;
using System.Linq;
using IP_SharedLibrary.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IP_SharedLibrary.Packet.Response
{
    public class PullResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-PULL";

        public List<User> Data { get; private set; }

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

            JToken dataToken;
            if (!(json.TryGetValue("DATA", StringComparison.CurrentCultureIgnoreCase, out dataToken)))
                throw new ArgumentException("Data is not found in json \n" + json);

            JArray array = (JArray)dataToken;
            var data = array.ToObject<List<User>>();

            Initialize(data);
        }

        private void Initialize(List<dynamic> users)
        {
            Data = new List<User>();
            foreach (dynamic value in users)
            {
                var v = JsonConvert.DeserializeObject(value) as User;
                Data.Add(v);
            }
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
