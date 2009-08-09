namespace Forms
{
    partial class UpdateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.rchUpdate = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rchUpdate
            // 
            this.rchUpdate.Location = new System.Drawing.Point(12, 12);
            this.rchUpdate.Name = "rchUpdate";
            this.rchUpdate.Size = new System.Drawing.Size(345, 116);
            this.rchUpdate.TabIndex = 0;
            this.rchUpdate.Text = "";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(237, 134);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(57, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(300, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 163);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rchUpdate);
            this.Name = "UpdateForm";
            this.Text = "UpdateForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rchUpdate;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}