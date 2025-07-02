namespace UModbus
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ModeBox = new System.Windows.Forms.GroupBox();
            this.ModeUdp = new System.Windows.Forms.RadioButton();
            this.ModeTcp = new System.Windows.Forms.RadioButton();
            this.ModeAscii = new System.Windows.Forms.RadioButton();
            this.ModeRtu = new System.Windows.Forms.RadioButton();
            this.LinkBox = new System.Windows.Forms.GroupBox();
            this.SlaveAdrDesc = new System.Windows.Forms.Label();
            this.SlaveScan = new System.Windows.Forms.LinkLabel();
            this.Connect = new System.Windows.Forms.Button();
            this.SlaveAddress = new System.Windows.Forms.NumericUpDown();
            this.LinkAddressDesc = new System.Windows.Forms.Label();
            this.PhysicalAddress = new System.Windows.Forms.TextBox();
            this.LinkReqGapDesc = new System.Windows.Forms.Label();
            this.MapBox = new System.Windows.Forms.GroupBox();
            this.MapMoveDown = new System.Windows.Forms.Button();
            this.MapMoveUp = new System.Windows.Forms.Button();
            this.MapSave = new System.Windows.Forms.Button();
            this.MapDel = new System.Windows.Forms.Button();
            this.MapAdd = new System.Windows.Forms.Button();
            this.MapOpen = new System.Windows.Forms.Button();
            this.DataMapView = new System.Windows.Forms.DataGridView();
            this.RowEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ItemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemFormat = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemRaw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataMapViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DataViewEnableAll = new System.Windows.Forms.ToolStripMenuItem();
            this.DataViewDisableAll = new System.Windows.Forms.ToolStripMenuItem();
            this.DataViewSave = new System.Windows.Forms.ToolStripMenuItem();
            this.AccessBox = new System.Windows.Forms.GroupBox();
            this.AccessUserReq = new System.Windows.Forms.Button();
            this.LogEnable = new System.Windows.Forms.CheckBox();
            this.CycleRead = new System.Windows.Forms.CheckBox();
            this.AccessAbort = new System.Windows.Forms.Button();
            this.AccessWrite = new System.Windows.Forms.Button();
            this.AccessRead = new System.Windows.Forms.Button();
            this.SettingsBox = new System.Windows.Forms.GroupBox();
            this.LinkTimeoutDesc = new System.Windows.Forms.Label();
            this.Timeout = new System.Windows.Forms.NumericUpDown();
            this.ReqGap = new System.Windows.Forms.NumericUpDown();
            this.StatusBox = new System.Windows.Forms.StatusStrip();
            this.StatusMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusPhysical = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusCurrentRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSelRows = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusReqError = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.AccessProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.ModeBox.SuspendLayout();
            this.LinkBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SlaveAddress)).BeginInit();
            this.MapBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataMapView)).BeginInit();
            this.DataMapViewMenu.SuspendLayout();
            this.AccessBox.SuspendLayout();
            this.SettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Timeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReqGap)).BeginInit();
            this.StatusBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModeBox
            // 
            this.ModeBox.Controls.Add(this.ModeUdp);
            this.ModeBox.Controls.Add(this.ModeTcp);
            this.ModeBox.Controls.Add(this.ModeAscii);
            this.ModeBox.Controls.Add(this.ModeRtu);
            this.ModeBox.Location = new System.Drawing.Point(5, 5);
            this.ModeBox.Name = "ModeBox";
            this.ModeBox.Size = new System.Drawing.Size(110, 74);
            this.ModeBox.TabIndex = 2;
            this.ModeBox.TabStop = false;
            this.ModeBox.Text = "Mode";
            // 
            // ModeUdp
            // 
            this.ModeUdp.BackColor = System.Drawing.SystemColors.Control;
            this.ModeUdp.Location = new System.Drawing.Point(60, 47);
            this.ModeUdp.Name = "ModeUdp";
            this.ModeUdp.Size = new System.Drawing.Size(48, 17);
            this.ModeUdp.TabIndex = 3;
            this.ModeUdp.Tag = "4";
            this.ModeUdp.Text = "UDP";
            this.ModeUdp.UseVisualStyleBackColor = false;
            this.ModeUdp.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // ModeTcp
            // 
            this.ModeTcp.BackColor = System.Drawing.SystemColors.Control;
            this.ModeTcp.Location = new System.Drawing.Point(60, 19);
            this.ModeTcp.Name = "ModeTcp";
            this.ModeTcp.Size = new System.Drawing.Size(46, 17);
            this.ModeTcp.TabIndex = 2;
            this.ModeTcp.Tag = "3";
            this.ModeTcp.Text = "TCP";
            this.ModeTcp.UseVisualStyleBackColor = false;
            this.ModeTcp.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // ModeAscii
            // 
            this.ModeAscii.BackColor = System.Drawing.SystemColors.Control;
            this.ModeAscii.Location = new System.Drawing.Point(6, 47);
            this.ModeAscii.Name = "ModeAscii";
            this.ModeAscii.Size = new System.Drawing.Size(52, 17);
            this.ModeAscii.TabIndex = 1;
            this.ModeAscii.Tag = "2";
            this.ModeAscii.Text = "ASCII";
            this.ModeAscii.UseVisualStyleBackColor = false;
            this.ModeAscii.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // ModeRtu
            // 
            this.ModeRtu.BackColor = System.Drawing.SystemColors.Control;
            this.ModeRtu.Checked = true;
            this.ModeRtu.Location = new System.Drawing.Point(6, 19);
            this.ModeRtu.Name = "ModeRtu";
            this.ModeRtu.Size = new System.Drawing.Size(48, 17);
            this.ModeRtu.TabIndex = 0;
            this.ModeRtu.TabStop = true;
            this.ModeRtu.Tag = "1";
            this.ModeRtu.Text = "RTU";
            this.ModeRtu.UseVisualStyleBackColor = false;
            this.ModeRtu.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // LinkBox
            // 
            this.LinkBox.Controls.Add(this.SlaveAdrDesc);
            this.LinkBox.Controls.Add(this.SlaveScan);
            this.LinkBox.Controls.Add(this.Connect);
            this.LinkBox.Controls.Add(this.SlaveAddress);
            this.LinkBox.Controls.Add(this.LinkAddressDesc);
            this.LinkBox.Controls.Add(this.PhysicalAddress);
            this.LinkBox.Location = new System.Drawing.Point(121, 5);
            this.LinkBox.Name = "LinkBox";
            this.LinkBox.Size = new System.Drawing.Size(189, 74);
            this.LinkBox.TabIndex = 3;
            this.LinkBox.TabStop = false;
            this.LinkBox.Text = "Link";
            // 
            // SlaveAdrDesc
            // 
            this.SlaveAdrDesc.Location = new System.Drawing.Point(2, 50);
            this.SlaveAdrDesc.Name = "SlaveAdrDesc";
            this.SlaveAdrDesc.Size = new System.Drawing.Size(48, 13);
            this.SlaveAdrDesc.TabIndex = 9;
            this.SlaveAdrDesc.Text = "Slave:";
            this.SlaveAdrDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SlaveScan
            // 
            this.SlaveScan.LinkColor = System.Drawing.Color.Black;
            this.SlaveScan.Location = new System.Drawing.Point(2, 50);
            this.SlaveScan.Name = "SlaveScan";
            this.SlaveScan.Size = new System.Drawing.Size(48, 13);
            this.SlaveScan.TabIndex = 9;
            this.SlaveScan.TabStop = true;
            this.SlaveScan.Text = "Slave:";
            this.SlaveScan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SlaveScan.Click += new System.EventHandler(this.SlaveScanClick);
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(101, 45);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(81, 23);
            this.Connect.TabIndex = 3;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.ConnectClick);
            // 
            // SlaveAddress
            // 
            this.SlaveAddress.Location = new System.Drawing.Point(50, 47);
            this.SlaveAddress.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.SlaveAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SlaveAddress.Name = "SlaveAddress";
            this.SlaveAddress.Size = new System.Drawing.Size(45, 20);
            this.SlaveAddress.TabIndex = 8;
            this.SlaveAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LinkAddressDesc
            // 
            this.LinkAddressDesc.BackColor = System.Drawing.SystemColors.Control;
            this.LinkAddressDesc.Cursor = System.Windows.Forms.Cursors.Default;
            this.LinkAddressDesc.Location = new System.Drawing.Point(2, 21);
            this.LinkAddressDesc.Name = "LinkAddressDesc";
            this.LinkAddressDesc.Size = new System.Drawing.Size(48, 13);
            this.LinkAddressDesc.TabIndex = 1;
            this.LinkAddressDesc.Text = "Address:";
            this.LinkAddressDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PhysicalAddress
            // 
            this.PhysicalAddress.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PhysicalAddress.Location = new System.Drawing.Point(50, 18);
            this.PhysicalAddress.Name = "PhysicalAddress";
            this.PhysicalAddress.Size = new System.Drawing.Size(132, 20);
            this.PhysicalAddress.TabIndex = 0;
            this.PhysicalAddress.Text = "COM1:9600:8N1";
            this.PhysicalAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LinkReqGapDesc
            // 
            this.LinkReqGapDesc.Location = new System.Drawing.Point(2, 49);
            this.LinkReqGapDesc.Name = "LinkReqGapDesc";
            this.LinkReqGapDesc.Size = new System.Drawing.Size(54, 13);
            this.LinkReqGapDesc.TabIndex = 5;
            this.LinkReqGapDesc.Text = "Req. gap:";
            this.LinkReqGapDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MapBox
            // 
            this.MapBox.Controls.Add(this.MapMoveDown);
            this.MapBox.Controls.Add(this.MapMoveUp);
            this.MapBox.Controls.Add(this.MapSave);
            this.MapBox.Controls.Add(this.MapDel);
            this.MapBox.Controls.Add(this.MapAdd);
            this.MapBox.Controls.Add(this.MapOpen);
            this.MapBox.Location = new System.Drawing.Point(431, 5);
            this.MapBox.Name = "MapBox";
            this.MapBox.Size = new System.Drawing.Size(92, 74);
            this.MapBox.TabIndex = 4;
            this.MapBox.TabStop = false;
            this.MapBox.Text = "Map";
            // 
            // MapMoveDown
            // 
            this.MapMoveDown.Enabled = false;
            this.MapMoveDown.Image = global::UModbus.Properties.Resources.down;
            this.MapMoveDown.Location = new System.Drawing.Point(34, 43);
            this.MapMoveDown.Name = "MapMoveDown";
            this.MapMoveDown.Size = new System.Drawing.Size(24, 24);
            this.MapMoveDown.TabIndex = 10;
            this.MapMoveDown.UseVisualStyleBackColor = true;
            this.MapMoveDown.Click += new System.EventHandler(this.MapMoveDownClick);
            // 
            // MapMoveUp
            // 
            this.MapMoveUp.Enabled = false;
            this.MapMoveUp.Image = global::UModbus.Properties.Resources.up;
            this.MapMoveUp.Location = new System.Drawing.Point(34, 15);
            this.MapMoveUp.Name = "MapMoveUp";
            this.MapMoveUp.Size = new System.Drawing.Size(24, 24);
            this.MapMoveUp.TabIndex = 9;
            this.MapMoveUp.UseVisualStyleBackColor = true;
            this.MapMoveUp.Click += new System.EventHandler(this.MapMoveUpClick);
            // 
            // MapSave
            // 
            this.MapSave.Enabled = false;
            this.MapSave.Image = global::UModbus.Properties.Resources.save;
            this.MapSave.Location = new System.Drawing.Point(62, 43);
            this.MapSave.Name = "MapSave";
            this.MapSave.Size = new System.Drawing.Size(24, 24);
            this.MapSave.TabIndex = 3;
            this.MapSave.UseVisualStyleBackColor = true;
            this.MapSave.Click += new System.EventHandler(this.MapSaveClick);
            // 
            // MapDel
            // 
            this.MapDel.Enabled = false;
            this.MapDel.Image = global::UModbus.Properties.Resources.del;
            this.MapDel.Location = new System.Drawing.Point(6, 43);
            this.MapDel.Name = "MapDel";
            this.MapDel.Size = new System.Drawing.Size(24, 24);
            this.MapDel.TabIndex = 1;
            this.MapDel.UseVisualStyleBackColor = true;
            this.MapDel.Click += new System.EventHandler(this.MapDelClick);
            // 
            // MapAdd
            // 
            this.MapAdd.Image = global::UModbus.Properties.Resources.add;
            this.MapAdd.Location = new System.Drawing.Point(6, 15);
            this.MapAdd.Name = "MapAdd";
            this.MapAdd.Size = new System.Drawing.Size(24, 24);
            this.MapAdd.TabIndex = 0;
            this.MapAdd.UseVisualStyleBackColor = true;
            this.MapAdd.Click += new System.EventHandler(this.MapAddClick);
            // 
            // MapOpen
            // 
            this.MapOpen.Image = global::UModbus.Properties.Resources.open;
            this.MapOpen.Location = new System.Drawing.Point(62, 15);
            this.MapOpen.Name = "MapOpen";
            this.MapOpen.Size = new System.Drawing.Size(24, 24);
            this.MapOpen.TabIndex = 2;
            this.MapOpen.UseVisualStyleBackColor = true;
            this.MapOpen.Click += new System.EventHandler(this.MapOpenClick);
            // 
            // DataMapView
            // 
            this.DataMapView.AllowUserToAddRows = false;
            this.DataMapView.AllowUserToDeleteRows = false;
            this.DataMapView.AllowUserToResizeColumns = false;
            this.DataMapView.AllowUserToResizeRows = false;
            this.DataMapView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataMapView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataMapView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RowEnable,
            this.ItemType,
            this.ItemFormat,
            this.ItemOrder,
            this.ItemAddress,
            this.ItemRaw,
            this.ItemValue,
            this.ItemDescription});
            this.DataMapView.ContextMenuStrip = this.DataMapViewMenu;
            this.DataMapView.Location = new System.Drawing.Point(5, 84);
            this.DataMapView.Name = "DataMapView";
            this.DataMapView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.DataMapView.RowHeadersVisible = false;
            this.DataMapView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataMapView.Size = new System.Drawing.Size(807, 265);
            this.DataMapView.TabIndex = 5;
            this.DataMapView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataMapViewCellEndEdit);
            this.DataMapView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataMapViewSelected);
            // 
            // RowEnable
            // 
            this.RowEnable.HeaderText = "E";
            this.RowEnable.Name = "RowEnable";
            this.RowEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RowEnable.Width = 25;
            // 
            // ItemType
            // 
            this.ItemType.HeaderText = "Type";
            this.ItemType.Items.AddRange(new object[] {
            "Coil",
            "DiscreteInput",
            "HoldingRegister",
            "InputRegister"});
            this.ItemType.Name = "ItemType";
            this.ItemType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ItemType.Width = 105;
            // 
            // ItemFormat
            // 
            this.ItemFormat.HeaderText = "Format";
            this.ItemFormat.Name = "ItemFormat";
            this.ItemFormat.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemFormat.Width = 75;
            // 
            // ItemOrder
            // 
            this.ItemOrder.HeaderText = "Order";
            this.ItemOrder.Name = "ItemOrder";
            this.ItemOrder.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemAddress
            // 
            this.ItemAddress.HeaderText = "Address";
            this.ItemAddress.Name = "ItemAddress";
            this.ItemAddress.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemAddress.Width = 60;
            // 
            // ItemRaw
            // 
            this.ItemRaw.HeaderText = "Raw";
            this.ItemRaw.Name = "ItemRaw";
            this.ItemRaw.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemRaw.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemRaw.Width = 130;
            // 
            // ItemValue
            // 
            this.ItemValue.HeaderText = "Value";
            this.ItemValue.Name = "ItemValue";
            this.ItemValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemValue.Width = 155;
            // 
            // ItemDescription
            // 
            this.ItemDescription.HeaderText = "Description";
            this.ItemDescription.Name = "ItemDescription";
            this.ItemDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemDescription.Width = 150;
            // 
            // DataMapViewMenu
            // 
            this.DataMapViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DataViewEnableAll,
            this.DataViewDisableAll,
            this.DataViewSave});
            this.DataMapViewMenu.Name = "DataMapViewMenu";
            this.DataMapViewMenu.Size = new System.Drawing.Size(130, 70);
            // 
            // DataViewEnableAll
            // 
            this.DataViewEnableAll.Image = global::UModbus.Properties.Resources.pass;
            this.DataViewEnableAll.Name = "DataViewEnableAll";
            this.DataViewEnableAll.Size = new System.Drawing.Size(129, 22);
            this.DataViewEnableAll.Text = "Enable All";
            this.DataViewEnableAll.Click += new System.EventHandler(this.DataViewEnableAllClick);
            // 
            // DataViewDisableAll
            // 
            this.DataViewDisableAll.Image = global::UModbus.Properties.Resources.reject;
            this.DataViewDisableAll.Name = "DataViewDisableAll";
            this.DataViewDisableAll.Size = new System.Drawing.Size(129, 22);
            this.DataViewDisableAll.Text = "Disable All";
            this.DataViewDisableAll.Click += new System.EventHandler(this.DataViewDisableAllClick);
            // 
            // DataViewSave
            // 
            this.DataViewSave.Image = global::UModbus.Properties.Resources.save;
            this.DataViewSave.Name = "DataViewSave";
            this.DataViewSave.Size = new System.Drawing.Size(129, 22);
            this.DataViewSave.Text = "Save";
            this.DataViewSave.Click += new System.EventHandler(this.DataViewSaveClick);
            // 
            // AccessBox
            // 
            this.AccessBox.Controls.Add(this.AccessUserReq);
            this.AccessBox.Controls.Add(this.LogEnable);
            this.AccessBox.Controls.Add(this.CycleRead);
            this.AccessBox.Controls.Add(this.AccessAbort);
            this.AccessBox.Controls.Add(this.AccessWrite);
            this.AccessBox.Controls.Add(this.AccessRead);
            this.AccessBox.Location = new System.Drawing.Point(529, 5);
            this.AccessBox.Name = "AccessBox";
            this.AccessBox.Size = new System.Drawing.Size(154, 74);
            this.AccessBox.TabIndex = 6;
            this.AccessBox.TabStop = false;
            this.AccessBox.Text = "Access";
            // 
            // AccessUserReq
            // 
            this.AccessUserReq.Enabled = false;
            this.AccessUserReq.Location = new System.Drawing.Point(53, 15);
            this.AccessUserReq.Name = "AccessUserReq";
            this.AccessUserReq.Size = new System.Drawing.Size(41, 23);
            this.AccessUserReq.TabIndex = 10;
            this.AccessUserReq.Text = "User";
            this.AccessUserReq.UseVisualStyleBackColor = true;
            this.AccessUserReq.Click += new System.EventHandler(this.DoUserRequestClick);
            // 
            // LogEnable
            // 
            this.LogEnable.AutoSize = true;
            this.LogEnable.Location = new System.Drawing.Point(100, 48);
            this.LogEnable.Name = "LogEnable";
            this.LogEnable.Size = new System.Drawing.Size(44, 17);
            this.LogEnable.TabIndex = 11;
            this.LogEnable.Text = "Log";
            this.LogEnable.UseVisualStyleBackColor = true;
            this.LogEnable.CheckedChanged += new System.EventHandler(this.LogEnableCheckedChanged);
            // 
            // CycleRead
            // 
            this.CycleRead.Location = new System.Drawing.Point(100, 19);
            this.CycleRead.Name = "CycleRead";
            this.CycleRead.Size = new System.Drawing.Size(52, 17);
            this.CycleRead.TabIndex = 10;
            this.CycleRead.Text = "Cycle";
            this.CycleRead.UseVisualStyleBackColor = true;
            this.CycleRead.CheckedChanged += new System.EventHandler(this.CycleReadCheckedChanged);
            // 
            // AccessAbort
            // 
            this.AccessAbort.Enabled = false;
            this.AccessAbort.Location = new System.Drawing.Point(53, 44);
            this.AccessAbort.Name = "AccessAbort";
            this.AccessAbort.Size = new System.Drawing.Size(41, 23);
            this.AccessAbort.TabIndex = 9;
            this.AccessAbort.Text = "Abort";
            this.AccessAbort.UseVisualStyleBackColor = true;
            this.AccessAbort.Click += new System.EventHandler(this.AccessAbortClick);
            // 
            // AccessWrite
            // 
            this.AccessWrite.Enabled = false;
            this.AccessWrite.Location = new System.Drawing.Point(6, 44);
            this.AccessWrite.Name = "AccessWrite";
            this.AccessWrite.Size = new System.Drawing.Size(41, 23);
            this.AccessWrite.TabIndex = 1;
            this.AccessWrite.Text = "Write";
            this.AccessWrite.UseVisualStyleBackColor = true;
            this.AccessWrite.Click += new System.EventHandler(this.AccessWriteClick);
            // 
            // AccessRead
            // 
            this.AccessRead.Enabled = false;
            this.AccessRead.Location = new System.Drawing.Point(6, 15);
            this.AccessRead.Name = "AccessRead";
            this.AccessRead.Size = new System.Drawing.Size(41, 23);
            this.AccessRead.TabIndex = 0;
            this.AccessRead.Text = "Read";
            this.AccessRead.UseVisualStyleBackColor = true;
            this.AccessRead.Click += new System.EventHandler(this.AccessReadClick);
            // 
            // SettingsBox
            // 
            this.SettingsBox.Controls.Add(this.LinkTimeoutDesc);
            this.SettingsBox.Controls.Add(this.Timeout);
            this.SettingsBox.Controls.Add(this.ReqGap);
            this.SettingsBox.Controls.Add(this.LinkReqGapDesc);
            this.SettingsBox.Location = new System.Drawing.Point(316, 5);
            this.SettingsBox.Name = "SettingsBox";
            this.SettingsBox.Size = new System.Drawing.Size(109, 74);
            this.SettingsBox.TabIndex = 7;
            this.SettingsBox.TabStop = false;
            this.SettingsBox.Text = "Settings";
            // 
            // LinkTimeoutDesc
            // 
            this.LinkTimeoutDesc.Location = new System.Drawing.Point(2, 21);
            this.LinkTimeoutDesc.Name = "LinkTimeoutDesc";
            this.LinkTimeoutDesc.Size = new System.Drawing.Size(54, 13);
            this.LinkTimeoutDesc.TabIndex = 9;
            this.LinkTimeoutDesc.Text = "Timeout:";
            this.LinkTimeoutDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Timeout
            // 
            this.Timeout.Location = new System.Drawing.Point(57, 19);
            this.Timeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Timeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Timeout.Name = "Timeout";
            this.Timeout.Size = new System.Drawing.Size(45, 20);
            this.Timeout.TabIndex = 9;
            this.Timeout.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // ReqGap
            // 
            this.ReqGap.Location = new System.Drawing.Point(57, 47);
            this.ReqGap.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ReqGap.Name = "ReqGap";
            this.ReqGap.Size = new System.Drawing.Size(45, 20);
            this.ReqGap.TabIndex = 8;
            this.ReqGap.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ReqGap.ValueChanged += new System.EventHandler(this.ReqGapValueChanged);
            // 
            // StatusBox
            // 
            this.StatusBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMode,
            this.StatusSep1,
            this.StatusPhysical,
            this.StatusSep2,
            this.StatusConnection,
            this.StatusSep3,
            this.StatusCurrentRow,
            this.StatusSep4,
            this.StatusSelRows,
            this.StatusSep5,
            this.StatusReqError,
            this.StatusSep6,
            this.AccessProgress});
            this.StatusBox.Location = new System.Drawing.Point(0, 353);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(817, 22);
            this.StatusBox.SizingGrip = false;
            this.StatusBox.TabIndex = 8;
            this.StatusBox.Text = "Status";
            // 
            // StatusMode
            // 
            this.StatusMode.Name = "StatusMode";
            this.StatusMode.Size = new System.Drawing.Size(27, 17);
            this.StatusMode.Text = "RTU";
            // 
            // StatusSep1
            // 
            this.StatusSep1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.StatusSep1.ForeColor = System.Drawing.Color.DarkGray;
            this.StatusSep1.Name = "StatusSep1";
            this.StatusSep1.Size = new System.Drawing.Size(16, 17);
            this.StatusSep1.Text = "|";
            // 
            // StatusPhysical
            // 
            this.StatusPhysical.Name = "StatusPhysical";
            this.StatusPhysical.Size = new System.Drawing.Size(92, 17);
            this.StatusPhysical.Text = "COM1:9600:8N1";
            // 
            // StatusSep2
            // 
            this.StatusSep2.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.StatusSep2.ForeColor = System.Drawing.Color.DarkGray;
            this.StatusSep2.Name = "StatusSep2";
            this.StatusSep2.Size = new System.Drawing.Size(16, 17);
            this.StatusSep2.Text = "|";
            // 
            // StatusConnection
            // 
            this.StatusConnection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.StatusConnection.Name = "StatusConnection";
            this.StatusConnection.Size = new System.Drawing.Size(83, 17);
            this.StatusConnection.Text = "Disconnected";
            // 
            // StatusSep3
            // 
            this.StatusSep3.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.StatusSep3.ForeColor = System.Drawing.Color.DarkGray;
            this.StatusSep3.Name = "StatusSep3";
            this.StatusSep3.Size = new System.Drawing.Size(16, 17);
            this.StatusSep3.Text = "|";
            // 
            // StatusCurrentRow
            // 
            this.StatusCurrentRow.Name = "StatusCurrentRow";
            this.StatusCurrentRow.Size = new System.Drawing.Size(65, 17);
            this.StatusCurrentRow.Text = "Row: None";
            // 
            // StatusSep4
            // 
            this.StatusSep4.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.StatusSep4.ForeColor = System.Drawing.Color.DarkGray;
            this.StatusSep4.Name = "StatusSep4";
            this.StatusSep4.Size = new System.Drawing.Size(16, 17);
            this.StatusSep4.Text = "|";
            // 
            // StatusSelRows
            // 
            this.StatusSelRows.Name = "StatusSelRows";
            this.StatusSelRows.Size = new System.Drawing.Size(60, 17);
            this.StatusSelRows.Text = "Sel.: None";
            // 
            // StatusSep5
            // 
            this.StatusSep5.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.StatusSep5.ForeColor = System.Drawing.Color.DarkGray;
            this.StatusSep5.Name = "StatusSep5";
            this.StatusSep5.Size = new System.Drawing.Size(16, 17);
            this.StatusSep5.Text = "|";
            // 
            // StatusReqError
            // 
            this.StatusReqError.Name = "StatusReqError";
            this.StatusReqError.Size = new System.Drawing.Size(72, 17);
            this.StatusReqError.Text = "Errors: None";
            // 
            // StatusSep6
            // 
            this.StatusSep6.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.StatusSep6.ForeColor = System.Drawing.Color.DarkGray;
            this.StatusSep6.Name = "StatusSep6";
            this.StatusSep6.Size = new System.Drawing.Size(16, 17);
            this.StatusSep6.Text = "|";
            // 
            // AccessProgress
            // 
            this.AccessProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.AccessProgress.AutoSize = false;
            this.AccessProgress.MarqueeAnimationSpeed = 10;
            this.AccessProgress.Name = "AccessProgress";
            this.AccessProgress.Size = new System.Drawing.Size(180, 16);
            this.AccessProgress.Step = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 375);
            this.Controls.Add(this.StatusBox);
            this.Controls.Add(this.SettingsBox);
            this.Controls.Add(this.AccessBox);
            this.Controls.Add(this.DataMapView);
            this.Controls.Add(this.MapBox);
            this.Controls.Add(this.LinkBox);
            this.Controls.Add(this.ModeBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(704, 414);
            this.Name = "MainForm";
            this.Text = "UModbus";
            this.ModeBox.ResumeLayout(false);
            this.LinkBox.ResumeLayout(false);
            this.LinkBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SlaveAddress)).EndInit();
            this.MapBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataMapView)).EndInit();
            this.DataMapViewMenu.ResumeLayout(false);
            this.AccessBox.ResumeLayout(false);
            this.AccessBox.PerformLayout();
            this.SettingsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Timeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReqGap)).EndInit();
            this.StatusBox.ResumeLayout(false);
            this.StatusBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox ModeBox;
        private System.Windows.Forms.RadioButton ModeUdp;
        private System.Windows.Forms.RadioButton ModeTcp;
        private System.Windows.Forms.RadioButton ModeAscii;
        private System.Windows.Forms.RadioButton ModeRtu;
        private System.Windows.Forms.GroupBox LinkBox;
        private System.Windows.Forms.Label LinkAddressDesc;
        private System.Windows.Forms.TextBox PhysicalAddress;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Label LinkReqGapDesc;
        private System.Windows.Forms.GroupBox MapBox;
        private System.Windows.Forms.Button MapSave;
        private System.Windows.Forms.Button MapOpen;
        private System.Windows.Forms.Button MapDel;
        private System.Windows.Forms.Button MapAdd;
        private System.Windows.Forms.DataGridView DataMapView;
        private System.Windows.Forms.GroupBox AccessBox;
        private System.Windows.Forms.Button AccessWrite;
        private System.Windows.Forms.Button AccessRead;
        private System.Windows.Forms.GroupBox SettingsBox;
        private System.Windows.Forms.NumericUpDown ReqGap;
        private System.Windows.Forms.NumericUpDown SlaveAddress;
        private System.Windows.Forms.StatusStrip StatusBox;
        private System.Windows.Forms.ToolStripStatusLabel StatusMode;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep1;
        private System.Windows.Forms.ToolStripStatusLabel StatusPhysical;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep2;
        private System.Windows.Forms.ToolStripStatusLabel StatusConnection;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep3;
        private System.Windows.Forms.ToolStripStatusLabel StatusSelRows;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep4;
        private System.Windows.Forms.ToolStripStatusLabel StatusReqError;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep6;
        private System.Windows.Forms.ToolStripProgressBar AccessProgress;
        private System.Windows.Forms.ToolStripStatusLabel StatusCurrentRow;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep5;
        private System.Windows.Forms.Button AccessAbort;
        private System.Windows.Forms.Label LinkTimeoutDesc;
        private System.Windows.Forms.NumericUpDown Timeout;
        private System.Windows.Forms.CheckBox CycleRead;
        private System.Windows.Forms.CheckBox LogEnable;
        private System.Windows.Forms.Button AccessUserReq;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RowEnable;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemFormat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemRaw;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDescription;
        private System.Windows.Forms.ContextMenuStrip DataMapViewMenu;
        private System.Windows.Forms.ToolStripMenuItem DataViewEnableAll;
        private System.Windows.Forms.ToolStripMenuItem DataViewDisableAll;
        private System.Windows.Forms.ToolStripMenuItem DataViewSave;
        private System.Windows.Forms.LinkLabel SlaveScan;
        private System.Windows.Forms.Label SlaveAdrDesc;
        private System.Windows.Forms.Button MapMoveDown;
        private System.Windows.Forms.Button MapMoveUp;
    }
}

