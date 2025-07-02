using System;
using System.Drawing;
using System.Windows.Forms;


namespace UModbus
{
    class RequestBox : Control
    {
        #region Fields
        private TextBox _Func;
        private TextBox _Params;
        #endregion

        #region Ctors
        public RequestBox()
        {
            BackColor = Color.White;

            _Func                  = new TextBox();
            _Func.Location         = new Point(1, 1);
            _Func.Size             = new Size(22, 23);
            _Func.BorderStyle      = BorderStyle.None;
            _Func.Font             = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
            _Func.ContextMenuStrip = new ContextMenuStrip();
            _Func.TextAlign        = HorizontalAlignment.Center;
            _Func.MaxLength        = 2;
            _Func.KeyPress        += FuncKeyPress;
            _Func.TextChanged     += FuncTextChanged;
            Controls.Add(_Func);

            _Params                  = new TextBox();
            _Params.Location         = new Point(_Func.Left+_Func.Width+3, 1);
            _Params.Size             = new Size(Width-_Func.Width-5, 23);
            _Params.BorderStyle      = BorderStyle.None;
            _Params.Anchor           = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            _Params.Font             = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
            _Params.ContextMenuStrip = new ContextMenuStrip();
            _Params.KeyPress        += ParamsKeyPress;
            _Params.TextChanged     += ParamsTextChanged;
            Controls.Add(_Params);
        }
        #endregion

        #region Internal
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, Width-1, Height-1));
            int x = _Func.Left + _Func.Width;
            e.Graphics.DrawLine(Pens.Gray, x, 1, x, Height);
        }

        private bool HandleHexSysmbol(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'ф':
                case 'Ф':
                case 'a': e.KeyChar = 'A'; break;
                case 'и':
                case 'И':
                case 'b': e.KeyChar = 'B'; break;
                case 'с':
                case 'С':
                case 'c': e.KeyChar = 'C'; break;
                case 'в':
                case 'В':
                case 'd': e.KeyChar = 'D'; break;
                case 'у':
                case 'У':
                case 'e': e.KeyChar = 'E'; break;
                case 'а':
                case 'А':
                case 'f': e.KeyChar = 'F'; break;
            }

            return "0123456789ABCDEF\b".IndexOf(e.KeyChar) == -1;
        }

        private void FuncKeyPress(object sender, KeyPressEventArgs e) => e.Handled = HandleHexSysmbol(e);

        private void ParamsKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = HandleHexSysmbol(e) && e.KeyChar != '-';

            if (!e.Handled)
            {
                int len = _Params.TextLength;
                int pos = _Params.SelectionStart;

                if (e.KeyChar == '\b')
                {
                    if (pos == 1)
                    {
                        if (len > 1 && _Params.Text[pos] == '-')
                        {
                            _Params.SelectionStart  = 0;
                            _Params.SelectionLength = 2;
                        }
                    }
                    else if (pos > 1)
                    {
                        if (_Params.SelectionLength == 1 && (_Params.Text[pos] == '-' || _Params.Text[pos-1] == '-'))
                        {
                            e.Handled = true;
                        }
                        else if (_Params.Text[pos-2] == '-' && (pos == len || _Params.Text[pos] == '-'))
                        {
                            _Params.Select(pos-2, 2);
                        }
                        else if (_Params.Text[pos-1] == '-')
                        {
                            e.Handled = pos != len;
                        }
                    }
                }
                else if (e.KeyChar == '-')
                {
                    e.Handled = len < 1 ||
                                pos < 1 ||
                                len > 1   && _Params.Text[pos-1] == '-' ||
                                pos < len && _Params.Text[pos]     == '-';
                }
                else
                {
                    if (len >= 2)
                    {
                        if (pos > 1)
                        {
                            if (pos == len)
                            {
                                if (_Params.Text[pos-1] != '-' && _Params.Text[pos-2] != '-')
                                {
                                    _Params.Text += "-";
                                    _Params.SelectionStart = pos + 1;
                                }
                            }
                            else if (pos < len)
                            {
                                if (_Params.Text[pos-1] != '-' && _Params.Text[pos-2] != '-')
                                {
                                    _Params.Paste("-");
                                    _Params.SelectionStart = pos + 1;
                                }
                                else if (_Params.Text[pos-1] == '-' && (pos+1) < len && _Params.Text[pos+1] != '-')
                                {
                                    _Params.Paste("-");
                                    _Params.SelectionStart = pos;
                                }
                                else if (_Params.Text[pos-1] != '-' && _Params.Text[pos] != '-')
                                {
                                    _Params.Paste(e.KeyChar + "-");
                                    e.Handled = true;
                                }
                            }
                        }
                        else
                        {
                            if (_Params.Text[0] != '-' && _Params.Text[1] != '-')
                            {
                                _Params.Paste(e.KeyChar + "-");
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        private void FuncTextChanged(object sender, EventArgs e)
        {
            if (_Func.BackColor == Color.LightPink)
            {
                _Func.BackColor = Color.White;
            }
        }

        private void ParamsTextChanged(object sender, EventArgs e)
        {
            if (_Params.BackColor == Color.LightPink)
            {
                _Params.BackColor = Color.White;
                BackColor         = Color.White;
            }
        }
        #endregion

        #region Public
        public int Function
        {
            get
            {
                if (_Func.Text == "")
                {
                    _Func.BackColor = Color.LightPink;
                    return -1;
                }

                return Convert.ToByte(_Func.Text, 16);
            }
        }

        public byte[] Parameters
        {
            get
            {
                if (_Params.TextLength > 0)
                {
                    try
                    {
                        return Array.ConvertAll(_Params.Text.Split('-'), img => Convert.ToByte(img, 16));
                    }
                    catch
                    {
                        BackColor         = Color.LightPink;
                        _Params.BackColor = Color.LightPink;

                        return null;
                    }
                }

                return new byte[0];
            }
        }
        #endregion
    }
}