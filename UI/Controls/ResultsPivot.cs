using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraPrinting;
using DSEV.Schemas;
using DSEV.Schemas.Reports;
using DSEV.UI.Forms;
using MvUtils;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DSEV.UI.Controls
{
    public partial class ResultsPivot : XtraUserControl
    {
        public ResultsPivot() => InitializeComponent();

        private Results.LocalizationResults 번역 = new Results.LocalizationResults();
        private 검사내역 검사내역;

        public void Init()
        {
            this.BindLocalization.DataSource = this.번역;
            this.e시작일자.DateTime = DateTime.Today;
            this.e종료일자.DateTime = DateTime.Today;
            this.b자료조회.Click += 자료조회;
            this.b엑셀파일.Click += 엑셀파일;

            foreach(검사그룹 그룹 in typeof(검사그룹).GetEnumValues())
            {
                if (그룹 == 검사그룹.None) continue;
                GridBand band = new GridBand() { Caption = Utils.GetDescription(그룹), VisibleIndex = this.GridView1.Bands.Count };
                band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                band.AppearanceHeader.Options.UseTextOptions = true;
                this.GridView1.Bands.Add(band);
            }
            foreach(장치구분 장치 in typeof(장치구분).GetEnumValues())
            {
                if (장치 == 장치구분.None) continue;
                GridBand band = new GridBand() { Caption = Utils.GetDescription(장치), VisibleIndex = this.GridView1.Bands.Count };
                band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                band.AppearanceHeader.Options.UseTextOptions = true;
                this.GridView1.Bands.Add(band);
            }

            this.GridView1.Columns.Clear();
            this.GridView1.Init();
            this.GridView1.CustomDrawFooterCell += GridView1_CustomDrawFooterCell;
            this.GridView1.DoubleClick += 검사결과보기;
        }

        private GridBand GetBand(String Caption) => this.GridView1.Bands.Where(g => g.Caption == Caption).FirstOrDefault();
        private void 자료조회(object sender, EventArgs e) => this.LoadData(this.e시작일자.DateTime, this.e종료일자.DateTime);
        private void LoadData(DateTime 시작, DateTime 종료) => LoadData(Global.환경설정.선택모델, 시작, 종료);
        private void LoadData(모델구분 모델, DateTime 시작, DateTime 종료)
        {
            this.GridControl1.DataSource = null;
            this.GridView1.Columns.Clear();
            this.GridView1.FormatRules.Clear();
            if (모델 == 모델구분.None) return;

            this.검사내역?.Dispose();
            this.검사내역 = new 검사내역(모델);
            this.검사내역.Load(Global.검사자료.GetData(시작, 종료, 모델));

            this.GridControl1.DataSource = this.검사내역.검사자료;
            foreach (BandedGridColumn gCol in this.GridView1.Columns)
            {
                if (!this.검사내역.검사자료.Columns.Contains(gCol.FieldName)) continue;
                DataColumn dCol = this.검사내역.검사자료.Columns[gCol.FieldName];
                gCol.Caption = dCol.Caption;
                this.GetBand(this.검사내역.GetBand(dCol))?.Columns.Add(gCol);
                gCol.Visible = this.검사내역.GetVisiable(dCol);
                gCol.Fixed = this.검사내역.GetFixedStyle(dCol);
                gCol.ToolTip = this.검사내역.GetToolTip(dCol);
                FormatType FormatType = this.검사내역.GetFormatType(dCol);
                if (FormatType != DevExpress.Utils.FormatType.None)
                {
                    gCol.DisplayFormat.FormatType = FormatType;
                    gCol.DisplayFormat.FormatString = this.검사내역.GetFormatString(dCol);
                    if (FormatType == DevExpress.Utils.FormatType.Numeric)
                    {
                        if (gCol.DisplayFormat.FormatString.Contains(",")) gCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Default;
                        else gCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    }
                    else if (FormatType == DevExpress.Utils.FormatType.DateTime)
                        gCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;

                    Object sVal = this.검사내역.GetStandardValue(dCol);
                    if (sVal != null)
                    {
                        gCol.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                        gCol.SummaryItem.DisplayFormat = gCol.DisplayFormat.FormatString;
                        gCol.SummaryItem.FieldName = gCol.FieldName;
                        gCol.SummaryItem.Tag = sVal;
                    }
                }
                else gCol.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            }

            foreach (GridBand band in this.GridView1.Bands)
                band.Visible = band.Columns.Count > 0;

            foreach (검사정보 설정 in Global.모델자료.GetItem(모델).검사설정)
                this.CreateFormatConditionRule(설정);

            this.CreateFormatConditionRule(this.GridView1.Columns[nameof(검사결과.측정결과)], 결과구분.OK);
            this.GridView1.BestFitColumns();
        }

        private void CreateFormatConditionRule(GridColumn tCol, Object value)
        {
            if (tCol == null) return;
            GridFormatRule Rule = new GridFormatRule();
            FormatConditionRuleValue Condition = new FormatConditionRuleValue();
            Rule.Name = tCol.FieldName + "_Rule";
            Condition.PredefinedName = "Red Text";
            Condition.Condition = FormatCondition.NotEqual;
            Condition.Value1 = value;
            Rule.Rule = Condition;
            Rule.Column = tCol;
            Rule.ColumnApplyTo = tCol;
            this.GridView1.FormatRules.Add(Rule);
        }

        private void CreateFormatConditionRule(검사정보 설정)
        {
            String name = $"C{설정.검사항목}";
            BandedGridColumn tCol = this.GridView1.Columns[name];
            BandedGridColumn rCol = this.GridView1.Columns[name + "R"];
            if (tCol == null || rCol == null) return;
            GridFormatRule Rule = new GridFormatRule();
            FormatConditionRuleValue Condition = new FormatConditionRuleValue();
            Rule.Name = name + "_Rule";
            Condition.PredefinedName = "Red Text";
            Condition.Condition = FormatCondition.NotEqual;
            Condition.Value1 = true;
            Rule.Rule = Condition;
            Rule.Column = rCol;
            Rule.ColumnApplyTo = tCol;
            this.GridView1.FormatRules.Add(Rule);
        }

        private void GridView1_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.SummaryItem.Tag == null) return;
            e.Info.DisplayText = Convert.ToString(e.Column.SummaryItem.Tag);// Utils.FormatNumeric(e.Column.SummaryItem.Tag, e.Column.SummaryItem.DisplayFormat);
            if (e.Column.AppearanceCell.TextOptions.HAlignment == HorzAlignment.Center)
                e.Info.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        }

        private void 검사결과보기(object sender, EventArgs e)
        {
            DataRow row = this.GridView1.GetFocusedDataRow();
            if (row == null) return;
            DateTime 일시 = Convert.ToDateTime(row[nameof(검사결과.검사일시)]);
            Int32 코드 = Utils.IntValue(row[nameof(검사결과.검사코드)]);
            검사결과 결과 = Global.검사자료.GetItem(일시, Global.환경설정.선택모델, 코드);
            if (결과 == null) { Utils.WarningMsg(번역.결과없음); return; }
            ResultViewer viewer = new ResultViewer(결과);
            viewer.Show(this.FindForm());
        }

        private void 엑셀파일(object sender, EventArgs e)
        {
            String path = String.Empty;
            using (SaveFileDialog dialog = Utils.GetSaveFileDialog("xlsx", "Excel File|*.xlsx", "Reports"))
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                path = dialog.FileName;
            }
            if (String.IsNullOrEmpty(path)) return;

            IPrintable Component = this.GridControl1 as IPrintable;
            PrintingSystem PrintingSystem = new PrintingSystem();

            PrintableComponentLink PrintableComponentLink1 = new PrintableComponentLink();
            PrintingSystem.Links.Add(PrintableComponentLink1);
            PrintingSystem.PageSettings.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A3Extra;
            PrintingSystem.PageSettings.Landscape = true;
            PrintableComponentLink1.Component = Component;
            PrintableComponentLink1.PrintingSystem = PrintingSystem;
            PrintableComponentLink1.PaperKind = PrintingSystem.PageSettings.PaperKind;
            PrintableComponentLink1.Landscape = PrintingSystem.PageSettings.Landscape;
            PrintableComponentLink1.CreateDocument(PrintingSystem);
            PrintingSystem.ExportToXlsx(path);
            Process.Start(path);
        }
    }
}
