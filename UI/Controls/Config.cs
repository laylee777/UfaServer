using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Windows.Forms;
using DSEV.Schemas;

namespace DSEV.UI.Controls
{
    public partial class Config : XtraUserControl
    {
        private LocalizationConfig 번역 = new LocalizationConfig();
        public Config()
        {
            InitializeComponent();
            this.BindLocalization.DataSource = this.번역;
            this.g환경설정.Text = 환경설정.로그영역.GetString();
        }

        public void Init()
        {
            //this.e사진저장.Init();
            this.Bind환경설정.DataSource = Global.환경설정;
            this.d기본경로.SelectedPath = Global.환경설정.기본경로;
            this.d사본저장.SelectedPath = Global.환경설정.사진저장;
            this.d원본저장.SelectedPath = Global.환경설정.원본보관폴더;
            this.e기본경로.ButtonClick += 기본경로설정;
            this.e원본경로.ButtonClick += 원본경로설정;
            this.e사본경로.ButtonClick += 사본경로설정;
            this.b설정저장.Click += 환경설정저장;
        }

        public void Close() { }

        private void 사본경로설정(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d사본저장.ShowDialog() == DialogResult.OK)
                this.e사본경로.Text = this.d사본저장.SelectedPath;
        }

        private void 원본경로설정(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d원본저장.ShowDialog() == DialogResult.OK)
                this.e원본경로.Text = this.d원본저장.SelectedPath;
        }

        private void 기본경로설정(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d기본경로.ShowDialog() == DialogResult.OK)
                this.e기본경로.Text = this.d기본경로.SelectedPath;
        }

        private void 환경설정저장(object sender, EventArgs e)
        {
            this.Bind환경설정.EndEdit();
            if (!Utils.Confirm(this.FindForm(), 번역.저장확인, Localization.확인.GetString())) return;
            Global.환경설정.Save();
            Global.정보로그(환경설정.로그영역.GetString(), 번역.설정저장, 번역.저장완료, true);
        }

        private class LocalizationConfig
        {
            private enum Items
            {
                [Translation("Save", "설정저장")]
                설정저장,
                [Translation("It's saved.", "저장되었습니다.")]
                저장완료,
                [Translation("Save your preferences?", "환경설정을 저장하시겠습니까?")]
                저장확인,
            }

            public String 기본경로   => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.기본경로)));
            public String 문서저장   => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.문서저장)));
            public String 사진저장   => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.사진저장)));
            public String 원본보관폴더 => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.원본보관폴더)));
            public String 원본보관일수 => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.원본보관일수)));
            public String 결과보관   => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.결과보관)));
            public String 로그보관   => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.로그보관)));
            public String 결과자릿수 => Localization.GetString(typeof(환경설정).GetProperty(nameof(환경설정.결과자릿수)));
            public String 설정저장 => Localization.GetString(Items.설정저장);
            public String 저장완료 => Localization.GetString(Items.저장완료);
            public String 저장확인 => Localization.GetString(Items.저장확인);
        }
    }
}
