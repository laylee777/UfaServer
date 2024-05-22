using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using MvUtils;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using DSEV.Schemas;

namespace DSEV.UI.Forms
{
    public partial class CogToolEdit : DevExpress.XtraBars.ToolbarForm.ToolbarForm
    {
        public CogToolEdit()
        {
            InitializeComponent();
            this.Shown += CogToolEditShown;
            this.FormClosed += CogToolEdit_FormClosed;
            this.b검사수행.ItemClick += 검사수행;
            this.b사진열기.ItemClick += 이미지로드;
            this.b마스터로드.ItemClick += 마스터로드;
            this.b마스터저장.ItemClick += 마스터저장;
            this.b설정저장.ItemClick += 설정저장;
            this.b사진분할.ItemClick += 분할하기;
            Global.검사자료.수동검사알림 += 수동검사알림;
        }

        public String 사진파일 { get; set; } = String.Empty;
        private const String 로그영역 = "Vision Tools";
        private CogToolGroupEditV2 CogControl = null;
        비전도구 검사도구 = null;
        사진분할 사진분할 = new 사진분할();

        private void CogToolEditShown(object sender, EventArgs e)
        {
            if (검사도구 == null || String.IsNullOrEmpty(사진파일)) return;
            검사도구.이미지로드(사진파일);
            this.e결과목록.RefreshData();
        }
        private void CogToolEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.검사자료.수동검사알림 -= 수동검사알림;
            this.CogControl?.Dispose();
        }

        public void Init(비전도구 도구)
        {
            this.검사도구 = 도구;
            this.Text = 도구.도구명칭;
            this.CogControl = new CogToolBlockEditV2() { Subject = this.검사도구.ToolBlock, Dock = DockStyle.Fill };
            this.Controls.Add(this.CogControl);
            this.e결과목록.Init();
            Global.검사자료.수동검사.Reset(도구.카메라);
            this.e결과목록.SetResults(Global.검사자료.수동검사);
        }

        private void 수동검사알림(카메라구분 카메라, 검사결과 결과) => this.e결과목록.RefreshData();
        private void 검사수행(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => this.검사도구.다시검사();
        private void 이미지로드(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => this.검사도구.이미지로드();
        private void 마스터로드(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => this.검사도구.마스터로드(b자동교정.Checked);
        private void 마스터저장(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.검사도구.InputImage == null) return;
            if (!Utils.Confirm(this.FindForm(), "현재 이미지를 마스터로 저장하시겠습니까?")) return;
            if (this.검사도구.마스터저장()) Global.정보로그(로그영역, "마스터 저장", $"저장하였습니다.", this);
        }
        private void 설정저장(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!Utils.Confirm(this.FindForm(), "비젼도구 설정을 저장하시겠습니까?")) return;
            try { this.검사도구.Save(); }
            catch(Exception ex) {
                Global.오류로그(로그영역, "저장오류", $"오류가 발생하였습니다.\n{ex.Message}", this);
            }
        }

        private void 분할하기(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (검사도구.InputImage == null) { Utils.WarningMsg("이미지가 없습니다."); return; }
            CogToolBlock tool = 검사도구.GetTool("BaseTools") as CogToolBlock;
            if (tool == null || !tool.Outputs.Contains("Perspective")) { Utils.WarningMsg("분할대상 정보가 없습니다."); return; }
            String json = tool.Outputs["Perspective"].Value.ToString();
            if (String.IsNullOrEmpty(json)) { Utils.WarningMsg("분할정보가 없습니다."); return; }
            MvLibs.RectanglePerspectiveTransform trans = new MvLibs.RectanglePerspectiveTransform();
            if (!trans.Load(json)) { Utils.WarningMsg("분할정보가 올바르지 않습니다."); return; }
            Point2d p = 비전도구.PointTransform(검사도구.OutputImage, "@", trans.Left, trans.Top);
            Rect2d rect = new Rect2d(p.X, p.Y, trans.Width, trans.Height);
            사진분할자료 자료 = 사진분할.분할하기(검사도구.InputImage, rect, out String path, out Exception ex);
            if (ex == null)
            {
                if (String.IsNullOrEmpty(path)) return;
                Process.Start(path);
            }
            else Utils.ErrorMsg($"오류: {ex.Message}");
        }
    }
}