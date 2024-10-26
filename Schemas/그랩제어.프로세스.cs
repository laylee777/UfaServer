using Euresys.MultiCam;
using MvCamCtrl.NET;
using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DSEV.Multicam;

namespace DSEV.Schemas
{
    public class 그랩제어 : Dictionary<카메라구분, 그랩장치>
    {
        public static List<카메라구분> 대상카메라 = new List<카메라구분>() { 카메라구분.Cam01, 카메라구분.Cam02, 카메라구분.Cam03 }; //, 카메라구분.Cam02, 카메라구분.Cam03

        public delegate void 그랩완료대리자(그랩장치 장치);
        public event 그랩완료대리자 그랩완료보고;

        [JsonIgnore]
        public EuresysLink 카메라1 = null;
        [JsonIgnore]
        public HikeGigE 카메라2 = null;
        [JsonIgnore]
        public HikeGigE 카메라3 = null;
        //[JsonIgnore]
        //public HikeGigE 카메라3 = null;

        [JsonIgnore]
        private const string 로그영역 = "Camera";
        [JsonIgnore]
        private string 저장파일 => Path.Combine(Global.환경설정.기본경로, "Cameras.json");
        [JsonIgnore]
        public Boolean 정상여부 => !this.Values.Any(e => !e.상태);

        public Boolean Init()
        {
            Dalsa16K cam1 = new Dalsa16K(카메라구분.Cam01) { AcquisitionMode = AcquisitionMode.PAGE, PageLength_Ln = 37000 };
            //this.카메라1 = new EuresysLink(cam1) { 코드 = "BOTTOMCAMERA", 가로 = 14336, 세로 = cam1.PageLength_Ln };
            this.카메라1 = new EuresysLink(cam1) { 코드 = "BOTTOMCAMERA", 가로 = 16384, 세로 = cam1.PageLength_Ln };
            this.카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam02, 코드 = "DA1698483", 가로 = 2048, 세로 = 26000};
            this.카메라3 = new HikeGigE() { 구분 = 카메라구분.Cam03, 코드 = "DA1698485", 가로 = 2048, 세로 = 26000};

            this.Add(카메라구분.Cam01, this.카메라1);
            this.Add(카메라구분.Cam02, this.카메라2);
            this.Add(카메라구분.Cam03, this.카메라3);

            // 카메라 설정 저장정보 로드
            그랩장치 정보;
            List<그랩장치> 자료 = Load();
            if (자료 != null)
            {
                foreach (그랩장치 설정 in 자료)
                {
                    정보 = this.GetItem(설정.구분);
                    if (정보 == null) continue;
                    정보.Set(설정);
                }
            }
            if (Global.환경설정.동작구분 != 동작구분.Live) return true;

            MC.OpenDriver();
            // CameraLink 초기화
            foreach (그랩장치 장치 in this.Values)
                if (장치.GetType() == typeof(EuresysLink))
                    장치.Init();

            // GigE 카메라 초기화
            List<CCameraInfo> 카메라들 = new List<CCameraInfo>();
            Int32 nRet = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE, ref 카메라들);// | CSystem.MV_USB_DEVICE
            if (!Validate("Enumerate devices fail!", nRet, true)) return false;

            for (int i = 0; i < 카메라들.Count; i++)
            {
                CGigECameraInfo gigeInfo = 카메라들[i] as CGigECameraInfo;
                HikeGigE gige = this.GetItem(gigeInfo.chSerialNumber) as HikeGigE;
                if (gige == null) continue;
                //Debug.WriteLine(gigeInfo.chSerialNumber, "시리얼");
                gige.Init(gigeInfo);
                //if (gige.상태) gige.Start();
            }

            Debug.WriteLine($"카메라 갯수: {this.Count}");
            GC.Collect();
            return true;
        }

        private List<그랩장치> Load()
        {
            if (!File.Exists(this.저장파일)) return null;
            return JsonConvert.DeserializeObject<List<그랩장치>>(File.ReadAllText(this.저장파일), Utils.JsonSetting());
        }

        public void Save()
        {
            if (!Utils.WriteAllText(저장파일, JsonConvert.SerializeObject(this.Values, Utils.JsonSetting())))
                Global.오류로그(로그영역, "카메라 설정 저장", "카메라 설정 저장에 실패하였습니다.", true);
        }

        public void Close()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            foreach (그랩장치 장치 in this.Values)
                장치?.Close();
        }
        public void Active(카메라구분 구분) => this.GetItem(구분)?.Active();

        public 그랩장치 GetItem(카메라구분 구분)
        {
            if (this.ContainsKey(구분)) return this[구분];
            return null;
        }

        private 그랩장치 GetItem(String serial) => this.Values.Where(e => e.코드 == serial).FirstOrDefault();

        public void 그랩완료(그랩장치 장치)
        {
            장치.TurnOff();
            if (Global.장치상태.자동수동)
            {
                Int32 검사번호 = Global.장치통신.촬영위치번호(장치.구분);
                검사결과 검사 = Global.검사자료.검사항목찾기(검사번호);
                if (검사 == null) return;
                Global.비전검사.Run(장치, 검사);
            }
            else
            {
                Global.비전검사.Run(장치.구분, 장치.CogImage(), Global.검사자료.수동검사);
                this.그랩완료보고?.Invoke(장치);
            }
        }

        #region 오류메세지
        public static Boolean Validate(String message, Int32 errorNum, Boolean show)
        {
            if (errorNum == CErrorDefine.MV_OK) return true;

            String errorMsg = String.Empty;
            switch (errorNum)
            {
                case CErrorDefine.MV_E_HANDLE: errorMsg = "Error or invalid handle"; break;
                case CErrorDefine.MV_E_SUPPORT: errorMsg = "Not supported function"; break;
                case CErrorDefine.MV_E_BUFOVER: errorMsg = "Cache is full"; break;
                case CErrorDefine.MV_E_CALLORDER: errorMsg = "Function calling order error"; break;
                case CErrorDefine.MV_E_PARAMETER: errorMsg = "Incorrect parameter"; break;
                case CErrorDefine.MV_E_RESOURCE: errorMsg = "Applying resource failed"; break;
                case CErrorDefine.MV_E_NODATA: errorMsg = "No data"; break;
                case CErrorDefine.MV_E_PRECONDITION: errorMsg = "Precondition error, or running environment changed"; break;
                case CErrorDefine.MV_E_VERSION: errorMsg = "Version mismatches"; break;
                case CErrorDefine.MV_E_NOENOUGH_BUF: errorMsg = "Insufficient memory"; break;
                case CErrorDefine.MV_E_UNKNOW: errorMsg = "Unknown error"; break;
                case CErrorDefine.MV_E_GC_GENERIC: errorMsg = "General error"; break;
                case CErrorDefine.MV_E_GC_ACCESS: errorMsg = "Node accessing condition error"; break;
                case CErrorDefine.MV_E_ACCESS_DENIED: errorMsg = "No permission"; break;
                case CErrorDefine.MV_E_BUSY: errorMsg = "Device is busy, or network disconnected"; break;
                case CErrorDefine.MV_E_NETER: errorMsg = "Network error"; break;
                default: errorMsg = "Unknown error"; break;
            }

            Global.오류로그("Camera", "Error", $"[{errorNum}] {message} {errorMsg}", show);
            return false;
        }
        #endregion
    }
}