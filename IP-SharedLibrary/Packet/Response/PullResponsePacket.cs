using System;
using System.Collections.Generic;
using System.Linq;
using IP_SharedLibrary.Entity;
using Newtonsoft.Json.Linq;

namespace IP_SharedLibrary.Packet.Response
{
    public class PullResponsePacket<T> : ResponsePacket
    {
        public const string DefCmd = "RESP-PULL";

        public IEnumerable<T> Data { get; private set; }

        public PullResponsePacket(Statuscode.Status status, IEnumerable<T> dataEnumerable)
            : base(status, DefCmd)
        {
            Initialize(dataEnumerable);
        }

        public PullResponsePacket(string status,
            IEnumerable<T> dataEnumerable)
            : base(status, null, DefCmd)
        {
            Initialize(dataEnumerable);
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
            var data = array.Select(value => value.ToObject<T>()).ToList();
            Initialize(data);
        }

        private void Initialize(IEnumerable<T> dataEnumerable)
        {
            Data = dataEnumerable;
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
