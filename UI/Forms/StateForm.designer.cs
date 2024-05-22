namespace DSEV.UI.Forms
{
    partial class StateForm
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.e순번 = new DevExpress.XtraEditors.LabelControl();
            this.e결과 = new DevExpress.XtraEditors.LabelControl();
            this.e큐알 = new DevExpress.XtraEditors.LabelControl();
            this.e모델 = new DevExpress.XtraEditors.LabelControl();
            this.e뷰어 = new DSEV.UI.Controls.Viewport3D();
            this.e시계 = new DSEV.UI.Controls.AnalogClock();
            this.e양품수율 = new DSEV.UI.Controls.CountViewer();
            this.BindLocalization = new System.Windows.Forms.BindingSource(this.components);
            this.모델자료Bind = new System.Windows.Forms.BindingSource(this.components);
            this.e전체수량 = new DSEV.UI.Controls.CountViewer();
            this.e불량수량 = new DSEV.UI.Controls.CountViewer();
            this.e양품수량 = new DSEV.UI.Controls.CountViewer();
            this.e시간 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindLocalization)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.모델자료Bind)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.tablePanel1);
            this.splitContainerControl1.Panel1.Controls.Add(this.e뷰어);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.e시계);
            this.splitContainerControl1.Panel2.Controls.Add(this.e양품수율);
            this.splitContainerControl1.Panel2.Controls.Add(this.e전체수량);
            this.splitContainerControl1.Panel2.Controls.Add(this.e불량수량);
            this.splitContainerControl1.Panel2.Controls.Add(this.e양품수량);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1920, 780);
            this.splitContainerControl1.SplitterPosition = 1600;
            this.splitContainerControl1.TabIndex = 0;
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 80F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 60F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 60F)});
            this.tablePanel1.Controls.Add(this.e시간);
            this.tablePanel1.Controls.Add(this.e순번);
            this.tablePanel1.Controls.Add(this.e결과);
            this.tablePanel1.Controls.Add(this.e큐알);
            this.tablePanel1.Controls.Add(this.e모델);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F)});
            this.tablePanel1.Size = new System.Drawing.Size(1600, 120);
            this.tablePanel1.TabIndex = 3;
            this.tablePanel1.UseSkinIndents = true;
            // 
            // e순번
            // 
            this.e순번.Appearance.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e순번.Appearance.Options.UseFont = true;
            this.e순번.Appearance.Options.UseTextOptions = true;
            this.e순번.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e순번.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel1.SetColumn(this.e순번, 2);
            this.e순번.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e순번.Location = new System.Drawing.Point(962, 62);
            this.e순번.Name = "e순번";
            this.tablePanel1.SetRow(this.e순번, 1);
            this.e순번.Size = new System.Drawing.Size(316, 55);
            this.e순번.TabIndex = 6;
            this.e순번.Text = "0000";
            // 
            // e결과
            // 
            this.e결과.Appearance.Font = new System.Drawing.Font("맑은 고딕", 64F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e결과.Appearance.Options.UseFont = true;
            this.e결과.Appearance.Options.UseTextOptions = true;
            this.e결과.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e결과.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel1.SetColumn(this.e결과, 1);
            this.e결과.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e결과.Location = new System.Drawing.Point(429, 3);
            this.e결과.Name = "e결과";
            this.tablePanel1.SetRow(this.e결과, 0);
            this.tablePanel1.SetRowSpan(this.e결과, 2);
            this.e결과.Size = new System.Drawing.Size(529, 114);
            this.e결과.TabIndex = 4;
            this.e결과.Text = "Waiting";
            // 
            // e큐알
            // 
            this.e큐알.Appearance.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e큐알.Appearance.Options.UseFont = true;
            this.e큐알.Appearance.Options.UseTextOptions = true;
            this.e큐알.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e큐알.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel1.SetColumn(this.e큐알, 2);
            this.tablePanel1.SetColumnSpan(this.e큐알, 2);
            this.e큐알.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e큐알.Location = new System.Drawing.Point(962, 3);
            this.e큐알.Name = "e큐알";
            this.tablePanel1.SetRow(this.e큐알, 0);
            this.e큐알.Size = new System.Drawing.Size(635, 55);
            this.e큐알.TabIndex = 5;
            this.e큐알.Text = "MPL02616AB;231200A";
            // 
            // e모델
            // 
            this.e모델.Appearance.Font = new System.Drawing.Font("맑은 고딕", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e모델.Appearance.Options.UseFont = true;
            this.e모델.Appearance.Options.UseTextOptions = true;
            this.e모델.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e모델.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel1.SetColumn(this.e모델, 0);
            this.e모델.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e모델.Location = new System.Drawing.Point(3, 3);
            this.e모델.Name = "e모델";
            this.tablePanel1.SetRow(this.e모델, 0);
            this.tablePanel1.SetRowSpan(this.e모델, 2);
            this.e모델.Size = new System.Drawing.Size(422, 114);
            this.e모델.TabIndex = 1;
            this.e모델.Text = "EV2020 TPA";
            // 
            // e뷰어
            // 
            this.e뷰어.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e뷰어.Location = new System.Drawing.Point(0, 0);
            this.e뷰어.Name = "e뷰어";
            this.e뷰어.Size = new System.Drawing.Size(1600, 780);
            this.e뷰어.TabIndex = 2;
            // 
            // e시계
            // 
            this.e시계.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.e시계.Location = new System.Drawing.Point(0, 463);
            this.e시계.Name = "e시계";
            this.e시계.Size = new System.Drawing.Size(310, 317);
            this.e시계.TabIndex = 6;
            // 
            // e양품수율
            // 
            this.e양품수율.BaseColor = System.Drawing.Color.Empty;
            this.e양품수율.Caption = "Yield";
            this.e양품수율.CaptionFont = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e양품수율.DataBindings.Add(new System.Windows.Forms.Binding("Caption", this.BindLocalization, "양품수율", true));
            this.e양품수율.DataBindings.Add(new System.Windows.Forms.Binding("ValueText", this.모델자료Bind, "양품수율표현", true));
            this.e양품수율.Dock = System.Windows.Forms.DockStyle.Top;
            this.e양품수율.Location = new System.Drawing.Point(0, 450);
            this.e양품수율.Name = "e양품수율";
            this.e양품수율.Size = new System.Drawing.Size(310, 150);
            this.e양품수율.TabIndex = 5;
            this.e양품수율.TextFont = new System.Drawing.Font("맑은 고딕", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e양품수율.ValueText = "100.0";
            // 
            // BindLocalization
            // 
            this.BindLocalization.DataSource = typeof(DSEV.UI.Forms.StateForm.LocalizationState);
            // 
            // 모델자료Bind
            // 
            this.모델자료Bind.DataSource = typeof(DSEV.Schemas.모델자료);
            // 
            // e전체수량
            // 
            this.e전체수량.BaseColor = System.Drawing.Color.Empty;
            this.e전체수량.Caption = "Total";
            this.e전체수량.CaptionFont = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e전체수량.DataBindings.Add(new System.Windows.Forms.Binding("Caption", this.BindLocalization, "전체갯수", true));
            this.e전체수량.DataBindings.Add(new System.Windows.Forms.Binding("ValueText", this.모델자료Bind, "전체갯수표현", true));
            this.e전체수량.Dock = System.Windows.Forms.DockStyle.Top;
            this.e전체수량.Location = new System.Drawing.Point(0, 300);
            this.e전체수량.Name = "e전체수량";
            this.e전체수량.Size = new System.Drawing.Size(310, 150);
            this.e전체수량.TabIndex = 4;
            this.e전체수량.TextFont = new System.Drawing.Font("맑은 고딕", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e전체수량.ValueText = "100.0";
            // 
            // e불량수량
            // 
            this.e불량수량.BaseColor = System.Drawing.Color.Empty;
            this.e불량수량.Caption = "NG";
            this.e불량수량.CaptionFont = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e불량수량.DataBindings.Add(new System.Windows.Forms.Binding("Caption", this.BindLocalization, "불량갯수", true));
            this.e불량수량.DataBindings.Add(new System.Windows.Forms.Binding("ValueText", this.모델자료Bind, "불량갯수표현", true));
            this.e불량수량.Dock = System.Windows.Forms.DockStyle.Top;
            this.e불량수량.Location = new System.Drawing.Point(0, 150);
            this.e불량수량.Name = "e불량수량";
            this.e불량수량.Size = new System.Drawing.Size(310, 150);
            this.e불량수량.TabIndex = 3;
            this.e불량수량.TextFont = new System.Drawing.Font("맑은 고딕", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e불량수량.ValueText = "100.0";
            // 
            // e양품수량
            // 
            this.e양품수량.BaseColor = System.Drawing.Color.Empty;
            this.e양품수량.Caption = "OK";
            this.e양품수량.CaptionFont = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e양품수량.DataBindings.Add(new System.Windows.Forms.Binding("Caption", this.BindLocalization, "양품갯수", true));
            this.e양품수량.DataBindings.Add(new System.Windows.Forms.Binding("ValueText", this.모델자료Bind, "양품갯수표현", true));
            this.e양품수량.Dock = System.Windows.Forms.DockStyle.Top;
            this.e양품수량.Location = new System.Drawing.Point(0, 0);
            this.e양품수량.Name = "e양품수량";
            this.e양품수량.Size = new System.Drawing.Size(310, 150);
            this.e양품수량.TabIndex = 2;
            this.e양품수량.TextFont = new System.Drawing.Font("맑은 고딕", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e양품수량.ValueText = "100.0";
            // 
            // e시간
            // 
            this.e시간.Appearance.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e시간.Appearance.Options.UseFont = true;
            this.e시간.Appearance.Options.UseTextOptions = true;
            this.e시간.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e시간.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel1.SetColumn(this.e시간, 3);
            this.e시간.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e시간.Location = new System.Drawing.Point(1281, 62);
            this.e시간.Name = "e시간";
            this.tablePanel1.SetRow(this.e시간, 1);
            this.e시간.Size = new System.Drawing.Size(316, 55);
            this.e시간.TabIndex = 7;
            this.e시간.Text = "00:00:00";
            // 
            // StateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 780);
            this.Controls.Add(this.splitContainerControl1);
            this.IconOptions.SvgImage = global::DSEV.Properties.Resources.vision;
            this.Name = "StateForm";
            this.Text = "Inspection Status";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindLocalization)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.모델자료Bind)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private Controls.CountViewer e양품수량;
        private Controls.CountViewer e불량수량;
        private Controls.CountViewer e전체수량;
        private Controls.CountViewer e양품수율;
        private System.Windows.Forms.BindingSource 모델자료Bind;
        private System.Windows.Forms.BindingSource BindLocalization;
        private Controls.AnalogClock e시계;
        private DevExpress.XtraEditors.LabelControl e모델;
        private Controls.Viewport3D e뷰어;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.LabelControl e결과;
        private DevExpress.XtraEditors.LabelControl e큐알;
        private DevExpress.XtraEditors.LabelControl e순번;
        private DevExpress.XtraEditors.LabelControl e시간;
    }
}