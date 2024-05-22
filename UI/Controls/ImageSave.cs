using DevExpress.XtraEditors;
using DSEV.Schemas;
using MvUtils;
using System;

namespace DSEV.UI.Controls
{
    public partial class ImageSave : XtraUserControl
    {
        public ImageSave()
        {
            InitializeComponent();
        }

        LocalizationUsers 번역 = new LocalizationUsers();
        public void Init()
        {
            this.GridView1.Init(this.barManager1);
            this.GridView1.OptionsBehavior.Editable = true;
            this.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            this.GridControl1.DataSource = Global.사진자료.Values;
            this.GridView1.BestFitColumns();
            this.b정보저장.Click += 정보저장;
            Localization.SetColumnCaption(this.GridView1, typeof(사진저장));
            this.g사진관리.Text = 사진자료.로그영역.GetString();
            this.b정보저장.Text = 번역.사진저장;
        }

        private void 정보저장(object sender, EventArgs e)
        {
            if (!Utils.Confirm(this.FindForm(), 번역.저장확인, Localization.확인.GetString())) return;
            Global.사진자료.Save();
            Global.정보로그(사진자료.로그영역.GetString(), 번역.정보저장, 번역.저장완료, this.FindForm());
        }

        public void Close() { }

        private class LocalizationUsers
        {
            private enum Items
            {
                [Translation("Save", "정보저장")]
                정보저장,
                [Translation("It's saved.", "저장되었습니다.")]
                저장완료,
                [Translation("Save this data?", "정보를 저장하시겠습니까?")]
                저장확인,
            }

            public String 정보저장 { get { return Localization.GetString(Items.정보저장); } }
            public String 저장완료 { get { return Localization.GetString(Items.저장완료); } }
            public String 저장확인 { get { return Localization.GetString(Items.저장확인); } }
            public String 사진저장 { get { return Localization.저장.GetString(); } }
        }
    }
}
