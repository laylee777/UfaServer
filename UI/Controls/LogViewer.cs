using DevExpress.XtraEditors;
using DSEV.Schemas;
using MvUtils;
using System;

namespace DSEV.UI.Controls
{
    public partial class LogViewer : XtraUserControl
    {
        public LogViewer() => InitializeComponent();

        private LocalizationLogs 번역 = new LocalizationLogs();
        public void Init()
        {
            this.BindLocalization.DataSource = 번역;
            Localization.SetColumnCaption(this.GridView1, typeof(로그정보));

            e시작.DateTime = DateTime.Today;
            e종료.DateTime = DateTime.Today;
            b검색.ImageOptions.SvgImage = Resources.GetSvgImage(SvgImageType.검색);
            GridView1.Init();
            GridControl1.DataSource = Global.로그자료;
            this.b검색.Click += B검색_Click;
        }

        public void Close() { }
        public void Shown()
        {
            this.GridView1.RefreshData();
            this.GridView1.BestFitColumns();
        }

        private void B검색_Click(object sender, EventArgs e)
        {
            Global.로그자료.Load(this.e시작.DateTime, this.e종료.DateTime);
            this.GridView1.RefreshData();
        }

        private class LocalizationLogs
        {
            private enum Items
            {
                [Translation("Start Day", "시작일자")]
                시작일자,
                [Translation("End Day", "종료일자")]
                종료일자,
                [Translation("Search", "조  회")]
                조회버튼,
            }

            public String 시작일자 { get { return Localization.GetString(Items.시작일자); } }
            public String 종료일자 { get { return Localization.GetString(Items.종료일자); } }
            public String 조회버튼 { get { return Localization.GetString(Items.조회버튼); } }
        }
    }
}