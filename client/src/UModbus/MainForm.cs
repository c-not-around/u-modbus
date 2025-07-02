using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using UModbus.Client;


namespace UModbus
{
    public partial class MainForm : Form
    {
        #region Fields
        private List<ModbusData> Map;
        private ModbusClient     Client;
        private bool             Connected        = false;
        private bool             AccessInProgress = false;
        private bool             AccessAborting   = false;
        private int              RequestGap       = 100;
        private bool             ContinousRead    = false;
        private bool             ClosingFlag      = false;
        private string           LogFileName      = "";
        private UserReqForm      UserRequestForm;
        private SlaveScanForm    ScanForm;
        #endregion

        #region Columns
        private const int ENABLE  = 0;
        private const int TYPE    = 1;
        private const int FORMAT  = 2;
        private const int ORDER   = 3;
        private const int ADDRESS = 4;
        private const int RAW     = 5;
        private const int VALUE   = 6;
        private const int DESC    = 7;
        #endregion

        #region Form
        public MainForm()
        {
            InitializeComponent();

            UserRequestForm = new UserReqForm();
            ScanForm        = new SlaveScanForm();

            for (int i = 0; i < DESC; i++)
            {
                DataMapView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            for (int i = ORDER; i <= VALUE; i++)
            {
                var style       = DataMapView.Columns[i].DefaultCellStyle;
                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                style.Font      = new Font("Consolas", i == ORDER ? 8 : 9, FontStyle.Regular, GraphicsUnit.Point);
            }

            DataMapView.Columns[DESC].Resizable = DataGridViewTriState.True;

            DataMapViewMenu.Enabled = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (AccessInProgress)
            {
                AccessAbort.PerformClick();

                ClosingFlag = 
                e.Cancel    = true;
            }
            else if (Client != null && Client.IsConnected)
            {
                Client.Disconnect();
            }

            base.OnFormClosing(e);
        }
        #endregion

        #region Mode
        private enum Mode { NONE, RTU, ASCII, TCP, UDP };

        private Mode GetMode()
        {
            foreach (var m in ModeBox.Controls)
            {
                RadioButton mode = m as RadioButton;

                if (mode.Checked)
                {
                    return (Mode)Convert.ToInt32(mode.Tag);
                }
            }

            return Mode.NONE;
        }
        #endregion

        #region Manage
        private void ManageAbility()
        {
            ModeBox.Enabled       = !Connected;

            Connect.Enabled       = !AccessInProgress;
            Connect.Text          = Connected ? "Disconnect" : "Connect";

            SlaveScan.Visible     = Connected && !AccessInProgress;
            SlaveAdrDesc.Visible  = 
            SlaveAddress.Enabled  = !SlaveScan.Visible;

            MapBox.Enabled        = !AccessInProgress;
            MapSave.Enabled       =
            MapDel.Enabled        = DataMapView.RowCount > 0;
            MapMoveUp.Enabled     = DataMapView.RowCount > 1 && DataMapView.CurrentCellAddress.Y > 0;
            MapMoveDown.Enabled   = DataMapView.RowCount > 1 && DataMapView.CurrentCellAddress.Y < (DataMapView.RowCount-1);

            AccessRead.Enabled    =
            AccessWrite.Enabled   = Connected && !AccessInProgress && DataMapView.RowCount > 0;
            AccessUserReq.Enabled = Connected && !AccessInProgress;
            AccessAbort.Enabled   = Connected && AccessInProgress;

            DataMapView.ReadOnly  = AccessInProgress;
            DataMapView.Columns[RAW].ReadOnly = true;

            DataMapViewMenu.Enabled = Map != null && Map.Count > 0;

            if (Map != null)
            {
                for (int row = 0; row < Map.Count; row++)
                {
                    if (!Map[row].IsWord())
                    {
                        DataMapView[ORDER, row].ReadOnly = true;
                    }
                }
            }

            if (ClosingFlag)
            {
                Close();
            }
        }

        private void StatusUpdate()
        {
            Mode mode       = GetMode();
            StatusMode.Text = mode.ToString();

            if (mode < Mode.TCP)
            {
                ModbusSerialClient mbsc = Client as ModbusSerialClient;
                StatusPhysical.Text     = $"{mbsc.Port}:{mbsc.Baudrate}:8{mbsc.Parity.ToString()[0]}{(int)mbsc.StopBits}";
            }
            else
            {
                ModbusIpClient mbip = Client as ModbusIpClient;
                StatusPhysical.Text = $"{mbip.IpAddress}:{mbip.Port}";
            }

            StatusConnection.Text = Connected ? "Connected" : "Disconnected";
        }

        private void ManageSelectedRows()
        {
            int sel = Map.Sum(row => row.Enable ? 1 : 0);

            StatusSelRows.Text = $"Sel.: {sel}/{Map.Count}";

            DataViewEnableAll.Enabled  = sel < Map.Count;
            DataViewDisableAll.Enabled = sel > 0;
        }
        #endregion

        #region Routines
        private void DataViewSetEnableAll(bool enable)
        {
            for (int row = 0; row < Map.Count; row++)
            {
                Map[row].Enable = enable;
                DataGridViewCheckBoxCell EnableBox = DataMapView[ENABLE, row] as DataGridViewCheckBoxCell;
                EnableBox.Value = enable;
            }

            ManageSelectedRows();
        }
        #endregion

        #region Map<->Grid
        private void DataMapRowUpdate(int row)
        {
            if (row >= DataMapView.RowCount)
            {
                DataMapView.Rows.Add();
            }

            DataGridViewCheckBoxCell EnableBox  = DataMapView[ENABLE, row]  as DataGridViewCheckBoxCell;
            DataGridViewComboBoxCell TypeBox    = DataMapView[TYPE, row]    as DataGridViewComboBoxCell;
            DataGridViewComboBoxCell FormatBox  = DataMapView[FORMAT, row]  as DataGridViewComboBoxCell;
            DataGridViewTextBoxCell  OrderBox   = DataMapView[ORDER, row]   as DataGridViewTextBoxCell;
            DataGridViewTextBoxCell  AddressBox = DataMapView[ADDRESS, row] as DataGridViewTextBoxCell;
            DataGridViewTextBoxCell  RawBox     = DataMapView[RAW, row]     as DataGridViewTextBoxCell;
            DataGridViewTextBoxCell  ValueBox   = DataMapView[VALUE, row]   as DataGridViewTextBoxCell;
            DataGridViewTextBoxCell  DescBox    = DataMapView[DESC, row]    as DataGridViewTextBoxCell;

            EnableBox.Value = Map[row].Enable;
            TypeBox.Value   = Map[row].Type.ToString();
            FormatBox.Value = null;
            FormatBox.Items.Clear();
            if (Map[row].IsWord())
            {
                FormatBox.Items.AddRange
                (
                    ModbusData.ModbusDataFormat.Uint16.ToString(),
                    ModbusData.ModbusDataFormat.Int16.ToString(),
                    ModbusData.ModbusDataFormat.Uint32.ToString(),
                    ModbusData.ModbusDataFormat.Int32.ToString(),
                    ModbusData.ModbusDataFormat.Uint64.ToString(),
                    ModbusData.ModbusDataFormat.Int64.ToString(),
                    ModbusData.ModbusDataFormat.Float32.ToString(),
                    ModbusData.ModbusDataFormat.Float64.ToString()
                );
            }
            else
            {
                FormatBox.Items.AddRange
                (
                    ModbusData.ModbusDataFormat.Digital.ToString(),
                    ModbusData.ModbusDataFormat.Logic.ToString(),
                    ModbusData.ModbusDataFormat.Level.ToString()
                );
            }
            FormatBox.Value  = Map[row].Format.ToString();
            OrderBox.Value   = Map[row].OrderToString();
            AddressBox.Value = Map[row].Address.ToString();
            RawBox.Value     = Map[row].RawToString();
            ValueBox.Value   = Map[row].ValueToString();
            DescBox.Value    = Map[row].Description;

            OrderBox.ReadOnly = !Map[row].IsWord();
            ValueBox.ReadOnly = !Map[row].IsWriteable();

            using (var desc = DataMapView.Columns[DESC])
            {
                desc.Width = desc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, false);
            }
        }

