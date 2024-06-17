namespace UModbus
{
    partial class UserReqForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserReqForm));
            this.RequestSend = new System.Windows.Forms.Button();
            this.RequestLog = new UModbus.LogBox();
            this.Request = new UModbus.RequestBox();
            this.SuspendLayout();
            // 
            // RequestSend
            // 
            this.RequestSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestSend.Location = new System.Drawing.Point(362, 6);
            this.RequestSend.Name = "RequestSend";
            this.RequestSend.Size = new System.Drawing.Size(75, 20);
            this.RequestSend.TabIndex = 2;
            this.RequestSend.Text = "Send";
            this.RequestSend.UseVisualStyleBackColor = true;
            this.RequestSend.Click += new System.EventHandler(this.RequestSendClick);
            // 
            // RequestLog
            // 
            this.RequestLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestLog.Location = new System.Drawing.Point(6, 29);
            this.RequestLog.Name = "RequestLog";
            this.RequestLog.Size = new System.Drawing.Size(430, 230);
            this.RequestLog.TabIndex = 3;
            this.RequestLog.Text = "logBox1";
            // 
            // Request
            // 
            this.Request.Location = new System.Drawing.Point(6, 7);
            this.Request.Name = "Request";
            this.Request.Size = new System.Drawing.Size(355, 18);
            this.Request.TabIndex = 4;
            this.Request.Text = "requestBox1";
            // 
            // UserReqForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 265);
            this.Controls.Add(this.Request);
            this.Controls.Add(this.RequestLog);
            this.Controls.Add(this.RequestSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(458, 299);
            this.Name = "UserReqForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Requests";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button RequestSend;
        private LogBox RequestLog;
        private RequestBox Request;
    }
}