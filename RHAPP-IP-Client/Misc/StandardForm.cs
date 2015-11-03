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

        public void ExitApplication()
        {
            Application.Exit();
        }
    }
}
