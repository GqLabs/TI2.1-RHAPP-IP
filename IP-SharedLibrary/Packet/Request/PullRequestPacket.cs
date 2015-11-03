using System;
using Newtonsoft.Json.Linq;

namespace IP_SharedLibrary.Packet.Request
{
    public class PullRequestPacket : RequestPacket
    {
        //Inherited field: CMD, AuthToken
        //Introduced fields: Datatype (enum)

        public const string DefCmd = "PULL";
        public enum RequestType
        {
            UsersByStatus
        }

        public RequestType Request { get; private set; }
        public String SearchKey { get; private set; }
        public string Username { get; private set; }


        public PullRequestPacket(string username) : base(DefCmd)
        {
            
            Initialize(RequestType.UsersByStatus, username);
        }

        public PullRequestPacket(string searchKey, string username)
            : base(DefCmd)
        {
            Initialize(RequestType.UsersByStatus, username, searchKey);
        }

        public PullRequestPacket(JObject json) : base(json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Loginpacket ctor: json is null!");

            JToken requestTypeToken;
            JToken searchKeyToken;
            JToken username;

            if (!(json.TryGetValue("REQUESTTYPE", StringComparison.CurrentCultureIgnoreCase, out requestTypeToken)))
                throw new ArgumentException("RequestType is not found in json: \n" + json);

            if (!(json.TryGetValue("USERNAME", StringComparison.CurrentCultureIgnoreCase, out username)))
                throw new ArgumentException("Username is not found in json: \n" + json);


            var requestType = (RequestType) Enum.Parse(typeof (RequestType), (string) requestTypeToken);
            if (!Enum.IsDefined(typeof (RequestType), requestType))
                throw new ArgumentException("RequestType is found, but is invalid in json: \n" + json);

            //Check if searchkey is present. If so: Initialize with the key, else: init without it. (the key is not neccesary)
            if (json.TryGetValue("SEARCHKEY", StringComparison.CurrentCultureIgnoreCase, out searchKeyToken))
                Initialize(requestType, (string) searchKeyToken);
            else
                Initialize(requestType, (string)username);
        }

        private void Initialize(RequestType requestType, string username, string searchKey = null)
        {
            Request = requestType;
            SearchKey = searchKey;
            Username = username;
        }

        public override JObject ToJsonObject()
        {
            var json = base.ToJsonObject();
            json.Add("REQUESTTYPE", Request.ToString());
            json.Add("USERNAME", Username);
            if (SearchKey != null)
                json.Add("SEARCHKEY", SearchKey);
            return json;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
