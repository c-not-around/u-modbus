using System;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;


namespace UModbus
{
    #region Types
    /// <summary>
    /// Статус ответа на modbus запрос
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Ошибок не обнаружено.
        /// </summary>
        Valid                  = 0x00,

        /// <summary>
        /// Стандарный код ошибки протокла modbus. Функция запроса не поддерживается устройством.
        /// </summary>
        IllegalFunction        = 0x01,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Некорректный для устройства начальный адрес флага/регистра.
        /// </summary>
        IllegalDataAddress     = 0x02,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Значение, содержащееся в поле данных запроса, не является допустимым.
        /// </summary>
        IllegalDataValue       = 0x03,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Произошла неисправимая ошибка, когда slave пытался выполнить запрошенное действие.
        /// </summary>
        SlaveDeviceFailure     = 0x04,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Slave принял запрос и обрабатывает его, но ответные данные будут готовы через продолжительный период времени.
        /// </summary>
        Acknowledge            = 0x05,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Slave занимается обработкой длительной команды. Клиент должен повторно передать сообщение позже.
        /// </summary>
        SlaveDeviceBusy        = 0x06,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Slave попытался прочитать запись из файла, но обнаружил ошибку четности в памяти.
        /// </summary>
        MemoryParityError      = 0x08,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Обычно означает, что шлюз настроен неправильно или перегружен.
        /// </summary>
        GateWayPathUnavailable = 0x0A,
        /// <summary>
        /// Стандарный код ошибки протокла modbus. Указывает на то, что ответ не был получен от Slave устройство. 
        /// </summary>
        GateWayTargetFailed    = 0x0B,

        /// <summary>
        /// Получен пакет меньшей длины чем минимально возможная.
        /// </summary>
        ResponseLengthInvalid  = 0x10,
        /// <summary>
        /// Получен ответ от другого устройства.
        /// </summary>
        DifferentSlaveAddress  = 0x11,
        /// <summary>
        /// Получен ответ на другой запрос.
        /// </summary>
        DifferentFunction      = 0x12,
        /// <summary>
        /// Получен ответ на другой запрос.
        /// </summary>
        DifferentSubFunction   = 0x22,
        /// <summary>
        /// Получен пакет с длиной блока данных меньшей минимально возможной.
        /// </summary>
        DataLengthInvalid      = 0x13,
        /// <summary>
        /// Получен ответ с другим начальным адресом флага/регистра.
        /// </summary>
        DifferentWriteAddress  = 0x14,

        /// <summary>
        /// Получен IP пакет меньшей длины чем минимально возможная.
        /// </summary>
        IpPacketLengthInvalid  = 0x15,
        /// <summary>
        /// Получен ответ с другим идентификатором IP транзакции.
        /// </summary>
        IpDifferentTransaction = 0x16,
        /// <summary>
        /// Получен ответ с другим типом протокола.
        /// </summary>
        IpDifferentProtocol    = 0x17,
        /// <summary>
        /// Получен пакет с длиной блока данных меньшей минимально возможной.
        /// </summary>
        IpDataLengthInvalid    = 0x18,
        
        /// <summary>
        /// Некорректная контрольная сумма ответного пакета.
        /// </summary>      
        SerialBadCheckSum      = 0x19,
        /// <summary>
        /// Ошибка при передаче через последовательный порт.
        /// </summary>
        SerialPortError        = 0x1A,
        /// <summary>
        /// Получен пакет меньшей длины чем минимально возможная.
        /// </summary>
        SerialLengthInvalid    = 0x1B,
        
        /// <summary>
        /// В полученом ответном ASCII пакете отсутствует стартовая последовательность.
        /// </summary>
        AsciiMissingStartMark  = 0x1C,
        /// <summary>
        /// В полученом ответном ASCII пакете отсутствует стоповая последовательность.
        /// </summary>
        AsciiMissingStopMark   = 0x1D,
        /// <summary>
        /// В полученом ответном ASCII пакете обнаружен недопустимый символ.
        /// </summary>
        AsciiIllegalSymbol     = 0x1E,
        
        /// <summary>
        /// Ошибка TCP подключения.
        /// </summary>        
        TcpNoConnection        = 0x1F,
        /// <summary>
        /// Ошибка передачи/приема пакета TCP/IP.
        /// </summary>
        TcpNetworkError        = 0x20,

