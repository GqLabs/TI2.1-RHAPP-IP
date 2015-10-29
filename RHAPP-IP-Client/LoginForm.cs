using IP_SharedLibrary.Packet.Request;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if ((username != "") && (password != ""))
            {
                var packet = new LoginPacket(username, password);
                _appGlobal.Send(packet);
                _appGlobal.Receive();
            }
            else
                MessageBox.Show("Vul een Gebruikersnaam en Wachtwoord in","Inlog fout", MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
        public void HandleLoginStatus(string status)
        {
            switch (status)
            {
                case "200":
                    if (this.InvokeRequired)
                    {
                        this.Invoke((new Action(() => HandleLoginStatus(status))));
                        return;
                    }
                    Visible = false;     
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
