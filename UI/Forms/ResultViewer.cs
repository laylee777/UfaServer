using DevExpress.XtraEditors;
using DSEV.Schemas;
using System;

namespace DSEV.UI.Forms
{
    public partial class ResultViewer : XtraForm
    {
        public 검사결과 결과 { get; set; } = null;
        public ResultViewer() => InitializeComponent();
        public ResultViewer(검사결과 결과)
        {
            InitializeComponent();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.결과 = 결과;
            this.e결과뷰어.Init(UI.Controls.ResultInspection.ViewTypes.Manual);
            this.Shown += FormShown;
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (this.결과 == null) return;
            this.e결과뷰어.검사완료알림(this.결과);
        }
    }
}