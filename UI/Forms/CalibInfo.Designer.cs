namespace DSEV.UI.Forms
{
    partial class CalibInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibInfo));
            this.calibration1 = new DSEV.UI.Controls.Calibration();
            this.SuspendLayout();
            // 
            // calibration1
            // 
            this.calibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibration1.Location = new System.Drawing.Point(0, 0);
            this.calibration1.Name = "calibration1";
            this.calibration1.Size = new System.Drawing.Size(1600, 770);
            this.calibration1.TabIndex = 0;
            // 
            // CalibInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 770);
            this.Controls.Add(this.calibration1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("CalibInfo.IconOptions.SvgImage")));
            this.Name = "CalibInfo";
            this.Text = "CalibInfo";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Calibration calibration1;
    }
}