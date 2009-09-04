/*
 * Created by SharpDevelop.
 * User: Peter
 * Date: 07/05/2009
 * Time: 17:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.picProfileImage = new System.Windows.Forms.PictureBox();
            this.btnFriendsTimeline = new System.Windows.Forms.Button();
            this.btnMessage = new System.Windows.Forms.Button();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tblTweets = new System.Windows.Forms.TableLayoutPanel();
            this.rchStatus = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picProfileImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picProfileImage
            // 
            this.picProfileImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picProfileImage.Location = new System.Drawing.Point(295, 9);
            this.picProfileImage.Name = "picProfileImage";
            this.picProfileImage.Size = new System.Drawing.Size(48, 48);
            this.picProfileImage.TabIndex = 1;
            this.picProfileImage.TabStop = false;
            this.ToolTip.SetToolTip(this.picProfileImage, "Settings");
            this.picProfileImage.Click += new System.EventHandler(this.picProfileImage_Click);
            // 
            // btnFriendsTimeline
            // 
            this.btnFriendsTimeline.BackColor = System.Drawing.Color.Transparent;
            this.btnFriendsTimeline.BackgroundImage = global::Core.Properties.Resource.Twitter;
            this.btnFriendsTimeline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFriendsTimeline.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnFriendsTimeline.FlatAppearance.BorderSize = 0;
            this.btnFriendsTimeline.Location = new System.Drawing.Point(9, 9);
            this.btnFriendsTimeline.Margin = new System.Windows.Forms.Padding(0);
            this.btnFriendsTimeline.Name = "btnFriendsTimeline";
            this.btnFriendsTimeline.Size = new System.Drawing.Size(40, 40);
            this.btnFriendsTimeline.TabIndex = 5;
            this.ToolTip.SetToolTip(this.btnFriendsTimeline, "Get Tweets");
            this.btnFriendsTimeline.UseVisualStyleBackColor = false;
            this.btnFriendsTimeline.Click += new System.EventHandler(this.btnFriendsTimeline_Click);
            // 
            // btnMessage
            // 
            this.btnMessage.Location = new System.Drawing.Point(9, 63);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(330, 23);
            this.btnMessage.TabIndex = 6;
            this.btnMessage.UseVisualStyleBackColor = true;
            this.btnMessage.Visible = false;
            this.btnMessage.Click += new System.EventHandler(this.btnMessage_Click);
            // 
            // tblTweets
            // 
            this.tblTweets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tblTweets.AutoScroll = true;
            this.tblTweets.ColumnCount = 2;
            this.tblTweets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tblTweets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblTweets.Location = new System.Drawing.Point(3, 63);
            this.tblTweets.Name = "tblTweets";
            this.tblTweets.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.tblTweets.RowCount = 1;
            this.tblTweets.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblTweets.Size = new System.Drawing.Size(340, 488);
            this.tblTweets.TabIndex = 8;
            // 
            // rchStatus
            // 
            this.rchStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rchStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rchStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rchStatus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rchStatus.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchStatus.Location = new System.Drawing.Point(52, 9);
            this.rchStatus.MaxLength = 140;
            this.rchStatus.Name = "rchStatus";
            this.rchStatus.ReadOnly = true;
            this.rchStatus.Size = new System.Drawing.Size(237, 48);
            this.rchStatus.TabIndex = 9;
            this.rchStatus.Text = "";
            this.rchStatus.Visible = false;
            this.rchStatus.Click += new System.EventHandler(this.rchStatus_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(348, 555);
            this.Controls.Add(this.rchStatus);
            this.Controls.Add(this.tblTweets);
            this.Controls.Add(this.btnMessage);
            this.Controls.Add(this.btnFriendsTimeline);
            this.Controls.Add(this.picProfileImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Tweety";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picProfileImage)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.PictureBox picProfileImage;
        private System.Windows.Forms.Button btnFriendsTimeline;
        private System.Windows.Forms.Button btnMessage;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.TableLayoutPanel tblTweets;
        private System.Windows.Forms.RichTextBox rchStatus;
	}
}
