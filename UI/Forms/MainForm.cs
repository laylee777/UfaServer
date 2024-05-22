using DevExpress.XtraWaitForm;
using MvUtils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSEV.Schemas;
using DSEV.UI.Controls;
using System.Diagnostics;
using DSEV.UI.Forms;

namespace DSEV
{
    public partial class MainForm : DevExpress.XtraBars.TabForm
    {
        private LocalizationMain 번역 = new LocalizationMain();
        private UI.Controls.WaitForm WaitForm;
        private StateForm StateForm = null;
        private Int32 TestIndexNum = 1;
        public MainForm()
        {
            InitializeComponent();
            this.ShowWaitForm();
            this.e프로젝트.Caption = $"IVM: {환경설정.프로젝트번호}";
            this.SetLocalization();
            this.TabFormControl.SelectedPage = this.p결과뷰어;
            this.p환경설정.Enabled = false;
            this.p검사내역.Enabled = false;
            this.Shown += MainFormShown;
            this.FormClosing += MainFormClosing;
            //this.TabFormControl.SelectedPageChanged += SelectedPageChanged;
            //this.t환경설정.SelectedPageChanged += SelectedTabPageChanged;
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;



            //MES테스트용
            this.타이틀.ItemDoubleClick += 타이틀_ItemDoubleClick;
        }

        private void 타이틀_ItemDoubleClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            MESSAGE message = new MESSAGE();
            message.MSG_ID = "REQ_PROCESS_END";
            message.SYSTEMID = "IVM01";
            message.DATE_TIME = DateTime.Now.ToString();
            message.BARCODE_ID = "F00395AB231;F00395AB231";
            message.KEY = "52";

            Global.mes통신.자료송신(message);

            Debug.WriteLine("자료송신");
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.H)
            {
                Debug.WriteLine("R : 카메라전체 Ready");

            }
            if (e.KeyCode == Keys.Q)
            {
                Debug.WriteLine("Q눌림^^");
                Global.그랩제어.Active(카메라구분.Cam01);


            }
            if (e.KeyCode == Keys.W)
            {
                Debug.WriteLine("W눌림^^");
                Global.그랩제어.Active(카메라구분.Cam02);
                Global.그랩제어.Active(카메라구분.Cam03);

            }
            if (e.KeyCode == Keys.S)
            {
                Debug.WriteLine("S눌림^^");
                //Global.그랩제어.Active(카메라구분.Cam02);
                //Global.그랩제어.Active(카메라구분.Cam03);
                Debug.WriteLine($"투입버퍼 : {Global.장치통신.제품투입번호}\n검사지그1 : {Global.장치통신.상부촬영번호}\n검사지그2 : {Global.장치통신.상부인슐폭촬영번호}\n이송장치1 : {Global.장치통신.하부표면검사번호}\n검사지그4 : {Global.장치통신.레이져각인검사번호}\n검사지그5 : {Global.장치통신.큐알검증기검사번호}\n배출버퍼 : {Global.장치통신.결과요청번호}");
            }
            if (e.KeyCode == Keys.L)
            {
                Debug.WriteLine("L눌림^^");
                Global.라벨부착기제어.라벨부착(999);
                //Global.레이져마킹제어.레이져마킹시작(25);
                //Global.그랩제어.Active(카메라구분.Cam02);
                //Global.그랩제어.Active(카메라구분.Cam03);
                //Debug.WriteLine($"투입버퍼 : {Global.장치통신.제품투입번호}\n검사지그1 : {Global.장치통신.상부촬영번호}\n검사지그2 : {Global.장치통신.상부인슐폭촬영번호}\n이송장치1 : {Global.장치통신.하부표면검사번호}\n검사지그4 : {Global.장치통신.레이져각인검사번호}\n검사지그5 : {Global.장치통신.큐알검증기검사번호}\n배출버퍼 : {Global.장치통신.결과요청번호}");
            }
            if (e.KeyCode == Keys.D)
            {
                try {
                    Debug.WriteLine("D눌림^^");
                    Global.센서제어.ReadValues(센서컨트롤러.컨트롤러1, 999);
                    Global.센서제어.ReadValues(센서컨트롤러.컨트롤러2, 999);
                    Global.센서제어.ReadValues(센서컨트롤러.컨트롤러3, 999);
                } 
                catch(Exception a)
                {
                    Global.오류로그("Testing", "센서제어", a.Message, true);
                }
                //Debug.WriteLine("D눌림^^");
                Global.센서제어.ReadValues(센서컨트롤러.컨트롤러1, 999);
                Global.센서제어.ReadValues(센서컨트롤러.컨트롤러2, 999);
                Global.센서제어.ReadValues(센서컨트롤러.컨트롤러3, 999);
                //Global.레이져마킹제어.레이져마킹시작(25);
                //Global.그랩제어.Active(카메라구분.Cam02);
                //Global.그랩제어.Active(카메라구분.Cam03);
                //Debug.WriteLine($"투입버퍼 : {Global.장치통신.제품투입번호}\n검사지그1 : {Global.장치통신.상부촬영번호}\n검사지그2 : {Global.장치통신.상부인슐폭촬영번호}\n이송장치1 : {Global.장치통신.하부표면검사번호}\n검사지그4 : {Global.장치통신.레이져각인검사번호}\n검사지그5 : {Global.장치통신.큐알검증기검사번호}\n배출버퍼 : {Global.장치통신.결과요청번호}");
            }


