using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace UModbus
{
    public class ModbusData
    {
        #region Type
        private const byte IS_READONLY = 0x00;
        private const byte IS_WRITABLE = 0x01;
        private const byte IS_BIT      = 0x00;
        private const byte IS_WORD     = 0x02;

        public enum ModbusDataType
        {
            DiscreteInput   = IS_BIT  | IS_READONLY,
            Coil            = IS_BIT  | IS_WRITABLE,
            InputRegister   = IS_WORD | IS_READONLY,
            HoldingRegister = IS_WORD | IS_WRITABLE
        }
        #endregion

        #region Format
        private const byte DIGITAL  = 0x04;
        private const byte LOGIC    = 0x08;
        private const byte LEVEL    = 0x0C;
        private const byte UNSIGNED = 0x00;
        private const byte SIGNED   = 0x01;
        private const byte FLOAT    = 0x02;
        private const byte SWORD    = 0x10;
        private const byte DWORD    = 0x20;
        private const byte QWORD    = 0x40;

        public enum ModbusDataFormat
        {
            Digital = DIGITAL,
            Logic   = LOGIC,
            Level   = LEVEL,
            Uint16  = SWORD | UNSIGNED,
            Int16   = SWORD | SIGNED,
            Uint32  = DWORD | UNSIGNED,
            Int32   = DWORD | SIGNED,
            Float32 = DWORD | FLOAT,
            Uint64  = QWORD | UNSIGNED,
            Int64   = QWORD | SIGNED,
            Float64 = QWORD | FLOAT,
        }
        #endregion

        #region Fields
        private byte[] _Value;
        #endregion

        #region Ctors
        public ModbusData()
        {
            
        }
        #endregion

        #region Properties
        public ModbusDataType Type
        {
            get => _Type;
            set => _Type = Enum.IsDefined(typeof(ModbusDataType), value) ? value : ModbusDataType.Coil;
        } ModbusDataType _Type;

        public ModbusDataFormat Format
        {
            get => _Format;
            set
            {
                bool IsValidEnumValue   = Enum.IsDefined(typeof(ModbusDataFormat), value);
                bool IsValidFormatValue = IsWord() ? (byte)value >= SWORD : (byte)value <= LEVEL;

                _Format = IsValidEnumValue && IsValidFormatValue ? value : DefaultFormat();

                int len = Length();
                if (_Value == null)
                {
                    _Value = new byte[len];
                }
                else if (_Value.Length != len)
                {
                    Array.Resize(ref _Value, len);
                }

                if (!CheckByteOrder(_ByteOrder))
                {
                    _ByteOrder = DefaultByteOrder();
                }
            }
        } ModbusDataFormat _Format;

        public int[] ByteOrder
        {
            get => _ByteOrder;
            set => _ByteOrder = CheckByteOrder(value) ? value : DefaultByteOrder();
        } int[] _ByteOrder;

        public ushort Address { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool Enable { get; set; }
        #endregion

        #region Methods
        public bool IsWord() => ((byte)_Type & IS_WORD) == IS_WORD;

        public bool IsWriteable() => ((byte)_Type & IS_WRITABLE) == IS_WRITABLE;

        public int Length() => IsWord() ? ((byte)_Format & 0x70) >> 3 : 1;

        public string OrderToString()
        {
            string order = "";

            if (IsWord())
            {
                for (int i = 0; i < _ByteOrder.Length; i++)
                {
                    if (i > 0)
                    {
                        order += "-";
                    }
                    order += _ByteOrder[i].ToString();
                }
            }

            return order;
        }

        public ModbusDataFormat DefaultFormat() => IsWord() ? ModbusDataFormat.Uint16 : ModbusDataFormat.Digital;

        public int[] DefaultByteOrder()
        {
            int   bytes = Length();
            int[] order = new int[bytes];

            for (int i = 0; i < bytes; i++)
            {
                order[i] = bytes - (1 + i);
            }

            return order;
        }

        public bool CheckByteOrder(int[] order)
        {
            int  bytes  = Length();
            bool result = order == null ? false : order.Length == bytes;

            if (result)
            {
                for (int i = 0; i < bytes; i++)
                {
                    if (bytes <= order[i] || order[i] < 0)
                    {
                        return false;
                    }
                }
            }

            return result;
        }

        public byte[] ConvertForward(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            int    bytes  = Length();
            byte[] result = new byte[bytes];

            for (int i = 0; i < bytes; i++)
            {
                result[i] = data[_ByteOrder[i]];
            }

            return result;
        }

        public byte[] ConvertBackward(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            int    bytes  = Length();
            byte[] result = new byte[bytes];

            for (int i = 0; i < bytes; i++)
            {
                result[_ByteOrder[i]] = data[i];
            }

            return result;
        }

        public byte[] GetRawValue() => IsWord() ? ConvertForward(_Value) : _Value;

        public string RawToString()
        {
            string raw = "";
            
            for (int i = 0; i < _Value.Length; i++)
            {
                raw += _Value[i].ToString("X2");
            }

            return raw;
        }

        public string ValueToString()
        {
            switch (_Format)
            {
                case ModbusDataFormat.Digital: return _Value[0] == 0x01 ? "1"    : "0";
                case ModbusDataFormat.Logic  : return _Value[0] == 0x01 ? "TRUE" : "FALSE";
                case ModbusDataFormat.Level  : return _Value[0] == 0x01 ? "HIGH" : "LOW";

                case ModbusDataFormat.Uint16 : return BitConverter.ToUInt16(ConvertForward(_Value), 0).ToString();
                case ModbusDataFormat.Int16  : return BitConverter.ToInt16(ConvertForward(_Value), 0).ToString();
                case ModbusDataFormat.Uint32 : return BitConverter.ToUInt32(ConvertForward(_Value), 0).ToString();
                case ModbusDataFormat.Int32  : return BitConverter.ToInt32(ConvertForward(_Value), 0).ToString();
                case ModbusDataFormat.Uint64 : return BitConverter.ToUInt64(ConvertForward(_Value), 0).ToString();
                case ModbusDataFormat.Int64  : return BitConverter.ToInt64(ConvertForward(_Value), 0).ToString();

                case ModbusDataFormat.Float32: return BitConverter.ToSingle(ConvertForward(_Value), 0).ToString();
                case ModbusDataFormat.Float64: return BitConverter.ToDouble(ConvertForward(_Value), 0).ToString();
            }

            return "0";
        }

        private static bool CheckIsHex(string image, int size)
        {
            string regex = "^0x[0-9a-f]{1," + (2*size).ToString() + "}$";
            return Regex.IsMatch(image, regex);
        }

        public bool SetValue(params byte[] value)
        {
            if (value.Length == _Value.Length)
            {
                _Value = value;
                return true;
            }

            return false;
        }
        
        public bool SetValue(string image)
        {
            switch (_Format)
            {
                case ModbusDataFormat.Digital: _Value[0] = (byte)(image           == "1"    ? 0x01 : 0x00); break;
                case ModbusDataFormat.Logic  : _Value[0] = (byte)(image.ToUpper() == "TRUE" ? 0x01 : 0x00); break;
                case ModbusDataFormat.Level  : _Value[0] = (byte)(image.ToUpper() == "HIGH" ? 0x01 : 0x00); break;

                case ModbusDataFormat.Uint16:
                {
                    ushort value;

                    if (CheckIsHex(image, 2))
                    {
                        value = Convert.ToUInt16(image, 16);
                    }
                    else if (!UInt16.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;
                case ModbusDataFormat.Int16:
                {
                    short value;

                    if (CheckIsHex(image, 2))
                    {
                        value = Convert.ToInt16(image, 16);
                    }
                    else if (!Int16.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;

                case ModbusDataFormat.Uint32:
                {
                    uint value;

                    if (CheckIsHex(image, 4))
                    {
                        value = Convert.ToUInt32(image, 16);
                    }
                    else if (!UInt32.TryParse(image, out value))
                    {
                        return false;
                    }

                        _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;
                case ModbusDataFormat.Int32:
                {
                    int value;

                    if (CheckIsHex(image, 4))
                    {
                        value = Convert.ToInt32(image, 16);
                    }
                    else if (!Int32.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;

                case ModbusDataFormat.Uint64:
                {
                    ulong value;

                    if (CheckIsHex(image, 8))
                    {
                        value = Convert.ToUInt64(image, 16);
                    }
                    else if (!UInt64.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;
                case ModbusDataFormat.Int64:
                {
                    long value;

                    if (CheckIsHex(image, 8))
                    {
                        value = Convert.ToInt64(image, 16);
                    }
                    else if (!Int64.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;
                
                case ModbusDataFormat.Float32:
                {
                    float value;

                    if (!Single.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;
                case ModbusDataFormat.Float64:
                {
                    double value;

                    if (!Double.TryParse(image, out value))
                    {
                        return false;
                    }

                    _Value = ConvertBackward(BitConverter.GetBytes(value));
                } break;

                default: return false;
            }

            return true;
        }

        public ModbusData Clone(bool address = false, bool value = false)
        {
            ModbusData line = new ModbusData();

            line._Type      = this._Type;
            line._Format    = this._Format;
            line._ByteOrder = this._ByteOrder;
            line._Value     = new byte[this._Value.Length];

            if (address)
            {
                line.Address = (ushort)(this.Address + (this.IsWord() ? this.Length()/2 : 1));
            }

            if (value)
            {
                Array.Copy(this._Value, 0, line._Value, 0, this._Value.Length);
            }

            return line;
        }
        #endregion

        #region Static
        public static ModbusData Default(int address = 0)
        {
            ModbusData line = new ModbusData();

            line.Type    = ModbusDataType.Coil;
            line.Format  = ModbusDataFormat.Digital;
            line.Address = (ushort)address;

            return line;
        }

        public static ModbusData[] OpenDataMap(string file) => JsonConvert.DeserializeObject<ModbusData[]>(File.ReadAllText(file));

        public static void SaveDataMap(string file, ModbusData[] map)
        {
            string legend = "/*\r\nTypes:\r\n";
            foreach (var t in Enum.GetValues(typeof(ModbusDataType)))
            {
                ModbusDataType type = (ModbusDataType)t;
                legend += $"\t{(int)type:g} - {type}\r\n";
            }
            legend += "Formats:\r\n";
            foreach (var f in Enum.GetValues(typeof(ModbusDataFormat)))
            {
                ModbusDataFormat format = (ModbusDataFormat)f;
                legend += $"\t{(int)format:g} - {format}\r\n";
            }
            legend += "*/\r\n";

            string saved = JsonConvert.SerializeObject(map, Formatting.Indented);
            saved = saved.Replace("  ", "\t");
            File.WriteAllText(file, legend + saved);
        }
        #endregion
    }
}