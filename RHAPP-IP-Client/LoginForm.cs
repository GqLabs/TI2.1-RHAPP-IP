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
            _appGlobal.LoginResultEvent += HandleLogin;
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
                MessageBox.Show("Vul een Gebruikersnaam en Wachtwoord in","Inlog fout", MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
        public void HandleLogin(Packet packet)
        {
            LoginResponsePacket responsePacket = packet as LoginResponsePacket;

            switch (responsePacket.Status)
            {
                case "200":
                    if (this.InvokeRequired)
                    {
                        this.Invoke((new Action(() => HandleLogin(packet))));
                        return;
                    }
                    Visible = false;
                    if (responsePacket.isDoctor)
                    {
                        new DoctorForm();
                    }
                    else
                    {
                        new PatientForm();
                    }
                    break;
                case "430":
                    MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK);
                    break;
                default:
                    MessageBox.Show("Unhandled error occured", "Error", MessageBoxButtons.OK);
                    break;
            }
        }
    }
}
