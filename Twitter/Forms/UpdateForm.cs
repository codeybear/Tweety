using System;
using System.Windows.Forms;
using Core;

namespace Forms
{
    public partial class UpdateForm : Form
    {
        public event Action<string> StatusChanged = delegate { };

        public UpdateForm(string sStatus) {
            InitializeComponent();

            rchUpdate.Text = sStatus;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            try {
                Twitter.UpdateStatus(rchUpdate.Text, SettingHelper.UserName, SettingHelper.Password);
                StatusChanged(rchUpdate.Text);
                this.Close();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
