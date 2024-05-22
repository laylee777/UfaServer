using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DSEV.UI.Controls
{
    public partial class Login : XtraForm
    {
        private LocalizationLogin 번역 = new LocalizationLogin();
        private String 로그영역 { get { return 번역.타이틀; } }
        public Login()
        {
            InitializeComponent();
            this.BindLocalization.DataSource = this.번역;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.e영어.Text = Utils.GetDescription(Language.EN);
            this.e한글.Text = Utils.GetDescription(Language.KO);
            this.e영어.Tag = Language.EN;
            this.e한글.Tag = Language.KO;
            this.e영어.CheckedChanged += 언어선택;
            this.e한글.CheckedChanged += 언어선택;
            if (Localization.CurrentLanguage == Language.KO) this.e한글.Checked = true;
            else this.e영어.Checked = true;
        }

        private void 언어변경(Language 언어)
        {
            Properties.Settings.Default.Language = (Int32)언어;
            if (언어 == Language.KO) MvUtils.Localization.CurrentLanguage = MvUtils.Localization.Language.KO;
            else MvUtils.Localization.CurrentLanguage = MvUtils.Localization.Language.EN;
        }
        private void 언어선택(object sender, EventArgs e)
        {
            CheckButton btn = sender as CheckButton;
            if (!btn.Checked) return;
            this.언어변경((Language)btn.Tag);
            this.BindLocalization.ResetBindings(false);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.e사용자명.Properties.Items.AddRange(Global.유저자료.사용자목록());
            if (!String.IsNullOrEmpty(Properties.Settings.Default.UserName) && Global.유저자료.GetItem(Properties.Settings.Default.UserName) != null)
                this.e사용자명.Text = Properties.Settings.Default.UserName;

            this.Shown += Login_Shown;
            this.b인증.Click += B인증_Click;
            this.b취소.Click += B취소_Click;
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.e사용자명.Text))
                this.e비밀번호.Focus();
        }

        private void B취소_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void B인증_Click(object sender, EventArgs e)
        {
            string 사용자명 = Utils.StrValue(this.e사용자명.Text);
            string 비밀번호 = Utils.StrValue(this.e비밀번호.Text);
            Global.유저자료.비밀번호확인(사용자명, 비밀번호);
            if (Global.환경설정.사용권한 != Schemas.유저권한구분.없음) this.DialogResult = DialogResult.OK;
            else
            {
                Global.환경설정.시스템관리자인증(사용자명, 비밀번호);
                if (Global.환경설정.사용권한 == Schemas.유저권한구분.시스템) this.DialogResult = DialogResult.OK;
                else
                {
                    Global.정보로그(로그영역, 번역.로그인, $"[{사용자명}] {번역.인증오류}", false);
                    Utils.WarningMsg(번역.인증오류, Localization.경고.GetString());
                    this.e비밀번호.Focus();
                }
            }
        }

        private class LocalizationLogin
        {
            private enum Items
            {
                [Translation("User Authentication", "사용자 인증", "Overenie používateľa")]
                타이틀,
                [Translation("Login", "로그인", "Prihlásiť sa")]
                로그인,
                [Translation("User", "사용자명", "Používateľ")]
                사용자명,
                [Translation("Password", "비밀번호", "Heslo")]
                비밀번호,
                [Translation("Username or password is incorrect.", "사용자명 또는 비밀번호가 올바르지 않습니다.", "Uživateľské meno alebo heslo je nesprávne.")]
                인증오류,
            }
            private String GetString(Items item) { return Localization.GetString(item); }

            public String 타이틀 { get { return GetString(Items.타이틀); } }
            public String 로그인 { get { return GetString(Items.로그인); } }
            public String 사용자명 { get { return GetString(Items.사용자명); } }
            public String 비밀번호 { get { return GetString(Items.비밀번호); } }
            public String 인증오류 { get { return GetString(Items.인증오류); } }
            public String 취소 { get { return Localization.취소.GetString(); } }
        }
    }
}