            if (e.KeyCode == Keys.C)
            {
                try
                {
                    Debug.WriteLine("C눌림^^");
                    //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러1, 999);
                    //Global.피씨통신.CTQ1검사(999);

                    Global.피씨통신.검사시작(999);
                }
                catch (Exception a)
                {
                    Global.오류로그("Testing", "센서제어", a.Message, true);
                }
                //Debug.WriteLine("D눌림^^");
                //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러1, 999);
                //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러2, 999);
                //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러3, 999);
                //Global.레이져마킹제어.레이져마킹시작(25);
                //Global.그랩제어.Active(카메라구분.Cam02);
                //Global.그랩제어.Active(카메라구분.Cam03);
                //Debug.WriteLine($"투입버퍼 : {Global.장치통신.제품투입번호}\n검사지그1 : {Global.장치통신.상부촬영번호}\n검사지그2 : {Global.장치통신.상부인슐폭촬영번호}\n이송장치1 : {Global.장치통신.하부표면검사번호}\n검사지그4 : {Global.장치통신.레이져각인검사번호}\n검사지그5 : {Global.장치통신.큐알검증기검사번호}\n배출버퍼 : {Global.장치통신.결과요청번호}");
            }

            if (e.KeyCode == Keys.T)
            {
                try
                {
                    Debug.WriteLine("T눌림^^");
                    //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러1, 999);
                    //Global.피씨통신.CTQ1검사(999);

                    Global.피씨통신.검사시작(999);
                }
                catch (Exception a)
                {
                    Global.오류로그("Testing", "센서제어", a.Message, true);
                }
                //Debug.WriteLine("D눌림^^");
                //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러1, 999);
                //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러2, 999);
                //Global.센서제어.ReadValues(센서컨트롤러.컨트롤러3, 999);
                //Global.레이져마킹제어.레이져마킹시작(25);
                //Global.그랩제어.Active(카메라구분.Cam02);
                //Global.그랩제어.Active(카메라구분.Cam03);
                //Debug.WriteLine($"투입버퍼 : {Global.장치통신.제품투입번호}\n검사지그1 : {Global.장치통신.상부촬영번호}\n검사지그2 : {Global.장치통신.상부인슐폭촬영번호}\n이송장치1 : {Global.장치통신.하부표면검사번호}\n검사지그4 : {Global.장치통신.레이져각인검사번호}\n검사지그5 : {Global.장치통신.큐알검증기검사번호}\n배출버퍼 : {Global.장치통신.결과요청번호}");
            }
            if (e.KeyCode == Keys.Z)
            {
                MESSAGE message = new MESSAGE();
                message.MSG_ID = "REQ_PROCESS_START";
                message.SYSTEMID = "EQU050";
                message.DATE_TIME = "2024-04-03 14:35:29.55808";
                message.BARCODE_ID = "F00395AB231;F00395AB231";
                message.KEY = TestIndexNum.ToString("0000");
                
                TestIndexNum++;
                
                Global.mes통신.자료송신(message);

                Debug.WriteLine("자료송신");
            }
            if (e.KeyCode == Keys.X)
            {
                MESSAGE message = new MESSAGE();
                message.MSG_ID = "REQ_PROCESS_END";
                message.SYSTEMID = "EQU050";
                message.DATE_TIME = "2024-04-03 14:35:29.55808";
                message.BARCODE_ID = "F00395AB231;F00395AB231";
                message.KEY = TestIndexNum.ToString("0000");
                Global.mes통신.자료송신(message);
                


                Debug.WriteLine("자료송신");

            }


        }

        private void ShowWaitForm()
        {
            WaitForm = new UI.Controls.WaitForm() { ShowOnTopMode = ShowFormOnTopMode.AboveAll };
            WaitForm.Show(this);
        }
        private void HideWaitForm() => WaitForm.Close();

        private void MainFormShown(object sender, EventArgs e)
        {
            Global.Initialized += GlobalInitialized;
            Task.Run(() => { Global.Init(); });
        }

        private void GlobalInitialized(object sender, Boolean e) =>
            this.BeginInvoke(new Action(() => GlobalInitialized(e)));
        private void GlobalInitialized(Boolean e)
        {
            Global.Initialized -= GlobalInitialized;
            if (!e) { this.Close(); return; }
            this.HideWaitForm();
            Common.SetForegroundWindow(this.Handle.ToInt32());

            //// 로그인
            //Login login = new Login();
            //if (Utils.ShowDialog(login, this) == DialogResult.OK)
            //{
            //    Global.DxLocalization();
            //    this.Init();
            //    Global.Start();
            //}
            //else this.Close();

            //if (Global.환경설정.동작구분 == 동작구분.Live)
            //{
            //}
            //else
            //{
            ////자동로그인
            Global.환경설정.시스템관리자로그인();
            Localization.SetCulture();
            Global.DxLocalization();
            this.Init();
            Global.Start();
            //}
        }

        private void Init()
        {
            this.SetLocalization();
            this.e결과뷰어.Init(ResultInspection.ViewTypes.Auto);
            this.e검사도구.Init();
            this.e검사설정.Init();
            this.e장치설정.Init();
            this.e검사내역.Init();
            this.e검사피봇.Init();
            this.e결과검색.Init();
            this.e상태뷰어.Init();
            this.e로그내역.Init();
            this.e큐알검증.Init();
            this.p환경설정.Enabled = Global.환경설정.권한여부(유저권한구분.시스템);
            this.p검사내역.Enabled = Global.환경설정.권한여부(유저권한구분.관리자);
            this.TabFormControl.AllowMoveTabs = false;
            this.TabFormControl.AllowMoveTabsToOuterForm = false;

            if (Global.환경설정.동작구분 == 동작구분.Live)
                this.WindowState = FormWindowState.Maximized;
            //this.ShowHideControl();

            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            foreach (Screen s in Screen.AllScreens)
            {
                Debug.WriteLine(s.Bounds, s.DeviceName);
                if (s.Primary) continue;
                ShowStateForm(s);
            }
            // 창이 생성되지 않았으면 메인 모니터에 띄움
            ShowStateForm(Screen.PrimaryScreen);
        }

        private void ShowStateForm(Screen s)
        {
            if (this.StateForm != null) return;
            this.StateForm = new StateForm() { StartPosition = FormStartPosition.Manual, WindowState = FormWindowState.Maximized };
            this.StateForm.SetBounds(s.WorkingArea.X, s.WorkingArea.Y, s.WorkingArea.Width, s.WorkingArea.Height);
            this.StateForm.Show(this);
        }

        private void CloseForm()
        {
            this.e장치설정.Close();
            this.e로그내역.Close();
            this.e상태뷰어.Close();
            Global.Close();
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.환경설정.사용권한 == 유저권한구분.없음) this.CloseForm();
            else
            {
                e.Cancel = !Utils.Confirm(this, 번역.종료확인, Localization.확인.GetString());
                if (!e.Cancel) this.CloseForm();
            }
        }

        //private void SelectedPageChanged(object sender, DevExpress.XtraBars.TabFormSelectedPageChangedEventArgs e)
        //{
        //    ShowHideControl();
        //}
        //private void SelectedTabPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        //{
        //    ShowHideControl();
        //}
        //private void ShowHideControl()
        //{
        //    //this.e그랩뷰어.Shown(this.TabFormControl.SelectedPage == this.p그랩뷰어);
        //    //this.e장치설정.Shown(this.TabFormControl.SelectedPage == this.p환경설정 && this.t환경설정.SelectedTabPage == this.t장치설정);
        //    //this.e환경설정.Shown(this.TabFormControl.SelectedPage == this.p환경설정 && this.t환경설정.SelectedTabPage == this.t검사설정);
        //}

        private void SetLocalization()
        {
            this.Text = this.번역.타이틀;
            this.타이틀.Caption = this.번역.타이틀;
            this.p결과뷰어.Text = this.번역.검사하기;
            this.p검사도구.Text = this.번역.카메라;
            this.p검사내역.Text = this.번역.검사내역;
            this.p환경설정.Text = this.번역.환경설정;
            this.t검사설정.Text = this.번역.검사설정;
            this.t장치설정.Text = this.번역.장치설정;
            this.t큐알검증.Text = this.번역.큐알검증;
            this.t로그내역.Text = this.번역.로그내역;
        }

        private class LocalizationMain
        {
            private enum Items
            {
                [Translation("Inspection", "검사하기", "Inšpekcia")]
                검사하기,
                [Translation("History", "검사내역", "História")]
                검사내역,
                [Translation("Preferences", "환경설정", "Predvoľby")]
                환경설정,
                [Translation("Settings", "검사설정", "Nastavenie")]
                검사설정,
                [Translation("Devices", "장치설정", "Zariadenia")]
                장치설정,
                [Translation("Cameras", "카메라", "Kamery")]
                카메라,
                [Translation("QR Validate", "큐알검증", "QR Validate")]
                큐알검증,
                [Translation("Logs", "로그내역", "Denníky")]
                로그내역,
                [Translation("Are you want to exit the program?", "프로그램을 종료하시겠습나까?", "Naozaj chcete ukončiť program?")]
                종료확인,
            }
            private String GetString(Items item) { return Localization.GetString(item); }

            public String 타이틀   { get => Localization.제목.GetString(); }
            public String 검사하기 { get => GetString(Items.검사하기); }
            public String 검사내역 { get => GetString(Items.검사내역); }
            public String 환경설정 { get => GetString(Items.환경설정); }
            public String 검사설정 { get => GetString(Items.검사설정); }
            public String 장치설정 { get => GetString(Items.장치설정); }
            public String 카메라   { get => GetString(Items.카메라); }
            public String 큐알검증 { get => GetString(Items.큐알검증); }
            public String 로그내역 { get => GetString(Items.로그내역); }
            public String 종료확인 { get => GetString(Items.종료확인); }
        }
    }
}