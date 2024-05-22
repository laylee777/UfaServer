
namespace DSEV
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.tabFormControl1 = new DevExpress.XtraBars.TabFormControl();
            this.타이틀 = new DevExpress.XtraBars.BarStaticItem();
            this.skinPaletteDropDownButtonItem1 = new DevExpress.XtraBars.SkinPaletteDropDownButtonItem();
            this.e프로젝트 = new DevExpress.XtraBars.BarStaticItem();
            this.p결과뷰어 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer1 = new DevExpress.XtraBars.TabFormContentContainer();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.e결과뷰어 = new DSEV.UI.Controls.ResultInspection();
            this.e상태뷰어 = new DSEV.UI.Controls.State();
            this.p검사도구 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer4 = new DevExpress.XtraBars.TabFormContentContainer();
            this.e검사도구 = new DSEV.UI.Controls.CamViewers();
            this.p검사내역 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer3 = new DevExpress.XtraBars.TabFormContentContainer();
            this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.t내역조회 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.e검사내역 = new DSEV.UI.Controls.Results();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.e검사피봇 = new DSEV.UI.Controls.ResultsPivot();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.e결과검색 = new DSEV.UI.Controls.ResultSearch();
            this.t결과보고 = new DevExpress.XtraTab.XtraTabPage();
            this.p환경설정 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer2 = new DevExpress.XtraBars.TabFormContentContainer();
            this.t환경설정 = new DevExpress.XtraTab.XtraTabControl();
            this.t검사설정 = new DevExpress.XtraTab.XtraTabPage();
            this.e검사설정 = new DSEV.UI.Controls.SetInspection();
            this.t장치설정 = new DevExpress.XtraTab.XtraTabPage();
            this.e장치설정 = new DSEV.UI.Controls.DeviceSettings();
            this.t큐알검증 = new DevExpress.XtraTab.XtraTabPage();
            this.e큐알검증 = new DSEV.UI.Controls.QrValidate();
            this.t로그내역 = new DevExpress.XtraTab.XtraTabPage();
            this.e로그내역 = new DSEV.UI.Controls.LogViewer();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabFormControl1)).BeginInit();
            this.tabFormContentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tabFormContentContainer4.SuspendLayout();
            this.tabFormContentContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).BeginInit();
            this.xtraTabControl2.SuspendLayout();
            this.t내역조회.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.xtraTabPage3.SuspendLayout();
            this.tabFormContentContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.t환경설정)).BeginInit();
            this.t환경설정.SuspendLayout();
            this.t검사설정.SuspendLayout();
            this.t장치설정.SuspendLayout();
            this.t큐알검증.SuspendLayout();
            this.t로그내역.SuspendLayout();
            this.SuspendLayout();
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.AutoHeight = false;
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            // 
            // tabFormControl1
            // 
            this.tabFormControl1.AllowMoveTabs = false;
            this.tabFormControl1.AllowMoveTabsToOuterForm = false;
            this.tabFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.타이틀,
            this.skinPaletteDropDownButtonItem1,
            this.e프로젝트});
            this.tabFormControl1.Location = new System.Drawing.Point(0, 0);
            this.tabFormControl1.Name = "tabFormControl1";
            this.tabFormControl1.Pages.Add(this.p결과뷰어);
            this.tabFormControl1.Pages.Add(this.p검사도구);
            this.tabFormControl1.Pages.Add(this.p검사내역);
            this.tabFormControl1.Pages.Add(this.p환경설정);
            this.tabFormControl1.SelectedPage = this.p결과뷰어;
            this.tabFormControl1.ShowAddPageButton = false;
            this.tabFormControl1.ShowTabCloseButtons = false;
            this.tabFormControl1.ShowTabsInTitleBar = DevExpress.XtraBars.ShowTabsInTitleBar.True;
            this.tabFormControl1.Size = new System.Drawing.Size(1920, 30);
            this.tabFormControl1.TabForm = this;
            this.tabFormControl1.TabIndex = 0;
            this.tabFormControl1.TabLeftItemLinks.Add(this.타이틀);
            this.tabFormControl1.TabRightItemLinks.Add(this.e프로젝트);
            this.tabFormControl1.TabRightItemLinks.Add(this.skinPaletteDropDownButtonItem1);
            this.tabFormControl1.TabStop = false;
            // 
            // 타이틀
            // 
            this.타이틀.Caption = "VDA590 UFA Vision Inspection System";
            this.타이틀.Id = 0;
            this.타이틀.ImageOptions.SvgImage = global::DSEV.Properties.Resources.vision;
            this.타이틀.Name = "타이틀";
            this.타이틀.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // skinPaletteDropDownButtonItem1
            // 
            this.skinPaletteDropDownButtonItem1.Id = 0;
            this.skinPaletteDropDownButtonItem1.Name = "skinPaletteDropDownButtonItem1";
            // 
            // e프로젝트
            // 
            this.e프로젝트.Caption = "IVM: 22-0104-011";
            this.e프로젝트.Id = 0;
            this.e프로젝트.Name = "e프로젝트";
            // 
            // p결과뷰어
            // 
            this.p결과뷰어.ContentContainer = this.tabFormContentContainer1;
            this.p결과뷰어.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p결과뷰어.ImageOptions.SvgImage")));
            this.p결과뷰어.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.p결과뷰어.Name = "p결과뷰어";
            this.p결과뷰어.Text = "Inspection";
            // 
            // tabFormContentContainer1
            // 
            this.tabFormContentContainer1.Controls.Add(this.panelControl1);
            this.tabFormContentContainer1.Controls.Add(this.e상태뷰어);
            this.tabFormContentContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer1.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer1.Name = "tabFormContentContainer1";
            this.tabFormContentContainer1.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer1.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.e결과뷰어);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 104);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1920, 906);
            this.panelControl1.TabIndex = 1;
            // 
            // e결과뷰어
            // 
            this.e결과뷰어.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e결과뷰어.Location = new System.Drawing.Point(2, 2);
            this.e결과뷰어.Name = "e결과뷰어";
            this.e결과뷰어.Size = new System.Drawing.Size(1916, 902);
            this.e결과뷰어.TabIndex = 0;
            // 
            // e상태뷰어
            // 
            this.e상태뷰어.Dock = System.Windows.Forms.DockStyle.Top;
            this.e상태뷰어.Location = new System.Drawing.Point(0, 0);
            this.e상태뷰어.Name = "e상태뷰어";
            this.e상태뷰어.Size = new System.Drawing.Size(1920, 104);
            this.e상태뷰어.TabIndex = 2;
            // 
            // p검사도구
            // 
            this.p검사도구.ContentContainer = this.tabFormContentContainer4;
            this.p검사도구.Name = "p검사도구";
            this.p검사도구.Text = "Cameras";
            // 
            // tabFormContentContainer4
            // 
            this.tabFormContentContainer4.Controls.Add(this.e검사도구);
            this.tabFormContentContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer4.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer4.Name = "tabFormContentContainer4";
            this.tabFormContentContainer4.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer4.TabIndex = 3;
            // 
            // e검사도구
            // 
            this.e검사도구.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사도구.Location = new System.Drawing.Point(0, 0);
            this.e검사도구.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.e검사도구.Name = "e검사도구";
            this.e검사도구.Size = new System.Drawing.Size(1920, 1010);
            this.e검사도구.TabIndex = 1;
            // 
            // p검사내역
            // 
            this.p검사내역.ContentContainer = this.tabFormContentContainer3;
            this.p검사내역.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p검사내역.ImageOptions.SvgImage")));
            this.p검사내역.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.p검사내역.Name = "p검사내역";
            this.p검사내역.Text = "History";
            // 
            // tabFormContentContainer3
            // 
            this.tabFormContentContainer3.Controls.Add(this.xtraTabControl2);
            this.tabFormContentContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer3.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer3.Name = "tabFormContentContainer3";
            this.tabFormContentContainer3.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer3.TabIndex = 2;
            // 
            // xtraTabControl2
            // 
            this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl2.Name = "xtraTabControl2";
            this.xtraTabControl2.SelectedTabPage = this.t내역조회;
            this.xtraTabControl2.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl2.Size = new System.Drawing.Size(1920, 1010);
            this.xtraTabControl2.TabIndex = 1;
            this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.t내역조회,
            this.t결과보고});
            // 
            // t내역조회
            // 
            this.t내역조회.Controls.Add(this.xtraTabControl1);
            this.t내역조회.Name = "t내역조회";
            this.t내역조회.Size = new System.Drawing.Size(1918, 1008);
            this.t내역조회.Text = "내역조회";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1918, 1008);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.e검사내역);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1916, 977);
            this.xtraTabPage1.Text = "List";
            // 
            // e검사내역
            // 
            this.e검사내역.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사내역.Location = new System.Drawing.Point(0, 0);
            this.e검사내역.Name = "e검사내역";
            this.e검사내역.Size = new System.Drawing.Size(1916, 977);
            this.e검사내역.TabIndex = 0;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.e검사피봇);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1916, 977);
            this.xtraTabPage2.Text = "Pivot";
            // 
            // e검사피봇
            // 
            this.e검사피봇.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사피봇.Location = new System.Drawing.Point(0, 0);
            this.e검사피봇.Name = "e검사피봇";
            this.e검사피봇.Size = new System.Drawing.Size(1916, 977);
            this.e검사피봇.TabIndex = 0;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.e결과검색);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1916, 977);
            this.xtraTabPage3.Text = "Search";
            // 
            // e결과검색
            // 
            this.e결과검색.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e결과검색.Location = new System.Drawing.Point(0, 0);
            this.e결과검색.Name = "e결과검색";
            this.e결과검색.Size = new System.Drawing.Size(1916, 977);
            this.e결과검색.TabIndex = 0;
            // 
            // t결과보고
            // 
            this.t결과보고.Name = "t결과보고";
            this.t결과보고.Size = new System.Drawing.Size(1918, 1008);
            this.t결과보고.Text = "결과 보고서";
            // 
            // p환경설정
            // 
            this.p환경설정.ContentContainer = this.tabFormContentContainer2;
            this.p환경설정.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p환경설정.ImageOptions.SvgImage")));
            this.p환경설정.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.p환경설정.Name = "p환경설정";
            this.p환경설정.Text = "Preferences";
            // 
            // tabFormContentContainer2
            // 
            this.tabFormContentContainer2.Controls.Add(this.t환경설정);
            this.tabFormContentContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer2.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer2.Name = "tabFormContentContainer2";
            this.tabFormContentContainer2.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer2.TabIndex = 2;
            // 
            // t환경설정
            // 
            this.t환경설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t환경설정.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.t환경설정.Location = new System.Drawing.Point(0, 0);
            this.t환경설정.Name = "t환경설정";
            this.t환경설정.SelectedTabPage = this.t검사설정;
            this.t환경설정.Size = new System.Drawing.Size(1920, 1010);
            this.t환경설정.TabIndex = 1;
            this.t환경설정.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.t검사설정,
            this.t장치설정,
            this.t큐알검증,
            this.t로그내역});
            // 
            // t검사설정
            // 
            this.t검사설정.Controls.Add(this.e검사설정);
            this.t검사설정.Name = "t검사설정";
            this.t검사설정.Size = new System.Drawing.Size(1918, 979);
            this.t검사설정.Text = "검사설정";
            // 
            // e검사설정
            // 
            this.e검사설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사설정.Location = new System.Drawing.Point(0, 0);
            this.e검사설정.Name = "e검사설정";
            this.e검사설정.Size = new System.Drawing.Size(1918, 979);
            this.e검사설정.TabIndex = 0;
            // 
            // t장치설정
            // 
            this.t장치설정.Controls.Add(this.e장치설정);
            this.t장치설정.Name = "t장치설정";
            this.t장치설정.Size = new System.Drawing.Size(1918, 979);
            this.t장치설정.Text = "장치설정";
            // 
            // e장치설정
            // 
            this.e장치설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e장치설정.Location = new System.Drawing.Point(0, 0);
            this.e장치설정.Name = "e장치설정";
            this.e장치설정.Size = new System.Drawing.Size(1918, 979);
            this.e장치설정.TabIndex = 0;
            // 
            // t큐알검증
            // 
            this.t큐알검증.Controls.Add(this.e큐알검증);
            this.t큐알검증.Name = "t큐알검증";
            this.t큐알검증.Size = new System.Drawing.Size(1918, 979);
            this.t큐알검증.Text = "큐알검증";
            // 
            // e큐알검증
            // 
            this.e큐알검증.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e큐알검증.Location = new System.Drawing.Point(0, 0);
            this.e큐알검증.Name = "e큐알검증";
            this.e큐알검증.Size = new System.Drawing.Size(1918, 979);
            this.e큐알검증.TabIndex = 0;
            // 
            // t로그내역
            // 
            this.t로그내역.Controls.Add(this.e로그내역);
            this.t로그내역.Name = "t로그내역";
            this.t로그내역.Size = new System.Drawing.Size(1918, 979);
            this.t로그내역.Text = "로그내역";
            // 
            // e로그내역
            // 
            this.e로그내역.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e로그내역.Location = new System.Drawing.Point(0, 0);
            this.e로그내역.Name = "e로그내역";
            this.e로그내역.Size = new System.Drawing.Size(1918, 979);
            this.e로그내역.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1040);
            this.Controls.Add(this.tabFormContentContainer1);
            this.Controls.Add(this.tabFormControl1);
            this.IconOptions.SvgImage = global::DSEV.Properties.Resources.vision;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TabFormControl = this.tabFormControl1;
            this.Text = "자동 치수 검사기";
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabFormControl1)).EndInit();
            this.tabFormContentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tabFormContentContainer4.ResumeLayout(false);
            this.tabFormContentContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).EndInit();
            this.xtraTabControl2.ResumeLayout(false);
            this.t내역조회.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage3.ResumeLayout(false);
            this.tabFormContentContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.t환경설정)).EndInit();
            this.t환경설정.ResumeLayout(false);
            this.t검사설정.ResumeLayout(false);
            this.t장치설정.ResumeLayout(false);
            this.t큐알검증.ResumeLayout(false);
            this.t로그내역.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.TabFormControl tabFormControl1;
        private DevExpress.XtraBars.TabFormPage p결과뷰어;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer1;
        private DevExpress.XtraBars.BarStaticItem 타이틀;
        private DevExpress.XtraBars.SkinPaletteDropDownButtonItem skinPaletteDropDownButtonItem1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private UI.Controls.State e상태뷰어;
        private DevExpress.XtraBars.TabFormPage p환경설정;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer2;
        private DevExpress.XtraBars.BarStaticItem e프로젝트;
        private DevExpress.XtraBars.TabFormPage p검사내역;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer3;
        private DevExpress.XtraTab.XtraTabControl t환경설정;
        private DevExpress.XtraTab.XtraTabPage t검사설정;
        private DevExpress.XtraTab.XtraTabPage t로그내역;
        private UI.Controls.LogViewer e로그내역;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl2;
        private DevExpress.XtraTab.XtraTabPage t내역조회;
        private DevExpress.XtraTab.XtraTabPage t결과보고;
        private DevExpress.XtraTab.XtraTabPage t장치설정;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private DevExpress.XtraBars.TabFormPage p검사도구;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer4;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private UI.Controls.DeviceSettings e장치설정;
        private UI.Controls.Results e검사내역;
        private UI.Controls.SetInspection e검사설정;
        private UI.Controls.ResultsPivot e검사피봇;
        private DevExpress.XtraTab.XtraTabPage t큐알검증;
        private UI.Controls.QrValidate e큐알검증;
        private UI.Controls.CamViewers e검사도구;
        private UI.Controls.ResultInspection e결과뷰어;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private UI.Controls.ResultSearch e결과검색;
    }
}