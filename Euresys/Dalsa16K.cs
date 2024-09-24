using Euresys.MultiCam;
using System;
using System.Diagnostics;
using System.IO;
using DSEV.Schemas;

namespace DSEV.Multicam
{
    public class Dalsa16K : CamControl
    {
        public override 카메라구분 Camera { get; set; } = 카메라구분.None;
        public override String CamFile { get; set; } = "Bottom.cam";
        public override UInt32 DriverIndex { get; set; } = 0;
        public override AcquisitionMode AcquisitionMode { get; set; } = AcquisitionMode.PAGE;
        public override LineRateMode LineRateMode { get; set; } = LineRateMode.PULSE;
        public override TrigMode TrigMode { get; set; } = TrigMode.HARD;
        public override NextTrigMode NextTrigMode { get; set; } = NextTrigMode.HARD;
        public override EndTrigMode EndTrigMode { get; set; } = EndTrigMode.AUTO;
        public override Int32 SeqLength_Pg { get; set; } = 1;
        public override Int32 PageLength_Ln { get; set; } = 31000;

        public Dalsa16K(카메라구분 camera)
        {
            this.Camera = camera;
        }


        public override void Init()
        {
            //base.Init();
            try
            {
                String camfile = Path.Combine(Global.환경설정.기본경로, this.CamFile);
                MC.Create("CHANNEL", out this.Channel);
                MC.SetParam(this.Channel, "DriverIndex", this.DriverIndex);
                MC.SetParam(this.Channel, "Connector", this.Connector.ToString());
                MC.SetParam(this.Channel, "CamFile", camfile);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Global.오류로그("카메라초기화", "바닥카메라", ex.Message, true);
            }

            //MC.SetParam(this.Channel, "AcquisitionMode", this.AcquisitionMode.ToString());
            //MC.SetParam(this.Channel, "TapConfiguration", "FULL_8T8");
            //MC.SetParam(this.Channel, "TapGeometry", "1X8");
            //MC.SetParam(this.Channel, "LineRateMode", this.LineRateMode.ToString());
            //MC.SetParam(this.Channel, "TrigMode", this.TrigMode.ToString());
            //MC.SetParam(this.Channel, "NextTrigMode", this.NextTrigMode.ToString());
            //MC.SetParam(this.Channel, "EndTrigMode", this.EndTrigMode.ToString());

            //MC.SetParam(this.Channel, "SeqLength_Pg", this.SeqLength_Pg);
            //MC.SetParam(this.Channel, "PageLength_Ln", this.PageLength_Ln);
            //MC.SetParam(this.Channel, "ImageFlipX", "ON");
            //MC.SetParam(this.Channel, "ImageFlipY", "ON");

            //MC.SetParam(this.Channel, "BreakEffect", this.BreakEffect.ToString());
            //MC.SetParam(this.Channel, "LineTrigCtl", "DIFF_PAIRED");
            //MC.SetParam(this.Channel, "LineTrigLine", "DIN1_DIN2");
            //MC.SetParam(this.Channel, "LineTrigEdge", "RISING_A");   // RISING_A == GOHIGH == GOLOW, FALLING_A, ALL_A, ALL_A_B

            //DebugParam("SeqLength_Pg");
            //DebugParam("PageLength_Ln");
            //DebugParam("SeqLength_Ln");
            //DebugParam("TapConfiguration");
            //DebugParam("TapGeometry");
            //DebugParam("LineRateMode");
            //DebugParam("TrigMode");
            //DebugParam("NextTrigMode");
            //DebugParam("EndTrigMode");
            //DebugParam("LineTrigLine");
            //DebugParam("TrigCtl");
            //DebugParam("LineTrigCtl");
            //DebugParam("LineTrigEdge");
        }
    }
}
