using Cognex.VisionPro;
using DSEV.Multicam;
using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DSEV.Schemas
{
    public class 그랩장치 : IDisposable
    {
        [JsonProperty("Camera"), Translation("Camera", "카메라")]
        public virtual 카메라구분 구분 { get; set; } = 카메라구분.None;
        [JsonIgnore, Translation("Index", "번호", "Index")]
        public virtual Int32 번호 { get; set; } = 0;
        [JsonProperty("Serial"), Translation("Serial", "Serial", "Serial")]
        public virtual String 코드 { get; set; } = String.Empty;
        [JsonIgnore, Translation("Name", "명칭", "Názov")]
        public virtual String 명칭 { get; set; } = String.Empty;
        [JsonProperty("Description"), Translation("Description", "설명", "Popis")]
        public virtual String 설명 { get; set; } = String.Empty;
        [JsonProperty("IpAddress"), Translation("IP", "IP", "IP")]
        public virtual String 주소 { get; set; } = String.Empty;
        [JsonProperty("Width"), Description("Width"), Translation("Width", "가로")]
        public virtual Int32 가로 { get; set; } = 0;
        [JsonProperty("Height"), Description("Height"), Translation("Height", "세로")]
        public virtual Int32 세로 { get; set; } = 0;
        [JsonProperty("CalibX"), Description("CalibX(µm)"), Translation("CalibX(µm)", "CalibX(µm)")]
        public virtual Double 교정X { get; set; } = 0;
        [JsonProperty("CalibY"), Description("CalibY(µm)"), Translation("CalibY(µm)", "CalibY(µm)")]
        public virtual Double 교정Y { get; set; } = 0;
        [JsonIgnore, Description("카메라 초기화 상태"), Translation("Live", "상태")]
        public virtual Boolean 상태 { get; set; } = false;
        [JsonIgnore]
        internal virtual MatType ImageType => MatType.CV_8UC1;
        [JsonIgnore]
        internal virtual Boolean UseMemoryCopy => false;
        [JsonIgnore]
        internal Int32 ImageWidth = 0;
        [JsonIgnore]
        internal Int32 ImageHeight = 0;
        [JsonIgnore]
        internal Object BufferLock = new Object();
        [JsonIgnore]
        internal UInt32 BufferSize = 0;
        [JsonIgnore]
        internal IntPtr BufferAddress = IntPtr.Zero;
        [JsonIgnore]
        internal Queue<Mat> Images = new Queue<Mat>();
        [JsonIgnore]
        internal Mat Image => Images.LastOrDefault<Mat>();
        [JsonIgnore]
        public const String 로그영역 = "Camera";

        public void Dispose()
        {
            while (this.Images.Count > 3)
                this.Dispose(this.Images.Dequeue());
        }
        internal void Dispose(Mat image)
        {
            if (image == null || image.IsDisposed) return;
            image.Dispose();
        }

        public virtual void Set(그랩장치 장치)
        {
            if (장치 == null) return;
            this.코드 = 장치.코드;
            this.설명 = 장치.설명;
            this.교정X = 장치.교정X;
            this.교정Y = 장치.교정Y;
        }
        public virtual Boolean Init() => false;
        public virtual Boolean Active() => false;
        public virtual Boolean Stop() => false;
        public virtual Boolean Close()
        {
            while (this.Images.Count > 0)
                this.Dispose(this.Images.Dequeue());
            return true;
        }
        public virtual void TurnOn() => Global.조명제어.TurnOn(this.구분);
        public virtual void TurnOff() => Global.조명제어.TurnOff(this.구분);

        #region 이미지그랩
        internal void InitBuffers(Int32 width, Int32 height)
        {
            if (width == 0 || height == 0) return;
            Int32 channels = ImageType == MatType.CV_8UC3 ? 3 : 1;
            Int32 imageSize = width * height * channels;
            if (BufferAddress != IntPtr.Zero && imageSize == BufferSize) return;
            this.ImageWidth = width; this.ImageHeight = height;
            if (BufferAddress != IntPtr.Zero)
            {
                Marshal.Release(BufferAddress);
                BufferAddress = IntPtr.Zero;
                BufferSize = 0;
            }

            BufferAddress = Marshal.AllocHGlobal(imageSize);
            if (BufferAddress == IntPtr.Zero) return;
            BufferSize = (UInt32)imageSize;
            Debug.WriteLine(this.구분.ToString(), "InitBuffers");
        }

        internal void CopyMemory(IntPtr surfaceAddr, Int32 width, Int32 height)
        {
            // 메모리 복사
            lock (this.BufferLock)
            {
                try
                {
                    this.InitBuffers(width, height);
                    Common.CopyMemory(BufferAddress, surfaceAddr, BufferSize);
                }
                catch (Exception e)
                {
                    Global.오류로그(로그영역, "Acquisition", $"[{this.구분.ToString()}] {e.Message}", true);
                }
            }
        }

        internal void AcquisitionFinished(IntPtr surfaceAddr, Int32 width, Int32 height)
        {
            if (surfaceAddr == IntPtr.Zero) { AcquisitionFinished("Failed."); return; }
            try
            {
                if (this.UseMemoryCopy) this.CopyMemory(surfaceAddr, width, height);
                else
                {
                    this.BufferAddress = surfaceAddr;
                    this.ImageWidth = width;
                    this.ImageHeight = height;
                }
                Global.그랩제어.그랩완료(this);
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "Acquisition", $"[{this.구분}] {ex.Message}", true);
            }
        }

        internal void AcquisitionFinished(String error) =>
            Global.오류로그(로그영역, "Acquisition", $"[{this.구분.ToString()}] {error}", true);
        internal void AcquisitionFinished(Mat image)
        {
            if (image == null)
            {
                AcquisitionFinished("Failed.");
                return;
            }
            this.Images.Enqueue(image);
            this.Dispose();
            Global.그랩제어.그랩완료(this);
        }

        public ICogImage CogImage()
        {
            try
            {
                if (this.Image != null) return Common.ToCogImage(this.Image);
                if (this.BufferAddress == IntPtr.Zero) return null;
                using (CogImage8Root cogImage8Root = new CogImage8Root())
                {
                    CogImage8Grey image = new CogImage8Grey();
                    cogImage8Root.Initialize(ImageWidth, ImageHeight, BufferAddress, ImageWidth, null);
                    image.SetRoot(cogImage8Root);
                    return image;
                }
            }
            catch (Exception e)
            {
                Global.오류로그(로그영역, "Acquisition", $"[{this.구분.ToString()}] {e.Message}", true);
            }
            return null;
        }
        public Mat MatImage()
        {
            if (this.Image != null) return this.Image;
            if (BufferAddress == IntPtr.Zero) return null;
            return new Mat(ImageHeight, ImageWidth, ImageType, BufferAddress);
        }
        #endregion
    }

    public class HikeGigE : 그랩장치
    {
        internal override Boolean UseMemoryCopy => true;
        [JsonIgnore]
        private CCamera Camera = null;
        [JsonIgnore]
        private CCameraInfo Device;
        [JsonIgnore]
        private cbOutputExdelegate ImageCallBackDelegate;
        [JsonIgnore]
        public Int32 OffsetX { get; set; } = 0;
        [JsonIgnore]
        public Boolean ReverseX { get; set; } = false;
        [JsonIgnore]
        public Int32 number { get; set; } = 0;

        public Boolean Init(CGigECameraInfo info)
        {
            try
            {
                this.Camera = new CCamera();
                this.Device = info;
                this.ImageCallBackDelegate = new cbOutputExdelegate(ImageCallBack);

                this.명칭 = info.chManufacturerName + " " + info.chModelName;
                UInt32 ip1 = (info.nCurrentIp & 0xff000000) >> 24;
                UInt32 ip2 = (info.nCurrentIp & 0x00ff0000) >> 16;
                UInt32 ip3 = (info.nCurrentIp & 0x0000ff00) >> 8;
                UInt32 ip4 = info.nCurrentIp & 0x000000ff;
                this.주소 = $"{ip1}.{ip2}.{ip3}.{ip4}";
                this.상태 = this.Init();
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "초기화", $"초기화 오류: {ex.Message}", true);
                this.상태 = false;
            }

            Debug.WriteLine($"{this.명칭}, {this.코드}, {this.주소}, {this.상태}");
            return this.상태;
        }

        public override Boolean Init()
        {
            Int32 nRet = this.Camera.CreateHandle(ref Device);
            if (!그랩제어.Validate($"[{this.구분}] 카메라 초기화에 실패하였습니다.", nRet, true)) return false;
            nRet = this.Camera.OpenDevice();
            if (!그랩제어.Validate($"[{this.구분}] 카메라 연결 실패!", nRet, true)) return false;
            그랩제어.Validate("RegisterImageCallBackEx", this.Camera.RegisterImageCallBackEx(this.ImageCallBackDelegate, IntPtr.Zero), false);
            Global.정보로그(로그영역, "카메라 연결", $"[{this.구분}] 카메라 연결 성공!", false);
            return true;
        }

        public override Boolean Active()
        {
            this.Camera.ClearImageBuffer();
            if(this.구분 == 카메라구분.Cam08 || this.구분 == 카메라구분.Cam09)
            {
                return 그랩제어.Validate($"{this.구분} Active", Camera.StartGrabbing(), false);
            }
            return 그랩제어.Validate($"{this.구분} Active", Camera.StartGrabbing(), true);
        }

        public override Boolean Close()
        {
            base.Close();
            if (this.Camera == null || !this.상태) return true;
            return 그랩제어.Validate($"{this.구분} Close", Camera.CloseDevice(), false);
        }

        public override Boolean Stop()
        {
            return 그랩제어.Validate($"{this.구분} Stop", Camera.StopGrabbing(), false);
        }

        private void ImageCallBack(IntPtr surfaceAddr, ref MV_FRAME_OUT_INFO_EX frameInfo, IntPtr user)
        {
            this.AcquisitionFinished(surfaceAddr, frameInfo.nWidth, frameInfo.nHeight);
            this.Stop();
        }
    }

    public class EuresysLink : 그랩장치
    {
        [JsonIgnore]
        private CamControl Device = null;

        public EuresysLink(CamControl cam)
        {
            this.구분 = cam.Camera;
            this.Device = cam;
        }

        public override Boolean Init()
        {
            this.Device.Init();
            this.Device.Start();
            this.Device.AcquisitionFinishedEvent += AcquisitionFinishedEvent;
            Global.정보로그(로그영역, "카메라 연결", $"[{this.구분}] 카메라 연결 성공!", false);
            Debug.WriteLine($"가로 : {this.가로}, 세로 : {this.세로}");
            this.상태 = true;
            return this.상태;
        }

        private void AcquisitionFinishedEvent(IntPtr surfaceAddr, Int32 width, Int32 height, String error)
        {
            if (surfaceAddr == IntPtr.Zero) this.AcquisitionFinished(error);
            else this.AcquisitionFinished(surfaceAddr, width, height);
            //this.Stop();
        }

        public override Boolean Close() { base.Close(); this.Device.Free(); return true; }
        public override Boolean Stop() { return true; }
        public override Boolean Active() { this.Device.Active(); return true; }
    }

    public class Logitech : 그랩장치 // Logitech BIRO
    {
        internal override MatType ImageType => MatType.CV_8UC3;
        [JsonIgnore]
        public Int32 Index { get; set; } = 0;
        [JsonIgnore]
        private VideoCapture Device = null;

        public override Boolean Init()
        {
            try
            {
                this.Device = new VideoCapture(Index, VideoCaptureAPIs.DSHOW) { FrameWidth = 가로, FrameHeight = 세로 };
                //if (Device.IsOpened()) Debug.WriteLine(this.Device.GetBackendName(), "BackendName");
                this.상태 = Device.IsOpened();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, this.구분.ToString());
            }
            return this.상태;
        }

        public override Boolean Stop() => true;
        public override Boolean Close() { base.Close(); this.Device?.Dispose(); return true; }
        public override Boolean Active()
        {
            Debug.WriteLine(this.구분, "Active");
            if (!Device.IsOpened()) return false;
            Mat frame = new Mat();

            // 이전 frame 을 가져옴
            this.Device.Read(frame);
            frame.Dispose();
            Task.Delay(50).Wait();
            frame = new Mat();

            Boolean r = this.Device.Read(frame);
            if (r) this.AcquisitionFinished(frame);
            else
            {
                this.AcquisitionFinished("이미지 취득에 실패하였습니다.");
                frame.Dispose();
            }
            return r;
        }
    }

    public class ImageMerge : 그랩장치
    {
        public override Boolean Init() => true;
        public override Boolean Active() => true;
        public override Boolean Stop() => true;
        public override void TurnOn() { }
        public override void TurnOff() { }
    }
}
