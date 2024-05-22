using MvUtils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace DSEV
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Boolean createdNew = false;
            Mutex mtx = new Mutex(true, Global.GetGuid(), out createdNew);
            // 뮤텍스를 얻지 못하면 에러
            if (!createdNew)
            {
                Utils.ErrorMsg("프로그램이 이미 실행중입니다.");
                Application.Exit();
                return;
            }

            #pragma warning disable
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            DevExpress.XtraEditors.WindowsFormsSettings.LoadApplicationSettings();
            DevExpress.XtraEditors.WindowsFormsSettings.FormThickBorder = false;
            //DevExpress.XtraEditors.WindowsFormsSettings.ForceDirectXPaint();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            String paletteName = String.IsNullOrEmpty(Properties.Settings.Default.SvgPaletteName) ? Global.BlackPalette : Properties.Settings.Default.SvgPaletteName;
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(Global.SkinName, paletteName);
            DevExpress.LookAndFeel.UserLookAndFeel.Default.StyleChanged += Default_StyleChanged;
            //Debug.WriteLine($"{Properties.Settings.Default.SkinName}, {Properties.Settings.Default.SvgPaletteName}");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Global.MainForm = new MainForm();
                Application.Run(Global.MainForm);
            }
            catch (Exception ex)
            {
                Global.오류로그("System", "Exception", ex.Message, false);
                Global.Close();
            }
        }

        private static void Default_StyleChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSvgPaletteName, DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName);
            Properties.Settings.Default.SkinName = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName;
            Properties.Settings.Default.SvgPaletteName = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSvgPaletteName;
            Properties.Settings.Default.Save();
        }
    }
}
