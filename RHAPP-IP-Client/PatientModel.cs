using IP_SharedLibrary.Entity;
using RHAPP_IP_Client.Misc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
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

        public Boolean testStarted;

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
        private void HandleBikeData(Measurement m)
        {
            if (patientform.InvokeRequired)
            {
                patientform.Invoke((new Action(() => HandleBikeData(m))));
            }
            else
            {
                patientform.timeBox.Text = m.Time.Hour + ":" + m.Time.Minute;


                //fill graph pulse
                bpmPoints.Add(new DataPoint(m.Time.Second, Convert.ToDouble(m.Pulse)));
                patientform.bpmChart.Series[0].Points.Clear();
                for (int i = 0; i < bpmPoints.Count; i++)
                    patientform.bpmChart.Series[0].Points.Add(bpmPoints[i]);
                if (bpmPoints.Count > 25)
                    bpmPoints.RemoveAt(0);
                patientform.bpmChart.Update();

                //fill graph rpm
                rpmPoints.Add(new DataPoint(m.Time.Second, Convert.ToDouble(m.PedalRpm)));
                patientform.rpmChart.Series[0].Points.Clear();
                for (int i = 0; i < rpmPoints.Count; i++)
                    patientform.rpmChart.Series[0].Points.Add(rpmPoints[i]);
                if (rpmPoints.Count > 25)
                    rpmPoints.RemoveAt(0);
                patientform.rpmChart.Update();

                if (testStarted)
                {
                    if (m.Time.Minute >= 0 && m.Time.Hour == 1)
                        stopTest(m);
                }

                Console.WriteLine(m.DestPower);
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

        public void startTest()
        {
            if (patientform.gewichtBox2.Text != "" && patientform.leeftijdBox1.Text != "")
            {
                reset();
                setDistanceMode("100");
                testStarted = true;
            }
            else
            {
                MessageBox.Show("vul uw gegevens in");
            }

        }

        public void stopTest(Measurement m)
        {
            testStarted = false;
            bool isMan = true;
            if (patientform.geslachtComboBox2.Text == "Man")
                isMan = true;
            else if (patientform.geslachtComboBox2.Text == "Vrouw")
                isMan = false;
            int pulse = m.Pulse;
            int power = m.DestPower;
            double gewicht = Double.Parse(patientform.gewichtBox2.Text);
            int leeftijd = int.Parse(patientform.leeftijdBox1.Text);
            double vo2 = vo2MaxBerekenen(power, pulse, gewicht, leeftijd);
            var bt = new BikeTest(AppGlobal.Instance.Username, isMan, gewicht, leeftijd, pulse, AppGlobal.Instance.Measurements, vo2);
            var v = new IP_SharedLibrary.Packet.Request.BikeTestPacket(AppGlobal.Instance.Username,bt);
            AppGlobal.Instance.Send(v);

            MessageBox.Show("Uw vo2Max is:" + vo2);
        }

        public double vo2MaxBerekenen(int power, int pulse, double gewicht, int leeftijd)
        {
            if (patientform.geslachtComboBox2.Text == "Man")
            {
                double f = 174.2 * power;
                double g = f + 4020;
                double a = 103.2 * pulse;
                double b = a - 6298;
                double e = g / b;
                double l = leeftijd;
                double k = leeftijd;
                double h = e;
                if (l == 9) h = 1.22 * e;
                if (l == 10) h = 1.19 * e;
                if (l == 11) h = 1.16 * e;
                if (l == 12) h = 1.14 * e;
                if (l == 13) h = 1.11 * e;
                if (l == 14) h = 1.08 * e;
                if (l == 15) h = 1.06 * e;
                if (l == 16) h = 1.03 * e;
                if (l > 35 && k <= 40) h = 0.87 * e;
                if (l > 40 && k <= 45) h = 0.83 * e;
                if (l > 45 && k <= 50) h = 0.78 * e;
                if (l > 50 && k <= 55) h = 0.75 * e;
                if (l > 55 && k <= 60) h = 0.71 * e;
                if (l > 60 && k <= 65) h = 0.68 * e;
                if (l > 66) h = 0.65 * e;
                double d = h * 1000;
                double c = d / gewicht;
                return Math.Floor(c);
            }
            else if (patientform.geslachtComboBox2.Text == "Vrouw")
            {
                double f = 163.8 * power;
                double g = f + 3780;
                double a = 104.4 * pulse;
                double b = a - 7514;
                double e = g / b;
                double l = leeftijd;
                double k = leeftijd;
                double h = e;
                if (l == 9) h = 1.22 * e;
                if (l == 10) h = 1.19 * e;
                if (l == 11) h = 1.16 * e;
                if (l == 12) h = 1.14 * e;
                if (l == 13) h = 1.11 * e;
                if (l == 14) h = 1.08 * e;
                if (l == 15) h = 1.06 * e;
                if (l == 16) h = 1.03 * e;
                if (l > 35 && k <= 40) h = 0.87 * e;
                if (l > 40 && k <= 45) h = 0.83 * e;
                if (l > 45 && k <= 50) h = 0.78 * e;
                if (l > 50 && k <= 55) h = 0.75 * e;
                if (l > 55 && k <= 60) h = 0.71 * e;
                if (l > 60 && k <= 65) h = 0.68 * e;
                if (l > 66) h = 0.65 * e;
                double d = h * 1000;
                double c = d / gewicht;
                return Math.Floor(c);
            }
            else
            {
                return 0;
            }
        }
    }
}
