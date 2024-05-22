
namespace DSEV.UI.Controls
{
    partial class Login
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.e한글 = new DevExpress.XtraEditors.CheckButton();
            this.e영어 = new DevExpress.XtraEditors.CheckButton();
            this.b취소 = new DevExpress.XtraEditors.SimpleButton();
            this.BindLocalization = new System.Windows.Forms.BindingSource(this.components);
            this.b인증 = new DevExpress.XtraEditors.SimpleButton();
            this.e비밀번호 = new DevExpress.XtraEditors.TextEdit();
            this.e사용자명 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindLocalization)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e비밀번호.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e사용자명.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.e한글);
            this.layoutControl1.Controls.Add(this.e영어);
            this.layoutControl1.Controls.Add(this.b취소);
            this.layoutControl1.Controls.Add(this.b인증);
            this.layoutControl1.Controls.Add(this.e비밀번호);
            this.layoutControl1.Controls.Add(this.e사용자명);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(300, 230);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // e한글
            // 
            this.e한글.GroupIndex = 0;
            this.e한글.ImageOptions.SvgImage = global::DSEV.Properties.Resources.Korea;
            this.e한글.ImageOptions.SvgImageSize = new System.Drawing.Size(32, 32);
            this.e한글.Location = new System.Drawing.Point(160, 20);
            this.e한글.Name = "e한글";
            this.e한글.Size = new System.Drawing.Size(120, 36);
            this.e한글.StyleController = this.layoutControl1;
            this.e한글.TabIndex = 9;
            this.e한글.TabStop = false;
            this.e한글.Text = "Korean";
            // 
            // e영어
            // 
            this.e영어.GroupIndex = 0;
            this.e영어.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("e영어.ImageOptions.SvgImage")));
            this.e영어.ImageOptions.SvgImageSize = new System.Drawing.Size(32, 32);
            this.e영어.Location = new System.Drawing.Point(20, 20);
            this.e영어.Name = "e영어";
            this.e영어.Size = new System.Drawing.Size(120, 36);
            this.e영어.StyleController = this.layoutControl1;
            this.e영어.TabIndex = 8;
            this.e영어.TabStop = false;
            this.e영어.Text = "English";
            // 
            // b취소
            // 
            this.b취소.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindLocalization, "취소", true));
            this.b취소.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b취소.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b취소.ImageOptions.SvgImage")));
            this.b취소.Location = new System.Drawing.Point(152, 182);
            this.b취소.Name = "b취소";
            this.b취소.Size = new System.Drawing.Size(136, 36);
            this.b취소.StyleController = this.layoutControl1;
            this.b취소.TabIndex = 7;
            this.b취소.Text = "취  소";
            // 
            // BindLocalization
            // 
            this.BindLocalization.DataSource = typeof(DSEV.UI.Controls.Login.LocalizationLogin);
            // 
            // b인증
            // 
            this.b인증.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindLocalization, "로그인", true));
            this.b인증.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b인증.ImageOptions.SvgImage")));
            this.b인증.Location = new System.Drawing.Point(12, 182);
            this.b인증.Name = "b인증";
            this.b인증.Size = new System.Drawing.Size(136, 36);
            this.b인증.StyleController = this.layoutControl1;
            this.b인증.TabIndex = 6;
            this.b인증.Text = "로그인";
            // 
            // e비밀번호
            // 
            this.e비밀번호.EnterMoveNextControl = true;
            this.e비밀번호.Location = new System.Drawing.Point(80, 124);
            this.e비밀번호.Name = "e비밀번호";
            this.e비밀번호.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e비밀번호.Properties.Appearance.Options.UseFont = true;
            this.e비밀번호.Properties.PasswordChar = '*';
            this.e비밀번호.Properties.UseSystemPasswordChar = true;
            this.e비밀번호.Size = new System.Drawing.Size(200, 28);
            this.e비밀번호.StyleController = this.layoutControl1;
            this.e비밀번호.TabIndex = 5;
            // 
            // e사용자명
            // 
            this.e사용자명.EnterMoveNextControl = true;
            this.e사용자명.Location = new System.Drawing.Point(80, 76);
            this.e사용자명.Name = "e사용자명";
            this.e사용자명.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e사용자명.Properties.Appearance.Options.UseFont = true;
            this.e사용자명.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.e사용자명.Size = new System.Drawing.Size(200, 28);
            this.e사용자명.StyleController = this.layoutControl1;
            this.e사용자명.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(300, 230);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.e사용자명;
            this.layoutControlItem1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindLocalization, "사용자명", true));
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 56);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem1.Size = new System.Drawing.Size(280, 48);
            this.layoutControlItem1.Text = "사용자명";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 15);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 152);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(280, 18);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.e비밀번호;
            this.layoutControlItem2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindLocalization, "비밀번호", true));
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 104);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem2.Size = new System.Drawing.Size(280, 48);
            this.layoutControlItem2.Text = "비밀번호";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 15);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.b인증;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 170);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(140, 40);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.b취소;
            this.layoutControlItem4.Location = new System.Drawing.Point(140, 170);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(140, 40);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.e영어;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem5.Size = new System.Drawing.Size(140, 56);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.e한글;
            this.layoutControlItem6.Location = new System.Drawing.Point(140, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
            this.layoutControlItem6.Size = new System.Drawing.Size(140, 56);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 230);
            this.Controls.Add(this.layoutControl1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindLocalization, "타이틀", true));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Login.IconOptions.SvgImage")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "로그인";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BindLocalization)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e비밀번호.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e사용자명.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit e비밀번호;
        private DevExpress.XtraEditors.ComboBoxEdit e사용자명;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton b취소;
        private DevExpress.XtraEditors.SimpleButton b인증;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.BindingSource BindLocalization;
        private DevExpress.XtraEditors.CheckButton e한글;
        private DevExpress.XtraEditors.CheckButton e영어;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}