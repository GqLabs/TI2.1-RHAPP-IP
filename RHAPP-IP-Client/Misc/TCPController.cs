using IP_SharedLibrary.Packet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RHAPP_IP_Client.Misc
{
    public class TCPController
    {
        private TcpClient _client;
        private Thread _receiveThread;
        public Boolean IsConnected { get; private set; }
        public delegate void ReceivedPacket(Packet p);
        public event ReceivedPacket OnPacketReceived;

        public bool IsReading { get; private set; }
        private List<byte> _totalBuffer = new List<byte>();

        public TCPController()
        {
            //ReceiveTransmissionAsync();
        }

        public void RunClient()
        {
            if (Properties.Settings.Default.ServerIP.Length < 7)
                throw new ArgumentNullException("ServerIP", "ServerIP is not set or invalid");

            RunClient(IPAddress.Parse(Properties.Settings.Default.ServerIP));
        }

        public void RunClient(IPAddress IP)
        {
            if (IsConnected) return;

            IsReading = false;
            _client = new TcpClient();
            try
            {
                _client.Connect(IP, IP_SharedLibrary.Properties.Settings.Default.PortNumber);
                IsConnected = true;

                _totalBuffer = new List<byte>();

                // Signal that connected
                Console.WriteLine("TCPController: Connection active");
                StartReceive();
            }
            catch (SocketException)
            {
                MessageBox.Show("De server reageert niet");
                return;
            }
            
           
        }

        public void StopClient()
        {
            IsConnected = false;

            if (_client == null) return;
            _client.Close();
            _client = null;
            Console.WriteLine("Client closed...");
        }



        public async Task SendAsync(String data)
        {
            if (_client == null)
                return;

            var bytes = Packet.CreateByteData(data);
            await _client.GetStream().WriteAsync(bytes, 0, bytes.Length);
        }

        public void ReceiveTransmission()
        {
            while (IsConnected)
            {
                var buffer = new byte[1024];
                var bytesRead = 0;
                try
                {
                    bytesRead = _client.GetStream().Read(buffer, 0, buffer.Length);
                }
                catch (IOException e)
                {
                    Console.WriteLine("An exception occured in the TCPController.ReceiveTransmissionAsync function: " +
                                          e.Message);

                }

                if (bytesRead > 0)
                {
                    try
                    {
                        var rawData = new byte[bytesRead];
                        Array.Copy(buffer, 0, rawData, 0, bytesRead);
                        _totalBuffer = _totalBuffer.Concat(rawData).ToList();

                        var packetSize = Packet.GetLengthOfPacket(_totalBuffer);
                        if (packetSize == -1)
                            continue;

                        var p = Packet.RetrievePacket(packetSize, ref _totalBuffer);
                        if (p == null)
                            continue;


                        foreach (var @delegate in OnPacketReceived.GetInvocationList())
                        {
                            var deleg = (ReceivedPacket)@delegate;
                            deleg.BeginInvoke(p, null, null);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception occured in the TCPController.ReceiveTransmissionAsync function: " +
                                          e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("No bytes received. Connection has probably been closed.");
                    return;
                }
            }
        }

        internal void StartReceive()
        {
            if (_receiveThread == null)
            {
                _receiveThread = new Thread(ReceiveTransmission);
                _receiveThread.Start();
            }
            else if (_receiveThread.ThreadState == ThreadState.Stopped)
                _receiveThread.Start();
        }
        internal void StopReceive()
        {
            _receiveThread.Interrupt();
            Thread.Sleep(50);
            _receiveThread.Abort();
        }
    }
}
