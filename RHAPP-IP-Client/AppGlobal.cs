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
using System.Threading;
using System.Threading.Tasks;

namespace RHAPP_IP_Client
{
    public class AppGlobal
    {
        #region Fields + Constructors
        private static AppGlobal _instance;
        public static AppGlobal Instance { get { return _instance ?? (_instance = new AppGlobal()); } }

        public delegate void ResultDelegate(Packet packet);
        public event ResultDelegate BikeTestChangedEvent;
        public event ResultDelegate LoginResultEvent;

        public event ResultDelegate ResultEvent;
        public delegate void PushDelegate(Packet packet);
        public event PushDelegate IncomingMeasurementEvent;
        public delegate void UserDelegate(User u);
        public event UserDelegate UserChangedEvent;

        public TCPController Controller { get; private set; }
        public string Username { get; private set; }
        public bool Connected { get; private set; }

        // <only_needed_for_doctor>
        public List<User> Users { get; private set; }
        public List<Tuple<string, Measurement>> PatientMeasurements { get; private set; } // on doctor side
        // </only_needed>
        public List<Measurement> Measurements { get; private set; } // on patient side

        private AppGlobal()
        {
            Users = new List<User>();
            PatientMeasurements = new List<Tuple<string, Measurement>>();
            Measurements = new List<Measurement>();
            Controller = new TCPController();
            Controller.OnPacketReceived += PacketReceived;
            this.LoginResultEvent += CheckLoggedIn;
            DataHandler.IncomingDataEvent += SerialDataReceived;
            IncomingMeasurementEvent += MeasurementDataReceived;
        }

        #endregion

        public void OnResultEvent(Packet packet)
        {
            ResultDelegate handler = ResultEvent;
            if (handler != null) handler(packet);
        }

        public void OnLoginResultEvent(LoginResponsePacket packet)
        {
            ResultDelegate handler = LoginResultEvent;
            if (handler != null) handler(packet);
        }

        public void OnBikeTestChangedEvent(RequestBikeTestResponsePacket packet)
        {
            ResultDelegate handler = BikeTestChangedEvent;
            if (handler != null) handler(packet);
        }

        public void OnIncomingMeasurementEvent(SerialDataPushPacket packet)
        {
            PushDelegate handler = IncomingMeasurementEvent;
            if (handler != null) handler(packet);
        }

        public void OnUserChangedEvent(User u)
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

        public void SendRequestBikeTestPacket(string usernamePatient)
        {
            var v = new RequestBikeTestPacket(usernamePatient);
            Send(v);
        }

        private void CheckLoggedIn(Packet packet)
        {
            //Post logged-in method calls
            if (((LoginResponsePacket)packet).Status == "200")
            {
                Username = ((LoginResponsePacket)packet).username;
                Connected = true;
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
            Controller.StartReceive();
        }

        private void PacketReceived(Packet p)
        {
            

        }

        private void SerialDataReceived(Measurement m)
        {
            Controller.RunClient();
            var packet = new SerialDataPacket(m, Username);
            Measurements.Add(m);
            Send(packet);
        }

        private void MeasurementDataReceived(Packet p)
        {
            SerialDataPushPacket packet = p as SerialDataPushPacket;
            PatientMeasurements.Add(new Tuple<string, Measurement>(packet.Username, packet.Measurement));
        }

        public void ExitApplication()
        {
            Controller.StopReceive();
            Controller.StopClient();
            Properties.Settings.Default.Save();
        }
    }
}
