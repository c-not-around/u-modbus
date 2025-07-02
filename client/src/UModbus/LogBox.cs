using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;


namespace UModbus
{
    class LogBox : Control
    {
        #region Fields
        private ContextMenuStrip _LogMenu;
        private RichTextBox      _Log;
        #endregion

        #region Ctors
        public LogBox()
        {
            var _LogMenuCopy    = new ToolStripMenuItem();
            _LogMenuCopy.Text   = "Copy";
            _LogMenuCopy.Image  = Properties.Resources.copy;
            _LogMenuCopy.Click += (sender, e) => _Log.Copy();

			var _LogMenuClear   = new ToolStripMenuItem();
			_LogMenuClear.Text  = "Clear";
			_LogMenuClear.Image = Properties.Resources.clear;
			_LogMenuClear.Click += (sender, e) => _Log.Clear();

			var _LogMenuSave    = new ToolStripMenuItem();
            _LogMenuSave.Text   = "Save";
            _LogMenuSave.Image  = Properties.Resources.save;
			_LogMenuSave.Click += (sender, e) =>
			{
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.Filter = "Plain text file (*.txt)|*.txt|Log file (*.log)|*.log";
				dialog.FilterIndex = 1;
				dialog.FileName = DateTime.Now.ToString("yyyyMMdd-HHmmss.lo\\g");

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllText(dialog.FileName, _Log.Text);
				}
			};

			_LogMenu = new ContextMenuStrip();
            _LogMenu.Items.AddRange(new ToolStripItem[] 
            {
                _LogMenuCopy,
                _LogMenuSave,
                _LogMenuClear
            });

            _Log                  = new RichTextBox();
            _Log.Location         = new Point(1, 1);
            _Log.Size             = new Size(Width-2, Height-2);
            _Log.ReadOnly         = true;
            _Log.BackColor        = Color.White;
            _Log.BorderStyle      = BorderStyle.None;
            _Log.ScrollBars       = RichTextBoxScrollBars.ForcedVertical;
            _Log.Font             = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
            _Log.ContextMenuStrip = _LogMenu;
            Controls.Add(_Log);
        }
        #endregion

        #region Internal
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, Width-1, Height-1));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            _Log.Size = new Size(Width-2, Height-2);
        }
        #endregion

        #region Public
        public new Size Size
        {
			get => base.Size;
            set
            {
                _Log.Size = new Size(value.Width-2, value.Height-2);
                base.Size = value;
            }
        }

        public void Append(string text, Color color, bool time = true, bool newline = true)
        {
            if (time)
            {
                _Log.SelectionColor = Color.Black;
                _Log.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + ": ");
            }

            _Log.SelectionColor = color;
            _Log.AppendText(text);

            if (newline)
            {
                _Log.SelectionColor = Color.Black;
                _Log.AppendText(DateTime.Now.ToString("\r\n"));
            }

            _Log.ScrollToCaret();
        }

        public static string BytesToHexString(byte[] data)
        {
            string result = "";

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (i > 0)
                    {
                        result += "-";
                    }

                    result += data[i].ToString("X2");
                }
            }

            return result;
        }
        #endregion
    }
}