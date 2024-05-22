namespace DSEV.UI.Controls
{
    partial class QrControls
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QrControls));
            DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions buttonImageOptions1 = new DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions();
            this.g코드리더 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.b리딩종료 = new DevExpress.XtraEditors.SimpleButton();
            this.b리딩시작 = new DevExpress.XtraEditors.SimpleButton();
            this.g통신내역 = new DevExpress.XtraEditors.GroupControl();
            this.e통신내역 = new DevExpress.XtraEditors.ListBoxControl();
            this.b장치설정 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.g코드리더)).BeginInit();
            this.g코드리더.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g통신내역)).BeginInit();
            this.g통신내역.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.e통신내역)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            this.SuspendLayout();
            // 
            // g코드리더
            // 
            this.g코드리더.Controls.Add(this.layoutControl2);
            this.g코드리더.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g코드리더.Location = new System.Drawing.Point(0, 0);
            this.g코드리더.Name = "g코드리더";
            this.g코드리더.Size = new System.Drawing.Size(645, 374);
            this.g코드리더.TabIndex = 2;
            this.g코드리더.Text = "QR Reader (V430-F)";
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.b리딩종료);
            this.layoutControl2.Controls.Add(this.b리딩시작);
            this.layoutControl2.Controls.Add(this.g통신내역);
            this.layoutControl2.Controls.Add(this.b장치설정);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(2, 27);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(641, 345);
            this.layoutControl2.TabIndex = 0;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // b리딩종료
            // 
            this.b리딩종료.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b코드리딩종료.ImageOptions.SvgImage")));
            this.b리딩종료.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.b리딩종료.Location = new System.Drawing.Point(20, 46);
            this.b리딩종료.Name = "b리딩종료";
            this.b리딩종료.Size = new System.Drawing.Size(113, 22);
            this.b리딩종료.StyleController = this.layoutControl2;
            this.b리딩종료.TabIndex = 7;
            this.b리딩종료.Text = "Stop";
            // 
            // b리딩시작
            // 
            this.b리딩시작.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b코드리딩시작.ImageOptions.SvgImage")));
            this.b리딩시작.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.b리딩시작.Location = new System.Drawing.Point(20, 20);
            this.b리딩시작.Name = "b리딩시작";
            this.b리딩시작.Size = new System.Drawing.Size(113, 22);
            this.b리딩시작.StyleController = this.layoutControl2;
            this.b리딩시작.TabIndex = 6;
            this.b리딩시작.Text = "Reading";
            // 
            // g코드통신내역
            // 
            this.g통신내역.Controls.Add(this.e통신내역);
            buttonImageOptions1.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("buttonImageOptions1.SvgImage")));
            buttonImageOptions1.SvgImageSize = new System.Drawing.Size(20, 20);
            this.g통신내역.CustomHeaderButtons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("Clear", true, buttonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Clear contents", -1, true, null, true, false, true, null, -1)});
            this.g통신내역.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.g통신내역.Location = new System.Drawing.Point(145, 12);
            this.g통신내역.Name = "g코드통신내역";
            this.g통신내역.Size = new System.Drawing.Size(484, 321);
            this.g통신내역.TabIndex = 12;
            this.g통신내역.Text = "Communications";
            // 
            // e통신내역
            // 
            this.e통신내역.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e통신내역.Location = new System.Drawing.Point(2, 27);
            this.e통신내역.Name = "e통신내역";
            this.e통신내역.Size = new System.Drawing.Size(480, 292);
            this.e통신내역.TabIndex = 11;
            // 
            // b장치설정
            // 
            this.b장치설정.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b코드리더오류.ImageOptions.SvgImage")));
            this.b장치설정.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.b장치설정.Location = new System.Drawing.Point(22, 301);
            this.b장치설정.Name = "b장치설정";
            this.b장치설정.Size = new System.Drawing.Size(109, 22);
            this.b장치설정.StyleController = this.layoutControl2;
            this.b장치설정.TabIndex = 9;
            this.b장치설정.Text = "Settings";
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup3,
            this.layoutControlItem11});
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(641, 345);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem13,
            this.layoutControlItem14,
            this.layoutControlItem9,
            this.emptySpaceItem2});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup3.Size = new System.Drawing.Size(133, 325);
            this.layoutControlGroup3.Text = "명령전송";
            this.layoutControlGroup3.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.b장치설정;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 279);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem9.Size = new System.Drawing.Size(117, 30);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.b리딩시작;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(117, 26);
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 52);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(117, 227);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.b리딩종료;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(117, 26);
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.g통신내역;
            this.layoutControlItem11.Location = new System.Drawing.Point(133, 0);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(488, 325);
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // QrControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.g코드리더);
            this.Name = "QrControls";
            this.Size = new System.Drawing.Size(645, 374);
            ((System.ComponentModel.ISupportInitialize)(this.g코드리더)).EndInit();
            this.g코드리더.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g통신내역)).EndInit();
            this.g통신내역.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.e통신내역)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl g코드리더;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraEditors.GroupControl g통신내역;
        private DevExpress.XtraEditors.ListBoxControl e통신내역;
        private DevExpress.XtraEditors.SimpleButton b장치설정;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.SimpleButton b리딩종료;
        private DevExpress.XtraEditors.SimpleButton b리딩시작;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
    }
}
