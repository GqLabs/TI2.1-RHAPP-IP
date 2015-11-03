using IP_SharedLibrary.Packet;
using IP_SharedLibrary.Packet.Request;
using IP_SharedLibrary.Packet.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RHAPP_IP_Client
{
    public partial class LoginForm : StandardForm
    {
        public LoginForm()
        {
            InitializeComponent();
            _appGlobal.LoginResultEvent += HandleLoginEvent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if ((username != "") && (password != ""))
            {
                _appGlobal.LoginToServer(username, password);
                _appGlobal.Receive();
            }
            else
                MessageBox.Show("Vul een Gebruikersnaam en Wachtwoord in", "Inlog fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void HandleLoginEvent(Packet packet)
        {
            LoginResponsePacket responsePacket = packet as LoginResponsePacket;

            switch (responsePacket.Status)
            {
                case "200":
                    if (this.InvokeRequired)
                    {
                        this.Invoke((new Action(() => HandleLoginEvent(packet))));
                        return;
                    }
                    if (responsePacket.isDoctor)
                    {
                        DoctorForm d = new DoctorForm();
                        this.Hide();
                        d.Show();
                    }
                    else
                    {
                        PatientForm p = new PatientForm();
                        this.Hide();
                        p.Show();
                    }
                    break;
                case "201":
                    MessageBox.Show("You can only log in once", "Error", MessageBoxButtons.OK);
                    break;
                case "430":
                    MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK);
                    break;
                default:
                    MessageBox.Show("Unhandled error occured", "Error", MessageBoxButtons.OK);
                    break;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                button1_Click(sender, e);
            }
        }
    }
}
