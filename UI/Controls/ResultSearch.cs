using DevExpress.XtraEditors;
using DSEV.Schemas;
using MvUtils;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DSEV.UI.Controls
{
    public partial class ResultSearch : XtraUserControl
    {
        public ResultSearch() => InitializeComponent();
        private Results.LocalizationResults 번역 = new Results.LocalizationResults();

        public void Init()
        {
            this.BindLocalization.DataSource = this.번역;
            this.e시작일자.DateTime = DateTime.Today.AddDays(-Global.큐알검증.중복일수);
            this.e종료일자.DateTime = DateTime.Today;
            this.b자료조회.Click += 자료조회;
            this.e결과뷰어.Init(ResultInspection.ViewTypes.Manual);

        }
        public void Close() { }
        private void 자료조회(object sender, EventArgs e)
        {
            String 큐알 = this.e큐알코드.Text.Trim();
            String 로트 = String.Empty;
            if (String.IsNullOrEmpty(큐알)) { Utils.WarningMsg(번역.큐알입력); return; }
            Regex rx = new Regex("^([0-9]{10})[A-Z]?$");
            Match match = rx.Match(큐알);
            if (match.Success)
            {
                로트 = match.Groups[1].Value;
                큐알 = String.Empty;
            }
            //else if(!Global.큐알검증.검증수행(큐알, out String 오류내용, out int[] indexs)) { Utils.WarningMsg($"QR Error: {오류내용}"); return; }

            검사결과 결과 = Global.검사자료.GetItem(this.e시작일자.DateTime, this.e종료일자.DateTime, Global.환경설정.선택모델, 큐알, 로트);
            if (결과 == null) { Utils.WarningMsg(번역.결과없음); return; }
            this.e결과뷰어.검사완료알림(결과);
        }
    }
}