        private int DataMapSetValue(int row, RequestStatus status, params byte[] raw)
        {
            int result = 0;

            if (status == RequestStatus.Valid)
            {
                Map[row].SetValue(raw);
            }

            Invoke((MethodInvoker)delegate
            {
                DataGridViewTextBoxCell RawBox   = DataMapView[RAW, row]   as DataGridViewTextBoxCell;
                DataGridViewTextBoxCell ValueBox = DataMapView[VALUE, row] as DataGridViewTextBoxCell;

                if (status == RequestStatus.Valid)
                {
                    RawBox.Style.ForeColor = Color.Green;
                    RawBox.Value           = Map[row].RawToString();
                    ValueBox.Value         = Map[row].ValueToString();
                }
                else
                {
                    RawBox.Style.ForeColor = Color.Red;
                    RawBox.Value           = status.ToString();
                    ValueBox.Value         = "";

                    result = 1;
                }
            });

            return result;
        }

        private int DataMapSetWriteResult(int row, RequestStatus status, int recorded)
        {
            int result = 0;

            int len = Map[row].IsWord() ? Map[row].Length() / 2 : 1;
            DataGridViewTextBoxCell RawBox = DataMapView[RAW, row] as DataGridViewTextBoxCell;

            Invoke((MethodInvoker)delegate
            {
                if (status == RequestStatus.Valid && recorded == len)
                {
                    RawBox.Style.ForeColor = Color.Green;
                }
                else
                {
                    RawBox.Style.ForeColor = Color.Red;
                    RawBox.Value = status != RequestStatus.Valid ? status.ToString() : $"recorded {recorded}/{len}";

                    result = 1;
                }
            });

            return result;
        }