        /// <summary>
        /// Ошибка передачи/приема пакета UDP/IP.
        /// </summary>
        UdpNetworkError        = 0x21
    }

    /// <summary>
    /// Тип ответа на запрос чтения флагов.
    /// </summary>
    public struct BitResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Сырые данные.
        /// </summary>
        public byte[]        Raw;
        /// <summary>
        /// Массив считанных значений флагов.
        /// </summary>
        public bool[]        Data;
    }

    /// <summary>
    /// Тип ответа на запрос чтения регистров.
    /// </summary>
    public struct WordResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Сырые данные.
        /// </summary>
        public byte[]        Raw;
        /// <summary>
        /// Массив считанных значений регистров. 
        /// </summary>
        public ushort[]      Data;
    }

    /// <summary>
    /// Тип не структурированного ответа на запрос.
    /// </summary>
    public struct ByteResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Сырые данные из ответного пакета.
        /// </summary>
        public byte[]        Data;


        /*public string[] AsStrings()
        {
            int length = 1;

            for (int i = 0; i < Data.Length; i++)
            {
                if 
            }
        }*/
    }

    /// <summary>
    /// Тип ответа на запрос чтения DeviceIdentification (FC43).
    /// </summary>
    public struct IdResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public byte          ProductId;
        /// <summary>
        /// Уровень представления
        /// </summary>
        public byte          ConformityLevel;
        /// <summary>
        /// Массив строк описания устройства.
        /// </summary>
        public string[]      Objects;

        /// <summary>
        /// Возвращает строковое представление.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = String.Format("Product ID       : {0}\r\nConformity Level : {1}\r\n", ProductId, ConformityLevel);

            if (Objects.Length > 0)
            {
                result += String.Format("Manufacturer Name: \"{0}\"\r\n", Objects[0]);
            }

            if (Objects.Length > 1)
            {
                result += String.Format("Product Code     : \"{0}\"\r\n", Objects[1]);
            }

            if (Objects.Length > 2)
            {
                result += String.Format("Version Number   : \"{0}\"\r\n", Objects[2]);
            }

            if (Objects.Length > 3)
            {
                result += String.Format("Manufacturer URL : \"{0}\"\r\n", Objects[3]);
            }

            if (Objects.Length > 4)
            {
                result += String.Format("Product Name     : \"{0}\"\r\n", Objects[4]);
            }

            if (Objects.Length > 5)
            {
                result += String.Format("Model Name       : \"{0}\"\r\n", Objects[5]);
            }

            if (Objects.Length > 6)
            {
                result += String.Format("Application Name : \"{0}\"\r\n", Objects[6]);
            }

            return result;
        }
    }

    /// <summary>
    /// Тип ответа на запрос записи.
    /// </summary>
    public struct WriteResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Количество записанных флагов/регистров.
        /// </summary>
        public int           Recorded;
    }

    /// <summary>
    /// Тип ответа на запрос чтения ExceptionStatus (FC07).
    /// </summary>
    public struct ExceptionResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Код ошибки.
        /// </summary>
        public byte          ExceptionCode;
    }

    /// <summary>
    /// Тип ответа на запрос состояния (FC11).
    /// </summary>
    public struct StatusResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Слово состояния.
        /// </summary>
        public ushort        StatusWord;
        /// <summary>
        /// Количество записей в журнале событий.
        /// </summary>
        public ushort        EventCount;
    }

    /// <summary>
    /// Тип ответа на запрос записи журнала (FC12).
    /// </summary>
    public struct EventLogResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Слово состояния.
        /// </summary>
        public ushort        StatusWord;
        /// <summary>
        /// Количество записей в журнале событий.
        /// </summary>
        public ushort        EventCount;
        /// <summary>
        /// Количество сообщений.
        /// </summary>
        public ushort        MessageCount;
        /// <summary>
        /// до 64-х однобайтовых записей о событиях.
        /// </summary>
        public byte[]        Events;
    }

    /// <summary>
    /// Тип элемента запроса чтения записей из файла (FC20).
    /// </summary>
    public struct FileRecordSubReq
    {
        /// <summary>
        /// Номер файла.
        /// </summary>
        public ushort File;
        /// <summary>
        /// Номер записи в выбранном файле.
        /// В файле может быть до 10000 записей (0-9999).
        /// </summary>
        public ushort Record;
        /// <summary>
        /// Длина записи.
        /// </summary>
        public ushort Length;
    }

    /// <summary>
    /// Тип ответа на чтение записей из файла (FC20)
    /// </summary>
    public struct FileRecordResponse
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public RequestStatus Status;
        /// <summary>
        /// Сырые данные.
        /// </summary>
        public byte[]        Raw;
        /// <summary>
        /// Массив считанных значений записей.
        /// </summary>
        public ushort[][]    Records;
    }

    /// <summary>
    /// Тип записи для записи в файл (FC21)
    /// </summary>
    public struct FileRecord
    {
        /// <summary>
        /// Номер файла.
        /// </summary>
        public ushort   File;
        /// <summary>
        /// Номер записи в выбранном файле.
        /// В файле может быть до 10000 записей (0-9999).
        /// </summary>
        public ushort   Record;
        /// <summary>
        /// Данные записи.
        /// </summary>
        public ushort[] Data;
    }
    #endregion

    #region Base
    /// <summary>
    /// Базовый класс Modbus-клиента.
    /// </summary>
    public abstract class ModbusClient
    {
        #region Modbus Functions
        /// <summary>
        /// Флаг ошибки, добавляемый устройством к коду функции запроса, при ошибке.
        /// </summary>
        public const byte MB_ERROR_FLAG              = 0x80;
        /// <summary>
        /// Стандартная modbus функция FC01 - чтение одного/нескольких флагов (coils).
        /// </summary>
        public const byte MB_READ_COIL_STATUS        = 0x01;
        /// <summary>
        /// Стандартная modbus функция FC02 - чтение одного/нескольких флагов (discrete inputs).
        /// </summary>
        public const byte MB_READ_DISCRETE_INPUTS    = 0x02;
        /// <summary>
        /// Стандартная modbus функция FC03 - чтение одного/нескольких регистров хранения.
        /// </summary>
        public const byte MB_READ_HOLDING_REGISTERS  = 0x03;
        /// <summary>
        /// Стандартная modbus функция FC04 - чтение одного/нескольких входных регистров.
        /// </summary>
        public const byte MB_READ_INPUT_REGISTERS    = 0x04;
        /// <summary>
        /// Стандартная modbus функция FC05 - запись значения одного флага (coil).
        /// </summary>
        public const byte MB_WRITE_SINGLE_COIL       = 0x05;
        /// <summary>
        /// Стандартная modbus функция FC06 - запись значения одного регистра хранения.
        /// </summary>
        public const byte MB_WRITE_SINGLE_REGISTER   = 0x06;
        /// <summary>
        /// Стандартная modbus функция FC07 - чтение кода ошибки.
        /// </summary>
        public const byte MB_READ_EXCEPTION_STATUS   = 0x07;
        /// <summary>
        /// Стандартная modbus функция FC08 - диагностика устройства.
        /// </summary>
        public const byte MB_DIAGNOSTICS             = 0x08;
        /// <summary>
        /// Стандартная modbus функция FC11 - чтение слова состояния и количества событий в журнале.
        /// </summary>
        public const byte MB_GET_COMM_EVENT_COUNTER  = 0x0B;
        /// <summary>
        /// Стандартная modbus функция FC12 - чтение записей событий из журнала.
        /// </summary>
        public const byte MB_GET_COMM_EVENT_LOG      = 0x0C;
        /// <summary>
        /// Стандартная modbus функция FC15 - запись значений нескольких флагов (coils).
        /// </summary>
        public const byte MB_WRITE_MULTIPLE_COILS    = 0x0F;
        /// <summary>
        /// Стандартная modbus функция FC16 - запись значений нескольких регистров хранения.
        /// </summary>
        public const byte MB_WRITE_MULTIPLE_REGISTER = 0x10;
        /// <summary>
        /// Стандартная modbus функция FC17 - чтение описания типа, текущего состояния и другой информации, 
        /// специфичной для конкретного устройства.
        /// </summary>
        public const byte MB_REPORT_SLAVE_ID         = 0x11;
        /// <summary>
        /// Стандартная modbus функция FC20 - чтение массива записей из файла.
        /// </summary>
        public const byte MB_READ_FILE_RECORD        = 0x14;
        /// <summary>
        /// Стандартная modbus функция FC21 - зфписб массива записей в файл.
        /// </summary>
        public const byte MB_WRITE_FILE_RECORD       = 0x15;
        /// <summary>
        /// Стандартная modbus функция FC22 - модификация значения регистра с помощью масок.
        /// </summary>
        public const byte MB_MASK_WRITE_REGISTER     = 0x16;
        /// <summary>
        /// Стандартная modbus функция FC23 - комбинация чтения и записи нескольких регистров в одной транзакции.
        /// </summary>
        public const byte MB_READWRITE_MULTIPLE_REGS = 0x17;
        /// <summary>
        /// Стандартная modbus функция FC24 - чтение содержимого очереди регистров в порядке очереди (FIFO).
        /// </summary>
        public const byte MB_READ_FIFO_QUEUE         = 0x18;
        /// <summary>
        /// Стандартная modbus функция FC43 - чтение идентификатор устройства.
        /// </summary>
        public const byte MB_READ_DEVICE_ID          = 0x2B;
        
        /// <summary>
        /// Стандартная modbus функция SF13 - подфункция функции FC43.
        /// </summary>
        ///public const byte MB_READ_DEVICE_ID_SUB      = 0x0E;
        public const byte MB_MEI_TYPE_CAN_OPEN       = 0x0D;
        /// <summary>
        /// Стандартная modbus функция SF14 - подфункция функции FC43.
        /// </summary>
        public const byte MB_MEI_TYPE_DEVICE_ID      = 0x0E;
        /// <summary>
        /// Стандартная modbus функция SF01 - подфункция функции FC43.
        /// </summary>
        public const byte MB_READ_ID_CODE_BASIC      = 0x01;
        /// <summary>
        /// Стандартная modbus функция SF02 - подфункция функции FC43.
        /// </summary>
        public const byte MB_READ_ID_CODE_REGULAR    = 0x02;
        /// <summary>
        /// Стандартная modbus функция SF03 - подфункция функции FC43.
        /// </summary>
        public const byte MB_READ_ID_CODE_EXTENDED   = 0x03;
        /// <summary>
        /// Стандартная modbus функция SF04 - подфункция функции FC43.
        /// </summary>
        public const byte MB_READ_ID_CODE_SPECIFIC   = 0x04;
        /// <summary>
        /// Стандартная modbus функция SF00 - подфункция функции FC43.
        /// </summary>
        public const byte MB_READ_ID_OBJECT_ID       = 0x00;

        /// <summary>
        /// Стандартная modbus функция SF00 - подфункция функции FC08.
        /// Эхо-команда: данные переданные этой командой должны быть возвращены в ответном сообщении.
        /// ReqDataField:  любое количество любых байт.
        /// RespDataField: echo ReqDataField.
        /// </summary>
        public const ushort MB_DIAG_RETURN_QUERY_DATA          = 0x0000;
        /// <summary>
        /// Стандартная modbus функция SF01 - подфункция функции FC08.
        /// Команда реинициализации подсистемы связи подчиненного устройства.
        /// Все счетчики событий очищаются. Выводит порт из режима "только прослушивание".
        /// ReqDataField:  0000 - журнал событий не будет очищен
        ///                FF00 - журнал событий будет очищен
        /// RespDataField: echo ReqDataField.
        /// </summary>
        public const ushort MB_DIAG_RESTART_COMM_OPTION        = 0x0001;
        /// <summary>
        /// Стандартная modbus функция SF02 - подфункция функции FC08.
        /// Получение содержимого диагностического регистра подчиненного устройства.
        /// ReqDataField:  0000
        /// RespDataField: содержимого диагностического регистра
        /// </summary>
        public const ushort MB_DIAG_RETURN_DIAGNOSTIC_REGISTER = 0x0002;
        /// <summary>
        /// Стандартная modbus функция SF03 - подфункция функции FC08.
        /// Установка символа разделителя сообщений
        /// ReqDataField:  новый символ-разделитель
        /// RespDataField: echo ReqDataField.
        /// </summary>
        public const ushort MB_DIAG_CHANGE_ASCII_DELIMITER     = 0x0003;
        /// <summary>
        /// Стандартная modbus функция SF04 - подфункция функции FC08.
        /// Переводит подчиненное устройство в режим "только прослушивание" -
        /// все запросы принимаются и обрабатываются, но ответы на них не отсылаются.
        /// ReqDataField:  0000
        /// RespDataField: ----
        /// </summary>
        public const ushort MB_DIAG_FORCE_LISTEN_ONLY_MODE     = 0x0004;
        /// <summary>
        /// Стандартная modbus функция SF10 - подфункция функции FC08.
        /// Очистка всех счетчиков и диагностического регистра.
        /// ReqDataField:  0000
        /// RespDataField: echo ReqDataField.
        /// </summary>
        public const ushort MB_DIAG_CLEAR_COUNTERS_DIAG_REGS   = 0x000A;
        /// <summary>
        /// Стандартная modbus функция SF11 - подфункция функции FC08.
        /// Запрос количества сообщений, обнаруженных подчиненным устройством.
        /// ReqDataField:  0000
        /// RespDataField: общее количество сообщений.
        /// </summary>
        public const ushort MB_DIAG_RETURN_BUS_MESSAGE_COUNT   = 0x000B;
        /// <summary>
        /// Стандартная modbus функция SF12 - подфункция функции FC08.
        /// Запрос количества поврежденных сообщений, обнаруженных подчиненным устройством.
        /// ReqDataField:  0000
        /// RespDataField: количество запросов с некорректной контрольной суммой.
        /// </summary>
        public const ushort MB_DIAG_RETURN_COMM_ERR_COUNT      = 0x000C;
        /// <summary>
        /// Стандартная modbus функция SF13 - подфункция функции FC08.
        /// Запрос количества ответных сообщений об ошибке.
        /// ReqDataField:  0000
        /// RespDataField: количество сообщений об ошибке.
        /// </summary>
        public const ushort MB_DIAG_RETURN_EXCEPT_ERR_COUNT    = 0x000D;
        /// <summary>
        /// Стандартная modbus функция SF14 - подфункция функции FC08.
        /// Запрос количества сообщений полученных подчиненным устройством.
        /// ReqDataField:  0000
        /// RespDataField: количество сообщений.
        /// </summary>
        public const ushort MB_DIAG_RETURN_SLAVE_MESSAGE_COUNT = 0x000E;
        /// <summary>
        /// Стандартная modbus функция SF15 - подфункция функции FC08.
        /// Запрос количества сообщений, на которые не был отправлен ответ.
        /// ReqDataField:  0000
        /// RespDataField: количество сообщений.
        /// </summary>
        public const ushort MB_DIAG_RETURN_SLAVE_NORESP_COUNT  = 0x000F;
        /// <summary>
        /// Стандартная modbus функция SF16 - подфункция функции FC08.
        /// Запрос количества сообщений, на которые был потправлен ответ NAK.
        /// ReqDataField:  0000
        /// RespDataField: количество сообщений.
        /// </summary>
        public const ushort MB_DIAG_RETURN_SLAVE_NAK_COUNT     = 0x0010;
        /// <summary>
        /// Стандартная modbus функция SF17 - подфункция функции FC08.
        /// Запрос количества сообщений, на которые был потправлен ответ BUSY.
        /// ReqDataField:  0000
        /// RespDataField: количество сообщений.
        /// </summary>
        public const ushort MB_DIAG_RETURN_SLAVE_BUSY_COUNT    = 0x0011;
        /// <summary>
        /// Стандартная modbus функция SF18 - подфункция функции FC08.
        /// Запрос количества сообщений, необработанных из-за переполнения.
        /// ReqDataField:  0000
        /// RespDataField: количество сообщений.
        /// </summary>
        public const ushort MB_DIAG_RETURN_BUS_CHAR_OVERRUN    = 0x0012;
        /// <summary>
        /// Стандартная modbus функция SF20 - подфункция функции FC08.
        /// Очищистка счетчика ошибок переполнения и сброс флага ошибки.
        /// ReqDataField:  0000
        /// RespDataField: echo ReqDataField.
        /// </summary>
        public const ushort MB_DIAG_CLEAR_OVERRUN_COUNTER_FLAG = 0x0014;
        #endregion

        #region Types
        /// <summary>
        /// Тип ответа на запрос.
        /// </summary>
        protected struct TransferResponse
        {
            /// <summary>
            /// Статус ответа.
            /// </summary>
            public RequestStatus Status;
            /// <summary>
            /// Блок данных.
            /// </summary>
            public byte[]        Data;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Selected modbus-slave address.
        /// </summary>
        private   byte _SlaveAddress;
        /// <summary>
        /// Time during which should wait for a response.
        /// </summary>
        protected int  _Timeout;
        #endregion

        #region Internal
        /// <summary>
        /// Дописывет HEX-представление массива байт в файл LogFileName.
        /// </summary>
        /// <param name="prefix">метка пакета.</param>
        /// <param name="length">количество байт, которые следует записать в файл.</param>
        /// <param name="data">массив записываемых байт.</param>
        protected void SaveToLogFile(string prefix, int length, params byte[] data)
        {
            if (LogFileName != null && LogFileName != "")
            {
                string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + prefix + ":";

                for (int i = 0; i < length; i++)
                {
                    line += " " + data[i].ToString("X2");
                }

                File.AppendAllText(LogFileName, line + "\r\n");
            }
        }

        /// <summary>
        /// Отправляет PDU запроса через транспортный уровень, 
        /// определенный в конкретной реализации клиента
        /// и возвращает PDU ответного пакета.
        /// </summary>
        /// <param name="data">Protocol Data Unit</param>
        /// <returns>Protocol Data Unit.</returns>
        protected abstract TransferResponse TransferPDU(params byte[] data);

        private RequestStatus CheckResponse(TransferResponse response, byte function, Func<byte[],bool> CheckLength)
        {
            if (response.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - request-depends data or error code
                if (response.Data.Length > 2)
                {
                    if (response.Data[0] == _SlaveAddress)
                    {
                        if (response.Data[1] == function)
                        {
                            return CheckLength(response.Data) ? RequestStatus.Valid : RequestStatus.DataLengthInvalid;
                        }

                        if (response.Data[1] == (function | MB_ERROR_FLAG))
                        {
                            return (RequestStatus)response.Data[2];
                        }

                        return RequestStatus.DifferentFunction;
                    }

                    return RequestStatus.DifferentSlaveAddress;
                }

                return  RequestStatus.ResponseLengthInvalid;
            }

            return response.Status;
        }

        private BitResponse ReadBits(byte function, ushort address, ushort count)
        {
            BitResponse result = new BitResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,          // 0: slave address
                function,               // 1: function read 
                (byte)(address >> 8),   // 2: start register (MSB)
                (byte)(address & 0xFF), // 3: start register (LSB)
                (byte)(count >> 8),     // 4: registers count (MSB)
                (byte)(count & 0xFF)    // 5: registers count (LSB)
            );

            result.Status = CheckResponse(response, function, data => (data.Length-3) >= (data[2]/8));

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function read 
                // 2 - data length or error code
                // 3...data bytes (MSB first)
                int bytes   = response.Data[2];
                int flags   = Math.Min(8*bytes, count);
                result.Raw  = new byte[bytes];
                result.Data = new bool[flags];

                Array.Copy(response.Data, 3, result.Raw, 0, bytes);

                for (int i = 0; i < flags; i++)
                {
                    int mask = 1 << (i % 8);
                    result.Data[i] = (response.Data[3+(i/8)] & mask) == mask;
                }
            }

            return result;
        }

        private WordResponse ReadWords(byte function, ushort address, ushort count)
        {
            WordResponse result = new WordResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,          // 0: slave address
                function,               // 1: function read 
                (byte)(address >> 8),   // 2: start register (MSB)
                (byte)(address & 0xFF), // 3: start register (LSB)
                (byte)(count >> 8),     // 4: registers count (MSB)
                (byte)(count & 0xFF)    // 5: registers count (LSB)
            );

            result.Status = CheckResponse(response, function, data => (data.Length-3) == data[2]);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function read 
                // 2 - data length or error code
                // 3...data bytes (MSB first)
                int bytes   = response.Data[2];
                result.Raw  = new byte[bytes];
                result.Data = new ushort[bytes/2];

                Array.Copy(response.Data, 3, result.Raw, 0, bytes);

                for (int i = 0; i < result.Data.Length; i++)
                {
                    result.Data[i] = (ushort)((response.Data[3+2*i] << 8) | response.Data[4+2*i]);
                }
            }

            return result;
        }

        private IdResponse ReadDeviceIdentification(byte IdCode)
        {
            IdResponse result = new IdResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,         // 0: slave address
                MB_READ_DEVICE_ID,     // 1: get info function code
                MB_MEI_TYPE_DEVICE_ID, // 2: get info sub-function code
                IdCode,                // 3: read device id code
                MB_READ_ID_OBJECT_ID   // 4: Object id
            );

            result.Status = CheckResponse(response, MB_READ_DEVICE_ID, data => data.Length > 8);

            if (result.Status == RequestStatus.Valid)
            {
                // 0    - slave address
                // 1    - function code
                // 2    - sub-function code
                // 3    - product id
                // 4    - conformity level
                // 5    - more follows (00/FF)
                // 6    - next object id
                // 7    - number of objects
                // 8    - object0 object number
                // 9    - object0 length (bytes)
                // 10...- object0 data bytes
                if (response.Data[2] == MB_MEI_TYPE_DEVICE_ID && response.Data[3] == IdCode)
                {
                    result.ProductId       = response.Data[3];
                    result.ConformityLevel = response.Data[4];
                    result.Objects         = new string[response.Data[7]];

                    try
                    {
                        int offset = 8;
                        for (int i = 0; i < result.Objects.Length; i++)
                        {
                            int ObjectNum = response.Data[offset++]; // object i number
                            int ObjectLen = response.Data[offset++]; // object i length
                            result.Objects[ObjectNum] = Encoding.ASCII.GetString(response.Data, offset, ObjectLen - 1);
                            offset += ObjectLen;                     // shift offset to next object
                        }
                    }
                    catch
                    {
                        result.Status = RequestStatus.DataLengthInvalid;
                    }
                }
                else
                {
                    result.Status = RequestStatus.DifferentSubFunction;
                }
            }

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusClient.
        /// </summary>
        public ModbusClient()
        {
            _SlaveAddress = 0;
            _Timeout      = 500;
        }

        /// <summary>
        /// Задает/возвращает modbus-адрес устройства в сети.
        /// </summary>
        public byte SlaveAddress
        {
            get
            {
                return _SlaveAddress;
            }

            set
            {
                _SlaveAddress = value;
            }
        }

        /// <summary>
        /// Задает/возвращает время в течении которого следует ждать ответ от устройства в мс.
        /// </summary>
        public int Timeout
        {
            get
            {
                return _Timeout;
            }

            set
            {
                _Timeout = value;
            }
        }

        /// <summary>
        /// Задает/возвращает разрешение записи данных запросов и ответов на них в файл.
        /// </summary>
        public bool LoggingEnable { get; set; }

        /// <summary>
        /// Задает/возвращает имя файла лога.
        /// </summary>
        public string LogFileName { get; set; }

        /// <summary>
        /// Возвращает статус соединения транспортного уровня.
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// Инициирует соединение транспортного уровня.
        /// </summary>
        /// <returns>статус соединения транспортного уровня.</returns>
        public abstract bool Connect();

        /// <summary>
        /// Закрывает соединение транспортного уровня.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// FC01 - чтение одного/нескольких флагов (ciols).
        /// </summary>
        /// <param name="address">адрес первого флага.</param>
        /// <param name="count">количество считываемых флагов.</param>
        /// <returns>массив значений флагов.</returns>
        public BitResponse ReadCoils(ushort address, ushort count) => ReadBits(MB_READ_COIL_STATUS, address, count);

        /// <summary>
        /// FC02 - чтение одного/нескольких флагов (discrete inputs).
        /// </summary>
        /// <param name="address">адрес первого флага.</param>
        /// <param name="count">количество считываемых флагов.</param>
        /// <returns>массив значений флагов.</returns>
        public BitResponse ReadDiscreteInputs(ushort address, ushort count) => ReadBits(MB_READ_DISCRETE_INPUTS, address, count);

        /// <summary>
        /// FC03 - чтение одного/нескольких регистров хранения.
        /// </summary>
        /// <param name="address">адрес первого регистра.</param>
        /// <param name="count">количество считываемых регистров.</param>
        /// <returns>массив значений регистров.</returns>
        public WordResponse ReadHoldingRegisters(ushort address, ushort count) => ReadWords(MB_READ_HOLDING_REGISTERS, address, count);

        /// <summary>
        /// FC04 - чтение одного/нескольких входных регистров.
        /// </summary>
        /// <param name="address">адрес первого регистра.</param>
        /// <param name="count">количество считываемых регистров.</param>
        /// <returns>массив значений регистров.</returns>
        public WordResponse ReadInputRegisters(ushort address, ushort count) => ReadWords(MB_READ_INPUT_REGISTERS, address, count);

        /// <summary>
        /// FC05 - запись значения одного флага (coil).
        /// </summary>
        /// <param name="address">адрес записываемого флага.</param>
        /// <param name="coil">значение записываемого флага.</param>
        /// <returns>количество записаных флагов.</returns>
        public WriteResponse WriteSingleCoil(ushort address, bool coil)
        {
            WriteResponse result = new WriteResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,              // 0: slave address
                MB_WRITE_SINGLE_COIL,       // 1: function code
                (byte)(address >> 8),       // 2: start coil (MSB)
                (byte)(address & 0xFF),     // 3: start coil (LSB)
                (byte)(coil ? 0xFF : 0x00), // 4: coil value (MSB)
                (byte)(0x00)                // 5: coil value (LSB)
            );

            result.Status = CheckResponse(response, MB_WRITE_SINGLE_COIL, data => data.Length == 6);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - start coil MSB
                // 3 - start coil LSB
                // 4 - coil value MSB
                // 5 - coil value LSB
                if ((ushort)((response.Data[2] << 8) | response.Data[3]) == address)
                {
                    ushort value    = (ushort)((response.Data[4] << 8) | response.Data[5]);
                    result.Recorded = (value ^ ((ushort)(coil ? 0xFF00 : 0x0000))) == 0x0000 ? 1 : 0;
                }
                else
                {
                    result.Status = RequestStatus.DifferentWriteAddress;
                }
            }

            return result;
        }

        /// <summary>
        /// FC06 - запись значения одного регистра хранения.
        /// </summary>
        /// <param name="address">адрес записываемого регистра.</param>
        /// <param name="register">значение записываемого регистра.</param>
        /// <returns>количество записаных регистров.</returns>
        public WriteResponse WriteSingleRegister(ushort address, ushort register)
        {
            WriteResponse result = new WriteResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,            // 0: slave address
                MB_WRITE_SINGLE_REGISTER, // 1: function code
                (byte)(address >> 8),     // 2: start register (MSB)
                (byte)(address & 0xFF),   // 3: start register (LSB)
                (byte)(register >> 8),    // 4: register value (MSB)
                (byte)(register & 0xFF)   // 5: register value (LSB)
            );

            result.Status = CheckResponse(response, MB_WRITE_SINGLE_REGISTER, data => data.Length == 6);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - start register MSB
                // 3 - start register LSB
                // 4 - register value MSB
                // 5 - register value LSB
                if ((ushort)((response.Data[2] << 8) | response.Data[3]) == address)
                {
                    ushort value    = (ushort)((response.Data[4] << 8) | response.Data[5]);
                    result.Recorded = (value ^ register) == 0x0000 ? 1 : 0;
                }
                else
                {
                    result.Status = RequestStatus.DifferentWriteAddress;
                }
            }

            return result;
        }

        /// <summary>
        /// FC07 - чтение кода ошибки.
        /// </summary>
        /// <returns>код ошибки от 0 до 255.</returns>
        public ExceptionResponse ReadExceptionStatus()
        {
            ExceptionResponse result = new ExceptionResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,           // 0: slave address
                MB_READ_EXCEPTION_STATUS // 1: read exception-status function code
            );

            result.Status = CheckResponse(response, MB_READ_EXCEPTION_STATUS, data => data.Length == 3);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function read exception-status
                // 2 - exception code or error code
                result.ExceptionCode = response.Data[2];
            }

            return result;
        }

        /// <summary>
        /// FC08 - диагностика починенного устройства.
        /// </summary>
        /// <param name="SubFunction">подфункция диагностики.</param>
        /// <param name="data">данные (в зависимости от подфункции).</param>
        /// <returns>данные (в зависимости от подфункции).</returns>
        public ByteResponse Diagnostics(ushort SubFunction, params byte[] data)
        {
            ByteResponse result = new ByteResponse();

            byte[] request = new byte[4 + data.Length];
            request[0] = _SlaveAddress;              // 0: slave address
            request[1] = MB_DIAGNOSTICS;             // 1: diagnostics function code
            request[2] = (byte)(SubFunction >> 8);   // 2: diagnostics sub-function code (MSB)
            request[3] = (byte)(SubFunction & 0xFF); // 3: diagnostics sub-function code (LSB)
            Array.Copy(data, 0, request, 4, data.Length);

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, MB_DIAGNOSTICS, dat => dat.Length > 3);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function diagnostics
                // 2 - diagnostics sub-function code (MSB) or error code
                // 3 - diagnostics sub-function code (LSB)
                // 4...user data structure bytes
                if ((ushort)((response.Data[2] << 8) | response.Data[3]) == SubFunction)
                {
                    result.Data = new byte[response.Data.Length-4];
                    Array.Copy(response.Data, 4, result.Data, 0, result.Data.Length);
                }
                else
                {
                    result.Status = RequestStatus.DifferentSubFunction;
                }
            }

            return result;
        }

        /// <summary>
        /// FC11 - чтение слова состояния и количества событий починенного устройства.
        /// </summary>
        /// <returns>слово состояния и количество событий.</returns>
        public StatusResponse GetCommEventCounter()
        {
            StatusResponse result = new StatusResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,            // 0: slave address
                MB_GET_COMM_EVENT_COUNTER // 1: read status function code
            );

            result.Status = CheckResponse(response, MB_GET_COMM_EVENT_COUNTER, data => data.Length == 6);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function read exception-status
                // 2 - status (MSB) or error code
                // 3 - status (LSB)
                // 4 - event count (MSB)
                // 5 - event count (LSB)
                result.StatusWord = (ushort)((response.Data[2] << 8) | response.Data[3]);
                result.EventCount = (ushort)((response.Data[4] << 8) | response.Data[5]);
            }

            return result;
        }

        /// <summary>
        /// FC12 - чтение записей журнала событий починенного устройства.
        /// </summary>
        /// <returns>до 64-х записей.</returns>
        public EventLogResponse GetCommEventLog()
        {
            EventLogResponse result = new EventLogResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,        // 0: slave address
                MB_GET_COMM_EVENT_LOG // 1: read event log function code
            );

            result.Status = CheckResponse(response, MB_GET_COMM_EVENT_LOG, data => (data.Length - 3) == data[2] && data.Length > 8);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function read event log
                // 2 - bytes count or error code
                // 3 - status (MSB)
                // 4 - status (LSB)
                // 5 - event count (MSB)
                // 6 - event count (LSB)
                // 7 - message count (MSB)
                // 8 - message count (LSB)
                // 9...events
                int len = response.Data[2] - 6;
                result.StatusWord   = (ushort)((response.Data[3] << 8) | response.Data[4]);
                result.EventCount   = (ushort)((response.Data[5] << 8) | response.Data[6]);
                result.MessageCount = (ushort)((response.Data[7] << 8) | response.Data[8]);
                result.Events       = new byte[len];
                Array.Copy(response.Data, 9, result.Events, 0, result.Events.Length);
            }

            return result;
        }

        /// <summary>
        /// FC15 - запись нескольких значений флагов (coils).
        /// </summary>
        /// <param name="address">адрес первого записываемого флага.</param>
        /// <param name="coils">значения записываемых флагов.</param>
        /// <returns>количество записаных флагов.</returns>
        public WriteResponse WriteMultipleCoils(ushort address, params bool[] coils)
        {
            WriteResponse result = new WriteResponse();

            int bits  = coils.Length;
            int bytes = bits / 8 + ((bits%8) == 0 ? 0 : 1);
            byte[] request = new byte[7+bytes];
            request[0] = _SlaveAddress;              // 0: slave address
            request[1] = MB_WRITE_MULTIPLE_COILS;    // 1: write function 
            request[2] = (byte)(address >> 8);       // 2: start coil (MSB)
            request[3] = (byte)(address & 0xFF);     // 3: start coil (LSB)
            request[4] = (byte)(bits >> 8);          // 4: coils count (MSB)
            request[5] = (byte)(bits & 0xFF);        // 5: coils count (LSB)
            request[6] = (byte)(bytes);              // 6: bytes count
            for (int i = 0; i < bits; i++)
            {
                int bit = i % 8;
                int num = 7 + i / 8;

                if (bit == 0)
                {
                    request[num] = 0x00;
                }

                if (coils[i])
                {
                    request[num] |= (byte)(1 << bit);
                }
            }

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, MB_WRITE_MULTIPLE_COILS, data => data.Length == 6);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - start address MSB
                // 3 - start address LSB
                // 4 - recorded count MSB
                // 5 - recorded count LSB
                if ((ushort)((response.Data[2] << 8) | response.Data[3]) == address)
                {
                    result.Recorded = (response.Data[4] << 8) | response.Data[5];
                }
                else
                {
                    result.Status = RequestStatus.DifferentWriteAddress;
                }
            }

            return result;
        }

        /// <summary>
        /// FC16 - запись нескольких значений регистров хранения.
        /// </summary>
        /// <param name="address">адрес первого записываемого регистра.</param>
        /// <param name="registers">значения записываемых регистров.</param>
        /// <returns>количество записаных регистров.</returns>
        public WriteResponse WriteMultipleRegisters(ushort address, params ushort[] registers)
        {
            WriteResponse result = new WriteResponse();

            int words = registers.Length;
            int bytes = 2 * words;
            byte[] request = new byte[7 + bytes];
            request[0] = _SlaveAddress;              // 0: slave address
            request[1] = MB_WRITE_MULTIPLE_REGISTER; // 1: write function 
            request[2] = (byte)(address >> 8);       // 2: start register (MSB)
            request[3] = (byte)(address & 0xFF);     // 3: start register (LSB)
            request[4] = (byte)(words >> 8);         // 4: registers count (MSB)
            request[5] = (byte)(words & 0xFF);       // 5: registers count (LSB)
            request[6] = (byte)(bytes);              // 6: bytes count
            int offset = 7;
            for (int i = 0; i < words; i++)
            {
                ushort word = registers[i];
                request[offset++] = (byte)(word >> 8);
                request[offset++] = (byte)(word & 0xFF);
            }

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, MB_WRITE_MULTIPLE_REGISTER, data => data.Length == 6);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - start address MSB
                // 3 - start address LSB
                // 4 - recorded count MSB
                // 5 - recorded count LSB
                if ((ushort)((response.Data[2] << 8) | response.Data[3]) == address)
                {
                    result.Recorded = (response.Data[4] << 8) | response.Data[5];
                }
                else
                {
                    result.Status = RequestStatus.DifferentWriteAddress;
                }
            }

            return result;
        }

        /// <summary>
        /// FC17 - чтение описания типа, текущего состояния и другой информации об устройстве.
        /// </summary>
        /// <returns>данные, структура которых специфична для конкретного устройства.</returns>
        public ByteResponse ReportSlaveId()
        {
            ByteResponse result = new ByteResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,     // 0: slave address
                MB_REPORT_SLAVE_ID // 1: report function code
            );

            result.Status = CheckResponse(response, MB_REPORT_SLAVE_ID, data => (data.Length - 3) == data[2]);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - report slave id function
                // 2 - byte count 
                // 3...device-depends data structure bytes or error code
                result.Data = new byte[response.Data[2]];
                Array.Copy(response.Data, 3, result.Data, 0, result.Data.Length);
            }

            return result;
        }

        /// <summary>
        /// FC20 - чтение массива записей из файла.
        /// </summary>
        /// <param name="SubRequests">массив под-запросов(файл,запись и ее длина).</param>
        /// <returns>значения рочитанных записей.</returns>
        public FileRecordResponse ReadFileRecord(params FileRecordSubReq[] SubRequests)
        {
            FileRecordResponse result = new FileRecordResponse();

            int bytes = 7 * SubRequests.Length;
            byte[] request = new byte[3+bytes];
            request[0] = _SlaveAddress;                                  // 0: slave address
            request[1] = MB_READ_FILE_RECORD;                            // 1: read file record function code
            request[2] = (byte)(bytes);                                  // 2: byte count
            int offset = 3;
            for (int i = 0; i < SubRequests.Length; i++)
            {
                request[offset++] = 0x06;                                 // 3: reference type
                request[offset++] = (byte)(SubRequests[i].File >> 8);     // 4: file number (MSB)
                request[offset++] = (byte)(SubRequests[i].File & 0xFF);   // 5: file number (LSB)
                request[offset++] = (byte)(SubRequests[i].Record >> 8);   // 6: record number (MSB)
                request[offset++] = (byte)(SubRequests[i].Record & 0xFF); // 7: record number (LSB)
                request[offset++] = (byte)(SubRequests[i].Length >> 8);   // 8: record length (MSB)
                request[offset++] = (byte)(SubRequests[i].Length & 0xFF); // 9: record length (LSB)
            }

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, MB_READ_FILE_RECORD, data => (data.Length-3) == data[2]);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - read file record function
                // 2 - byte count or error code
                // 3 - file resp. length
                // 4 - reference type
                // 5 - data0 (MSB)
                // 6 - data0 (LSB)
                // 7 - data1 (MSB)
                // 8 - data1 (LSB)
                int len = response.Data[2];
                result.Raw = new byte[len];
                Array.Copy(response.Data, 3, result.Raw, 0, len);

                result.Records = new ushort[0][];
                offset = 3;
                while (offset < response.Data.Length)
                {
                    len = response.Data[offset++];

                    if ((offset+len) <= response.Data.Length)
                    {
                        if (response.Data[offset++] == 0x06)
                        {
                            ushort[] rec = new ushort[(len-1)/2];
                            for (int i = 0; i < rec.Length; i++)
                            {
                                rec[i] = (ushort)((response.Data[offset++] << 8) | response.Data[offset++]);
                            }

                            Array.Resize(ref result.Records, result.Records.Length+1);
                            result.Records[result.Records.Length-1] = rec;
                        }
                        else
                        {
                            result.Status = RequestStatus.DifferentSubFunction;
                            break;
                        }
                    }
                    else
                    {
                        result.Status = RequestStatus.DataLengthInvalid;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// FC21 - запись массива записей в файл.
        /// </summary>
        /// <param name="records">массив записей.</param>
        /// <returns>количество записанных записей.</returns>
        public WriteResponse WriteFileRecord(params FileRecord[] records)
        {
            WriteResponse result = new WriteResponse();

            int bytes = 9 * records.Length;
            byte[] request = new byte[3+bytes];
            request[0] = _SlaveAddress;                                    // 0: slave address
            request[1] = MB_WRITE_FILE_RECORD;                             // 1: write file record function code
            request[2] = (byte)bytes;                                      // 2: byte count
            int offset = 3;
            for (int i = 0; i < records.Length; i++)
            {
                request[offset++] = 0x06;                                  // 3: reference type
                request[offset++] = (byte)(records[i].File >> 8);          // 4: file number (MSB)
                request[offset++] = (byte)(records[i].File & 0xFF);        // 5: file number (LSB)
                request[offset++] = (byte)(records[i].Record >> 8);        // 6: record number (MSB)
                request[offset++] = (byte)(records[i].Record & 0xFF);      // 7: record number (LSB)
                request[offset++] = (byte)(records[i].Data.Length >> 8);   // 8: record length (MSB)
                request[offset++] = (byte)(records[i].Data.Length & 0xFF); // 9: record length (LSB)
                for (int j = 0; j < records[i].Data.Length; j++)
                {
                    request[offset++] = (byte)(records[i].Data[j] >> 8);   // A: record data[j] (MSB)
                    request[offset++] = (byte)(records[i].Data[j] & 0xFF); // B: record data[j] (LSB)
                }
            }

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, MB_WRITE_FILE_RECORD, data => (data.Length - 3) == data[2]);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - write file record function
                // 2 - byte count or error code
                // 3 - reference type
                // 4 - file number (MSB)
                // 5 - file number (LSB)
                // 6 - record number (MSB)
                // 7 - record number (LSB)
                // 8 - record length (MSB)
                // 9 - record length (LSB)
                // A - data[0] (LSB)
                // B - dtaa[0] (MSB)

                result.Recorded = 0;

                offset  = 3;
                int rec = 0;
                while (offset < response.Data.Length)
                {
                    bool   match  = response.Data[offset++] == 0x06;
                    ushort file   = (ushort)((response.Data[offset++] << 8) | response.Data[offset++]);
                    ushort record = (ushort)((response.Data[offset++] << 8) | response.Data[offset++]);
                    ushort length = (ushort)((response.Data[offset++] << 8) | response.Data[offset++]);
                    match &= file == records[rec].File && record == records[rec].Record && length == records[rec].Data.Length;
                    for (int i = 0; i < length; i++)
                    {
                        ushort word = (ushort)((response.Data[offset++] << 8) | response.Data[offset++]);
                        match &= word == records[rec].Data[i];
                    }

                    if (match)
                    {
                        result.Recorded++;
                    }

                    rec++;
                }
            }

            return result;
        }

        /// <summary>
        /// FC22 - модификация значения регистра хранения.
        /// reg = (reg &amp; and) | (or &amp; ~and).
        /// </summary>
        /// <param name="address">адрес модифицируемого регистра.</param>
        /// <param name="and">маска "И".</param>
        /// <param name="or">маска "ИЛИ".</param>
        /// <returns>количество модифицированных регистров (1/0).</returns>
        public WriteResponse MaskWriteRegister(ushort address, ushort and, ushort or)
        {
            WriteResponse result = new WriteResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,          // 0: slave address
                MB_MASK_WRITE_REGISTER, // 1: read event log function code
                (byte)(address >> 8),   // 2: start register (MSB)
                (byte)(address & 0xFF), // 3: start register (LSB)
                (byte)(and >> 8),       // 4: and-mask (MSB)
                (byte)(and & 0xFF),     // 5: and-mask (LSB)
                (byte)(or >> 8),        // 6: or-mask (MSB)
                (byte)(or & 0xFF)       // 7: or-mask (LSB)
            );

            result.Status = CheckResponse(response, MB_MASK_WRITE_REGISTER, data => data.Length == 8);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - start register MSB
                // 3 - start register LSB
                // 4 - and-mask MSB
                // 5 - and-mask LSB
                // 6 - or-mask MSB
                // 7 - or-mask LSB
                if ((ushort)((response.Data[2] << 8) | response.Data[3]) == address)
                {
                    and ^= (ushort)((response.Data[4] << 8) | response.Data[5]);
                    or  ^= (ushort)((response.Data[6] << 8) | response.Data[7]);
                    result.Recorded = (and | or) == 0x0000 ? 1 : 0; 
                }
                else
                {
                    result.Status = RequestStatus.DifferentWriteAddress;
                }
            }

            return result;
        }

        /// <summary>
        /// FC23 - комбинация чтения и записи нескольких регистров в одной транзакции.
        /// </summary>
        /// <param name="ReadAddress">адрес первого считываемого регистра.</param>
        /// <param name="ReadCount">количество считываемых регистров.</param>
        /// <param name="WriteAddress">адрес первого записываемого регистра.</param>
        /// <param name="WriteData">значения записываемых регистров.</param>
        /// <returns>массив считанных значений регистров.</returns>
        public WordResponse ReadWriteMultipleRegisters(ushort ReadAddress, ushort ReadCount, ushort WriteAddress, params ushort[] WriteData)
        {
            WordResponse result = new WordResponse();

            int words = WriteData.Length;
            int bytes = 2 * words;
            byte[] request = new byte[11+bytes];
            request[0]  = _SlaveAddress;               //  0: slave address
            request[1]  = MB_READWRITE_MULTIPLE_REGS;  //  1: write function 
            request[2]  = (byte)(ReadAddress >> 8);    //  2: read start register (MSB)
            request[3]  = (byte)(ReadAddress & 0xFF);  //  3: read start register (LSB)
            request[4]  = (byte)(ReadCount >> 8);      //  4: read registers count (MSB)
            request[5]  = (byte)(ReadCount & 0xFF);    //  5: read registers count (LSB)
            request[6]  = (byte)(WriteAddress >> 8);   //  6: write start register (MSB)
            request[7]  = (byte)(WriteAddress & 0xFF); //  7: write start register (LSB)
            request[8]  = (byte)(words >> 8);          //  8: write registers count (MSB)
            request[9]  = (byte)(words & 0xFF);        //  9: write registers count (LSB)
            request[10] = (byte)(bytes);               // 10: write bytes count
            int offset = 11;
            for (int i = 0; i < words; i++)
            {
                ushort word = WriteData[i];
                request[offset++] = (byte)(word >> 8);
                request[offset++] = (byte)(word & 0xFF);
            }

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, MB_READWRITE_MULTIPLE_REGS, data => (data.Length - 3) == data[2]);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function read 
                // 2 - data length or error code
                // 3...data bytes (MSB first)
                bytes       = response.Data[2];
                result.Raw  = new byte[bytes];
                result.Data = new ushort[bytes/2];

                Array.Copy(response.Data, 3, result.Raw, 0, bytes);

                for (int i = 0; i < result.Data.Length; i++)
                {
                    result.Data[i] = (ushort)((response.Data[3+2*i] << 8) | response.Data[4+2*i]);
                }
            }

            return result;
        }

        /// <summary>
        /// FC24 - чтение содержимого очереди регистров в порядке очереди (FIFO).
        /// </summary>
        /// <param name="pointer">счетчик очереди.</param>
        /// <returns>данные в очереди (до 32 регистров).</returns>
        public WordResponse ReadFifoQueue(ushort pointer)
        {
            WordResponse result = new WordResponse();

            TransferResponse response = TransferPDU
            (
                _SlaveAddress,         // 0: slave address
                MB_READ_FIFO_QUEUE,    // 1: read fifo queue function code
                (byte)(pointer >> 8),  // 2: fifo pointer (MSB)
                (byte)(pointer & 0xFF) // 3: fifo pointer (LSB)
            );

            result.Status = CheckResponse(response, MB_READ_FIFO_QUEUE, data => data.Length > 5);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - function code
                // 2 - byte count MSB or error code
                // 3 - byte count LSB
                // 4 - fifo count MSB
                // 5 - fifo count LSB
                // 6 - fifo value0 MSB
                // 7 - fifo value0 LSB
                // ...
                int bytes = (response.Data[2] << 8) | response.Data[3];
                int fifos = (response.Data[4] << 8) | response.Data[5];
                if ((response.Data.Length-4) == bytes && ((response.Data.Length-6)/2) == fifos)
                {
                    result.Raw  = new byte[bytes];
                    result.Data = new ushort[fifos];

                    Array.Copy(response.Data, 4, result.Raw, 0, bytes);

                    for (int i = 0; i < result.Data.Length; i++)
                    {
                        result.Data[i] = (ushort)((response.Data[6+2*i] << 8) | response.Data[7+2*i]);
                    }
                }
                else
                {
                    result.Status = RequestStatus.DataLengthInvalid;
                }
            }

            return result;
        }

        /// <summary>
        /// FC43/01 - чтение базовой информации об устройстве.
        /// </summary>
        /// <returns>массив строк описания.</returns>
        public IdResponse ReadDeviceInfoBasic() => ReadDeviceIdentification(MB_READ_ID_CODE_BASIC);

        /// <summary>
        /// FC43/02 - чтение полной информации об устройстве.
        /// </summary>
        /// <returns>массив строк описания.</returns>
        public IdResponse ReadDeviceInfoComplete() => ReadDeviceIdentification(MB_READ_ID_CODE_REGULAR);

        /// <summary>
        /// Пользовательский запрос с функцией func.
        /// </summary>
        /// <param name="function">код функции пользовательского запроса.</param>
        /// <param name="parameters">параметры, определенные структурой пользовательской функции.</param>
        /// <returns>блок "сырых" данных.</returns>
        public ByteResponse UserRequest(byte function, params byte[] parameters)
        {
            ByteResponse result = new ByteResponse();

            byte[] request = new byte[2+parameters.Length];
            request[0] = _SlaveAddress; // 0: slave address
            request[1] = function;      // 1: user function code
            Array.Copy(parameters, 0, request, 2, parameters.Length);

            TransferResponse response = TransferPDU(request);

            result.Status = CheckResponse(response, function, data => data.Length > 2);

            if (result.Status == RequestStatus.Valid)
            {
                // 0 - slave address
                // 1 - user function 
                // 2...user data structure bytes or error code
                result.Data = new byte[response.Data.Length-2];
                Array.Copy(response.Data, 2, result.Data, 0, result.Data.Length);
            }

            return result;
        }
        #endregion
    }

    /// <summary>
    /// Базовый класс Modbus-клиента. Через последовательный порт.
    /// </summary>
    public abstract class ModbusSerialClient : ModbusClient
    {
        #region Fields
        private   string     _Port;
        private   int        _Baudrate;
        private   Parity     _Parity;
        private   StopBits   _StopBits;
        /// <summary>
        /// Serial port.
        /// </summary>
        protected SerialPort _Uart;
        /// <summary>
        /// Correct schecksum value.
        /// </summary>
        protected ushort     _CorrectCrc;
        /// <summary>
        /// Checksum length in bytes.
        /// </summary>
        protected int        _CrcLength;
        #endregion

        #region Internal
        /// <summary>
        /// Вычисляет контрольную сумму пакета.
        /// </summary>
        /// <param name="data">пакт данных.</param>
        /// <returns>16битная контрольная сумма.</returns>
        protected abstract ushort CheckSum(byte[] data);

        /// <summary>
        /// Отправляет ADU запроса через канальный уровень, 
        /// определенный в конкретной реализации клиента
        /// и возвращает ADU ответного пакета.
        /// </summary>
        /// <param name="data">Application Data Unit.</param>
        /// <returns>Application Data Unit.</returns>
        protected abstract TransferResponse TransferADU(params byte[] data);

        /// <summary>
        /// Реализация приема/передачи пакетов транспортного уровня.
        /// </summary>
        /// <param name="data">отправляемый пакет.</param>
        /// <returns>возвращаемый ответ.</returns>
        protected override TransferResponse TransferPDU(params byte[] data)
        {
            TransferResponse result = new TransferResponse();

            int    length    = data.Length;
            byte[] packet    = new byte[length+2];
            Array.Copy(data, 0, packet, 0, length);
            ushort crc       = CheckSum(data);
            packet[length+0] = (byte)(crc & 0xFF);
            packet[length+1] = (byte)(crc >> 8);

            TransferResponse response = TransferADU(packet);

            if (response.Status == RequestStatus.Valid)
            {
                if (CheckSum(response.Data) == _CorrectCrc)
                {
                    length        = response.Data.Length - _CrcLength;
                    result.Status = RequestStatus.Valid;
                    result.Data   = new byte[length];
                    Array.Copy(response.Data, 0, result.Data, 0, length);
                }
                else
                {
                    result.Status = RequestStatus.SerialBadCheckSum;
                }
            }
            else
            {
                result.Status = response.Status;
            }

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusSerialClient
        /// </summary>
        /// <param name="PhysicalAddress">строковое задание физического адреса например: "COM1:9600:8N1"</param>
        public ModbusSerialClient(string PhysicalAddress) : base()
        {
            _Uart = new SerialPort();

            PhysicalAddress = PhysicalAddress.ToUpper();

            if (Regex.IsMatch(PhysicalAddress, "^COM\\d{1,2}:\\d{1,6}:8(N|O|E|M|S)(1|2|3)$"))
            {
                var parts = PhysicalAddress.Split(':');
                _Port     = parts[0];
                _Baudrate = Convert.ToInt32(parts[1]);
                switch (parts[2][1])
                {
                    case 'N': _Parity = Parity.None;  break;
                    case 'O': _Parity = Parity.Odd;   break;
                    case 'E': _Parity = Parity.Even;  break;
                    case 'M': _Parity = Parity.Mark;  break;
                    case 'S': _Parity = Parity.Space; break;
                }
                _StopBits = (StopBits)Convert.ToInt32(parts[2].Substring(2));
            }
            else
            {
                _Port     = "COM1";
                _Baudrate = 9600;
                _Parity   = Parity.None;
                _StopBits = StopBits.One;
            }
        }

        /// <summary>
        /// Задает/возвращает имя последовательного порта.
        /// </summary>
        public string Port
        {
            get
            {
                return _Port;
            }

            set
            {
                _Port = value;
            }
        }

        /// <summary>
        /// Задает/возвращает скорсть последовательного порта.
        /// </summary>
        public int Baudrate
        {
            get
            {
                return _Baudrate;
            }

            set
            {
                _Baudrate = value;
            }
        }

        /// <summary>
        /// Задает/возвращает контроль четности последовательного порта.
        /// </summary>
        public Parity Parity
        {
            get
            {
                return _Parity;
            }

            set
            {
                _Parity = value;
            }
        }

        /// <summary>
        /// Задает/возвращает количество стоп-битов последовательного порта.
        /// </summary>
        public StopBits StopBits
        {
            get
            {
                return _StopBits;
            }

            set
            {
                _StopBits = value;
            }
        }

        /// <summary>
        /// Возвращает статус соединения по последовательному порту.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                if (_Uart == null)
                {
                    return false;
                }
                return _Uart.IsOpen;
            }
        }

        /// <summary>
        /// Устанавливает соединение по последовательному порту.
        /// </summary>
        /// <returns>статус соединения по последовательному порту.</returns>
        public override bool Connect()
        {
            _Uart.PortName = _Port;
            _Uart.BaudRate = _Baudrate;
            _Uart.DataBits = 8;
            _Uart.Parity   = _Parity;
            _Uart.StopBits = _StopBits;

            try
            {
                _Uart.Open();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Закрывает соединение по последовательному порту.
        /// </summary>
        public override void Disconnect()
        {
            if (_Uart.IsOpen)
            {
                _Uart.Close();
            }
        }
        #endregion
    }

    /// <summary>
    /// Базовый класс Modbus-клиента. Через Ethernet/IP.
    /// </summary>
    public abstract class ModbusIpClient : ModbusClient
    {
        #region Modbus-IP Protocol
        private const byte IP_HEADER_LENGTH = 6;
        private const byte IP_PROTOCOL      = 0x0000;
        #endregion

        #region Fields
        private   ushort _TransactionId;
        /// <summary>
        /// Destination IP address.
        /// </summary>
        protected string _IpAddress;
        /// <summary>
        /// Destination TCP/UDP port.
        /// </summary>
        protected ushort _Port;
        #endregion

        #region Internal
        /// <summary>
        /// Отправляет ADU запроса через канальный уровень, 
        /// определенный в конкретной реализации клиента
        /// и возвращает ADU ответного пакета.
        /// </summary>
        /// <param name="data">Application Data Unit.</param>
        /// <returns>Application Data Unit.</returns>
        protected abstract TransferResponse TransferADU(params byte[] data);
        
        /// <summary>
        /// Реализация приема/передачи пакетов транспортного уровня.
        /// </summary>
        /// <param name="data">отправляемый пакет.</param>
        /// <returns>возвращаемый ответ.</returns>
        protected override TransferResponse TransferPDU(params byte[] data)
        {
            TransferResponse result = new TransferResponse();

            int length    = data.Length;
            byte[] packet = new byte[IP_HEADER_LENGTH+length];
            packet[0] = (byte)(_TransactionId >> 8);    // transaction id MSB
            packet[1] = (byte)(_TransactionId & 0xFF);  // transaction id LSB
            packet[2] = (byte)(IP_PROTOCOL >> 8);       // protocol type MSB
            packet[3] = (byte)(IP_PROTOCOL & 0xFF);     // protocol type LSB
            packet[4] = (byte)(length >> 8);            // data length MSB
            packet[5] = (byte)(length & 0xFF);          // data length LSB
            Array.Copy(data, 0, packet, IP_HEADER_LENGTH, length);

            TransferResponse response = TransferADU(packet);

            if (response.Status == RequestStatus.Valid)
            {
                if (response.Data.Length > IP_HEADER_LENGTH)
                {
                    if (((response.Data[0] << 8) | response.Data[1]) == _TransactionId)
                    {
                        if (((response.Data[2] << 8) | response.Data[3]) == IP_PROTOCOL)
                        {
                            length = (response.Data[4] << 8) | response.Data[5];

                            if ((length + IP_HEADER_LENGTH) == response.Data.Length)
                            {
                                result.Status = RequestStatus.Valid;
                                result.Data   = new byte[length];
                                Array.Copy(response.Data, IP_HEADER_LENGTH, result.Data, 0, length);
                            }
                            else
                            {
                                result.Status = RequestStatus.IpDataLengthInvalid;
                            }
                        }
                        else
                        {
                            result.Status = RequestStatus.IpDifferentProtocol;
                        }
                    }
                    else
                    {
                        result.Status = RequestStatus.IpDifferentTransaction;
                    }
                }
                else
                {
                    result.Status = RequestStatus.IpPacketLengthInvalid;
                }
            }
            else
            {
                result.Status = response.Status;
            }

            _TransactionId++;

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusIpClient
        /// </summary>
        /// <param name="PhysicalAddress">строковое задание физического адреса например: "192.168.1.100:502"</param>
        public ModbusIpClient(string PhysicalAddress) : base()
        {
            _TransactionId = 0;

            if (Regex.IsMatch(PhysicalAddress, "^(\\d{1,3}\\.){3}\\d{1,3}(:\\d{1,5})?$"))
            {
                int s = PhysicalAddress.IndexOf(':');

                if (s == -1)
                {
                    _IpAddress = PhysicalAddress;
                    _Port      = 502;
                }
                else
                {
                    _IpAddress = PhysicalAddress.Substring(0, s);
                    _Port      = Convert.ToUInt16(PhysicalAddress.Substring(s + 1));
                }
            }
            else
            {
                _IpAddress = "127.0.0.1";
                _Port      = 502;
            }
        }

        /// <summary>
        /// Задает/возвращает IP адрес устройства.
        /// </summary>
        public string IpAddress
        {
            get
            {
                return _IpAddress;
            }

            set
            {
                _IpAddress = value;
            }
        }

        /// <summary>
        /// Задает/возвращает TCP/IP или UDP/IP порт устройства.
        /// </summary>
        public ushort Port
        {
            get
            {
                return _Port;
            }

            set
            {
                _Port = value;
            }
        }
        #endregion
    }
    #endregion

    #region Clients
    /// <summary>
    /// Реализует методы для работы с ModbusRTU устройствами через последовательный порт.
    /// </summary>
    public class ModbusRtuClient : ModbusSerialClient
    {
        #region Const
        private const ushort CRC16_POLYNOM    = 0xA001;
        private const ushort CRC16_INITIAL    = 0xFFFF;
        private const int    RESP_BUFFER_SIZE = 1024;
        #endregion

        #region Internal
        /// <summary>
        /// Реализация контрольной вычисления суммы CRC16.
        /// </summary>
        /// <param name="data">пакет данных.</param>
        /// <returns>16битная контрольная сумма.</returns>
        protected override ushort CheckSum(byte[] data)
        {
            ushort result = CRC16_INITIAL;

            for (int n = 0; n < data.Length; n++)
            {
                result ^= data[n];

                for (int i = 0; i < 8; i++)
                {
                    result = (ushort)((result >> 1) ^ ((result & 0x0001) == 0x0001 ? CRC16_POLYNOM : 0x0000));
                }
            }

            return result;
        }

        /// <summary>
        /// Отправляет ADU запроса через последовательный порт,
        /// и возвращает ADU ответного пакета.
        /// </summary>
        /// <param name="data">Application Data Unit.</param>
        /// <returns>Application Data Unit.</returns>
        protected override TransferResponse TransferADU(params byte[] data)
        {
            TransferResponse result = new TransferResponse();

            try
            {
                _Uart.DiscardInBuffer();
                _Uart.Write(data, 0, data.Length);

                if (LoggingEnable)
                {
                    SaveToLogFile("RTU_TX", data.Length, data);
                }

                int      reads  = 0;
                byte[]   buffer = new byte[RESP_BUFFER_SIZE];
                DateTime start  = DateTime.Now;
                while ((DateTime.Now - start).TotalMilliseconds < _Timeout)
                {
                    reads += _Uart.Read(buffer, reads, _Uart.BytesToRead);
                }

                if (LoggingEnable)
                {
                    SaveToLogFile("RTU_RX", reads, buffer);
                }

                if (reads > 3)
                {
                    result.Status = RequestStatus.Valid;
                    result.Data   = new byte[reads];
                    Array.Copy(buffer, 0, result.Data, 0, reads);
                }
                else
                {
                    result.Status = RequestStatus.SerialLengthInvalid;
                }
            }
            catch
            {
                result.Status = RequestStatus.SerialPortError;
            }

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusRtuClient
        /// </summary>
        /// <param name="PhysicalAddress">строковое задание физического адреса например: "COM1:9600:8N1"</param>
        public ModbusRtuClient(string PhysicalAddress) : base(PhysicalAddress)
        {
            _CrcLength  = 2;
            _CorrectCrc = 0x0000;
        }
        #endregion
    }

    /// <summary>
    /// Реализует методы для работы с ModbusASCII устройствами через последовательный порт.
    /// </summary>
    public class ModbusAsciiClient : ModbusSerialClient
    {
        #region Const
        private const int  ASCII_START_SIZE = 1;
        private const int  ASCII_BYTE_SIZE  = 2;
        private const int  ASCII_STOP_SIZE  = 2;
        private const byte ASCII_START_MARK = 0x3A;
        private const byte ASCII_STOP1_MARK = 0x0D;
        private const byte ASCII_STOP2_MARK = 0x0A;
        private const int  RESP_BUFFER_SIZE = 1024;
        #endregion

        #region Internal
        private byte TetradeEncode(byte tetrade)
        {
            tetrade &= 0x0F;
            return (byte)(tetrade + (tetrade < 10 ? 0x30 : 0x37));
        }

        private int TetradeDecode(byte tetrade)
        {
            if (0x2F < tetrade && tetrade < 0x3A)
            {
                return tetrade - 0x30;
            }

            tetrade &= 0xDF;
            if (0x40 < tetrade && tetrade < 0x47)
            {
                return tetrade - 0x37;
            }

            return -1;
        }

        /// <summary>
        /// Реализация контрольной вычисления суммы CRC8.
        /// </summary>
        /// <param name="data">пакет данных.</param>
        /// <returns>8битная контрольная сумма.</returns>
        protected override ushort CheckSum(byte[] data)
        {
            ushort result = 0;

            for (int i = 0; i < data.Length; i++)
            {
                result += data[i];
            }

            result = (ushort)(((byte)0 - result) & 0xFF);

            return (ushort)((TetradeEncode((byte)result) << 8) | TetradeEncode((byte)(result >> 4)));
        }

        /// <summary>
        /// Отправляет ADU запроса через последовательный порт, и возвращает ADU ответного пакета.
        /// </summary>
        /// <param name="data">Application Data Unit.</param>
        /// <returns>Application Data Unit.</returns>
        protected override TransferResponse TransferADU(params byte[] data)
        {
            TransferResponse result = new TransferResponse();

            byte[] packet = new byte[ASCII_START_SIZE+ASCII_BYTE_SIZE*(data.Length-_CrcLength)+ASCII_STOP_SIZE];
            int    n      = 0;
            int    p      = data.Length - ASCII_BYTE_SIZE * _CrcLength;
            packet[n++] = ASCII_START_MARK;
            for (int i = 0; i < p; i++)
            {
                packet[n++] = TetradeEncode((byte)(data[i] >> 4));
                packet[n++] = TetradeEncode(data[i]);
            }
            packet[n++] = data[p+0];
            packet[n++] = data[p+1];
            packet[n++] = ASCII_STOP1_MARK;
            packet[n++] = ASCII_STOP2_MARK;

            try
            {
                _Uart.DiscardInBuffer();
                _Uart.Write(packet, 0, packet.Length);

                if (LoggingEnable)
                {
                    SaveToLogFile("ASCII_TX", packet.Length, packet);
                }

                int      reads  = 0;
                byte[]   buffer = new byte[RESP_BUFFER_SIZE];
                DateTime start  = DateTime.Now;
                while ((DateTime.Now - start).TotalMilliseconds < _Timeout)
                {
                    reads += _Uart.Read(buffer, reads, _Uart.BytesToRead);
                }

                if (LoggingEnable)
                {
                    SaveToLogFile("ASCII_RX", reads, buffer);
                }

                if (reads > 3)
                {
                    if (buffer[0] == ASCII_START_MARK)
                    {
                        if (buffer[reads - ASCII_STOP_SIZE + 0] == ASCII_STOP1_MARK && buffer[reads - ASCII_STOP_SIZE + 1] == ASCII_STOP2_MARK)
                        {
                            result.Status = RequestStatus.Valid;
                            result.Data   = new byte[(reads - (ASCII_START_SIZE + ASCII_STOP_SIZE)) / ASCII_BYTE_SIZE];
                            n = ASCII_START_SIZE;
                            for (int i = 0; i < result.Data.Length; i++)
                            {
                                int msb = TetradeDecode(buffer[n++]);
                                int lsb = TetradeDecode(buffer[n++]);

                                if (msb < 0 || lsb < 0)
                                {
                                    result.Status = RequestStatus.AsciiIllegalSymbol;
                                    break;
                                }

                                result.Data[i] = (byte)((msb << 4) | lsb);
                            }
                        }
                        else
                        {
                            result.Status = RequestStatus.AsciiMissingStopMark;
                        }
                    }
                    else
                    {
                        result.Status = RequestStatus.AsciiMissingStartMark;
                    }
                }
                else
                {
                    result.Status = RequestStatus.SerialLengthInvalid;
                }
            }
            catch
            {
                result.Status = RequestStatus.SerialPortError;
            }

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusRtuClient
        /// </summary>
        /// <param name="PhysicalAddress">строковое задание физического адреса например: "COM1:9600:8N1"</param>
        public ModbusAsciiClient(string PhysicalAddress) : base(PhysicalAddress)
        {
            _CrcLength  = 1;
            _CorrectCrc = 0x3030;
        }
        #endregion
    }

    /// <summary>
    /// Реализует методы для работы с ModbusTCP устройствами через TCP/IP стек.
    /// </summary>
    public class ModbusTcpClient : ModbusIpClient
    {
        #region Const
        private const int RESP_BUFFER_SIZE = 1024;
        #endregion

        #region Fields
        private TcpClient     _TcpClient;
        private NetworkStream _NetworkStream;
        #endregion

        #region Internal
        /// <summary>
        /// Отправляет ADU запроса через TCP/IP стек, и возвращает ADU ответного пакета.
        /// </summary>
        /// <param name="data">Application Data Unit.</param>
        /// <returns>Application Data Unit.</returns>
        protected override TransferResponse TransferADU(params byte[] data)
        {
            TransferResponse result = new TransferResponse();

            if (_TcpClient.Client.Connected)
            {
                try
                {
                    _NetworkStream.Write(data, 0, data.Length);

                    if (LoggingEnable)
                    {
                        SaveToLogFile("TCP_TX", data.Length, data);
                    }

                    byte[] response = new byte[RESP_BUFFER_SIZE];
                    int    length   = _NetworkStream.Read(response, 0, RESP_BUFFER_SIZE);

                    if (LoggingEnable)
                    {
                        SaveToLogFile("TCP_RX", length, response);
                    }

                    result.Status = RequestStatus.Valid;
                    result.Data   = new byte[length];
                    Array.Copy(response, 0, result.Data, 0, length);
                }
                catch
                {
                    result.Status = RequestStatus.TcpNetworkError;
                }
            }
            else
            {
                result.Status = RequestStatus.TcpNoConnection;
            }

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusTcpClient
        /// </summary>
        /// <param name="PhysicalAddress">строковое задание физического адреса например: "192.168.1.100:502"</param>
        public ModbusTcpClient(string PhysicalAddress) : base(PhysicalAddress)
        {

        }

        /// <summary>
        /// Возвращает статус TCP соединения.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                if (_TcpClient == null)
                {
                    return false;
                }
                return _TcpClient.Connected;
            }
        }

        /// <summary>
        /// Устанавливает TCP соединение с устройством по указанному IP адресу.
        /// </summary>
        /// <returns>статус соединения.</returns>
        public override bool Connect()
        {
            _TcpClient = new TcpClient();
            var result = _TcpClient.BeginConnect(_IpAddress, _Port, null, null);

            if (result.AsyncWaitHandle.WaitOne(_Timeout))
            {
                try
                {
                    _TcpClient.EndConnect(result);
                    _NetworkStream             = _TcpClient.GetStream();
                    _NetworkStream.ReadTimeout = _Timeout;

                    return _TcpClient.Connected;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Закрывает TCP соединение.
        /// </summary>
        public override void Disconnect()
        {
            if (_NetworkStream != null)
            {
                _NetworkStream.Close();
                _NetworkStream.Dispose();
                _NetworkStream = null;
            }

            if (_TcpClient != null)
            {
                _TcpClient.Close();
                _TcpClient = null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Реализует методы для работы с ModbusUDP устройствами через UDP/IP стек.
    /// </summary>
    public class ModbusUdpClient : ModbusIpClient
    {
        #region Const
        private const int RESP_BUFFER_SIZE = 1024;
        #endregion

        #region Fields
        private UdpClient  _UdpClient;
        private IPEndPoint _SlaveEndPoint;
        private IPEndPoint _LocalEndPoint;
        #endregion

        #region Internal
        /// <summary>
        /// Отправляет ADU запроса через UDP/IP стек, и возвращает ADU ответного пакета.
        /// </summary>
        /// <param name="data">Application Data Unit.</param>
        /// <returns>Application Data Unit.</returns>
        protected override TransferResponse TransferADU(params byte[] data)
        {
            TransferResponse result = new TransferResponse();

            try
            {
                _UdpClient.Send(data, data.Length, _SlaveEndPoint);

                if (LoggingEnable)
                {
                    SaveToLogFile("UDP_TX", data.Length, data);
                }

                _LocalEndPoint = new IPEndPoint(_SlaveEndPoint.Address, ((IPEndPoint)_UdpClient.Client.LocalEndPoint).Port);
                result.Data    = _UdpClient.Receive(ref _LocalEndPoint);
                result.Status  = RequestStatus.Valid;

                if (LoggingEnable)
                {
                    SaveToLogFile("UDP_RX", result.Data.Length, result.Data);
                }
            }
            catch
            {
                result.Status = RequestStatus.UdpNetworkError;
            }

            return result;
        }
        #endregion

        #region Public
        /// <summary>
        /// Инициализирует новый экземпляр класса ModbusUdpClient
        /// </summary>
        /// <param name="PhysicalAddress">строковое задание физического адреса например: "192.168.1.100:502"</param>
        public ModbusUdpClient(string PhysicalAddress) : base(PhysicalAddress)
        {

        }

        /// <summary>
        /// Возвращает статус UDP соединения.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                return _UdpClient != null;
            }
        }

        /// <summary>
        /// Устанавливает UDP соединение с устройством по указанному IP адресу.
        /// </summary>
        /// <returns>статус соединения.</returns>
        public override bool Connect()
        {
            try
            {
                _UdpClient     = new UdpClient();
                _UdpClient.Client.ReceiveTimeout = _Timeout;
                IPAddress IP   = IPAddress.Parse(_IpAddress);
                _SlaveEndPoint = new IPEndPoint(IP, _Port);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Закрывает UDP соединение.
        /// </summary>
        public override void Disconnect()
        {
            if (_UdpClient != null)
            {
                _UdpClient.Close();
                _UdpClient = null;
            }
        }
        #endregion
    }
    #endregion
}