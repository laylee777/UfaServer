using DevExpress.XtraEditors;
using DSEV.Schemas;
using System;
using System.Diagnostics;

namespace DSEV.UI.Controls
{
    public partial class QrControls : XtraUserControl
    {
        public QrControls()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.b장치설정.Click += 장치설정;
            this.b리딩시작.Click += 리딩시작;
            this.b리딩종료.Click += 리딩종료;
            this.g통신내역.CustomButtonClick += 내역삭제;
            Global.큐알리더.송신수신알림 += 리더송신수신알림;
        }

        public void Close() { }

        private void 리더송신수신알림(큐알장치.통신구분 통신, 리더명령 명령, String mesg)
        {
            if (this.e통신내역.InvokeRequired)
            {
                this.e통신내역.BeginInvoke(new Action(() => 리더송신수신알림(통신, 명령, mesg)));
                return;
            }
            this.e통신내역.Items.Insert(0, $"{통신.ToString()}: {mesg}");
            this.e통신내역.SelectedIndex = 0;
            while (this.e통신내역.Items.Count > 100)
                this.e통신내역.Items.RemoveAt(this.e통신내역.Items.Count - 1);
        }

        private void 장치설정(object sender, EventArgs e) => Process.Start($"http://{Global.환경설정.큐알리더주소}/WebLink/");
        private void 리딩시작(object sender, EventArgs e) => Global.큐알리더.리딩시작(null);
        private void 리딩종료(object sender, EventArgs e) => Global.큐알리더.리딩종료();
        private void 내역삭제(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e) => this.e통신내역.Items.Clear();
    }
}
