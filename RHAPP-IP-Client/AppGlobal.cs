using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Packet;
using IP_SharedLibrary.Packet.Push;
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
        public delegate void PushDelegate(Packet packet);
        public event PushDelegate IncomingMeasurementEvent;
        public delegate void UserDelegate(User u);
        public event UserDelegate UserChangedEvent;

        private readonly TCPController Controller;
        public string Username { get; private set; }
        public bool Connected { get; private set; }

        // <only_needed_for_doctor>
        public List<User> Users { get; private set; }
        public Dictionary<string, Measurement> PatientMeasurements { get; private set; }
        // </only_needed>

        private AppGlobal()
        {
            Users = new List<User>();
            PatientMeasurements = new Dictionary<string, Measurement>();
            Controller = new TCPController();
            Controller.OnPacketReceived += PacketReceived;
            this.LoginResultEvent += CheckLoggedIn;
            DataHandler.IncomingDataEvent += SerialDataReceived;
            IncomingMeasurementEvent += MeasurementDataReceived;
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

        private void OnIncomingMeasurementEvent(SerialDataPushPacket packet)
        {
            PushDelegate handler = IncomingMeasurementEvent;
            if (handler != null) handler(packet);
        }

        private void OnUserChangedEvent(User u)
        {
            UserDelegate handler = UserChangedEvent;
            if (handler != null) handler(u);
        }

        public void LoginToServer(string username, string password)
        {
            Controller.RunClient();
            var passhash = Crypto.CreateSHA256(password);
            var packet = new LoginPacket(username, passhash);
            Send(packet);
            Controller.ReceiveTransmissionAsync();
        }

        public async void Logout()
        {
            if (!Connected)
                return;
            DisconnectPacket p = new DisconnectPacket(this.Username);
            try
            {
                await Controller.SendAsync(p);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("An exception occured in the AppGlobal.LogoutFromServer function: " +
                    e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An unknown exception occured: " + e.Message);
            }
            Users.Clear();
            Connected = false;
        }

        private void CheckLoggedIn(Packet packet)
        {
            //Post logged-in method calls
            if (((LoginResponsePacket)packet).Status == "200")
            {
                Username = ((LoginResponsePacket)packet).username;
                Connected = true;
                Receive();
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
            if (p is SerialDataPushPacket)
            {
                Console.WriteLine("push packet received!");
                OnIncomingMeasurementEvent(p as SerialDataPushPacket);
            }
            else if (p is UserChangedPacket)
            {
                var packet = p as UserChangedPacket;
                if (packet.Username == this.Username)
                    return;
                User x = Users.FirstOrDefault(u => u.Username == packet.Username);
                if (x == null)
                {
                    x = new User(packet.Nickname, packet.Username, null);
                    Users.Add(x);
                }
                x.OnlineStatus = packet.Status;
                OnUserChangedEvent(x);
            }
            else if (p is LoginResponsePacket)
            {
                OnLoginResultEvent(p as LoginResponsePacket);
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
                OnResultEvent(p as ResponsePacket);
            }
        }

        private void SerialDataReceived(Measurement m)
        {
            Controller.RunClient();
            var packet = new SerialDataPacket(m, Username);
            Send(packet);
            Controller.ReceiveTransmissionAsync();
        }

        private void MeasurementDataReceived(Packet p)
        {
            SerialDataPacket packet = p as SerialDataPacket;
            PatientMeasurements.Add(packet.PatientUsername, packet.Measurement);
        }
        
        public void ExitApplication()
        {
            Controller.StopClient();
            Properties.Settings.Default.Save();
        }
    }
}
