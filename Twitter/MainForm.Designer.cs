/*
 * Created by SharpDevelop.
 * User: Peter
 * Date: 07/05/2009
 * Time: 17:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Twitter
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grdFriendStatus = new System.Windows.Forms.DataGridView();
            this.FriendImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.FriendStatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.picProfileImage = new System.Windows.Forms.PictureBox();
            this.btnFriendsTimeline = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnMessage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdFriendStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProfileImage)).BeginInit();
            this.SuspendLayout();
            // 
            // grdFriendStatus
            // 
            this.grdFriendStatus.AllowUserToAddRows = false;
            this.grdFriendStatus.AllowUserToDeleteRows = false;
            this.grdFriendStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFriendStatus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.grdFriendStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFriendStatus.ColumnHeadersVisible = false;
            this.grdFriendStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FriendImageColumn,
            this.FriendStatusColumn});
            this.grdFriendStatus.Location = new System.Drawing.Point(9, 92);
            this.grdFriendStatus.Name = "grdFriendStatus";
            this.grdFriendStatus.ReadOnly = true;
            this.grdFriendStatus.RowHeadersVisible = false;
            this.grdFriendStatus.Size = new System.Drawing.Size(330, 451);
            this.grdFriendStatus.TabIndex = 4;
            // 
            // FriendImageColumn
            // 
            this.FriendImageColumn.HeaderText = "Image";
            this.FriendImageColumn.Name = "FriendImageColumn";
            this.FriendImageColumn.ReadOnly = true;
            this.FriendImageColumn.Width = 48;
            // 
            // FriendStatusColumn
            // 
            this.FriendStatusColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.FriendStatusColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.FriendStatusColumn.HeaderText = "Status";
            this.FriendStatusColumn.Name = "FriendStatusColumn";
            this.FriendStatusColumn.ReadOnly = true;
            // 
            // picProfileImage
            // 
            this.picProfileImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picProfileImage.Location = new System.Drawing.Point(288, 9);
            this.picProfileImage.Name = "picProfileImage";
            this.picProfileImage.Size = new System.Drawing.Size(48, 48);
            this.picProfileImage.TabIndex = 1;
            this.picProfileImage.TabStop = false;
            this.toolTip.SetToolTip(this.picProfileImage, "Change Settings");
            this.picProfileImage.Click += new System.EventHandler(this.picProfileImage_Click);
            // 
            // btnFriendsTimeline
            // 
            this.btnFriendsTimeline.BackColor = System.Drawing.Color.Transparent;
            this.btnFriendsTimeline.BackgroundImage = global::Twitter.Properties.Resource.Twitter;
            this.btnFriendsTimeline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFriendsTimeline.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnFriendsTimeline.FlatAppearance.BorderSize = 0;
            this.btnFriendsTimeline.Location = new System.Drawing.Point(9, 9);
            this.btnFriendsTimeline.Margin = new System.Windows.Forms.Padding(0);
            this.btnFriendsTimeline.Name = "btnFriendsTimeline";
            this.btnFriendsTimeline.Size = new System.Drawing.Size(40, 40);
            this.btnFriendsTimeline.TabIndex = 5;
            this.toolTip.SetToolTip(this.btnFriendsTimeline, "\\");
            this.btnFriendsTimeline.UseVisualStyleBackColor = false;
            this.btnFriendsTimeline.Click += new System.EventHandler(this.btnFriendsTimeline_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Twitter";
            this.notifyIcon.Visible = true;
            // 
            // btnMessage
            // 
            this.btnMessage.Location = new System.Drawing.Point(9, 63);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(330, 23);
            this.btnMessage.TabIndex = 6;
            this.btnMessage.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(238)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(348, 555);
            this.Controls.Add(this.btnMessage);
            this.Controls.Add(this.btnFriendsTimeline);
            this.Controls.Add(this.grdFriendStatus);
            this.Controls.Add(this.picProfileImage);
            this.Name = "MainForm";
            this.Text = "Twitter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grdFriendStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProfileImage)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.PictureBox picProfileImage;
        private System.Windows.Forms.DataGridView grdFriendStatus;
        private System.Windows.Forms.Button btnFriendsTimeline;
        private System.Windows.Forms.DataGridViewImageColumn FriendImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FriendStatusColumn;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnMessage;
	}
}
