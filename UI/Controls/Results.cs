using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DSEV.Schemas;
using DSEV.UI.Forms;
using MvUtils;
using System;
using System.Collections;

namespace DSEV.UI.Controls
{
    public partial class Results : XtraUserControl
    {
        public Results() => InitializeComponent();
        private LocalizationResults 번역 = new LocalizationResults();

        public void Init()
        {
            this.BindLocalization.DataSource = this.번역;
            this.e시작일자.DateTime = DateTime.Today;
            this.e종료일자.DateTime = DateTime.Today;
            this.b자료조회.Click += 자료조회;
            this.b엑셀파일.Click += 엑셀파일;
            this.col최소값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.col기준값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.col최대값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.col보정값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.col결과값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.GridView1.Init(this.barManager1);
            this.GridView1.AddRowSelectedEvent(검사결과보기);
            this.GridView1.AddPopupMemuItem(번역.결과보기, Resources.보기, 검사결과보기);

            if (Global.환경설정.권한여부(유저권한구분.관리자))
            {
                this.GridView1.AddDeleteMenuItem(정보삭제);
                //this.GridView1.AddExpandMasterPopMenuItems();
                //this.GridView1.AddSelectPopMenuItems();
            }
            else
            {
                this.layoutControl1.Visible = false;
                this.GridView1.OptionsDetail.AllowOnlyOneMasterRowExpanded = true;
            }

            this.GridControl1.DataSource = Global.검사자료;
            this.GridControl1.ViewRegistered += GridControl1_ViewRegistered;
            //this.GridView1.RowCountChanged += GridView1_RowCountChanged;
            this.GridView1.CustomDrawCell += GridView1_CustomDrawCell;
            this.GridView2.CustomDrawCell += GridView2_CustomDrawCell;

            Localization.SetColumnCaption(this.GridView1, typeof(검사결과));
            Localization.SetColumnCaption(this.GridView2, typeof(검사정보));
            this.col검사일자.Caption = Localization.일자.GetString();
            this.col검사시간.Caption = Localization.시간.GetString();
        }

        public void Close() { }

        private void 엑셀파일(object sender, EventArgs e) => this.GridView1.BtnXlsExportViewClick(null, null);
        private void 자료조회(object sender, EventArgs e)
        {
            Global.검사자료.Save();
            Global.검사자료.Load(this.e시작일자.DateTime, this.e종료일자.DateTime);
        }

        private void 정보삭제(object sender, ItemClickEventArgs e)
        {
            if (this.GridView1.SelectedRowsCount < 1) return;
            if (!Utils.Confirm(this.FindForm(), 번역.자료삭제, Localization.확인.GetString())) return;

            ArrayList 자료 = this.GridView1.SelectedData() as ArrayList;
            foreach (검사결과 결과 in 자료)
                Global.검사자료.결과삭제(결과);
        }

        private void 검사결과보기(object sender, ItemClickEventArgs e) =>
            검사결과보기(this.GridView1, this.GridView1.FocusedRowHandle);
        private void 검사결과보기(GridView view, Int32 RowHandle)
        {
            검사결과 결과 = view.GetRow(RowHandle) as 검사결과;
            if (결과 == null) return;
            ResultViewer viewer = new ResultViewer(결과);
            viewer.Show(this.FindForm());
        }

        private void 카메라검사보기(GridView view, Int32 RowHandle)
        {
            검사결과 결과 = this.GridView1.GetFocusedRow() as 검사결과;
            검사정보 검사 = view.GetRow(RowHandle) as 검사정보;
            결과?.카메라검사보기(검사);
        }
        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            if (!Global.환경설정.권한여부(유저권한구분.관리자)) return;
            CustomView view = e.View as CustomView;
            view.Init(this.barManager1);
            view.AddRowSelectedEvent(카메라검사보기);
        }

        //private void GridView1_RowCountChanged(object sender, EventArgs e) => (sender as GridView).MoveFirst();
        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle < 0) return;
            GridView view = sender as GridView;
            검사결과 결과 = view.GetRow(e.RowHandle) as 검사결과;
            if (결과 == null) return;
            결과.SetAppearance(e);
        }
        private void GridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle < 0) return;
            GridView view = sender as GridView;
            검사정보 정보 = view.GetRow(e.RowHandle) as 검사정보;
            if (정보 == null) return;
            정보.SetAppearance(e);
        }

        public class LocalizationResults
        {
            private enum Items
            {
                [Translation("Start", "시작")]
                시작일자,
                [Translation("End", "종료")]
                종료일자,
                [Translation("Search", "조  회")]
                조회버튼,
                [Translation("Export to Excel", "엑셀파일로 내보내기")]
                엑셀버튼,
                [Translation("Are you sure you want to delete the selection?", "선택한 검사결과를 삭제하시겠습니까?")]
                자료삭제,
                [Translation("View inspection results", "검사 결과 보기")]
                결과보기,
                [Translation("Enter the QR Code.", "QR Code를 입력하세요.")]
                큐알입력,
                [Translation("No information is available.", "검사정보가 없습니다.")]
                결과없음,
            }

            public String 시작일자 => Localization.GetString(Items.시작일자);
            public String 종료일자 => Localization.GetString(Items.종료일자);
            public String 조회버튼 => Localization.GetString(Items.조회버튼);
            public String 엑셀버튼 => Localization.GetString(Items.엑셀버튼);
            public String 자료삭제 => Localization.GetString(Items.자료삭제);
            public String 결과보기 => Localization.GetString(Items.결과보기);
            public String 큐알입력 => Localization.GetString(Items.큐알입력);
            public String 결과없음 => Localization.GetString(Items.결과없음);
        }
    }
}
