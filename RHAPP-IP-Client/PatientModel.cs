using RHAPP_IP_Client.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace RHAPP_IP_Client
{
    class PatientModel
    {
        private static PatientModel _patientModel;
        public PatientForm patientform { get; set; }

        public static PatientModel patientModel { get { return _patientModel ?? (_patientModel = new PatientModel()); } }

        private DataHandler dataHandler;
        private Thread workerThread;

        private string powerLog;
        public Boolean askdata;

        public string CurrentDoctorID { get; set; }

        public PatientModel()
        {
            dataHandler = new DataHandler();
            DataHandler.IncomingDataEvent += HandleBikeData; //initialize event
        }
        public void startComPort(string portname)
        {
            dataHandler.initComm(portname);
        }

        public void startAskingData()
        {
            askdata = true;
            speedPoints.Clear();
            bpmPoints.Clear();
            rpmPoints.Clear();
            workerThread = new Thread(() => workerThreadLoop());
            workerThread.Start();
        }

        public void stopAskingData()
        {
            askdata = false;
            dataHandler.sendData(DataHandler.RESET);
        }

        private void workerThreadLoop()
        {
            while (askdata)
            {
                Thread.Sleep(1000);

                if ((patientform.actualBox.Text != powerLog) && (powerLog != null) && (Int32.Parse(powerLog) >= 0))
                {
                    setPower(powerLog);
                }

                try
                {
                    dataHandler.sendData(DataHandler.STATUS);
                }
                catch (Exception)
                {
                    dataHandler.closeComm();
                }

            }
        }
        //event handler
        public List<DataPoint> speedPoints { get; set; } = new List<DataPoint>();
        public List<DataPoint> bpmPoints { get; set; } = new List<DataPoint>();
        public List<DataPoint> rpmPoints { set; get; } = new List<DataPoint>();
        private void HandleBikeData(string[] data)
        {
            if (patientform.InvokeRequired)
            {
                patientform.Invoke((new Action(() => HandleBikeData(data))));
            }
            else
            {
                //fill fields
                //patientform.pulseBox.Text = data[0];
                //patientform.rpmInfoBox.Text = data[1];
                //patientform.speedInfoBox.Text = data[2];
                //patientform.distanceInfoBox.Text = data[3];
                //patientform.requestedBox.Text = data[4];
                //patientform.energyInfoBox.Text = data[5];
                //patientform.timeBox.Text = data[6];
                //patientform.actualBox.Text = data[7];

                ////fill graph speed
                //speedPoints.Add(new DataPoint(Convert.ToDateTime(data[6]).ToOADate(), Convert.ToDouble(data[2])));
                //patientform.speedChart.Series[0].Points.Clear();
                //for (int i = 0; i < speedPoints.Count; i++)
                //    patientform.speedChart.Series[0].Points.Add(speedPoints[i]);
                //if (speedPoints.Count > 25)
                //    speedPoints.RemoveAt(0);
                //patientform.speedChart.Update();

                //fill graph pulse
                bpmPoints.Add(new DataPoint(Convert.ToDateTime(data[6]).ToOADate(), Convert.ToDouble(data[0])));
                patientform.bpmChart.Series[0].Points.Clear();
                for (int i = 0; i < bpmPoints.Count; i++)
                    patientform.bpmChart.Series[0].Points.Add(bpmPoints[i]);
                if (bpmPoints.Count > 25)
                    bpmPoints.RemoveAt(0);
                patientform.bpmChart.Update();

                //fill graph rpm
                rpmPoints.Add(new DataPoint(Convert.ToDateTime(data[6]).ToOADate(), Convert.ToDouble(data[1])));
                patientform.rpmChart.Series[0].Points.Clear();
                for (int i = 0; i < rpmPoints.Count; i++)
                    patientform.rpmChart.Series[0].Points.Add(rpmPoints[i]);
                if (rpmPoints.Count > 25)
                    rpmPoints.RemoveAt(0);
                patientform.rpmChart.Update();
            }

        }

        public void closeComPort()
        {
            stopAskingData();
            if (workerThread != null)
                workerThread.Interrupt();
            dataHandler.closeComm();
        }
        //change bike values
        public void setTimeMode(string time)
        {
            if (!dataHandler.checkBikeState(false)) return;
            dataHandler.sendData("CM");
            dataHandler.sendData("PT " + time);
        }

        public void setPower(string power)
        {
            powerLog = power;
            if (!dataHandler.checkBikeState(false)) return;
            dataHandler.sendData("CM");
            dataHandler.sendData("PW " + power);
        }

        public void setDistanceMode(string distance)
        {
            if (!dataHandler.checkBikeState(false)) return;
            dataHandler.sendData("CM");
            dataHandler.sendData("PD " + distance);
        }

        public void reset()
        {
            if (!dataHandler.checkBikeState(false)) return;
            dataHandler.sendData("RS");
        }
    }
}
