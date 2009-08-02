using System;
using System.Windows.Forms;

namespace Forms
{
    public partial class AlertForm : Form
    {
        Form _MainForm;
        Timer CloseTimer;

        public AlertForm(string sMessage, Form MainForm) {
            InitializeComponent();

            _MainForm = MainForm;
            linkMessage.Text = sMessage;

            // Setup timer to close form
            CloseTimer = new Timer();
            CloseTimer.Tick += new EventHandler(CloseTimer_Tick);
            CloseTimer.Interval = 1000 * 10;
            CloseTimer.Start();
        }

        void CloseTimer_Tick(object sender, EventArgs e) {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void AlertForm_Shown(object sender, EventArgs e) {
            int iSteps = 10;

            for (int iCount = 0; iCount < iSteps; iCount++) {
                System.Threading.Thread.Sleep(100);
                this.Opacity = (double)(iCount) / iSteps;
                this.Refresh();
                Application.DoEvents();
            }
        }

        private void linkMessage_Click(object sender, EventArgs e) {
            _MainForm.WindowState = FormWindowState.Normal;

            this.Close();
        }
    }
}
