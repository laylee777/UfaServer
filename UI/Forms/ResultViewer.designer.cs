namespace DSEV.UI.Forms
{
    partial class ResultViewer
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
            this.e결과뷰어 = new DSEV.UI.Controls.ResultInspection();
            this.SuspendLayout();
            // 
            // e결과뷰어
            // 
            this.e결과뷰어.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e결과뷰어.Location = new System.Drawing.Point(0, 0);
            this.e결과뷰어.Name = "e결과뷰어";
            this.e결과뷰어.Size = new System.Drawing.Size(1920, 1010);
            this.e결과뷰어.TabIndex = 0;
            // 
            // ResultViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1010);
            this.Controls.Add(this.e결과뷰어);
            this.IconOptions.SvgImage = global::DSEV.Properties.Resources.vision;
            this.Name = "ResultViewer";
            this.Text = "Result Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ResultInspection e결과뷰어;
    }
}