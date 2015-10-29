using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHAPP_IP_Client
{
    public class AppGlobal
    {
        private static AppGlobal _instance;
        public static AppGlobal Instance
        {
            get { return _instance ?? (_instance = new AppGlobal()); }
        }

        private AppGlobal()
        {

        }

        public void Send(JObject json)
        {

        }

        public void Receive(JObject json)
        {

        }
    }
}
