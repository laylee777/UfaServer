using DevExpress.XtraEditors;
using DSEV.Schemas;
using MvUtils;
using System;
using System.IO;
using System.Windows.Media.Media3D;

namespace DSEV.UI.Forms
{
    public partial class StateForm : XtraForm
    {
        public StateForm()
        {
            InitializeComponent();
            this.IconOptions.Icon = Properties.Resources.icon;
            this.Shown += FormShown;
        }

        VDA590UFA3D UFA = null;
        private void FormShown(object sender, EventArgs e) => this.Init();
        public void Init()
        {
            this.e양품수율.BaseColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Question;
            this.e양품수량.BaseColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.e불량수량.BaseColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            this.e전체수량.BaseColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText;
            Global.환경설정.모델변경알림 += 모델변경알림;
            Global.검사자료.검사완료알림 += 검사완료알림;
            Global.모델자료.검사수량변경 += 검사수량변경;
            this.e모델.DoubleClick += 화면갱신;

            UFA = new VDA590UFA3D()
            {
                CameraPosition = new Point3D(0, 30, 1150),
                CameraLookDirection = new Vector3D(0, 0, -1150),
                CameraUpDirection = new Vector3D(0, 1, 0),
            };
            this.e뷰어.Init(UFA);
            this.e시계.Init();
            this.모델변경알림(Global.환경설정.선택모델);
            this.e결과.Text = Utils.GetDescription(결과구분.WA);
        }

        private class LocalizationState
        {
            public String 양품갯수 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.양품갯수))); } }
            public String 불량갯수 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.불량갯수))); } }
            public String 전체갯수 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.전체갯수))); } }
            public String 양품수율 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.양품수율))); } }
        }

        private void 모델변경알림(모델구분 모델코드)
        {
            if (Global.모델자료.선택모델 == null) return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => 모델변경알림(모델코드)));
                return;
            }
            this.e모델.Text = $"{Utils.GetDescription(Global.환경설정.선택모델)}"; //{((Int32)Global.환경설정.선택모델).ToString("d2")}. 
            this.모델자료Bind.DataSource = Global.모델자료.선택모델;
            this.모델자료Bind.ResetBindings(false);
        }

        private void 검사수량변경() => 결과갱신(null);
        private void 검사완료알림(검사결과 결과) => 결과갱신(결과);
        private void 결과갱신(검사결과 결과)
        {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => { 결과갱신(결과); })); return; }
            this.모델자료Bind.ResetBindings(false);
            if (결과 == null) return;
            this.e뷰어.SetResults(결과);
            검사결과(결과);
        }
        private void 검사결과(검사결과 결과)
        {
            this.e결과.Text = Utils.GetDescription(결과.측정결과);
            this.e결과.Appearance.ForeColor = 환경설정.ResultColor(결과.측정결과);
            this.e순번.Text = $"{결과.검사코드.ToString("d4")}";
            this.e시간.Text = $"{Utils.FormatDate(결과.검사일시, "{0:HH:mm:ss}")}";
            this.e큐알.Text = 결과.큐알내용;
            this.e큐알.Appearance.ForeColor = 환경설정.ResultColor(결과.큐알결과());
        }
        private void 화면갱신(object sender, EventArgs e)
        {
            Global.검사자료.검사테스트();
            //this.e뷰어.SetResults(Global.검사자료.수동검사);
        }
    }
}