        private ModbusData.ModbusDataType DataMapViewRowGetType(int row)
        {
            DataGridViewComboBoxCell TypeBox = DataMapView[TYPE, row] as DataGridViewComboBoxCell;
            return (ModbusData.ModbusDataType)Enum.Parse(typeof(ModbusData.ModbusDataType), TypeBox.Value.ToString());
        }

        private ModbusData.ModbusDataFormat DataMapViewRowGetFormat(int row)
        {
            DataGridViewComboBoxCell FormatBox = DataMapView[FORMAT, row] as DataGridViewComboBoxCell;
            return (ModbusData.ModbusDataFormat)Enum.Parse(typeof(ModbusData.ModbusDataFormat), FormatBox.Value.ToString());
        }

        private int[] DataMapViewRowGetByteOrder(int row)
        {
            DataGridViewTextBoxCell OrderBox = DataMapView[ORDER, row] as DataGridViewTextBoxCell;

            if (OrderBox.Value != null)
            { 
                int    bytes = Map[row].Length();
                string image = OrderBox.Value.ToString();
                string regex = "^(\\d-){" + (bytes-1).ToString() + "}\\d$";

                if (Regex.IsMatch(image, regex))
                {
                    int[]    order = new int[bytes];
                    string[] parts = image.Split('-');

                    for (int i = 0; i < bytes; i++)
                    {
                        order[i] = Convert.ToInt32(parts[i]);
                    }

                    return order;
                }
            }

            return null;
        }

        private int DataMapViewRowGetAddress(int row)
        {
            DataGridViewTextBoxCell AddressBox = DataMapView[ADDRESS, row] as DataGridViewTextBoxCell;

            if (AddressBox.Value != null)
            {
                string image = AddressBox.Value.ToString();

                if (Regex.IsMatch(image, "^\\d{1,5}$"))
                {
                    return Convert.ToInt32(image);
                }

                if (Regex.IsMatch(image, "^0x[0-9A-F]{1,4}$"))
                {
                    return Convert.ToInt32(image, 16);
                }
            }

            return -1;
        }
        #endregion

