using DevExpress.XtraEditors;
using System;

namespace DSEV.UI.Forms
{
    public partial class CalibInfo : XtraForm
    {
        public CalibInfo()
        {
            InitializeComponent();
            this.Load += FormLoad;
            this.Shown += FormShown;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            this.calibration1.Init();
        }

        private void FormShown(object sender, EventArgs e)
        {
            this.calibration1.BestFit();
        }
    }
}