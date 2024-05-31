using MvUtils;
using DevExpress.XtraEditors;
using System;
using DSEV.Schemas;
using DevExpress.Utils.Extensions;
using DevExpress.XtraRichEdit.Import.OpenXml;
using System.Diagnostics;

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

            this.e강제레이져각인.IsOn = Global.환경설정.강제레이져각인;
            this.e레이져각인구분.IsOn = Global.환경설정.레이져각인양품불량;

            this.e강제검증.IsOn = Global.환경설정.강제검증;
            this.e검증구분.IsOn = Global.환경설정.검증양품불량;

            this.e강제라벨부착.IsOn = Global.환경설정.강제라벨부착;
            this.e라벨부착구분.IsOn = Global.환경설정.라벨부착양품불량;



            this.e작업번호.Value = (Decimal)Global.환경설정.레이져각인기작업번호;
            this.e큐알각인내용.Text = Global.환경설정.레이져각인기마킹내용;
            this.e서플라이어코드.Text = Global.환경설정.레이져각인기서플라이어코드;



            this.e강제배출.EditValueChanged += 강제배출Changed;
            this.e배출구분.EditValueChanged += 배출구분Changed;

            
            this.e강제레이져각인.EditValueChanged += 강제레이져각인Changed;
            this.e레이져각인구분.EditValueChanged += 레이져각인구분Changed;

            this.e강제검증.EditValueChanged += 강제검증Changed;
            this.e검증구분.EditValueChanged += 검증구분Changed;

            this.e강제라벨부착.EditValueChanged += 강제라벨부착Changed;
            this.e라벨부착구분.EditValueChanged += 라벨부착구분Changed;


            this.b레이져각인세팅저장.Click += B레이져각인세팅저장_Click;



           //this.e서플라이어코드.EditValueChanged += E서플라이어코드_EditValueChanged;

            //this.e작업번호.EditValueChanged += E작업번호_EditValueChanged;

            //this.e큐알각인내용.EditValueChanged += E큐알각인내용_EditValueChanged;

            this.b캠트리거리셋.Click += 캠트리거리셋;
            this.e센서리셋.IsOn = false;
            //this.e센서리셋.EditValueChanged += 센서리셋;

            this.e카메라.Init();
            this.e큐알장치.Init();
            this.e기본설정.Init();
            this.e유저관리.Init();
        }

        private void B레이져각인세팅저장_Click(object sender, EventArgs e)
        {
            Global.환경설정.레이져각인기마킹내용 = this.e큐알각인내용.Text;
            Global.환경설정.레이져각인기작업번호 = (int)this.e작업번호.Value;
            Global.환경설정.레이져각인기서플라이어코드 = this.e서플라이어코드.Text;
            Global.환경설정.Save();
            Debug.WriteLine("저장완료");
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
        
        private void 강제레이져각인Changed(object sender, EventArgs e) => Global.환경설정.강제레이져각인 = this.e강제레이져각인.IsOn;
        private void 레이져각인구분Changed(object sender, EventArgs e) => Global.환경설정.레이져각인양품불량 = this.e레이져각인구분.IsOn;
        private void 강제검증Changed(object sender, EventArgs e) => Global.환경설정.강제검증 = this.e강제검증.IsOn;
        private void 검증구분Changed(object sender, EventArgs e) => Global.환경설정.검증양품불량 = this.e검증구분.IsOn;
        private void 강제라벨부착Changed(object sender, EventArgs e) => Global.환경설정.강제라벨부착 = this.e강제라벨부착.IsOn;
        private void 라벨부착구분Changed(object sender, EventArgs e) => Global.환경설정.라벨부착양품불량 = this.e라벨부착구분.IsOn;




        //private void 센서리셋(object sender, EventArgs e) => Global.장치통신.센서제로수행(this.e센서리셋.IsOn);
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
