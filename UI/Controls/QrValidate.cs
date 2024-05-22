using MvUtils;
using DevExpress.XtraEditors;
using System;
using System.Diagnostics;
using System.Windows;

namespace DSEV.UI.Controls
{
    public partial class QrValidate : XtraUserControl
    {
        public QrValidate()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.GridView1.Init();
            this.GridView1.OptionsBehavior.Editable = true;
            this.Bind검증자료.DataSource = Global.큐알검증;
            this.GridControl1.DataSource = Global.큐알검증.검증자료;
            this.e샘플보기.ButtonClick += 샘플보기;
            this.e코드검증.ButtonClick += 코드검증;
            this.b설정저장.Click += 설정저장;
        }

        private void 설정저장(object sender, EventArgs e)
        {
            if (!Utils.Confirm(this.FindForm(), "설정을 저장하시겠습니까?")) return;
            Global.큐알검증.Save();
        }

        private void 코드검증(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
                this.e코드검증.Text = String.Empty;
                return;
            }
            String code = this.e코드검증.Text.Trim();
            if (String.IsNullOrEmpty(code)) return;
            Global.큐알검증.검증수행(code, out String 유형오류, out Int32[] indexs);
            Global.큐알검증.중복검사(code, indexs, out String 중복오류);
            //Debug.WriteLine($"{유형오류}, {중복오류}");
            if (String.IsNullOrEmpty(유형오류) && String.IsNullOrEmpty(중복오류)) Utils.InfoMsg($"정상입니다.");
            else Utils.WarningMsg($"오류: {유형오류} {중복오류}");
        }

        private void 샘플보기(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
                this.e샘플보기.Text = String.Empty;
                return;
            }
            this.e샘플보기.Text = Global.큐알검증.예제코드();
            Clipboard.SetText(this.e샘플보기.Text);
        }
    }
}
