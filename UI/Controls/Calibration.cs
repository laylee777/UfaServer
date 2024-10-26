using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DSEV.Schemas;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Collections;

namespace DSEV.UI.Controls
{
    public partial class Calibration : XtraUserControl
    {
        public Calibration()
        {
            InitializeComponent();
        }

        public void Init()
        {
            e시작.DateTime = DateTime.Today;
            e종료.DateTime = DateTime.Today;
            b검색.ImageOptions.SvgImage = Resources.GetSvgImage(SvgImageType.검색);
            this.b검색.Click += B검색_Click;

            this.b마스터샘플피팅.Click += B마스터샘플피팅_Click;
            this.b데이터추출.Click += B데이터추출_Click;

            this.GridView1.Init(this.barManager1);
            this.GridView1.AddDeleteMenuItem(정보삭제);
            this.GridView1.AddSelectPopMenuItems();
            this.GridView1.OptionsBehavior.Editable = true;
            this.GridView1.OptionsView.ColumnAutoWidth = false;
            this.GridControl1.DataSource = Global.캘리브;
        }

        private void B마스터샘플피팅_Click(object sender, EventArgs e)
        {
            List<string> filePath = Global.검사자료.검사일시추출실행((int)this.e반복횟수.Value, (int)this.e제품갯수.Value);
            List<decimal> OffsetSettings = new List<decimal>();
            Global.모델자료.선택모델.검사설정.ForEach(x => OffsetSettings.Add(x.보정값));

            string 저장경로 = Global.검사자료.알앤알문서작성(filePath, OffsetSettings);
            Boolean r = Global.검사자료.오프셋연산및적용(저장경로);
        }

        private void B데이터추출_Click(object sender, EventArgs e)
        {
            List<string> filePath = Global.검사자료.검사일시추출실행((int)this.e반복횟수.Value, (int)this.e제품갯수.Value);
            List<decimal> OffsetSettings = new List<decimal>();
            Global.모델자료.선택모델.검사설정.ForEach(x => OffsetSettings.Add(x.보정값));
            
            string 저장경로 =  Global.검사자료.알앤알문서작성(filePath, OffsetSettings);
            //Boolean r = Global.검사자료.오프셋연산및적용(저장경로);
        }

        public void Close() { }

        public void BestFit() => this.GridView1.BestFitColumns();

        private void B검색_Click(object sender, EventArgs e) =>
            Global.캘리브.Load(this.e시작.DateTime, this.e종료.DateTime);

        private void 정보삭제(object sender, ItemClickEventArgs e)
        {
            if (this.GridView1.SelectedRowsCount < 1) return;
            if (!Utils.Confirm(this.FindForm(), "선택한 자료를 삭제하시겠습니까?", Localization.확인.GetString())) return;
            ArrayList 자료 = this.GridView1.SelectedData() as ArrayList;
            List<캘리브정보> rows = new List<캘리브정보>();
            foreach (캘리브정보 정보 in 자료) rows.Add(정보);
            Global.캘리브.Removes(rows);
            this.GridView1.DeleteSelectedRows();
        }
    }
}
