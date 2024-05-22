using MvUtils;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DSEV.Schemas
{
    public class Serial : IDisposable
    {
        public enum Status : uint
        {
            OK = 0x0000,
            NAK = 0x00F1,
            NotReady = 0x00F2,
            ChecksumError = 0x00F3,
            FpgaDataMissMatch = 0x00F4,
            UnknownAddress = 0x00F5,
            FrameError = 0x00F6,
            FlashMissMatch = 0x00F7,
            RangeError = 0x00F8,
            DeviceTimeout = 0x00F9,
            NotOpen = 0xFFF2,
            ParsingError = 0xFFF6,
            Timeout = 0xFFF9,
            ReceiveError = 0xFFFA,
            Unknown = 0xFFFF
        }

        public class InvalidVariableException : Exception
        {
            public string VariableName { get; private set; }
            public int Value { get; private set; }

            public InvalidVariableException(string name, int value)
                : base($"{name}은 {value}값을 허용하지 않습니다.")
            {
                VariableName = name;
                Value = value;
            }
        }

        #region DLL FUNCTIONS
        const string DLL_NAME = "mvenc_x64.dll";
        //const string DLL_NAME = "mvenc_x86.dll";

        [DllImport(DLL_NAME)]
        private static extern void MvsEncRelease();

        [DllImport(DLL_NAME)]
        private static extern int MvsEncCreateHandler();
        [DllImport(DLL_NAME)]
        private static extern void MvsEncCloseHandler(int handle);
        [DllImport(DLL_NAME)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MvsEncIsValidHandle(int handle);

        [DllImport(DLL_NAME)]
        [return:MarshalAs(UnmanagedType.I1)]
        private static extern bool MvsEncOpen(int handle, string port);
        [DllImport(DLL_NAME)]
        private static extern void MvsEncClose(int handle);
        [DllImport(DLL_NAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MvsEncIsOpen(int handle);
        [DllImport(DLL_NAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MvsEncPurge(int handle);

        [DllImport(DLL_NAME)]
        private static extern uint MvsEncSendData(int handle, char command, ushort address, uint data, ref uint returnData);
        #endregion

        #region STATIC FUNCTIONS
        public static void Release()
        {
            MvsEncRelease();
        }
        #endregion

        private int _handle;

        public bool IsOpen
        {
            get
            {
                return MvsEncIsOpen(_handle);
            }
        }

        public bool IsValid
        {
            get
            {
                return IsOpen && Read(0, out _) == 0;
            }
        }

        public Serial()
        {
            _handle = MvsEncCreateHandler();
            if (_handle <= 0)
            {
                throw new InvalidOperationException("HANDLE 생성 실패.");
            }
        }

        public bool Open(string port)
        {
            return MvsEncOpen(_handle, port);
        }

        public void Purge()
        {
            MvsEncPurge(_handle);
        }

        public void Close()
        {
            MvsEncClose(_handle);
            Dispose();
        }

        public Status Read(ushort address, out uint data)
        {
            uint returnTemp = 0;
            var status = MvsEncSendData(_handle, 'R', address, 0, ref returnTemp);
            data = returnTemp;
            if (!Enum.IsDefined(typeof(Status), status))
            {
                return Status.Unknown;
            }
            return (Status)status;
        }

        public Status Write(ushort address, uint data)
        {
            uint returnTemp = 0;
            var status = MvsEncSendData(_handle, 'W', address, data, ref returnTemp);
            if (Enum.IsDefined(typeof(Status), status))
            {
                return Status.Unknown;
            }
            return (Status)status;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (MvsEncIsValidHandle(_handle))
                {
                    MvsEncPurge(_handle);
                    if (IsOpen)
                    {
                        Close();
                    }
                    MvsEncCloseHandler(_handle);
                    _handle = 0;
                }
                disposedValue = true;
            }
        }

        ~Serial() {
          Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class Enc852 : Serial
    {
        #region Enums
        public enum EncoderType
        {
            DIFF,
            TTL,
            Virtual
        }

        public enum EncoderMulti
        {
            x1 = 1,
            x2 = 2,
            x4 = 4
        }

        public enum ConditionFactor
        {
            DI0,
            DI1,
            DI2,
            DI3,
            High = 7,
            Low,
        }

        public enum LogicalOperator
        {
            And,
            Or,
            Xor,
            Nand
        }

        public enum ResetFactor
        {
            Disable,
            DI0,
            DI1,
            DI2,
            DI3
        }

        public enum TriggerPosDirection
        {
            Disable,
            Positive,
            Negative,
            BiDirection
        }

        public enum TriggerBase
        {
            Count,
            Position
        }

        public enum EncoderDirection
        {
            Normal,
            Reverse
        }

        public enum TriggerOutputMode
        {
            Auto,
            Time
        }
        #endregion

        public 직렬포트 포트 = 직렬포트.None;
        public Enc852(직렬포트 port) => this.포트 = port;

        // com포트변경 by LHD
        public Boolean Clear()
        {
            try
            {
                this.Open(this.포트.ToString());
                this.ClearEncoderPositionAll();
                this.ClearTriggerAll();
                Debug.WriteLine("트리거보드 초기화 완료", "트리거보드");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString(), "트리거보드 오류");
                Utils.MessageBox("트리거보드", e.ToString(), 2000);
                return false;
            }
            return true;
        }


        #region Inner Class
        public struct TriggerControl
        {
            private uint _data;
            public EncoderType EncoderType
            {
                get
                {
                    int temp = (int)_data & 0xF;
                    if (!Enum.IsDefined(typeof(EncoderType), temp))
                    {
                        throw new InvalidVariableException("EncoderType", temp);
                    }
                    return (EncoderType)temp;
                }

                set
                {
                    _data = (_data & ~0xFu) | ((uint)value & 0xFu);
                }
            }

            public int EncoderChannel
            {
                get
                {
                    int temp = (int)(_data >> 4) & 0xF;
                    if (temp < 0 || temp > 3)
                    {
                        throw new InvalidVariableException("EncoderChannel", temp);
                    }
                    return temp;
                }

                set
                {
                    if (value < 0 || value > 3)
                    {
                        throw new InvalidVariableException("EncoderChannel", value);
                    }
                    _data = (_data & ~(0xFu << 4)) | ((uint)(value & 0xFu) << 4);
                }
            }

            public EncoderMulti Multi
            {
                get
                {
                    int temp = (int)(_data >> 8) & 0xF;
                    if (!Enum.IsDefined(typeof(EncoderMulti), temp))
                    {
                        throw new InvalidVariableException("Multi", temp);
                    }
                    return (EncoderMulti)temp;
                }

                set
                {
                    _data = (_data & ~(0xFu << 8)) | (((uint)value & 0xFu) << 8);
                }
            }

            public ConditionFactor ConditionFactor
            {
                get
                {
                    int temp = (int)(_data >> 12) & 0xF;
                    if (!Enum.IsDefined(typeof(ConditionFactor), temp))
                    {
                        throw new InvalidVariableException("ConditionFactor", temp);
                    }
                    return (ConditionFactor)temp;
                }

                set
                {
                    _data = (_data & ~(0xFu << 12)) | (((uint)value & 0xFu) << 12);
                }
            }

            public LogicalOperator TriggerOut
            {
                get
                {
                    int temp = (int)(_data >> 16) & 0xF;
                    if (!Enum.IsDefined(typeof(LogicalOperator), temp))
                    {
                        throw new InvalidVariableException("TriggerOut", temp);
                    }
                    return (LogicalOperator)temp;
                }

                set
                {
                    _data = (_data & ~(0xFu << 16)) | (((uint)value & 0xFu) << 16);
                }
            }

            public ResetFactor ResetFactor
            {
                get
                {
                    int temp = (int)(_data >> 20) & 0xF;
                    if (!Enum.IsDefined(typeof(ResetFactor), temp))
                    {
                        throw new InvalidVariableException("ResetFactor", temp);
                    }
                    return (ResetFactor)temp;
                }

                set
                {
                    _data = (_data & ~(0xFu << 20)) | (((uint)value & 0xFu) << 20);
                }
            }

            public TriggerPosDirection PosDirection
            {
                get
                {
                    int temp = (int)(_data >> 24) & 0xF;
                    if (!Enum.IsDefined(typeof(TriggerPosDirection), temp))
                    {
                        throw new InvalidVariableException("PosDirection", temp);
                    }
                    return (TriggerPosDirection)temp;
                }

                set
                {
                    _data = (_data & ~(0xFu << 24)) | (((uint)value & 0xFu) << 24);
                }
            }

            public bool EncoderCorrection
            {
                get
                {
                    int temp = (int)(_data >> 30) & 0x1;
                    if (temp < 0 || temp > 1)
                    {
                        throw new InvalidVariableException("EncoderCorrection", temp);
                    }
                    return temp != 0;
                }

                set
                {
                    _data = (_data & ~(0x1u << 30)) | (((value ? 1u : 0u) & 0x1u) << 30);
                }
            }

            public TriggerBase TriggerBase
            {
                get
                {
                    int temp = (int)(_data >> 31) & 0x1;
                    if (!Enum.IsDefined(typeof(TriggerBase), temp))
                    {
                        throw new InvalidVariableException("TriggerBase", temp);
                    }
                    return (TriggerBase)temp;
                }

                set
                {
                    _data = (_data & ~(0x1u << 31)) | (((uint)value & 0x1u) << 31);
                }
            }

            private TriggerControl(uint data)
            {
                _data = data;
            }

            public override string ToString()
            {
                return $"0x{_data:X8}";
            }

            public static implicit operator uint(TriggerControl data)
            {
                return data._data;
            }

            public static implicit operator TriggerControl(uint data)
            {
                return new TriggerControl(data);
            }
        }
        #endregion

        #region Setter & Getter
        private Status Read(int ch, ushort address, out uint data)
        {
            return Read((ushort)(ch * 0x100 + (address & 0xFF)), out data);
        }

        private Status Write(int ch, ushort address, uint data)
        {
            return Write((ushort)(ch * 0x100 + (address & 0xFF)), data);
        }

        public Status GetTriggerControl(int ch, out TriggerControl control)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0000, out temp);

            }
            finally
            {
                control = temp;
            }
        }

        public Status SetTriggerControl(int ch, TriggerControl control)
        {
            return Write(ch, 0x0000, control);
        }

        public Status GetTriggerGenerator(int ch, out ushort cycle, out ushort pulseHigh)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0001, out temp);

            }
            finally
            {
                cycle = (ushort)temp;
                pulseHigh = (ushort)temp;
            }
        }

        public Status SetTriggerGenerator(int ch, ushort cycle, ushort pulseHigh)
        {
            uint temp = (uint)cycle << 16 | (uint)pulseHigh;
            return Write(ch, 0x0001, temp);
        }

        public Status GetTriggerGeneratorCycle32(int ch, out uint cycle)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0001, out temp);
            }
            finally
            {
                cycle = temp;
            }
        }

        public Status SetTriggerGeneratorCycle32(int ch, uint cycle)
        {
            return Write(ch, 0x0001, cycle);
        }

        public Status GetTriggerPosition0(int ch, out int position)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0002, out temp);
            }
            finally
            {
                position = (int)((long)temp - 0x80000000);
            }
        }

        public Status SetTriggerPosition0(int ch, int position)
        {
            uint temp = (uint)((long)position + 0x80000000);
            return Write(ch, 0x0002, temp);
        }

        public Status GetTriggerPosition1(int ch, out int position)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0003, out temp);
            }
            finally
            {
                position = (int)((long)temp - 0x80000000);
            }
        }

        public Status SetTriggerPosition1(int ch, int position)
        {
            uint temp = (uint)((long)position + 0x80000000);
            return Write(ch, 0x0003, temp);
        }

        public Status GetEncoderDirection(int ch, out EncoderDirection direction)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0007, out temp);
            }
            finally
            {
                direction = temp != 0 ? EncoderDirection.Reverse : EncoderDirection.Normal;
            }
        }

        public Status SetEncoderDirection(int ch, EncoderDirection direction)
        {
            return Write(ch, 0x0007, (uint)direction);
        }

        public Status GetTriggerOutputMode(int ch, out TriggerOutputMode mode)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x0008, out temp);
            }
            finally
            {
                mode = temp != 0 ? TriggerOutputMode.Time : TriggerOutputMode.Auto;
            }
        }

        public Status SetTriggerOutputMode(int ch, TriggerOutputMode mode)
        {
            return Write(ch, 0x0008, (uint)mode);
        }

        public Status GetTriggerPulseWidth(int ch, out uint pulseWidth)
        {
            return Read(ch, 0x0009, out pulseWidth);
        }

        public Status SetTriggerPulseWidth(int ch, uint pulseWidth)
        {
            return Write(ch, 0x0009, pulseWidth);
        }

        public Status GetTriggerDelay(int ch, out uint delay)
        {
            return Read(ch, 0x000A, out delay);
        }

        public Status SetTriggerDelay(int ch, uint delay)
        {
            return Write(ch, 0x000A, delay);
        }

        public Status GetEncoderResetValue(int ch, out int position)
        {
            uint temp = 0;
            try
            {
                return Read(ch, 0x000B, out temp);
            }
            finally
            {
                position = (int)((long)temp - 0x80000000);
            }
        }
        public Status SetEncoderResetValue(int ch, int position)
        {
            uint temp = (uint)((long)position + 0x80000000);
            return Write(ch, 0x000B, temp);
        }

        public Status GetVirtualEncoderFrequency(int ch, out int frequency)
        {
            uint temp = 0;
            try
            {
                return Read((ushort)(0x0400 + ch), out temp);
            }
            finally
            {
                frequency = (int)temp; ;
            }
        }

        public Status SetVirtualEncoderFrequency(int ch, int frequency)
        {
            return Write((ushort)(0x0400 + ch), (uint)frequency);
        }

        public Status GetGetDiState(out bool[] state)
        {
            uint temp = 0;
            try
            {
                return Read(0x0500, out temp);
            }
            finally
            {
                state = new bool[4];
                for (int i = 0; i < 4; i++)
                {
                    state[i] = ((temp >> i) & 0x1) != 0;
                }
            }
        }

        public Status GetDiCount(int ch, out uint count)
        {
            return Read((ushort)(0x0501 + ch), out count);
        }

        public Status GetTriggerCount(int ch, out uint count)
        {
            return Read((ushort)(0x0508 + ch), out count);
        }

        public Status GetEncoderPosition(int ch, out int position)
        {
            uint temp = 0;
            try
            {
                return Read((ushort)(0x050C + ch), out temp);
            }
            finally
            {
                position = (int)((long)temp - 0x80000000);
            }
        }

        public Status GetErrorCount(int ch, out uint count)
        {
            return Read((ushort)(0x0510 + ch), out count);
        }

        public Status GetFirmwareVersion(out double version)
        {
            uint temp = 0;
            try
            {
                return Read(0xF000, out temp);
            }
            finally
            {
                version = (double)(temp & 0xFFFF) / 100;
            }
        }

        public Status GetLogicVersion(out double version)
        {
            uint temp = 0;
            try
            {
                return Read(0xF001, out temp);
            }
            finally
            {
                version = (double)(temp & 0xFFFF) / 100;
            }
        }

        public Status LoadDefaultParameters()
        {
            return Write(0xF002, 1);
        }

        public Status SaveFlash()
        {
            return Write(0xF003, 1);
        }

        public Status LoadFlash()
        {
            return Write(0xF004, 1);
        }

        public Status ClearAll()
        {
            return Write(0xF005, 0xFFFFFFFF);
        }

        public Status ClearDiAll()
        {
            return Write(0xF005, 0x7Fu);
        }

        public Status ClearDiCount(int ch)
        {
            return Write(0xF005, 0x1u << ch);
        }

        public Status ClearTriggerAll()
        {
            return Write(0xF005, 0x780u);
        }

        public Status ClearTriggerCount(int ch)
        {
            return Write(0xF005, 0x80u << ch);
        }

        public Status ClearEncoderPositionAll()
        {
            return Write(0xF005, 0x7800u);
        }

        public Status ClearEncoderPosition(int ch)
        {
            return Write(0xF005, 0x800u << ch);
        }

        public Status ClearErrorAll()
        {
            return Write(0xF005, 0x78000u);
        }

        public Status ClearErrorCount(int ch)
        {
            return Write(0xF005, 0x8000u << ch);
        } 
        #endregion

        #region IDisposable
        private bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
