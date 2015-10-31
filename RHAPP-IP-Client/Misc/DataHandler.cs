using IP_SharedLibrary.Entity;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHAPP_IP_Client.Misc
{
     class DataHandler
    {
        #region (static) properties, (static) fields, (static) events
        // vaste waarden
        public static readonly string COMMAND = "CU";
        public static readonly string CMD_TIME = "PT";
        public static readonly string CMD_DISTANCE = "PD";
        public static readonly string CMD_POWER = "PW";
        public static readonly string CMD_ENERGY = "PE";
        public static readonly string RESET = "RS";
        public static readonly string STATUS = "ST";

        // private fields
        private string portname;
        private int baudrate = 9600;
        private string bufferOut;
        private string[] bufferIn;

        // public fields
        public enum State { notConnected, connected, reset, command }
        public enum ReturnData { ERROR, ACK, RUN, STATUS }

        public State state = State.notConnected;
        public ReturnData returnData;

        private SerialPort ComPort;

        // custom events
        public delegate void DataDelegate(Measurement measurement);
        public static event DataDelegate IncomingDataEvent;

        public delegate void ErrorDelegate(string error);
        public static event ErrorDelegate IncomingErrorEvent;
        #endregion

        private static void OnIncomingDataEvent(Measurement measurement)
        {
            DataDelegate handler = IncomingDataEvent;
            if (handler != null) handler(measurement);
        }

        public static void OnIncomingErrorEvent(string error)
        {
            ErrorDelegate handler = IncomingErrorEvent;
            if (handler != null) handler(error);
        }

        public void initComm(string portname)
        {
            if (ComPort != null)
            {
                ComPort.Close();
                state = State.notConnected;
            }

            this.portname = portname;
            try
            {
                ComPort = new SerialPort(this.portname, this.baudrate);
                ComPort.Open();
                state = State.connected;
                ComPort.WriteLine(RESET);
                state = State.reset;
                ComPort.DataReceived += new SerialDataReceivedEventHandler(ComPort_DataReceived);
            }
            catch (Exception)
            {
                OnIncomingErrorEvent("WrongComPort");
                try { ComPort.Close(); } catch (Exception) { } // probeer om de ComPort wel te sluiten.
                state = State.notConnected;
            }


        }

        public void closeComm()
        {
            ComPort.Close();
        }

        public void sendData(string data)
        {
            bufferOut = data;
            ComPort.WriteLine(data);
        }

        private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string buffer = ComPort.ReadLine();
            buffer = buffer.TrimEnd('\r');
            switch (buffer) //kijk wat er binnenkomt
            {
                case "ERROR": //wanneer "Error"
                    returnData = ReturnData.ERROR;
                    handleError();
                    break;
                case "ACK": // ACK betekent acknowledged.
                    returnData = ReturnData.ACK;
                    break;
                case "RUN":
                    returnData = ReturnData.RUN;
                    break;
                default:    // alle andere waarden.
                    returnData = ReturnData.STATUS;
                    handleBikeValues(buffer);
                    break;
            }
        }

        int trycount = 0;
        private void handleError()
        {
            if (bufferOut == "RS" && trycount < 3)
            {
                sendData("RS");  //gewoon nog een keer proberen tot 3 keer toe, net zolang totdat hij werkt.
                trycount++;
            }
        }

        private void handleBikeValues(string buffer)
        {
            Measurement m = new Measurement(buffer);
            bufferIn = buffer.Split('\t');
            OnIncomingDataEvent(m);
        }

        public bool checkBikeState(bool commandMode)
        {
            if (ComPort == null || !ComPort.IsOpen)
            {
                OnIncomingErrorEvent("NotConnectedToBike");
                state = State.notConnected;
                return false;
            }
            switch (state)
            {
                case State.reset:
                    if (commandMode) setCommandMode();
                    if (returnData != ReturnData.ERROR)
                        return true;
                    return false;
                case State.connected:
                    if (commandMode) setCommandMode();
                    return true;
                case State.command:
                    return true;
                case State.notConnected:
                    OnIncomingErrorEvent("NotConnectedToBike");
                    Console.WriteLine("ERROR: not connected to bike.");
                    return false;
                default:
                    OnIncomingErrorEvent("NotConnectedToBike");
                    Console.WriteLine("ERROR: unknown error.");
                    return false;
            }
        }

        public void setCommandMode()
        {
            sendData(COMMAND);
        }

        //public void saveToJson(string path, Session session)
        //{
        //    string json = JsonConvert.SerializeObject(session);
        //    File.WriteAllText(path, json);
        //}
        //public Session loadFromJson(string path)
        //{
        //    string jsonFile = File.ReadAllText(path);

        //    Session sessionFromJson = JsonConvert.DeserializeObject<Session>(jsonFile);
        //    return sessionFromJson;
        //}
    }
}
