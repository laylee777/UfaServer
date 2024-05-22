using DevExpress.XtraEditors;
using DSEV.Schemas;

using System;



namespace DSEV.UI.Controls
{
    public partial class CamViewers : DevExpress.XtraEditors.XtraUserControl
    {
        public CamViewers() => InitializeComponent();

        public void Init()
        {
            this.e하부캠.Init(false);
            this.e상부인슐왼쪽캠.Init(false);
            this.e상부인슐오른쪽캠.Init(false);

            Global.비전검사.SetDisplay(카메라구분.Cam01, this.e하부캠);
            Global.비전검사.SetDisplay(카메라구분.Cam02, this.e상부인슐왼쪽캠);
            Global.비전검사.SetDisplay(카메라구분.Cam03, this.e상부인슐오른쪽캠);
        }
        public void Close() { }
    }
}
