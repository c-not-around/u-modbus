namespace UModbus
{
    partial class SlaveScanForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveScanForm));
            this.BeginAddressDesc = new System.Windows.Forms.Label();
            this.BeginAddress = new System.Windows.Forms.NumericUpDown();
            this.EndAddress = new System.Windows.Forms.NumericUpDown();
            this.EndAddressDesc = new System.Windows.Forms.Label();
            this.AddressBox = new System.Windows.Forms.GroupBox();
            this.TestReqBox = new System.Windows.Forms.GroupBox();
            this.TestRequest = new UModbus.RequestBox();
            this.ScanClose = new System.Windows.Forms.Button();
            this.ScanAbort = new System.Windows.Forms.Button();
            this.ScanStart = new System.Windows.Forms.Button();
            this.ScanLog = new UModbus.LogBox();
            this.ScanProgress = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.BeginAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndAddress)).BeginInit();
            this.AddressBox.SuspendLayout();
            this.TestReqBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BeginAddressDesc
            // 
            this.BeginAddressDesc.AutoSize = true;
            this.BeginAddressDesc.Location = new System.Drawing.Point(8, 21);
            this.BeginAddressDesc.Name = "BeginAddressDesc";
            this.BeginAddressDesc.Size = new System.Drawing.Size(76, 13);
            this.BeginAddressDesc.TabIndex = 0;
            this.BeginAddressDesc.Text = "begin address:";
            this.BeginAddressDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BeginAddress
            // 
            this.BeginAddress.Location = new System.Drawing.Point(85, 19);
            this.BeginAddress.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.BeginAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BeginAddress.Name = "BeginAddress";
            this.BeginAddress.Size = new System.Drawing.Size(42, 20);
            this.BeginAddress.TabIndex = 1;
            this.BeginAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EndAddress
            // 
            this.EndAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EndAddress.Location = new System.Drawing.Point(223, 19);
            this.EndAddress.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.EndAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.EndAddress.Name = "EndAddress";
            this.EndAddress.Size = new System.Drawing.Size(42, 20);
            this.EndAddress.TabIndex = 3;
            this.EndAddress.Value = new decimal(new int[] {
            247,
            0,
            0,
            0});
            // 
            // EndAddressDesc
            // 
            this.EndAddressDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EndAddressDesc.AutoSize = true;
            this.EndAddressDesc.Location = new System.Drawing.Point(154, 21);
            this.EndAddressDesc.Name = "EndAddressDesc";
            this.EndAddressDesc.Size = new System.Drawing.Size(68, 13);
            this.EndAddressDesc.TabIndex = 2;
            this.EndAddressDesc.Text = "end address:";
            this.EndAddressDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AddressBox
            // 
            this.AddressBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressBox.Controls.Add(this.EndAddress);
            this.AddressBox.Controls.Add(this.BeginAddressDesc);
            this.AddressBox.Controls.Add(this.EndAddressDesc);
            this.AddressBox.Controls.Add(this.BeginAddress);
            this.AddressBox.Location = new System.Drawing.Point(12, 12);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(275, 49);
            this.AddressBox.TabIndex = 4;
            this.AddressBox.TabStop = false;
            this.AddressBox.Text = "Address range";
            // 
            // TestReqBox
            // 
            this.TestReqBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TestReqBox.Controls.Add(this.TestRequest);
            this.TestReqBox.Location = new System.Drawing.Point(12, 67);
            this.TestReqBox.Name = "TestReqBox";
            this.TestReqBox.Size = new System.Drawing.Size(275, 45);
            this.TestReqBox.TabIndex = 5;
            this.TestReqBox.TabStop = false;
            this.TestReqBox.Text = "Test request";
            // 
            // TestRequest
            // 
            this.TestRequest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TestRequest.BackColor = System.Drawing.Color.White;
            this.TestRequest.Location = new System.Drawing.Point(6, 19);
            this.TestRequest.Name = "TestRequest";
            this.TestRequest.Size = new System.Drawing.Size(263, 18);
            this.TestRequest.TabIndex = 0;
            this.TestRequest.Text = "11";
            // 
            // ScanClose
            // 
            this.ScanClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanClose.Location = new System.Drawing.Point(213, 283);
            this.ScanClose.Name = "ScanClose";
            this.ScanClose.Size = new System.Drawing.Size(75, 23);
            this.ScanClose.TabIndex = 7;
            this.ScanClose.Text = "Close";
            this.ScanClose.UseVisualStyleBackColor = true;
            this.ScanClose.Click += new System.EventHandler(this.ScanCloseClick);
            // 
            // ScanAbort
            // 
            this.ScanAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanAbort.Enabled = false;
            this.ScanAbort.Location = new System.Drawing.Point(51, 283);
            this.ScanAbort.Name = "ScanAbort";
            this.ScanAbort.Size = new System.Drawing.Size(75, 23);
            this.ScanAbort.TabIndex = 8;
            this.ScanAbort.Text = "Abort";
            this.ScanAbort.UseVisualStyleBackColor = true;
            this.ScanAbort.Click += new System.EventHandler(this.ScanAbortClick);
            // 
            // ScanStart
            // 
            this.ScanStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanStart.Location = new System.Drawing.Point(132, 283);
            this.ScanStart.Name = "ScanStart";
            this.ScanStart.Size = new System.Drawing.Size(75, 23);
            this.ScanStart.TabIndex = 9;
            this.ScanStart.Text = "Start";
            this.ScanStart.UseVisualStyleBackColor = true;
            this.ScanStart.Click += new System.EventHandler(this.ScanStartClick);
            // 
            // ScanLog
            // 
            this.ScanLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanLog.Location = new System.Drawing.Point(13, 129);
            this.ScanLog.Name = "ScanLog";
            this.ScanLog.Size = new System.Drawing.Size(274, 149);
            this.ScanLog.TabIndex = 10;
            this.ScanLog.Text = "logBox1";
            // 
            // ScanProgress
            // 
            this.ScanProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanProgress.Location = new System.Drawing.Point(13, 115);
            this.ScanProgress.Name = "ScanProgress";
            this.ScanProgress.Size = new System.Drawing.Size(274, 11);
            this.ScanProgress.TabIndex = 11;
            // 
            // SlaveScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 314);
            this.Controls.Add(this.ScanProgress);
            this.Controls.Add(this.ScanLog);
            this.Controls.Add(this.ScanStart);
            this.Controls.Add(this.ScanAbort);
            this.Controls.Add(this.ScanClose);
            this.Controls.Add(this.TestReqBox);
            this.Controls.Add(this.AddressBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(316, 348);
            this.Name = "SlaveScanForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bus Scan";
            ((System.ComponentModel.ISupportInitialize)(this.BeginAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndAddress)).EndInit();
            this.AddressBox.ResumeLayout(false);
            this.AddressBox.PerformLayout();
            this.TestReqBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label BeginAddressDesc;
        private System.Windows.Forms.NumericUpDown BeginAddress;
        private System.Windows.Forms.NumericUpDown EndAddress;
        private System.Windows.Forms.Label EndAddressDesc;
        private System.Windows.Forms.GroupBox AddressBox;
        private System.Windows.Forms.GroupBox TestReqBox;
        private System.Windows.Forms.Button ScanClose;
        private System.Windows.Forms.Button ScanAbort;
        private System.Windows.Forms.Button ScanStart;
        private LogBox ScanLog;
        private RequestBox TestRequest;
        private System.Windows.Forms.ProgressBar ScanProgress;
    }
}