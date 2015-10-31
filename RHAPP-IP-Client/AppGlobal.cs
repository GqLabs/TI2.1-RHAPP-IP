using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Packet;
using IP_SharedLibrary.Packet.Request;
using IP_SharedLibrary.Packet.Response;
using IP_SharedLibrary.Utilities;
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
        #region Fields + Constructors
        private static AppGlobal _instance;
        public static AppGlobal Instance { get { return _instance ?? (_instance = new AppGlobal()); } }

        public delegate void ResultDelegate(Packet packet);
        public event ResultDelegate LoginResultEvent;
        public event ResultDelegate ResultEvent;

        private readonly TCPController Controller;
        public string Username { get; private set; }

        private AppGlobal()
        {
            Controller = new TCPController();
            Controller.OnPacketReceived += PacketReceived;
            this.LoginResultEvent += CheckLoggedIn;
            DataHandler.IncomingDataEvent += SerialDataReceived;
        }

        #endregion

        private void OnResultEvent(Packet packet)
        {
            ResultDelegate handler = ResultEvent;
            if (handler != null) handler(packet);
        }

        private void OnLoginResultEvent(LoginResponsePacket packet)
        {
            ResultDelegate handler = LoginResultEvent;
            if (handler != null) handler(packet);
        }

        public void LoginToServer(string username, string password)
        {
            Controller.RunClient();
            var passhash = Crypto.CreateSHA256(password);
            var packet = new LoginPacket(username, passhash);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Controller.SendAsync(packet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Controller.ReceiveTransmissionAsync();
        }

        private void CheckLoggedIn(Packet packet)
        {
            //Post logged-in method calls
            if (((LoginResponsePacket)packet).Status == Statuscode.Status.Ok.ToString())
            {
                Username = ((LoginResponsePacket)packet).username;
            }
        }

        public void Send(Packet packet)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Controller.SendAsync(packet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        // Get TCPController to receive a packet.
        public void Receive()
        {
            Controller.ReceiveTransmissionAsync();
        }

        private void PacketReceived(Packet p)
        {
            Console.WriteLine(p.ToJsonObject().ToString());
            //if (p is MessagePushPacket)
            //{
            //    var packet = p as MessagePushPacket;
            //    Console.WriteLine("push packet received!");
            //    IncomingMessageEvent(packet.Message, false, SelectedUser);
            //}
            //else if (p is UserChangedPacket)
            //{
            //    var packet = p as UserChangedPacket;
            //    if (packet.Username == Properties.Settings.Default.Username)
            //        return;
            //    User x = Users.FirstOrDefault(u => u.Username == packet.Username);
            //    if (x == null)
            //    {
            //        x = new User(packet.Nickname, packet.Username, null);
            //        Users.Add(x);
            //    }
            //    x.OnlineStatus = packet.Status;
            //    OnlineStatusOfContactEventChanged(x);

            //}
            //else
            if (p is LoginResponsePacket)
            {
                var packet = p as LoginResponsePacket;
                OnLoginResultEvent(packet);
            }
            //else if (p is RegisterResponsePacket)
            //{
            //    var packet = p as RegisterResponsePacket;
            //    OnRegisterResultEvent(packet.Status);
            //}
            //else if (p is PullResponsePacket<ChatMessage>)
            //{
            //    var packet = p as PullResponsePacket<ChatMessage>;
            //    FillChatMessageList(packet.Data.ToList());
            //    Console.WriteLine("PullResponsePacket<ChatMessage> received!");
            //}
            //else if (p is PullResponsePacket<User>)
            //{
            //    var packet = p as PullResponsePacket<User>;
            //    foreach (User u in packet.Data.ToList())
            //    {
            //        Users.Add(u);
            //    }
            //    InitializeContacts();
            //}
            else if (p is ResponsePacket) //this one should be last!
            {
                var packet = p as ResponsePacket;
                OnResultEvent(packet);
            }
        }

        private void SerialDataReceived(Measurement m)
        {
            Controller.RunClient();
            var packet = new SerialDataPacket(m, Username);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Controller.SendAsync(packet);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Controller.ReceiveTransmissionAsync();
        }

    }
}
