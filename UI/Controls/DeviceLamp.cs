using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Diagnostics;
using DSEV.Schemas;

namespace DSEV.UI.Controls
{
    public partial class DeviceLamp : XtraUserControl
    {
        public DeviceLamp()
        {
            InitializeComponent();
        }

        private 장치상태 장치통신;
        private 장치상태 큐알리더;
        private 장치상태 피씨통신;
        private 장치상태 조명장치;
        private 장치상태 카메라1;
        private 장치상태 카메라2;
        private 장치상태 카메라3;

        public void Init()
        {
            this.장치통신 = new 장치상태(this.e장치통신, true);
            this.큐알리더 = new 장치상태(this.e각인리더);
            this.피씨통신 = new 장치상태(this.e피씨통신);
            this.조명장치 = new 장치상태(this.e조명장치);
            this.카메라1 = new 장치상태(this.e카메라1);
            this.카메라2 = new 장치상태(this.e카메라2);
            this.카메라3 = new 장치상태(this.e카메라3);
            Global.장치통신.통신상태알림 += 통신상태알림;
            this.통신상태알림();
        }

        private void 통신상태알림()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(통신상태알림));
                return;
            }

            if (Global.장치통신.정상여부) this.장치통신.Set(Global.장치통신.통신확인핑퐁 ? 상태구분.정상 : 상태구분.대기);
            else this.장치통신.Set(상태구분.오류);
            this.카메라1.Set(Global.환경설정.동작구분 == 동작구분.LocalTest || Global.장치상태.카메라1);
            this.카메라2.Set(Global.환경설정.동작구분 == 동작구분.LocalTest || Global.장치상태.카메라2);
            this.카메라3.Set(Global.환경설정.동작구분 == 동작구분.LocalTest || Global.장치상태.카메라3);
            this.조명장치.Set(Global.환경설정.동작구분 == 동작구분.LocalTest || Global.장치상태.조명장치);
            this.큐알리더.Set(Global.환경설정.동작구분 == 동작구분.LocalTest || Global.장치상태.큐알리더);
            this.피씨통신.Set(Global.환경설정.동작구분 == 동작구분.LocalTest || Global.장치상태.피씨통신);
        }

        private enum 상태구분
        {
            없음,
            대기,
            정상,
            오류,
        }
        private class 장치상태
        {
            private SvgImageBox 도구;
            private 상태구분 현재상태 = 상태구분.없음;
            private DevExpress.Utils.Svg.SvgImage 대기 = null;
            private DevExpress.Utils.Svg.SvgImage 정상 = null;
            private DevExpress.Utils.Svg.SvgImage 오류 = null;

            public 장치상태(SvgImageBox tool, Boolean HasWait = false)
            {
                this.도구 = tool;
                this.정상 = Utils.SetSvgStyle(tool.SvgImage, Utils.SvgStyles.Green);
                this.오류 = Utils.SetSvgStyle(tool.SvgImage, Utils.SvgStyles.Red);
                if (HasWait) this.대기 = Utils.SetSvgStyle(tool.SvgImage, Utils.SvgStyles.Blue);
                this.도구.SvgImage = this.오류;
            }

            public void Set(Boolean 상태)
            {
                this.Set(상태 ? 상태구분.정상 : 상태구분.오류);
            }

            public void Set(상태구분 상태)
            {
                if (this.현재상태 == 상태) return;
                this.현재상태 = 상태;
                if (상태 == 상태구분.정상) this.도구.SvgImage = this.정상;
                else if (상태 == 상태구분.오류) this.도구.SvgImage = this.오류;
                else if (상태 == 상태구분.대기) this.도구.SvgImage = this.대기;
            }
        }
    }
}
