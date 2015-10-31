using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RHAPP_IP_Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            new Program();
            //"useless" method call, this forces the Datastorage to be initialized. 
            Datastorage.Instance.ToString();
            var userInput = Console.ReadLine();
            while (userInput != "exit")
            {
                switch (userInput)
                {
                    case "save":
                        Datastorage.Instance.SaveToFile();
                        break;
                    case "exit":
                        return;
                }
                userInput = Console.ReadLine();
            }
            Datastorage.Instance.SaveToFile();
            Environment.Exit(0);
        }

        public Program()
        {
            var thread = new Thread(RunServer);
            thread.Start();
        }

        public void RunServer()
        {
            Console.WriteLine("Server Status: Initializing");
            var serverListener = new TcpListener(IPAddress.Any, IP_SharedLibrary.Properties.Settings.Default.PortNumber);

            //Code for getting server IP
            var serverip = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
            //Display server IP:
            Console.WriteLine("Server IP: {0}", serverip);

            //Start the server listener
            serverListener.Start();
            Console.WriteLine("Server Status: Listening");

            //Console.WriteLine("Example json: Errorpacket");
            //Console.WriteLine(new LoginResponsePacket(Statuscode.Status.Ok, "Client", "Ahsdha7w27%^hsdja^&"));

            while (true)
            {
                var tcpclient = serverListener.AcceptTcpClient();
                Console.WriteLine("Server: Accepted new client");
                new ClientHandler(tcpclient);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
