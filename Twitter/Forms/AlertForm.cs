﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.Forms
{
    public partial class AlertForm : Form
    {
        public AlertForm(string sMessage) {
            InitializeComponent();

            linkMessage.Text = sMessage;
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
