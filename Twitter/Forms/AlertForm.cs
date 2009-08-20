using System;
using System.Windows.Forms;

namespace Forms
{
    /// <summary> Display a popup messenger style alert form </summary>
    public partial class AlertForm : Form
    {
        public event Action LinkClicked = delegate { };

        public AlertForm(string sMessage, System.Drawing.Bitmap bmp) {
            InitializeComponent();

            linkMessage.Text = sMessage;
            picImage.Image = bmp;

            // Display form in bottom right of screen above taskbar
            System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(rect.Width - this.Width, rect.Height - this.Height);

            // Setup timer to close form
            CloseTimer.Interval = 1000 * 10;
            CloseTimer.Start();

            this.Show();
            this.Activate();
        }

        /// <summary> Fade in/out the form </summary>
        /// <param name="iSteps">Number of fade steps, make this negative to fade out</param>
        /// <param name="iTime">Time between each step</param>
        private void ShowHide(int iSteps, int iTime) {
            for (int iCount = 0; iCount < Math.Abs(iSteps); iCount++) {
                System.Threading.Thread.Sleep(iTime);

                if (iSteps > 0)
                    this.Opacity = (double)(iCount) / iSteps;
                else
                    this.Opacity = 1 - (double)(iCount) / Math.Abs(iSteps);

                this.Refresh();
            }
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void AlertForm_Shown(object sender, EventArgs e) {
            ShowHide(10, 50);
        }

        private void linkMessage_Click(object sender, EventArgs e) {
            LinkClicked();

            this.Close();
        }

        private void CloseTimer_Tick(object sender, EventArgs e) {
            ShowHide(-10, 50);

            this.Close();
        }
    }
}
