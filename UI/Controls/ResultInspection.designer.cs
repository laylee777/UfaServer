namespace DSEV.UI.Controls
{
    partial class ResultInspection
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
            this.components = new System.ComponentModel.Container();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.e결과뷰어 = new DSEV.UI.Controls.Viewport3D();
            this.e외관결과 = new DevExpress.XtraEditors.TextEdit();
            this.Bind검사결과 = new System.Windows.Forms.BindingSource(this.components);
            this.eCTQ결과 = new DevExpress.XtraEditors.TextEdit();
            this.e큐알등급 = new DevExpress.XtraEditors.TextEdit();
            this.e검사순번 = new DevExpress.XtraEditors.TextEdit();
            this.e측정결과 = new DevExpress.XtraEditors.LabelControl();
            this.e검사시간 = new DevExpress.XtraEditors.TextEdit();
            this.e큐알코드 = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.e결과목록 = new DSEV.UI.Controls.ResultGrid();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.e외관결과.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind검사결과)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eCTQ결과.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e큐알등급.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e검사순번.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e검사시간.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e큐알코드.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AutoScroll = false;
            this.layoutControl1.Controls.Add(this.e결과뷰어);
            this.layoutControl1.Controls.Add(this.e외관결과);
            this.layoutControl1.Controls.Add(this.eCTQ결과);
            this.layoutControl1.Controls.Add(this.e큐알등급);
            this.layoutControl1.Controls.Add(this.e검사순번);
            this.layoutControl1.Controls.Add(this.e측정결과);
            this.layoutControl1.Controls.Add(this.e검사시간);
            this.layoutControl1.Controls.Add(this.e큐알코드);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsView.AlwaysScrollActiveControlIntoView = false;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1268, 900);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // e결과뷰어
            // 
            this.e결과뷰어.Location = new System.Drawing.Point(6, 96);
            this.e결과뷰어.Name = "e결과뷰어";
            this.e결과뷰어.Size = new System.Drawing.Size(1256, 798);
            this.e결과뷰어.TabIndex = 1;
            // 
            // e외관결과
            // 
            this.e외관결과.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검사결과, "외관문구", true));
            this.e외관결과.EditValue = "-";
            this.e외관결과.EnterMoveNextControl = true;
            this.e외관결과.Location = new System.Drawing.Point(382, 54);
            this.e외관결과.Name = "e외관결과";
            this.e외관결과.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e외관결과.Properties.Appearance.Options.UseFont = true;
            this.e외관결과.Properties.Appearance.Options.UseTextOptions = true;
            this.e외관결과.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e외관결과.Properties.ReadOnly = true;
            this.e외관결과.Size = new System.Drawing.Size(127, 32);
            this.e외관결과.StyleController = this.layoutControl1;
            this.e외관결과.TabIndex = 9;
            // 
            // Bind검사결과
            // 
            this.Bind검사결과.DataSource = typeof(DSEV.Schemas.검사결과);
            // 
            // eCTQ결과
            // 
            this.eCTQ결과.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검사결과, "품질문구", true));
            this.eCTQ결과.EditValue = "-";
            this.eCTQ결과.EnterMoveNextControl = true;
            this.eCTQ결과.Location = new System.Drawing.Point(382, 10);
            this.eCTQ결과.Name = "eCTQ결과";
            this.eCTQ결과.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.eCTQ결과.Properties.Appearance.Options.UseFont = true;
            this.eCTQ결과.Properties.Appearance.Options.UseTextOptions = true;
            this.eCTQ결과.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.eCTQ결과.Properties.ReadOnly = true;
            this.eCTQ결과.Size = new System.Drawing.Size(127, 32);
            this.eCTQ결과.StyleController = this.layoutControl1;
            this.eCTQ결과.TabIndex = 8;
            // 
            // e큐알등급
            // 
            this.e큐알등급.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검사결과, "큐알등급", true));
            this.e큐알등급.EditValue = "-";
            this.e큐알등급.EnterMoveNextControl = true;
            this.e큐알등급.Location = new System.Drawing.Point(1192, 10);
            this.e큐알등급.Name = "e큐알등급";
            this.e큐알등급.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e큐알등급.Properties.Appearance.Options.UseFont = true;
            this.e큐알등급.Properties.Appearance.Options.UseTextOptions = true;
            this.e큐알등급.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e큐알등급.Properties.ReadOnly = true;
            this.e큐알등급.Size = new System.Drawing.Size(66, 32);
            this.e큐알등급.StyleController = this.layoutControl1;
            this.e큐알등급.TabIndex = 7;
            // 
            // e검사순번
            // 
            this.e검사순번.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검사결과, "검사코드", true));
            this.e검사순번.EditValue = 0;
            this.e검사순번.EnterMoveNextControl = true;
            this.e검사순번.Location = new System.Drawing.Point(613, 54);
            this.e검사순번.Name = "e검사순번";
            this.e검사순번.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e검사순번.Properties.Appearance.Options.UseFont = true;
            this.e검사순번.Properties.Appearance.Options.UseTextOptions = true;
            this.e검사순번.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e검사순번.Properties.DisplayFormat.FormatString = "d4";
            this.e검사순번.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.e검사순번.Properties.EditFormat.FormatString = "d4";
            this.e검사순번.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.e검사순번.Properties.ReadOnly = true;
            this.e검사순번.Size = new System.Drawing.Size(169, 32);
            this.e검사순번.StyleController = this.layoutControl1;
            this.e검사순번.TabIndex = 1;
            // 
            // e측정결과
            // 
            this.e측정결과.Appearance.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e측정결과.Appearance.Options.UseFont = true;
            this.e측정결과.Appearance.Options.UseTextOptions = true;
            this.e측정결과.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e측정결과.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.e측정결과.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.Bind검사결과, "결과문구", true));
            this.e측정결과.Location = new System.Drawing.Point(6, 6);
            this.e측정결과.MinimumSize = new System.Drawing.Size(0, 50);
            this.e측정결과.Name = "e측정결과";
            this.e측정결과.Size = new System.Drawing.Size(276, 86);
            this.e측정결과.StyleController = this.layoutControl1;
            this.e측정결과.TabIndex = 2;
            this.e측정결과.Text = "Waiting";
            // 
            // e검사시간
            // 
            this.e검사시간.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검사결과, "검사일시", true));
            this.e검사시간.EnterMoveNextControl = true;
            this.e검사시간.Location = new System.Drawing.Point(886, 54);
            this.e검사시간.Name = "e검사시간";
            this.e검사시간.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e검사시간.Properties.Appearance.Options.UseFont = true;
            this.e검사시간.Properties.Appearance.Options.UseTextOptions = true;
            this.e검사시간.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e검사시간.Properties.DisplayFormat.FormatString = "{0:yyyy-MM-dd HH:mm:ss}";
            this.e검사시간.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.e검사시간.Properties.EditFormat.FormatString = "{0:yyyy-MM-dd HH:mm:ss}";
            this.e검사시간.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.e검사시간.Properties.ReadOnly = true;
            this.e검사시간.Size = new System.Drawing.Size(372, 32);
            this.e검사시간.StyleController = this.layoutControl1;
            this.e검사시간.TabIndex = 5;
            // 
            // e큐알코드
            // 
            this.e큐알코드.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검사결과, "큐알내용", true));
            this.e큐알코드.EditValue = "-";
            this.e큐알코드.EnterMoveNextControl = true;
            this.e큐알코드.Location = new System.Drawing.Point(613, 10);
            this.e큐알코드.Name = "e큐알코드";
            this.e큐알코드.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e큐알코드.Properties.Appearance.Options.UseFont = true;
            this.e큐알코드.Properties.Appearance.Options.UseTextOptions = true;
            this.e큐알코드.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e큐알코드.Properties.ReadOnly = true;
            this.e큐알코드.Size = new System.Drawing.Size(567, 32);
            this.e큐알코드.StyleController = this.layoutControl1;
            this.e큐알코드.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.AppearanceItemCaption.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Root.AppearanceItemCaption.Options.UseFont = true;
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(1268, 900);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.e큐알코드;
            this.layoutControlItem1.Location = new System.Drawing.Point(511, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem1.Size = new System.Drawing.Size(671, 44);
            this.layoutControlItem1.Text = "QR Code";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(80, 25);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.e측정결과;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(280, 90);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(280, 90);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(280, 90);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.e검사순번;
            this.layoutControlItem4.Location = new System.Drawing.Point(511, 44);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem4.Size = new System.Drawing.Size(273, 46);
            this.layoutControlItem4.Text = "Index";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(80, 25);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.e검사시간;
            this.layoutControlItem2.Location = new System.Drawing.Point(784, 44);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem2.Size = new System.Drawing.Size(476, 46);
            this.layoutControlItem2.Text = "Time";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(80, 25);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.e큐알등급;
            this.layoutControlItem5.Location = new System.Drawing.Point(1182, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem5.Size = new System.Drawing.Size(78, 44);
            this.layoutControlItem5.Text = "Legibility";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.eCTQ결과;
            this.layoutControlItem6.Location = new System.Drawing.Point(280, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem6.Size = new System.Drawing.Size(231, 44);
            this.layoutControlItem6.Text = "CTQ";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(80, 25);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.e외관결과;
            this.layoutControlItem7.Location = new System.Drawing.Point(280, 44);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlItem7.Size = new System.Drawing.Size(231, 46);
            this.layoutControlItem7.Text = "Surface";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(80, 25);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.e결과뷰어;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(1260, 802);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // dockManager1
            // 
            this.dockManager1.DockingOptions.ShowCloseButton = false;
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel1.FloatSize = new System.Drawing.Size(793, 857);
            this.dockPanel1.ID = new System.Guid("81df941b-8b06-4b13-9a7e-2c01f5ee4a1b");
            this.dockPanel1.Location = new System.Drawing.Point(1268, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(652, 200);
            this.dockPanel1.Size = new System.Drawing.Size(652, 900);
            this.dockPanel1.Text = "Inspection Results";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.e결과목록);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 30);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(645, 867);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // e결과목록
            // 
            this.e결과목록.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e결과목록.Location = new System.Drawing.Point(0, 0);
            this.e결과목록.Name = "e결과목록";
            this.e결과목록.Size = new System.Drawing.Size(645, 867);
            this.e결과목록.TabIndex = 0;
            // 
            // ResultInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.dockPanel1);
            this.Name = "ResultInspection";
            this.Size = new System.Drawing.Size(1920, 900);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.e외관결과.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind검사결과)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eCTQ결과.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e큐알등급.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e검사순번.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e검사시간.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e큐알코드.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource Bind검사결과;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit e검사시간;
        private DevExpress.XtraEditors.TextEdit e큐알코드;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.LabelControl e측정결과;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.TextEdit e검사순번;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private Viewport3D e결과뷰어;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.TextEdit e큐알등급;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit e외관결과;
        private DevExpress.XtraEditors.TextEdit eCTQ결과;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private ResultGrid e결과목록;
    }
}
