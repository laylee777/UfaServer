using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DSEV.Schemas;
using System;
using System.Windows.Media.Media3D;

namespace DSEV.UI.Controls
{
    public partial class ResultInspection : XtraUserControl
    {
        public ResultInspection()
        {
            InitializeComponent();
        }

        public enum ViewTypes { Auto, Manual }
        private ViewTypes RunType = ViewTypes.Manual;
        VDA590UFA3D UFA = null;
        public void Init(ViewTypes runType = ViewTypes.Manual)
        {
            RunType = runType;
            UFA = new VDA590UFA3D();
            //if (runType == ViewTypes.Auto)
            {
                UFA.CameraPosition = new Point3D(0.6, -2.6, 967);
                UFA.CameraLookDirection = new Vector3D(0, 0, -967);
                UFA.CameraUpDirection = new Vector3D(0, 1, 0);
            }

            this.e결과뷰어.Init(UFA);
            this.e결과목록.Init();

            if (this.RunType == ViewTypes.Auto)
            {
                Global.검사자료.검사완료알림 += 검사완료알림;
                검사완료알림(Global.검사자료.현재검사찾기());
            }
        }

        public void Close() { }

        public void 검사완료알림(검사결과 결과)
        {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => { 검사완료알림(결과); })); return; }
            if (Global.장치상태.자동수동) Global.검사자료.Save();
            this.e결과뷰어.SetResults(결과);
            this.e결과목록.SetResults(결과);
            this.e측정결과.Appearance.ForeColor = 환경설정.ResultColor(결과.측정결과);
            this.eCTQ결과.Properties.Appearance.ForeColor = 환경설정.ResultColor(결과.CTQ결과);
            this.e외관결과.Properties.Appearance.ForeColor = 환경설정.ResultColor(결과.외관결과);
            this.e큐알코드.Properties.Appearance.ForeColor = 환경설정.ResultColor(결과.큐알결과());
            this.e큐알등급.Properties.Appearance.ForeColor = 환경설정.ResultColor(결과.GetItem(검사항목.QrLegibility).측정결과);
            this.Bind검사결과.DataSource = 결과;
            this.Bind검사결과.ResetBindings(false);
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle < 0) return;
            GridView view = sender as GridView;
            검사정보 정보 = view.GetRow(e.RowHandle) as 검사정보;
            if (정보 == null) return;
            정보.SetAppearance(e);
        }
    }
}
