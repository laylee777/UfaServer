
namespace Cogutils
{
    partial class RecordsDisplay
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
            this.RecordsDisplay1 = new Cognex.VisionPro.CogRecordsDisplay();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.DisplayStatusBar1 = new Cogutils.DisplayStatusBar();
            this.DisplayToolBar1 = new Cogutils.DisplayToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RecordsDisplay1
            // 
            this.RecordsDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RecordsDisplay1.Location = new System.Drawing.Point(0, 32);
            this.RecordsDisplay1.Name = "RecordsDisplay1";
            this.RecordsDisplay1.SelectedRecordKey = null;
            this.RecordsDisplay1.ShowRecordsDropDown = true;
            this.RecordsDisplay1.Size = new System.Drawing.Size(785, 821);
            this.RecordsDisplay1.Subject = null;
            this.RecordsDisplay1.TabIndex = 33;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.DisplayStatusBar1);
            this.panelControl1.Controls.Add(this.DisplayToolBar1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(785, 32);
            this.panelControl1.TabIndex = 34;
            // 
            // DisplayStatusBar1
            // 
            this.DisplayStatusBar1.CoordinateSpaceName = "*\\#";
            this.DisplayStatusBar1.CoordinateSpaceName3D = "*\\#";
            this.DisplayStatusBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayStatusBar1.Location = new System.Drawing.Point(204, 2);
            this.DisplayStatusBar1.Name = "DisplayStatusBar1";
            this.DisplayStatusBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DisplayStatusBar1.Size = new System.Drawing.Size(579, 28);
            this.DisplayStatusBar1.TabIndex = 30;
            this.DisplayStatusBar1.Use3DCoordinateSpaceTree = false;
            // 
            // DisplayToolBar1
            // 
            this.DisplayToolBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.DisplayToolBar1.Location = new System.Drawing.Point(2, 2);
            this.DisplayToolBar1.Name = "DisplayToolBar1";
            this.DisplayToolBar1.Size = new System.Drawing.Size(202, 28);
            this.DisplayToolBar1.TabIndex = 31;
            // 
            // RecordsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RecordsDisplay1);
            this.Controls.Add(this.panelControl1);
            this.Name = "RecordsDisplay";
            this.Size = new System.Drawing.Size(785, 853);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.CogRecordsDisplay RecordsDisplay1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DisplayStatusBar DisplayStatusBar1;
        private DisplayToolBar DisplayToolBar1;
    }
}
