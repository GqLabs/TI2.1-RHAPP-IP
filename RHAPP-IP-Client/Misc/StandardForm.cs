using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Packet;
using IP_SharedLibrary.Packet.Push;
using IP_SharedLibrary.Packet.Response;
using RHAPP_IP_Client.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RHAPP_IP_Client
{
    public class StandardForm : Form
    {
        protected AppGlobal _appGlobal { get; private set; } = AppGlobal.Instance;     

        public void Logout(Form currentForm)
        {
            currentForm.Hide();
            _appGlobal.Logout();
            Program.LoginForm.Show();
            currentForm.Dispose();
        }

        protected void FormClosingMethod(object sender, FormClosingEventArgs e, Form currentForm)
        {
            Logout(currentForm);

            ExitApplication();
        }

        protected void PacketReceived(Packet p)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                Console.WriteLine(p.ToJsonObject().ToString());
                if (p is SerialDataPushPacket)
                {
                    Console.WriteLine("push packet received!");
                    _appGlobal.OnIncomingMeasurementEvent(p as SerialDataPushPacket);
                }
                else if (p is UserChangedPacket)
                {
                    var packet = p as UserChangedPacket;
                    if (packet.Username == _appGlobal.Username)
                        return;
                    User x = _appGlobal.Users.FirstOrDefault(u => u.Username == packet.Username);
                    if (x == null)
                    {
                        x = new User(packet.Nickname, packet.Username, null);
                        _appGlobal.Users.Add(x);
                    }
                    x.OnlineStatus = packet.Status;
                    _appGlobal.OnUserChangedEvent(x);
                }
                else if (p is StartTestPushPacket)
                {
                    PatientModel.patientModel.StartTest();
                }
                else if (p is CommandPushPacket)
                {
                    PatientModel.patientModel.SendData(((CommandPushPacket)p).Command);
                }
                else if (p is LoginResponsePacket)
                {
                    _appGlobal.OnLoginResultEvent(p as LoginResponsePacket);
                }
                //else if (p is PullResponsePacket<ChatMessage>)
                //{
                //    var packet = p as PullResponsePacket<ChatMessage>;
                //    FillChatMessageList(packet.Data.ToList());
                //    Console.WriteLine("PullResponsePacket<ChatMessage> received!");
                //}
                else if (p is PullResponsePacket)
                {
                    var packet = p as PullResponsePacket;
                    foreach (User u in packet.Data.ToList())
                    {
                        _appGlobal.Users.Add(u);
                        _appGlobal.OnUserChangedEvent(u);
                    }
                }
                else if (p is RequestBikeTestResponsePacket)
                {
                    var packet = p as RequestBikeTestResponsePacket;
                    _appGlobal.OnBikeTestChangedEvent(packet);
                }

                else if (p is ResponsePacket) //this one should be last!
                {
                    _appGlobal.OnResultEvent(p as ResponsePacket);
                }
            });
        }

        public void ExitApplication()
        {
            Application.Exit();
        }
    }
}
