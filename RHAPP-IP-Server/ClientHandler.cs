using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Packet;
using IP_SharedLibrary.Packet.Push;
using IP_SharedLibrary.Packet.Request;
using IP_SharedLibrary.Packet.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RHAPP_IP_Server
{
    public class ClientHandler
    {
        private readonly Thread _thread;
        private readonly byte[] _buffer = new byte[1024];
        private const int BufferSize = 1024;
        private readonly TcpClient _tcpclient;
        private readonly NetworkStream _networkStream;
        private List<byte> _totalBuffer;
        private readonly Datastorage _datastorage;
        private ClientDataHandler _clientDataHandler;

        public ClientHandler(TcpClient client)
        {
            _datastorage = Datastorage.Instance;
            _tcpclient = client;

            _networkStream = _tcpclient.GetStream();
            _totalBuffer = new List<byte>();
            _thread = new Thread(ThreadLoop);
            _thread.Start();

        }

        private void ThreadLoop()
        {
            while (true)
            {
                try
                {
                    //Is the client connected?
                    if (!_tcpclient.Connected)
                        throw new SocketException(0xDC);

                    //Recieve the data from the networkStream
                    var receiveCount = _networkStream.Read(_buffer, 0, BufferSize);

                    var rawData = new byte[receiveCount];
                    Array.Copy(_buffer, 0, rawData, 0, receiveCount);
                    _totalBuffer = _totalBuffer.Concat(rawData).ToList();

                    //Check the packetsize, did we recieve anything?
                    var packetSize = Packet.GetLengthOfPacket(_totalBuffer);
                    if (packetSize == -1)
                        continue;

                    //Retrieve the Json out of the received packet.
                    JObject json = null;
                    try
                    {
                        json = Packet.RetrieveJson(packetSize, ref _totalBuffer);
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Sending SyntaxError-packet to {0}", _tcpclient.Client.RemoteEndPoint);
                        Send(new ResponsePacket(Statuscode.Status.SyntaxError));
                    }

                    if (json == null)
                        continue;

                    JToken cmd;

                    if (!json.TryGetValue("CMD", out cmd))
                    {
                        Console.WriteLine("Got JSON that does not define a command.");
                        continue;
                    }

                    switch ((string)cmd)
                    {
                        case SerialDataPacket.DefCmd:
                            HandleSerialDataPacket(json);
                            break;
                        case LoginPacket.DefCmd:
                            HandleLoginPacket(json);
                            break;
                        case DisconnectPacket.DefCmd:
                            HandleDisconnectPacket(json);
                            break;
                        case BikeTestPacket.DefCmd:
                            HandleBikeTestPacket(json);
                            break;
                        case StartTestPacket.DefCmd:
                            HandleStartTestPacket(json);
                            break;
                        //case RegisterPacket.DefCmd:
                        //    HandleRegisterPacket(json);
                        //    break;
                        case PullRequestPacket.DefCmd:
                            HandlePullRequestPacket(json);
                            break;
                    }

                }
                catch (SocketException)
                {
                    Console.WriteLine("Client with IP-address: {0} has been disconnected",
                        _tcpclient.Client.RemoteEndPoint);
                    Authentication.TryDeAuthenticate(this);
                    _thread.Abort();
                }
                catch (Exception e)
                {
                    if (e.InnerException is SocketException)
                    {
                        Console.WriteLine("Client with IP-address: {0} has been disconnected",
                            _tcpclient.Client.RemoteEndPoint);
                            Authentication.TryDeAuthenticate(this);
                        _thread.Abort();
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        #region Packet_Handlers
        private void HandlePullRequestPacket(JObject json)
        {
            Console.WriteLine("PullRequestPacket Received");
            var packet = new PullRequestPacket(json);

            var u = Authentication.GetUser(packet.Username);
            if (u == null && !u.IsDoctor) return;
            
            var returnPacket = HandlePullRequestUsersByStatus();
#if DEBUG
            Console.WriteLine(packet);
            Console.WriteLine(returnPacket);
#endif
            Send(returnPacket);
        }
        private ResponsePacket HandlePullRequestUsersByStatus()
        {
            //Get all users and set their online-status to "false".
            var allUsers = _datastorage.GetUsers().ToList();
            var authenticatedUsers = Authentication.GetAllUsers();
            foreach (var user in allUsers)
            {
                user.OnlineStatus = authenticatedUsers.Contains(user);
            }
            var allPatients = allUsers.Where(user => user.IsDoctor == false).ToList();
            return new PullResponsePacket(allPatients);
        }

        //private ResponsePacket HandlePullRequestMessagesByUser(PullRequestPacket packet)
        //{
        //    var allMessages = _datastorage.GetMessages(packet.SearchKey);
        //    return new PullResponsePacket<ChatMessage>(Statuscode.Status.Ok,
        //        PullResponsePacket<ChatMessage>.DataType.ChatMessage,
        //        allMessages);
        //}

        //private ResponsePacket HandlePullRequestReceivedMessages(PullRequestPacket packet)
        //{
        //    var allMessages = _datastorage.GetMessagesSentTo(packet.SearchKey);
        //    return new PullResponsePacket<ChatMessage>(Statuscode.Status.Ok,
        //        PullResponsePacket<ChatMessage>.DataType.ChatMessage,
        //        allMessages);

        //}

        //        private void HandleRegisterPacket(JObject json)
        //        {
        //            Console.WriteLine("RegisterPacket Received");

        //            var packet = new RegisterPacket(json);
        //            var user = new User(packet.Nickname, packet.Username, packet.Passhash);
        //            Datastorage.Instance.AddUser(user);

        //            var returnPacket = new RegisterResponsePacket(Statuscode.Status.Ok);
        //            Send(returnPacket);

        //#if DEBUG
        //            Console.WriteLine(packet.ToString());
        //            Console.WriteLine(returnPacket.ToString());
        //#endif
        //        }

        private void HandleDisconnectPacket(JObject json)
        {
            Console.WriteLine("DisconnectPacket Received");

            var packet = new DisconnectPacket(json);
            
            var returnPacket = new ResponsePacket(Statuscode.Status.Unauthorized);
            if (Authentication.CheckLoggedIn(packet.Username))
            {
                Authentication.DeAuthenticate(packet.Username);
                returnPacket = new ResponsePacket(Statuscode.Status.Ok);
            }

            Send(returnPacket);

#if DEBUG
            Console.WriteLine(packet.ToString());
            Console.WriteLine(returnPacket.ToString());
#endif
        }

        private void HandleLoginPacket(JObject json)
        {
            Console.WriteLine("Login packet received");
            //Recieve the username and password from json.
            var packet = new LoginPacket(json);

            JObject returnJson;
            //Code to check User/pass here
            if (Authentication.Authenticate(packet.Username, packet.Passhash, this))
            {
                returnJson = new LoginResponsePacket(
                    Statuscode.Status.Ok,
                    packet.Username,
                    Authentication.GetUser(packet.Username).IsDoctor
                    );
                var user = _datastorage.GetUser(packet.Username);
                if (user != null)
                {
                    user.OnlineStatus = true;
                    if (!user.IsDoctor)
                        SendToAllOnlineDoctors(new UserChangedPacket(user));
                }

            }
            else if (Authentication.GetUser(packet.Username) != null)
            {
                if (Authentication.GetUser(packet.Username).OnlineStatus)
                    returnJson = new ResponsePacket(Statuscode.Status.AlreadyOnline, "RESP-LOGIN");
                else
                    returnJson = new ResponsePacket(Statuscode.Status.SyntaxError, "RESP-LOGIN");
            }
            else //If the code reaches this point, the authentification has failed.
            {
                returnJson = new ResponsePacket(Statuscode.Status.InvalidUsernameOrPassword, "RESP-LOGIN");
            }

            //Send the result back to the client.
            Send(returnJson.ToString());
#if DEBUG
            Console.WriteLine(packet.ToString());
            Console.WriteLine(returnJson.ToString());
#endif
        }

        private void HandleSerialDataPacket(JObject json)
        {
            Console.WriteLine("Handle SerialData Packet");
            var packet = new SerialDataPacket(json);

            var PatientUsername = Authentication.GetAllUsers()
                .Where(user => user.Username == packet.PatientUsername)
                .Select(user => user.Username).FirstOrDefault();
            if (_clientDataHandler == null)
            {
                _clientDataHandler = new ClientDataHandler();
            }
            ClientDataHandler.ReturnValue returnVal = _clientDataHandler.AddMeasurementToLastBikeTest(packet.Measurement);
            if (returnVal == ClientDataHandler.ReturnValue.NotStarted)
            {
#if DEBUG
                Console.WriteLine("Error: Tried to add measurement while BikeTest was not started of patient {0}", Authentication.GetUser(packet.PatientUsername).Nickname);
#endif
            }
            else
            {
#if DEBUG
                Console.WriteLine("Error: General error occured while tried to add measurement of patient {0}", Authentication.GetUser(packet.PatientUsername).Nickname);
#endif
            }

            //Generate PushPacket
            Packet pushPacket = new SerialDataPushPacket(packet.Measurement, packet.PatientUsername);

            // Determining the sockets to send the pushpacket (to send to all online doctors) 
            SendToAllOnlineDoctors(pushPacket);
#if DEBUG
            Console.WriteLine(packet);
#endif
        }

        private void HandleStartTestPacket(JObject json)
        {
            Console.WriteLine("Handle StartTest Packet");
            var packet = new StartTestPacket(json);
            var patientStream = Authentication.GetStream(packet.PatientUsername);
            if (patientStream != null)
            {

            }
            patientStream.Send(new ResponsePacket(Statuscode.Status.Ok.ToString(), "STARTTEST", null));
        }

        private void HandleBikeTestPacket(JObject json)
        {
            Console.WriteLine("Handle BikeTest Packet");
            var packet = new BikeTestPacket(json);
        }

        #endregion

        #region Send_methods
        private void SendToAllOnlineUsers(Packet p)
        {
            foreach (var clientHandler in Authentication.GetAllUsers()
                .Select(linqUser => Authentication.GetStream(linqUser.Username))
                .Where(clientHandler => clientHandler != null))
            {
#if DEBUG
                Console.WriteLine("SendToAllUsers: Sending a packet");
#endif
                clientHandler.Send(p);
            }
        }

        private void SendToAllOnlineDoctors(Packet packet)
        {
            List<string> onlineDoctors = Authentication.GetAllUsers()
                .Where(user => user.IsDoctor == true)
                .Select(user => user.Username).ToList();
            foreach (string u in onlineDoctors)
            {
                var clientHandler = Authentication.GetStream(u);
                if (clientHandler != null) clientHandler.Send(packet);
#if DEBUG
                if (clientHandler != null) Console.WriteLine("Notifing:\n{0}", packet);
#endif
            }
        }

        public void Send(String s)
        {
            var dataArray = Packet.CreateByteData(s);
            _networkStream.Write(dataArray, 0, dataArray.Length);
            _networkStream.Flush();
        }

        public void Send(Packet s)
        {
            Send(s.ToString());
        }
        #endregion Send
    }

    internal class ClientDataHandler
    {
        private BikeTest _lastBikeTestBuff;
        public enum BikeTestStatus { NotInitialized, Stopped, Started, Error }
        public enum ReturnValue { Success, AlreadyStarted, AlreadyStopped, NotStarted, NotStopped, Error }
        public BikeTestStatus TestStatus { get; private set; }
        public Measurement LastMeasurement { get { return _lastBikeTestBuff.Measurements.Last() ?? (null); } }

        public ClientDataHandler()
        {
            TestStatus = BikeTestStatus.Stopped;
        }

        public ReturnValue StartNewBikeTest(BikeTest bikeTest)
        {
            if (TestStatus == BikeTestStatus.Stopped)
            {
                if (bikeTest != null)
                {
                    _lastBikeTestBuff = bikeTest;
                }
                return ReturnValue.Success;
            }
            else if (TestStatus == BikeTestStatus.Started)
            {
                return ReturnValue.AlreadyStarted;
            }
            else
            {
                return ReturnValue.Error;
            }
        }

        public ReturnValue StopCurrentBikeTest(out BikeTest oldBikeTest)
        {
            if (TestStatus == BikeTestStatus.Started)
            {
                oldBikeTest = _lastBikeTestBuff;
                return ReturnValue.Success;
            }
            else if (TestStatus == BikeTestStatus.Stopped)
            {
                oldBikeTest = null;
                return ReturnValue.AlreadyStopped;
            }
            else
            {
                oldBikeTest = null;
                return ReturnValue.Error;
            }
        }

        public ReturnValue AddMeasurementToLastBikeTest(Measurement m)
        {
            if (TestStatus == BikeTestStatus.Started)
            {
                if (_lastBikeTestBuff != null)
                {
                    _lastBikeTestBuff.AddMeasurement(m);
                    return ReturnValue.Success;
                }
                else return ReturnValue.Error;
            }
            else if (TestStatus == BikeTestStatus.Stopped) return ReturnValue.NotStarted;
            else return ReturnValue.Error;

        }
    }
}
