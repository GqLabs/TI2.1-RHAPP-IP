using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IP_SharedLibrary.Entity;

namespace RHAPP_IP_Client
{
    public partial class DoctorForm : StandardForm
    {
        public DoctorForm()
        {
            InitializeComponent();
            _appGlobal.UserChangedEvent += HandleUserChanged;
            cmbOnlinePatients.ValueMember = null;
            cmbOnlinePatients.DisplayMember = "Nickname";
        }

        private void HandleUserChanged(User u)
        {
            if (u.Username == _appGlobal.Username)
                return;
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleUserChanged(u)));
                return;
            }
            RemoveUsersFromcmbBox();
            LoadUsers(_appGlobal.Users.Where(x => x.Username != _appGlobal.Username).ToList());
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

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        int j = 0;
        private void button3_Click(object sender, EventArgs e)
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
    }
}
