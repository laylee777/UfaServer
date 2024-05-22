using DevExpress.XtraEditors;
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

namespace DSEV.UI.Forms
{
    public partial class TestForm : XtraForm
    {
        public TestForm()
        {
            InitializeComponent();

            String addr = "W10A";
            Int32 start = Convert.ToInt32(addr.Substring(1), 16);
            for(int i = 0; i < 10; i++)
            {
                Int32 current = start + i;
                String hexString = current.ToString("X");
                Debug.WriteLine($"W{hexString}", i.ToString());
            }
        }
    }
}