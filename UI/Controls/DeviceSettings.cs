using MvUtils;
using DevExpress.XtraEditors;
using System;
using DSEV.Schemas;
using DevExpress.Utils.Extensions;

namespace DSEV.UI.Controls
{
    public partial class DeviceSettings : XtraUserControl
    {
        public DeviceSettings()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.e강제배출.IsOn = Global.환경설정.강제배출;
            this.e배출구분.IsOn = Global.환경설정.양품불량;
            this.e강제배출.EditValueChanged += 강제배출Changed;
            this.e배출구분.EditValueChanged += 배출구분Changed;
            this.b캠트리거리셋.Click += 캠트리거리셋;
            this.e센서리셋.IsOn = false;
            this.e센서리셋.EditValueChanged += 센서리셋;

            this.e카메라.Init();
            this.e큐알장치.Init();
            this.e기본설정.Init();
            this.e유저관리.Init();
        }

        public void Close()
        {
            this.e카메라.Close();
            this.e큐알장치.Close();
            this.e기본설정.Close();
            this.e유저관리.Close();
        }

        //public void Shown(Boolean shown) { }
        private void 강제배출Changed(object sender, EventArgs e) => Global.환경설정.강제배출 = this.e강제배출.IsOn;
        private void 배출구분Changed(object sender, EventArgs e) => Global.환경설정.양품불량 = this.e배출구분.IsOn;
        private void 센서리셋(object sender, EventArgs e) => Global.장치통신.센서제로수행(this.e센서리셋.IsOn);
        private void 캠트리거리셋(object sender, EventArgs e)
        {
            if (!Utils.Confirm(this.FindForm(), "트리거 보드의 위치를 초기화 하시겠습니까?")) return;
            직렬포트[] 포트들 = new 직렬포트[] { 직렬포트.COM7 };
            포트들.ForEach(port => {
                Enc852 트리거보드 = new Enc852(port);
                트리거보드.Clear();
                트리거보드.Close();
            });
            Global.정보로그("트리거보드", "초기화", "초기화 되었습니다.", true);
        }
    }
}
