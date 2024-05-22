using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Diagnostics;
using DSEV.Schemas;
using DSEV.UI.Forms;

namespace DSEV.UI.Controls
{
    public partial class State : XtraUserControl
    {
        private LocalizationState 번역 = new LocalizationState();
        public State()
        {
            InitializeComponent();
            this.BindLocalization.DataSource = this.번역;
            this.BindLocalization.ResetBindings(false);
        }

        public void Init()
        {
            this.e양품수율.BaseColor = DXSkinColors.ForeColors.Question;
            this.e양품수량.BaseColor = DXSkinColors.ForeColors.Information;
            this.e불량수량.BaseColor = DXSkinColors.ForeColors.Critical;
            this.e전체수량.BaseColor = DXSkinColors.ForeColors.ControlText;

            this.모델자료Bind.DataSource = Global.모델자료.선택모델;
            this.BindLocalization.ResetBindings(false);
            Localization.SetColumnCaption(this.e모델선택, typeof(모델정보));
            this.e모델선택.Properties.DataSource = Global.모델자료;
            this.e모델선택.EditValue = Global.환경설정.선택모델;
            this.e모델선택.Properties.ReadOnly = !Global.환경설정.슈퍼유저;
            this.e모델선택.CustomDisplayText += 선택모델표현;
            if (Global.환경설정.슈퍼유저)
            {
                this.e모델선택.EditValueChanging += 모델변경;
                this.b동작구분.DoubleClick += 수동검사;
            }
            else this.e모델선택.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.e모델선택.CustomDisplayText += 선택모델표현;
            this.b수량리셋.Click += 수량리셋_Click;

            Global.환경설정.모델변경알림 += 모델변경알림;
            Global.장치통신.동작상태알림 += 동작상태알림;
            Global.검사자료.검사완료알림 += 검사완료알림;

            this.검사상태표현(결과구분.WA);
            this.e모델선택.Refresh();
            this.e장치상태.Init();
            this.e저장용량.EditValue = Global.환경설정.저장비율;
        }

        public void Close() { }

        private void 선택모델표현(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try 
            {
                모델구분 모델 = (모델구분)e.Value;
                e.DisplayText = $"{((Int32)모델).ToString("d2")}. {Utils.GetDescription(모델)}";
            }
            catch { e.DisplayText = String.Empty; }
        }

        private void 모델변경알림(모델구분 모델코드)
        {
            if (Global.모델자료.선택모델 == null) return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => 모델변경알림(모델코드)));
                return;
            }
            this.e모델선택.EditValue = 모델코드;
            this.모델자료Bind.DataSource = Global.모델자료.선택모델;
            this.모델자료Bind.ResetBindings(false);
        }

        private void 모델변경(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            모델구분 모델 = (모델구분)e.NewValue;
            if (Global.환경설정.선택모델 == 모델) return;
            if (!Utils.Confirm(this.FindForm(), 번역.모델변경))
            {
                e.Cancel = true;
                return;
            }
            Global.환경설정.모델변경요청(모델);
        }

        private void 검사상태표현(결과구분 구분)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => 검사상태표현(구분)));
                return;
            }
            if (구분 == 결과구분.WA) return;
            this.모델자료Bind.ResetBindings(false);
        }

        private void 검사완료알림(검사결과 결과)
        {
            if (결과 == null) return;
            if (this.InvokeRequired) { this.BeginInvoke((Action)(() => 검사완료알림(결과))); return; }

            // DB 저장
            Global.검사자료.Save();
            this.검사상태표현(결과.측정결과);
            this.e저장용량.EditValue = Global.환경설정.저장비율;
            GC.Collect();
        }

        //private void 측정시작보고() => this.검사상태표현(결과구분.IN);
        private void 동작상태알림()
        {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(동작상태알림)); return; }
            this.b동작구분.Text = Global.장치상태.자동수동 ? 번역.자동 : 번역.수동;
            this.b동작구분.Appearance.ForeColor = Global.장치상태.시작정지 ? DXSkinColors.ForeColors.Information : DXSkinColors.ForeColors.DisabledText;
        }

        private void 수량리셋_Click(object sender, EventArgs e)
        {
            if (!Utils.Confirm(this.FindForm(), 번역.리셋확인)) return;
            Global.모델자료.수량리셋();
            this.모델자료Bind.ResetBindings(false);
            Global.피씨통신.동작상태알림();
        }

        private void 수동검사(object sender, EventArgs e)
        {
            //if (Global.장치상태.자동수동) return;
            //Global.검사자료.수동검사.Reset();
            //Global.비전검사.RunMaster();
            //Global.검사자료.검사테스트();
            StateForm state = new StateForm();
            state.Show(Global.MainForm);
        }

        private class LocalizationState
        {
            private enum Items
            {
                [Translation("Auto", "자동")]
                자동,
                [Translation("Manual", "수동")]
                수동,
                [Translation("Count\r\nReset", "수량\r\n초기화")]
                수량리셋,
                [Translation("Initialize the inspection quantity?", "검사수량을 초기화하시겠습니까?")]
                리셋확인,
                [Translation("Change the inspection model?", "검사모델을 변경하시겠습니까?")]
                모델변경,
            }

            private String GetString(Items item) { return Localization.GetString(item); }
            public String 자동 { get { return GetString(Items.자동); } }
            public String 수동 { get { return GetString(Items.수동); } }
            public String 수량리셋 { get { return GetString(Items.수량리셋); } }
            public String 리셋확인 { get { return GetString(Items.리셋확인); } }
            public String 모델변경 { get { return GetString(Items.모델변경); } }
            public String 양품갯수 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.양품갯수))); } }
            public String 불량갯수 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.불량갯수))); } }
            public String 전체갯수 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.전체갯수))); } }
            public String 양품수율 { get { return Localization.GetString(typeof(모델정보).GetProperty(nameof(모델정보.양품수율))); } }
        }
    }
}
