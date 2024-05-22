using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using DevExpress.XtraEditors;
using DSEV.Schemas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cogutils
{
    public partial class RecordsDisplay : XtraUserControl
    {
        public static Color 배경색상 = DevExpress.LookAndFeel.DXSkinColors.IconColors.Black;

        public RecordsDisplay()
        {
            InitializeComponent();
        }

        public CogRecordsDisplay CogRecordsDisplay { get { return this.RecordsDisplay1; } }
        public CogDisplay CogDisplay { get { return this.RecordsDisplay1.Display; } }
        public CogImage8Grey Image { get { return this.CogDisplay.Image as CogImage8Grey; } set { this.CogDisplay.Image = value; } }
        public Boolean IsOrigin { get; set; } = false;

        public void Init(Boolean ShowBars = true)
        {
            if (ShowBars)
            {
                this.DisplayToolBar1.Init(this.CogDisplay);
                this.DisplayStatusBar1.Init(this.CogDisplay);
            }
            else
            {
                this.CogRecordsDisplay.ShowRecordsDropDown = false;
                this.panelControl1.Hide();
            }

            this.CogDisplay.AutoFit = true;
            this.CogDisplay.MouseMode = CogDisplayMouseModeConstants.Pan;
            this.SetBackground();
            this.CogRecordsDisplay.RecordChange += CogRecordsDisplayRecordChange;
        }

        public void Close()
        {
            this.DisplayStatusBar1.Dispose();
            this.DisplayToolBar1.Dispose();
            this.RecordsDisplay1.Dispose();
        }

        public void SetImage(CogImage8Grey image, Boolean isOrigin, Boolean fit = true)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => { SetImage(image, isOrigin, fit); }));
                return;
            }
            if (isOrigin)
            {
                //this.CogDisplay.InteractiveGraphics.Clear();
                this.CogDisplay.StaticGraphics.Clear();
            }
            this.IsOrigin = isOrigin;
            this.Image = image;
            this.SetBackground();
            if (fit) this.CogDisplay.Fit(true);
        }

        public void SetBackground()
        {
            this.CogDisplay.BackColor = 배경색상;
        }

        public static void DebugRecords(ICogRecord record, Int32 Depth = 0, String PreKey = "")
        {
            if (record == null) return;
            if (String.IsNullOrEmpty(PreKey)) PreKey = record.RecordKey;
            else PreKey = $"{PreKey}.{record.RecordKey}";
            if (record.SubRecords.Count < 1)
            {
                Debug.WriteLine($"[{record.Annotation}] [{record.RecordUsage}] {record.ContentType.ToString()}", PreKey);
                //if (record.ContentType == typeof(ICogGraphicInteractive))
                //{
                //    ICogGraphicInteractive g = record.Content as ICogGraphicInteractive;
                //    Debug.WriteLine($"\t{g.TipText}: {String.Join(", ", g.StateFlags.Names)}");
                //}
            }
            else
            {
                foreach (ICogRecord r in record.SubRecords)
                    DebugRecords(r, Depth, PreKey);
                Depth++;
            }
        }

        private Dictionary<String, ICogRecord> 출력레코드 = null;
        private CogGraphicCollection StaticGraphics = null;
        private String CurrentRecordKey = String.Empty;
        private delegate void ViewResultImageDelegate(CogImage8Grey Image, CogGraphicCollection StaticGraphics, 결과구분 결과, ICogRecord LastRunRecords, String SelectedRecordKey, Dictionary<String, ICogRecord> 출력자료);
        public void ViewResultImage(CogImage8Grey Image, CogGraphicCollection StaticGraphics, 결과구분 결과, ICogRecord LastRunRecords, String SelectedRecordKey, Dictionary<String, ICogRecord> 출력레코드)
        {
            if (this.CogRecordsDisplay.InvokeRequired)
            {
                this.CogRecordsDisplay.BeginInvoke(new ViewResultImageDelegate(this.ViewResultImage), new Object[] { Image, StaticGraphics, 결과, LastRunRecords, SelectedRecordKey, 출력레코드 });
                return;
            }

            this.StaticGraphics?.Dispose();
            this.StaticGraphics = null;
            this.CurrentRecordKey = String.Empty;
            this.CogDisplay.InteractiveGraphics.Clear();
            this.CogDisplay.StaticGraphics.Clear();
            this.출력레코드?.Clear();

            if (결과 == 결과구분.WA)
            {
                this.CogRecordsDisplay.Subject = null;
                this.CogRecordsDisplay.SelectedRecordKey = String.Empty;
            }
            else
            {
                this.CogRecordsDisplay.Subject = LastRunRecords;
                this.CogRecordsDisplay.SelectedRecordKey = $"LastRun.{SelectedRecordKey}";
            }

            if (StaticGraphics != null && StaticGraphics.Count > 0)
                this.CogDisplay.StaticGraphics.AddList(StaticGraphics, this.CogRecordsDisplay.SelectedRecordKey);

            this.CogDisplay.Image = Image;
            this.CogDisplay.Fit(false);
            this.SetBackground();

            this.StaticGraphics = StaticGraphics;
            this.CurrentRecordKey = SelectedRecordKey;
            this.출력레코드 = 출력레코드;
        }

        private void CogRecordsDisplayRecordChange(object sender, EventArgs e)
        {
            if (this.StaticGraphics == null || this.StaticGraphics.Count < 1 || this.CurrentRecordKey == String.Empty) return;
            if(this.CogRecordsDisplay.SelectedRecordKey.EndsWith(this.CurrentRecordKey))
                this.CogDisplay.StaticGraphics.AddList(this.StaticGraphics, this.CogRecordsDisplay.SelectedRecordKey);
        }

        public void SetSelected(String 검사코드)
        {
            if (this.출력레코드 == null || !this.출력레코드.ContainsKey(검사코드) ||
                !this.CogRecordsDisplay.Subject.SubRecords.ContainsKey(this.CurrentRecordKey) || 
                !this.CogRecordsDisplay.SelectedRecordKey.EndsWith(this.CurrentRecordKey)) return;
            this.SetSelected(this.출력레코드[검사코드], true);
        }

        private void SetSelected(ICogRecord record, Boolean Selected)
        {
            if (record == null || record.SubRecords.Count < 1) return;
            foreach (ICogRecord r in record.SubRecords)
            {
                if (r.ContentType == typeof(CogGraphicCollection)) this.SetSelected(r.Content as CogGraphicCollection, Selected);
                else SetSelected(r, Selected);
            }
        }
        private void SetSelected(CogGraphicCollection Graphics, Boolean Selected)
        {
            if (Graphics == null || Graphics.Count < 1) return;
            ICogGraphicInteractive graphic = Graphics[0] as ICogGraphicInteractive;
            if (graphic.GetType() == typeof(CogCompositeShape))
            {
                foreach (var g in (graphic as CogCompositeShape).Shapes)
                {
                    //Debug.WriteLine(g.GetType().ToString());
                    if (g.GetType() == typeof(CogPolygon) || g.GetType() == typeof(CogRectangleAffine))
                    {
                        SetSelected(g as ICogGraphicInteractive, Selected);
                        return;
                    }
                }
            }
            else SetSelected(graphic, Selected);
        }
        private void SetSelected(ICogGraphicInteractive Graphic, Boolean Selected)
        {
            Graphic.SelectedColor = CogColorConstants.Magenta;
            Graphic.SelectedLineWidthInScreenPixels = 3;
            Graphic.Selected = Selected;
        }
    }
}
