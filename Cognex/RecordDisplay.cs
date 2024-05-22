using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogutils
{
    public class RecordDisplay : CogRecordDisplay
    {
        public static Color 배경색상 = DevExpress.LookAndFeel.DXSkinColors.IconColors.Black;

        public void Init(Boolean showScrollBar = true)
        {
            this.AutoFit = true;
            this.HorizontalScrollBar = showScrollBar;
            this.VerticalScrollBar = showScrollBar;
            this.BackColor = 배경색상;
            this.MouseMode = CogDisplayMouseModeConstants.Pan;
        }

        public void SetImage(ICogImage image, ICogRecord record, List<ICogGraphic> graphics)
        {
            if (image == null || !image.Allocated) return;
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => { SetImage(image, record, graphics); })); return; }

            this.Image = null;
            this.InteractiveGraphics.Clear();
            this.StaticGraphics.Clear();
            this.Image = image;
            this.Record = record;
            foreach (ICogGraphic graphic in graphics)
                this.StaticGraphics.Add(graphic, "Results");
            this.SetBackground();
            //this.Fit(true);
        }

        public void SetBackground() => this.BackColor = 배경색상;
    }
}
