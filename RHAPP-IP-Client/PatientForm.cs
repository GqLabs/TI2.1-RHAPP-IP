using RHAPP_IP_Client.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RHAPP_IP_Client
{
    public partial class PatientForm : StandardForm
    {
        private PatientModel patientModel;

        private Dictionary<string, Dictionary<string, List<string>>> DictionaryByGender;

        public PatientForm()
        {
            InitializeComponent();
            patientModel = PatientModel.patientModel;
            patientModel.patientform = this;
            
            DictionaryByGender.Add("Female", null);

            var dict1Male = new Dictionary<string, List<String>>();
            var list1Male = new List<string>();

            list1Male.Add("2.2");
            list1Male.Add("2.2");
            list1Male.Add("2.2");
            list1Male.Add("2.3");
            list1Male.Add("2.3");
            list1Male.Add("2.3");

            dict1Male.Add("50watt300kpm", list1Male);

            DictionaryByGender.Add("Male", dict1Male);

        }

        private void PatientForm_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            patientModel.startComPort(comboBox1.SelectedItem.ToString());
            Console.WriteLine(comboBox1.SelectedItem.ToString());
        }

        private void requestData_Click(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                if(!PatientModel.patientModel.testStarted)
                patientModel.setDistanceMode("10");
            patientModel.startAskingData();
            }

            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                patientModel.stopAskingData();
            }
        }

        private void PatientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosingMethod(sender, e, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            patientModel.startTest();
        }
    }
}
