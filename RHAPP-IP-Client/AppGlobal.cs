using IP_SharedLibrary.Packet;
using Newtonsoft.Json.Linq;
using RHAPP_IP_Client.Misc;
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

        private readonly TCPController Controller;

        public object _client { get; private set; }

        private AppGlobal()
        {

        }

        public void Send(String data)
        {
            Controller.SendAsync(data);
        }

        public void Receive()
        {

        }

    }
}
