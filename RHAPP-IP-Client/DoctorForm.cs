using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Packet;
using System.Windows.Forms.DataVisualization.Charting;
using IP_SharedLibrary.Packet.Push;
using IP_SharedLibrary.Packet.Request;
using System.Threading;
using IP_SharedLibrary.Packet.Response;

namespace RHAPP_IP_Client
{
    public partial class DoctorForm : StandardForm
    {
        public List<DataPoint> speedPoints { get; set; } = new List<DataPoint>();
        public List<DataPoint> bpmPoints { get; set; } = new List<DataPoint>();
        public List<DataPoint> rpmPoints { set; get; } = new List<DataPoint>();

        public Dictionary<int,BikeTest> bikeTests { get; set; } = new Dictionary<int,BikeTest>();
        private int _bikeTestCount = 1;

        public DoctorForm()
        {
            InitializeComponent();
            _appGlobal.UserChangedEvent += HandleUserChanged;
            _appGlobal.IncomingMeasurementEvent += HandleIncomingMeasurement;
            _appGlobal.BikeTestChangedEvent += HandleBikeTest;
            cmbOnlinePatients.ValueMember = null;
            cmbOnlinePatients.DisplayMember = "Nickname";
            comboBox1.ValueMember = null;
            comboBox1.DisplayMember = "Nickname";
        }

        private void HandleBikeTest(Packet packet)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleBikeTest(packet)));
            }
            else
            {
                var p = packet as RequestBikeTestResponsePacket;
                foreach(BikeTest test in p.Biketest)
                {
                    bikeTests.Add(_bikeTestCount, test);
                    _bikeTestCount++;
                }
                cmbTestNummer.Items.Clear();
                var biketests = bikeTests.Where(test => test.Value.Username == ((User)comboBox1.SelectedItem).Username);
                foreach (KeyValuePair<int, BikeTest> biketest in biketests)
                {
                    cmbTestNummer.Items.Add(biketest.Key);
                }
            }
        }

        private void HandleUserChanged(User u)
        {
            comboBox1.Items.Add(u);
            if (u.Username == _appGlobal.Username || !u.OnlineStatus)
                return;
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleUserChanged(u)));
                return;
            }
            RemoveUsersFromcmbBox();
            LoadUsers(_appGlobal.Users.Where(x => x.Username != _appGlobal.Username).ToList());
        }

        private void HandleIncomingMeasurement(Packet packet)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HandleIncomingMeasurement(packet)));
            }
            else
            {
                var selectedUser = (User)cmbOnlinePatients.SelectedItem;
                var resultPacket = ((SerialDataPushPacket)packet);
                if (selectedUser == null) return;
                if (selectedUser.Username == resultPacket.Username)
                {
                    HandleBikeData(resultPacket.Measurement);
                }
            }
        }


        private void HandleBikeData(Measurement m)
        {
            //fill graph pulse
            bpmPoints.Add(new DataPoint(m.Time.Second, Convert.ToDouble(m.Pulse)));
            crtPulse.Series[0].Points.Clear();
            foreach (DataPoint t in bpmPoints)
                crtPulse.Series[0].Points.Add(t);
            if (bpmPoints.Count > 25)
                bpmPoints.RemoveAt(0);
            crtPulse.Update();

            //fill graph rpm
            rpmPoints.Add(new DataPoint(m.Time.Second, Convert.ToDouble(m.PedalRpm)));
            crtRPM.Series[0].Points.Clear();
            foreach (DataPoint t in rpmPoints)
                crtRPM.Series[0].Points.Add(t);
            if (rpmPoints.Count > 25)
                rpmPoints.RemoveAt(0);
            crtRPM.Update();
        }

        public void LoadUsers(List<User> nicknames)
        {
            foreach (User u in nicknames)
            {
                cmbOnlinePatients.Items.Add(u);
            }
        }

        public void RemoveUsersFromcmbBox()
        {
            cmbOnlinePatients.Items.Clear();
        }

        int j = 0;
        private void btnTestButton_Click(object sender, EventArgs e)
        {
            int i;
            for (i = j; i < (j + 3); i++)
            {
                _appGlobal.Users.Add(new User("nickname" + i.ToString(), "username" + i.ToString(), null));
                HandleUserChanged(_appGlobal.Users.Last());
            }
            j = i;

        }

        private void DoctorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosingMethod(sender, e, this);
        }

        private void btnSetPower_Click(object sender, EventArgs e)
        {
            var selectedUser = (User)cmbOnlinePatients.SelectedItem;
            var v1 = new SendCommandPacket("CM", selectedUser.Username);
            //Thread.Sleep(200);
            var v2 = new SendCommandPacket("PW " + crtPower.Value.ToString(),selectedUser.Username);
            _appGlobal.Send(v1);
            _appGlobal.Send(v2);
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            var selectedUser = (User)cmbOnlinePatients.SelectedItem;
            var v3 = new StartTestPacket(selectedUser.Username);
            _appGlobal.Send(v3);
            var v1 = new SendCommandPacket("CM", selectedUser.Username);
            //Thread.Sleep(200);
            var v2 = new SendCommandPacket("PW " + crtPower.Value.ToString(), selectedUser.Username);
            //Thread.Sleep(200);
            //_appGlobal.Send(v1);
            //Thread.Sleep(700);
            //_appGlobal.Send(v2);
        }

        private void cmbOnlinePatients_SelectionChangeCommitted(object sender, EventArgs e)
        {
            speedPoints.Clear();
            bpmPoints.Clear();
            rpmPoints.Clear();
            List<Measurement> measurementsOfPatient = _appGlobal.PatientMeasurements
                .Where(patient => patient.Item1 == ((User)cmbOnlinePatients.SelectedItem).Username)
                .Select(allMeasurements => allMeasurements.Item2).ToList();
            foreach (Measurement measurement in measurementsOfPatient)
            {
                HandleBikeData(measurement);
            }
            Measurement lastMeasurement = measurementsOfPatient.LastOrDefault();
            if (lastMeasurement != null)
                crtPower.Value = lastMeasurement.DestPower;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                _appGlobal.SendRequestBikeTestPacket(((User)comboBox1.SelectedItem).Username);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbTestNummer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BikeTest p;
            bikeTests.TryGetValue((int)cmbTestNummer.SelectedItem, out p);
            leeftijdBox.Text = "" + p.Age;
            gewichtBox.Text = "" + p.Weight;
            vo2Box.Text = "" + p.Vo2Max;

            if (p.Gender)
                geslachtBox.Text = "Man";
            else if (!p.Gender)
                geslachtBox.Text = "Vrouw";
        }

        private void cmbTestNummer_Click(object sender, EventArgs e)
        {
            
            
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _bikeTestCount = 1;
            bikeTests.Clear();
            button1_Click(sender, e);
        }
    }
}
