using System;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Modbus;


namespace UModbus
{
    public partial class SlaveScanForm : Form
    {
        #region Fields
        private ModbusClient Client;
        private bool WaitResponse = false;
        private bool ClosingFlag  = false;
        private byte function;
        private byte[] param;
        private byte StartAddress;
        private byte StopAddress;
        private bool Aborting;
        #endregion

        #region Form
        public SlaveScanForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!WaitResponse)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                ClosingFlag = true;
            }
        }

        public void Show(ModbusClient client)
        {
            Client = client;
            ShowDialog();
        }
        #endregion

        #region Tasks
        private void ScanTask()
        {
            WaitResponse = true;

            for (byte address = StartAddress; address <= StopAddress; address++)
            {
                Client.SlaveAddress = address;
                ByteResponse resp   = Client.UserRequest(function, param);

                Color  color = resp.Status == RequestStatus.Valid                 ? Color.Green  : 
                              (resp.Status <  RequestStatus.ResponseLengthInvalid ? Color.Orange : Color.Red);
                string adres = address.ToString();
                string text  = resp.Status == RequestStatus.Valid ? LogBox.BytesToHexString(resp.Data) : resp.Status.ToString();

                int progress = Convert.ToInt32(100.0*(address-StartAddress+1)/(StopAddress-StartAddress+1));

                Invoke((MethodInvoker)delegate
                {
                    ScanLog.Append(adres, Color.DarkMagenta, false, false);
                    ScanLog.Append(" - ", Color.Black, false, false);
                    ScanLog.Append(text, color, false);

                    ScanProgress.Value = progress;
                });

                if (Aborting)
                {
                    break;
                }

                Thread.Sleep(100);
            }

            Invoke((MethodInvoker)delegate
            {
                ScanAbort.Enabled = false;
                ScanStart.Enabled = true;
            });

            WaitResponse = false;

            if (ClosingFlag)
            {
                ClosingFlag = false;

                Invoke((MethodInvoker)delegate
                {
                    Hide();
                });
            }
        }
        #endregion

        #region Handlers
        private void ScanStartClick(object sender, EventArgs e)
        {
            int func  = TestRequest.Function;
            param     = TestRequest.Parameters;

            if (func > -1 && param != null)
            {
                function = (byte)func;

                StartAddress = Convert.ToByte(BeginAddress.Value);
                StopAddress  = Convert.ToByte(EndAddress.Value);

                if (StopAddress > StartAddress)
                {
                    Aborting = false;

                    ScanStart.Enabled = false;
                    ScanAbort.Enabled = true;

                    ScanProgress.Value = 0;

                    Task.Factory.StartNew(ScanTask);
                }
                else
                {
                    MessageBox.Show("begin address less or equal end address!", "Bus Scan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ScanAbortClick(object sender, EventArgs e)
        {
            ScanAbort.Enabled = false;
            Aborting          = true;
        }

        private void ScanCloseClick(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}