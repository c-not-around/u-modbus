using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using UModbus.Client;


namespace UModbus
{
    public partial class UserReqForm : Form
    {
        #region Fields
        private ModbusClient Client;
        private bool         WaitResponse = false;
        private bool         ClosingFlag  = false;
        private byte         function;
        private byte[]       parameters;
        #endregion

        #region Form
        public UserReqForm()
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

        #region Utils
        private string DataToLog(byte[] data) => Client.SlaveAddress.ToString("X2") + " " +
                                                 function.ToString("X2") + " " +
                                                 LogBox.BytesToHexString(data);
        #endregion

        #region Tasks
        private void RequestTask()
        {
            WaitResponse = true;

            var    resp  = Client.UserRequest(function, parameters);
            Color  color = resp.Status == RequestStatus.Valid ? Color.Green : Color.Red;
            string text  = resp.Status == RequestStatus.Valid ? DataToLog(resp.Data) : resp.Status.ToString();
            

            Invoke((MethodInvoker)delegate
            {
                RequestLog.Append(text, color);
                RequestSend.Enabled = true;
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
        private void RequestSendClick(object sender, EventArgs e)
        {
            int func   = Request.Function;
            parameters = Request.Parameters;

            if (func != -1 && parameters != null)
            {
                function = (byte)func;

                RequestSend.Enabled = false;

                RequestLog.Append(DataToLog(parameters), Color.Blue);

                Task.Factory.StartNew(RequestTask);
            }
        }
        #endregion
    }
}