        #region Tasks
        private void ConnectTask()
        {
            Connected = Client.Connect();

            Invoke((MethodInvoker)delegate
            {
                StatusUpdate();
                ManageAbility();
            });

            if (!Connected)
            {
                MessageBox.Show($"Cant establish connection to {StatusPhysical.Text}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadTask()
        {
            int ReadCount = Map.Sum(d => d.Enable ? 1 : 0);

            do
            {
                Invoke((MethodInvoker)delegate
                {
                    AccessProgress.Value = 0;

                    for (int row = 0; row < Map.Count; row++)
                    {
                        if (Map[row].Enable)
                        {
                            DataMapView[RAW, row].Style.ForeColor = Color.Black;
                        }    
                    }
                });

                int ReadsCount = 0;
                int ErrorCount = 0;

                for (int row = 0; row < Map.Count; row++)
                {
                    if (Map[row].Enable)
                    {
                        switch (Map[row].Type)
                        {
                            case ModbusData.ModbusDataType.Coil:
                            {
                                var resp = Client.ReadCoils(Map[row].Address, 1);
                                ErrorCount += DataMapSetValue(row, resp.Status, resp.Raw);
                            }
                            break;

                            case ModbusData.ModbusDataType.DiscreteInput:
                            {
                                var resp = Client.ReadDiscreteInputs(Map[row].Address, 1);
                                ErrorCount += DataMapSetValue(row, resp.Status, resp.Raw);
                            }
                            break;

                            case ModbusData.ModbusDataType.HoldingRegister:
                            {
                                var resp = Client.ReadHoldingRegisters(Map[row].Address, (ushort)(Map[row].Length() / 2));
                                ErrorCount += DataMapSetValue(row, resp.Status, resp.Raw);
                            }
                            break;

                            case ModbusData.ModbusDataType.InputRegister:
                            {
                                var resp = Client.ReadInputRegisters(Map[row].Address, (ushort)(Map[row].Length() / 2));
                                ErrorCount += DataMapSetValue(row, resp.Status, resp.Raw);
                            }
                            break;
                        }

                        ReadsCount++;

                        Invoke((MethodInvoker)delegate
                        {
                            AccessProgress.Value = Convert.ToInt32(100.0 * ReadsCount / ReadCount);
                            StatusReqError.Text  = $"Errors: {ErrorCount}/{ReadCount}";
                        });

                        if (RequestGap > 0)
                        {
                            Thread.Sleep(RequestGap);
                        }

                        if (AccessAborting)
                        {
                            break;
                        }
                    }
                }
            } while (ContinousRead && !AccessAborting);

            AccessInProgress = false;
            AccessAborting   = false;
            Invoke((MethodInvoker)delegate
            {
                ManageAbility();
            });
        }

        private void WriteTask()
        {
            int WriteCount   = Map.Sum(d => d.Enable && d.IsWriteable() ? 1 : 0);
            int WrittenCount = 0;
            int ErrorCount   = 0;

            for (int row = 0; row < Map.Count; row++)
            {
                if (Map[row].IsWriteable() && Map[row].Enable)
                {
                    if (Map[row].Type == ModbusData.ModbusDataType.Coil)
                    {
                        var resp = Client.WriteSingleCoil(Map[row].Address, Map[row].GetRawValue()[0] == 1);
                        ErrorCount += DataMapSetWriteResult(row, resp.Status, resp.Recorded);
                    }
                    else if (Map[row].Type == ModbusData.ModbusDataType.HoldingRegister)
                    {
                        byte[]   data = Map[row].GetRawValue();
                        ushort[] regs = new ushort[Map[row].Length()/2];

                        for (int i = 0; i < regs.Length; i++)
                        {
                            regs[i] = (ushort)((data[2*i+1] << 8) | data[2*i+0]);
                        }

                        var resp = Client.WriteMultipleRegisters(Map[row].Address, regs);
                        ErrorCount += DataMapSetWriteResult(row, resp.Status, resp.Recorded);
                    }

                    WrittenCount++;

                    Invoke((MethodInvoker)delegate
                    {
                        AccessProgress.Value = Convert.ToInt32(100.0 * WrittenCount / WriteCount);
                        StatusReqError.Text  = $"Errors: {ErrorCount}/{WriteCount}";
                    });

                    if (RequestGap > 0)
                    {
                        Thread.Sleep(RequestGap);
                    }

                    if (AccessAborting)
                    {
                        break;
                    }
                }   
            }

            AccessInProgress = false;
            AccessAborting   = false;
            Invoke((MethodInvoker)delegate
            {
                ManageAbility();
            });
        }
        #endregion

        #region Handlers
        private void ModeChanged(object sender, EventArgs e)
        {
            PhysicalAddress.Text = GetMode() < Mode.TCP ? "COM1:9600:8N1" : "127.0.0.1:502";
        }

        private void SlaveScanClick(object sender, EventArgs e) => ScanForm.Show(Client);

        private void ConnectClick(object sender, EventArgs e)
        {
            if (Connect.Text == "Connect")
            {
                Mode   mode    = GetMode();
                string address = PhysicalAddress.Text;

                switch (mode)
                {
                    case Mode.RTU:   Client = new ModbusRtuClient(address);   break;
                    case Mode.ASCII: Client = new ModbusAsciiClient(address); break;
                    case Mode.TCP:   Client = new ModbusTcpClient(address);   break;
                    case Mode.UDP:   Client = new ModbusUdpClient(address);   break;
                }

                Client.SlaveAddress  = Convert.ToByte(SlaveAddress.Value);
                Client.Timeout       = Convert.ToInt32(Timeout.Value);
                Client.LoggingEnable = LogEnable.Checked;
                Client.LogFileName   = LogFileName;

                Task.Factory.StartNew(ConnectTask);
            }
            else if (Connect.Text == "Disconnect")
            {
                Client.Disconnect();
                Connected = false;
                StatusUpdate();
                ManageAbility();
            }
        }

        private void ReqGapValueChanged(object sender, EventArgs e) => RequestGap = Convert.ToInt32(ReqGap.Value);

        private void MapAddClick(object sender, EventArgs e)
        {
            if (Map == null)
            {
                Map = new List<ModbusData>();
            }

            int row = Map.Count == 0 ? 0 : DataMapView.CurrentRow.Index;

            if (Map.Count == 0)
            {
                Map.Add(ModbusData.Default(0));
            }
            else
            {
                ModbusData line = Map[row].Clone(true);

                if (row == (Map.Count - 1))
                {
                    Map.Add(line);
                    row++;
                }
                else
                {
                    Map.Insert(row+1, line);
                    DataMapView.Rows.Insert(++row, 1);
                }
            }
            DataMapRowUpdate(row);

            DataMapView.CurrentCell = DataMapView[ADDRESS, row];

            ManageAbility();
            ManageSelectedRows();
        }

        private void MapDelClick(object sender, EventArgs e)
        {
            int row = DataMapView.CurrentRow.Index;

            Map.RemoveAt(row);
            DataMapView.Rows.RemoveAt(row);

            ManageAbility();

            if (DataMapView.Rows.Count == 0)
            {
                StatusCurrentRow.Text = "Row: None";
                StatusSelRows.Text    = "Sel.: None";
            }
            else
            {
                ManageSelectedRows();
            }
        }

        private void MapMoveUpClick(object sender, EventArgs e)
        {
            int row = DataMapView.CurrentRow.Index;

            if (row > 0)
            {
                ModbusData line = Map[row];
                Map[row-0] = Map[row-1];
                Map[row-1] = line;
                DataMapRowUpdate(row-0);
                DataMapRowUpdate(row-1);

                Point p = DataMapView.CurrentCellAddress;
                DataMapView.CurrentCell = DataMapView[p.X, p.Y-1];
            }
        }

        private void MapMoveDownClick(object sender, EventArgs e)
        {
            int row = DataMapView.CurrentRow.Index;

            if ((row+1) < Map.Count)
            {
                ModbusData line = Map[row+1];
                Map[row+1] = Map[row+0];
                Map[row+0] = line;
                DataMapRowUpdate(row+0);
                DataMapRowUpdate(row+1);

                Point p = DataMapView.CurrentCellAddress;
                DataMapView.CurrentCell = DataMapView[p.X, p.Y+1];
            }
        }

        private void MapOpenClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "UModbus DataMap file (*.json)|*.json";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Map = new List<ModbusData>(ModbusData.OpenDataMap(dialog.FileName));

                    DataMapView.Rows.Clear();

                    for (int row = 0; row < Map.Count; row++)
                    {
                        DataMapRowUpdate(row);
                    }

                    PrevSelRow = -1;
                    DataMapView.CurrentCell = DataMapView[ADDRESS, DataMapView.Rows.Count-1];

                    ManageAbility();
                    ManageSelectedRows();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Read file \"{dialog.FileName}\" error: {ex.Message}", "DataMap file open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MapSaveClick(object sender, EventArgs e)
        {
            if (Map.Count > 0)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "UModbus DataMap file (*.json)|*.json";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ModbusData.SaveDataMap(dialog.FileName, Map.ToArray());
                }
            }
            else
            {
                MessageBox.Show("No DataMap fields to save.", "DataMap save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CycleReadCheckedChanged(object sender, EventArgs e) => ContinousRead = CycleRead.Checked;

        private void LogEnableCheckedChanged(object sender, EventArgs e)
        {
            LogFileName = LogEnable.Checked ? DateTime.Now.ToString("yyyyMMdd_HHmmss.lo\\g") : "";

            if (Client != null)
            {
                Client.LoggingEnable = LogEnable.Checked;
                Client.LogFileName   = LogFileName;
            }
        }

        private void AccessReadClick(object sender, EventArgs e)
        {
            if (Map.Sum(row => row.Enable ? 1 : 0) > 0)
            {
                AccessInProgress = true;
                ManageAbility();

                Task.Factory.StartNew(ReadTask);
            }
            else
            {
                MessageBox.Show("No rows are selected for reading.", "Read", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AccessWriteClick(object sender, EventArgs e)
        {
            if (Map.Sum(row => row.IsWriteable() && row.Enable ? 1 : 0) > 0)
            {
                AccessInProgress = true;
                ManageAbility();

                AccessProgress.Value = 0;
                for (int row = 0; row < Map.Count; row++)
                {
                    if (Map[row].IsWriteable() && Map[row].Enable)
                    {
                        DataMapView[RAW, row].Style.ForeColor = Color.Black;
                    }
                }

                Task.Factory.StartNew(WriteTask);
            }
            else
            {
                MessageBox.Show("No writable rows are selected.", "Write", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AccessAbortClick(object sender, EventArgs e)
        {
            AccessAbort.Enabled = false;
            AccessAborting      = true;
            ContinousRead       = false;
        }

        private void DoUserRequestClick(object sender, EventArgs e) => UserRequestForm.Show(Client);

        private void DataMapViewSelected(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            if (row == PrevSelRow) // crutch for line move down
            {
                return; // last move step incorrect event e.g.(N=6): 0->1->2->3->4->5->4
            }

            StatusCurrentRow.Text = $"Row: {row+1}/{DataMapView.Rows.Count}";

            MapMoveUp.Enabled   = row > 0;
            MapMoveDown.Enabled = row < (DataMapView.Rows.Count-1);

            PrevSelRow = row;
        } private int PrevSelRow = -1;

        private void DataMapViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            switch (e.ColumnIndex)
            {
                case ENABLE:
                {
                    DataGridViewCheckBoxCell EnableBox = DataMapView[ENABLE, row] as DataGridViewCheckBoxCell;
                    bool enable = Convert.ToBoolean(EnableBox.Value);
                    Map[row].Enable = enable;

                    ManageSelectedRows();
                } break;

                case TYPE:
                {
                    bool PrevTypeIsWord = Map[row].IsWord();

                    Map[row].Type = DataMapViewRowGetType(row);

                    if (PrevTypeIsWord ^ Map[row].IsWord())
                    {
                        Map[row].Format    = Map[row].DefaultFormat();
                        Map[row].ByteOrder = Map[row].DefaultByteOrder();
                    }
                } break;

                case FORMAT:
                {
                    Map[row].Format = DataMapViewRowGetFormat(row);
                } break;

                case ORDER:
                {
                    int[] order = DataMapViewRowGetByteOrder(row);

                    if (Map[row].CheckByteOrder(order))
                    {
                        Map[row].ByteOrder = order;
                    }
                } break;

                case ADDRESS:
                {
                    int address = DataMapViewRowGetAddress(row);

                    if (-1 < address && address < 65536)
                    {
                        Map[row].Address = (ushort)address;
                    }
                } break;

                case VALUE:
                {
                    DataGridViewTextBoxCell RawBox   = DataMapView[RAW, row]   as DataGridViewTextBoxCell;
                    DataGridViewTextBoxCell ValueBox = DataMapView[VALUE, row] as DataGridViewTextBoxCell;

                    if (ValueBox.Value != null)
                    {
                        RawBox.Style.ForeColor = Color.Black;
                        Map[row].SetValue(ValueBox.Value.ToString());
                    }
                } break;

                case DESC:
                {
                    DataGridViewTextBoxCell DescBox = DataMapView[DESC, row] as DataGridViewTextBoxCell;

                    if (DescBox.Value != null)
                    {
                        Map[row].Description = DescBox.Value.ToString();
                    }
                } break;

                default: return;
            }

            DataMapRowUpdate(row);
        }
        
        private void DataViewEnableAllClick(object sender, EventArgs e) => DataViewSetEnableAll(true);
        
        private void DataViewDisableAllClick(object sender, EventArgs e) => DataViewSetEnableAll(false);

        private void DataViewSaveClick(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Comma-separated file (*.csv)|*.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (var writer = File.AppendText(dialog.FileName))
                {
                    for (int row = 0; row < Map.Count; row++)
                    {
                        writer.WriteLine($"{Map[row].Type};{Map[row].Format};{Map[row].Address:X4};{Map[row].ValueToString()}\r\n");
                    }

                    writer.Dispose();
                }
            }
        }
        #endregion
    }
}