using DSEV.Schemas;
using Euresys.MultiCam;
using MvUtils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DSEV.Multicam
{
    [Description("카메라 설정")]
    public abstract class CamControl
    {
        [Description("이미지 그랩 이벤트")]
        public delegate void AcquisitionFinished(IntPtr surfaceAddr, Int32 width, Int32 height, String error);
        public event AcquisitionFinished AcquisitionFinishedEvent;

        // Callback 함수 가비지 수집된 대리자에서 콜백이 발생했습니다. 오류 방지를 위해 반드시 해당 콜백 함수를 상단에 정의
        private MC.CALLBACK CamCallBack;
        private UInt32[] SurfaceTable;
        private Int32 SurfaceCount = 1;

        [Description("카메라 구분")]
        public abstract 카메라구분 Camera { get; set; }
        [Description("카메라 설정 파일")]
        public abstract string CamFile { get; set; }
        [Description("그래버 보드 Index")]
        public abstract UInt32 DriverIndex { get; set; }
        [Description("Connector")]
        public virtual Connector Connector { get; set; } = Connector.M;
        [Description("Acquisition Mode")]
        public virtual AcquisitionMode AcquisitionMode { get; set; } = AcquisitionMode.WEB;
        [Description("LineRateMode")]
        public virtual LineRateMode LineRateMode { get; set; } = LineRateMode.CAMERA;
        [Description("LineCaptureMode")]
        public virtual LineCaptureMode LineCaptureMode { get; set; } = LineCaptureMode.ALL;
        [Description("Trig Mode")]
        public virtual TrigMode TrigMode { get; set; } = TrigMode.IMMEDIATE;
        [Description("Next Trig Mode")]
        public virtual NextTrigMode NextTrigMode { get; set; } = NextTrigMode.REPEAT;
        [Description("End Trig Mode")]
        public virtual EndTrigMode EndTrigMode { get; set; } = EndTrigMode.AUTO;
        [Description("Break Effect")]
        public virtual BreakEffect BreakEffect { get; set; } = BreakEffect.FINISH;
        [Description("Seq Length Page")]
        public virtual Int32 SeqLength_Pg { get; set; } = -1;
        [Description("Seq Length")]
        public virtual Int32 SeqLength_Ln { get; set; } = -1;
        [Description("Page Length")]
        public virtual Int32 PageLength_Ln { get; set; } = 500;
        [Description("Line Rate Hz")]
        public virtual Int32 LineRate_Hz { get; set; } = 85000;
        [Description("Expose_us")]
        public virtual Int32 Expose_us { get; set; } = 100;

        public virtual Int32 Hactive_Px { get; set; } = -1;
        public virtual Boolean ImageFlipX { get; set; } = false;
        public virtual Boolean ImageFlipY { get; set; } = false;

        [Description("Encoder Tick Count")]
        public UInt32 EncoderTickCount
        {
            get { MC.GetParam(this.Channel, "EncoderTickCount", out UInt32 count); return count; }
            set => MC.SetParam(this.Channel, "EncoderTickCount", value);
        }
        [Description("MaxSpeedEffective")]
        public Int32 MaxSpeedEffective
        {
            get { MC.GetParam(this.Channel, "MaxSpeedEffective", out Int32 maxSpeed); return maxSpeed; }
        }

        [Description("채널번호")]
        public UInt32 Channel;

        [Description("초기화")]
        public virtual void Init() { }

        [Description("그랩준비")]
        public virtual void Start()
        {
            this.InitSurfaceTable();
            // Callback 연결
            this.CamCallBack = new MC.CALLBACK(MultiCamCallback);
            MC.RegisterCallback(this.Channel, this.CamCallBack, this.Channel);
            // Enable the signals corresponding to the callback functions
            MC.SetParam(this.Channel, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
            MC.SetParam(this.Channel, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");
            this.Ready();
        }
        public String DebugParam(UInt32 channel, String name, Boolean print = true)
        {
            MC.GetParam(channel, name, out String value);
            if (print) Debug.WriteLine(value, $"{this.DriverIndex}.{this.Connector}.{channel}.{name}");
            return value;
        }
        public String DebugParam(String name, Boolean print = true) => DebugParam(this.Channel, name, print);

        [Description("메모리 링 버퍼")]
        private void InitSurfaceTable()
        {
            if (this.AcquisitionMode != AcquisitionMode.LONGPAGE) return;
            Int32 imageSizeX, imageSizeY, bufferSize, bufferPitch;
            MC.GetParam(this.Channel, "ImageSizeX", out imageSizeX);
            MC.GetParam(this.Channel, "ImageSizeY", out imageSizeY);
            MC.GetParam(this.Channel, "BufferSize", out bufferSize);
            MC.GetParam(this.Channel, "BufferPitch", out bufferPitch);

            this.SurfaceCount = (Int32)Math.Ceiling((Double)this.SeqLength_Ln / imageSizeY);
            this.SurfaceTable = new UInt32[this.SurfaceCount];
            Debug.WriteLine($"SurfaceCount={this.SurfaceCount}, BufferSize={bufferSize}, BufferPitch={bufferPitch}, ImageSizeX={imageSizeX}, ImageSizeY={imageSizeY}", this.Camera.ToString());
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize * this.SurfaceCount);
            for (Int32 i = 0; i < this.SurfaceCount; i++)
            {
                // Create a surface
                MC.Create(MC.DEFAULT_SURFACE_HANDLE, out SurfaceTable[i]);
                // Slicing the memory into small buffers
                IntPtr surfAddress = IntPtr.Add(buffer, i * bufferSize);
                // Set surface parameters
                MC.SetParam(SurfaceTable[i], "SurfaceSize", bufferSize);
                MC.SetParam(SurfaceTable[i], "SurfacePitch", bufferPitch);
                MC.SetParam(SurfaceTable[i], "SurfaceAddr", surfAddress);
                MC.SetParam(this.Channel, "Cluster:" + i.ToString(), SurfaceTable[i]);
            }
        }

        [Description("메모리 링 버퍼 해제")]
        private void FreeSufaceTable()
        {
            if (SurfaceTable == null) return;
            if (this.ChannelState() == Multicam.ChannelState.ACTIVE)
            {
                Debug.WriteLine("그래버 상태가 ACTIVE 입니다.");
                return;
            }
            for (Int32 i = 0; i < SurfaceCount; i++)
            {
                String state = DebugParam(SurfaceTable[i], "SurfaceState", false);
                if (state.Equals(Multicam.ChannelState.FREE)) continue;
                MC.SetParam(SurfaceTable[i], "SurfaceState", "FREE");
            }
        }

        internal virtual Boolean SetParamString(String name, String value)
        {
            try { MC.SetParam(this.Channel, name, value); return true; }
            catch (Exception ex)
            {
                Global.오류로그(this.Camera.ToString(), "SetParam", $"{name} => {value} : {ex.Message}", false);
            }
            return false;
        }

        [Description("채널 활성화 준비")]
        public void Ready()
        {
            String state = this.ChannelState();
            if (state == Multicam.ChannelState.READY) return;
            this.SetParamString("ChannelState", Multicam.ChannelState.READY);
            //this.FreeSufaceTable();
        }

        [Description("Acquisition 시작")]
        public void Active()
        {
            String state = this.ChannelState();
            this.EncoderTickCount = 0;
            if (state == Multicam.ChannelState.ACTIVE)
            {

            }
            if (state != Multicam.ChannelState.READY)
            {
                this.SetParamString("ChannelState", Multicam.ChannelState.READY);
                Thread.Sleep(100);
            }

            Debug.WriteLine("TOPactive!!");
            this.SetParamString("ChannelState", Multicam.ChannelState.ACTIVE);
            //this.FreeSufaceTable();
        }

        [Description("채널 IDLE")]
        public void Idle()
        {
            String state = this.ChannelState();
            if (state == Multicam.ChannelState.IDLE) return;
            this.SetParamString("ChannelState", Multicam.ChannelState.IDLE);
            //this.FreeSufaceTable();
        }

        [Description("채널 Release")]
        public void Free()
        {
            this.SetParamString("ChannelState", Multicam.ChannelState.FREE);
            //this.FreeSufaceTable();
        }

        /*
        [Description("Software Trig")]
        public void SoftTrig()
        {
            Debug.WriteLine(this.TrigDelay, "Delay");
            if (this.TrigDelay > 0)
            {
                Task.Run(() => {
                    Debug.WriteLine("Acquisition Start", this.Camera.ToString());
                    Task.Delay(this.TrigDelay).Wait();
                    this.Active();
                    //MC.SetParam(this.Channel, "ForceTrig", "TRIG");
                });
            }
            else
            {
                Debug.WriteLine("Acquisition Start", this.Camera.ToString());
                this.Active();
                //MC.SetParam(this.Channel, "ForceTrig", "TRIG");
            }
        }
        */

        [Description("채널 상태")]
        public String ChannelState()
        {
            MC.GetParam(this.Channel, "ChannelState", out String state);
            return state;
        }

        [Description("MultiCam CallBack Event")]
        private void MultiCamCallback(ref MC.SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                //case MC.SIG_START_ACQUISITION_SEQUENCE:
                //    Debug.WriteLine(signalInfo.Signal, "SIGNALINFO");
                //    break;
                //case MC.SIG_END_ACQUISITION_SEQUENCE:
                //    Debug.WriteLine(signalInfo.Signal, "SIGNALINFO");
                //    break;
                //case MC.SIG_SURFACE_FILLED:
                //    Debug.WriteLine(signalInfo.Signal, "SIGNALINFO");
                //    UInt32 LineIndex, Elapsed_Ln;
                //    MC.GetParam(this.Channel, "LineIndex", out LineIndex);
                //    MC.GetParam(this.Channel, "Elapsed_Ln", out Elapsed_Ln);
                //    Debug.WriteLine($"{LineIndex}, {Elapsed_Ln}", "LineIndex & Elapsed_Ln");
                //    break;
                case MC.SIG_SURFACE_PROCESSING:
                    ProcessingCallback(signalInfo);
                    break;
                case MC.SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    Debug.WriteLine(signalInfo.Signal, "SIGNALINFO");
                    throw new Euresys.MultiCamException("Unknown signal");
            }
        }

        [Description("Acquisition Process")]
        private void ProcessingCallback(MC.SIGNALINFO signalInfo)
        {
            try
            {
                UInt32 currentChannel = (UInt32)signalInfo.Context;
                Int32 imageSizeX, imageSizeY, bufferPitch;

                MC.GetParam(currentChannel, "ImageSizeX", out imageSizeX);
                MC.GetParam(currentChannel, "ImageSizeY", out imageSizeY);
                MC.GetParam(currentChannel, "BufferPitch", out bufferPitch);

                if (this.AcquisitionMode == AcquisitionMode.WEB || this.AcquisitionMode == AcquisitionMode.PAGE)
                {
                    //UInt32 Elapsed_Pg, Remaining_Pg;
                    //MC.GetParam(currentChannel, "Elapsed_Pg", out Elapsed_Pg);
                    //MC.GetParam(currentChannel, "Remaining_Pg", out Remaining_Pg);
                    //Debug.WriteLine($"{this.Camera}: Elapsed_Pg={Elapsed_Pg}, Remaining_Pg={Remaining_Pg}");
                    this.ImageGrap(currentChannel, signalInfo.SignalInfo, imageSizeX, imageSizeY, bufferPitch);
                }
                else if (this.AcquisitionMode == AcquisitionMode.LONGPAGE)
                {
                    Int32 remaining_Pg, remaining_Ln, rlapsed_Ln;
                    MC.GetParam(currentChannel, "Elapsed_Ln", out rlapsed_Ln);
                    MC.GetParam(currentChannel, "Remaining_Ln", out remaining_Ln);
                    MC.GetParam(currentChannel, "Remaining_Pg", out remaining_Pg);
                    //Debug.WriteLine($"{Utils.FormatDate(DateTime.Now, "{0:HHmmss.fff}")} {this.Camera}: Remaining_Pg={remaining_Pg}, Elapsed_Ln={rlapsed_Ln}, Remaining_Ln={remaining_Ln}");
                    if (remaining_Pg <= 0)
                        this.ImageGrap(currentChannel, SurfaceTable[0], imageSizeX, rlapsed_Ln, bufferPitch);
                }
            }
            catch (Euresys.MultiCamException ex)
            {
                Utils.DebugException(ex, 3, "MultiCamException");
                this.AcquisitionFinishedEvent?.Invoke(IntPtr.Zero, 0, 0, ex.Message);
            }
            catch (Exception ex)
            {
                Utils.DebugException(ex, 3, "MultiCamSystemException");
                this.AcquisitionFinishedEvent?.Invoke(IntPtr.Zero, 0, 0, ex.Message);
            }
        }

        private void ImageGrap(UInt32 channel, UInt32 bufferAddress, Int32 width, Int32 height, Int32 bufferPitch)
        {
            try
            {
                Debug.WriteLine("ImageGrap!!");
                MC.GetParam(bufferAddress, "SurfaceAddr", out IntPtr surfaceAddr);
                this.AcquisitionFinishedEvent?.Invoke(surfaceAddr, width, height, String.Empty);
            }
            catch (Exception ex)
            {
                this.AcquisitionFinishedEvent?.Invoke(IntPtr.Zero, 0, 0, ex.Message);
                Debug.WriteLine($"그랩오류: {ex.Message}", this.Camera.ToString());
            }
        }

        [Description("Acquisition Failed")]
        private void AcqFailureCallback(MC.SIGNALINFO signalInfo)
        {
            //this.Idle();
            ////UInt32 currentChannel = (UInt32)signalInfo.Context;
            //AcquisitionData Data = new AcquisitionData(this.Camera, "Acquisition Failure, Channel State: IDLE");
            //this.AcquisitionFinishedEvent?.Invoke(this.Camera, Data);
        }
    }

    #region 상수 정의
    public static class ChannelState
    {
        [Description("채널은 그래버를 소유하고 있지만 잠금상태는 아님.")]
        public const string IDLE = "IDLE";

        [Description("채널은 그래버를 사용합니다.")]
        public const string ACTIVE = "ACTIVE";

        [Description("채널에 그래버가 없습니다.")]
        public const string ORPHAN = "ORPHAN";

        [Description("채널은 그래버를 잠그고 acquisition sequence를 시작할 준비가 됨.")]
        public const string READY = "READY";

        [Description("채널의 상태를 ORPHAN으로 설정합니다.")]
        public const string FREE = "FREE";
    }

    // In order to use single camera on connector A
    // MC_Connector must be set to A for Grablink DualBase
    // For all other Grablink boards the parameter has to be set to M  
    public enum Connector
    {
        M,
        A,
        B,
    }

    public enum AcquisitionMode
    {
        PAGE,
        LONGPAGE,
        WEB,
    }

    public enum LineRateMode
    {
        PULSE,
        CONVERT,
        PERIOD,
        EXPOSE,
        CAMERA,
    }

    public enum LineCaptureMode
    {
        ALL,
        PICK,
        TAG,
        ADR,
    }

    public enum TrigMode
    {
        IMMEDIATE,
        HARD,
        SOFT,
        COMBINED,
    }

    public enum NextTrigMode
    {
        SAME,
        REPEAT,
        HARD,
        SOFT,
        COMBINED,
    }

    public enum EndTrigMode
    {
        AUTO,
        HARD,
    }

    public enum BreakEffect
    {
        FINISH,
        ABORT,
    }
    #endregion
}
