using DevExpress.XtraEditors;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraGauges.Win.Base;
using DevExpress.XtraGauges.Win.Gauges.Circular;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSEV.UI.Controls
{
    public partial class AnalogClock : DevExpress.XtraEditors.XtraUserControl
    {
        public AnalogClock()
        {
            InitializeComponent();
        }

        ArcScaleComponent scaleMinutes, scaleSeconds;
        int lockTimerCounter = 0;
        public void Init()
        {
            scaleMinutes = circularGauge1.AddScale();
            scaleSeconds = circularGauge1.AddScale();

            scaleMinutes.Assign(scaleHours);
            scaleSeconds.Assign(scaleHours);

            arcScaleNeedleComponent2.ArcScale = scaleMinutes;
            arcScaleNeedleComponent3.ArcScale = scaleSeconds;
            OnTimerTick(null, null);
            timer.Interval = 500;
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (lockTimerCounter == 0)
            {
                lockTimerCounter++;
                UpdateClock(DateTime.Now, scaleHours, scaleMinutes, scaleSeconds);
                lockTimerCounter--;
            }
        }
        private void UpdateClock(DateTime dt, IArcScale h, IArcScale m, IArcScale s)
        {
            int hour = dt.Hour < 12 ? dt.Hour : dt.Hour - 12;
            int min = dt.Minute;
            int sec = dt.Second;
            h.Value = (float)hour + (float)(min) / 60.0f;
            m.Value = ((float)min + (float)(sec) / 60.0f) / 5f;
            s.Value = sec / 5.0f;
        }
    }
}
