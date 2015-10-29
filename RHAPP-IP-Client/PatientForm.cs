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

        public PatientForm()
        {
            InitializeComponent();
            patientModel = PatientModel.patientModel;
            patientModel.patientform = this;
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
                patientModel.setDistanceMode("10");
            patientModel.startAskingData();
            }

            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                patientModel.stopAskingData();
            }
        }
    }
}
