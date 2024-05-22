using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using Cogutils;
using MvUtils;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace DSEV.Schemas
{
    public class 비전도구
    {
        #region 기본설정
        public String 로그영역 = "Vision Tool";
        public 모델구분 모델구분 = 모델구분.VDA590UFA;
        public 카메라구분 카메라 = 카메라구분.None;
        public String 도구명칭 { get => this.카메라.ToString(); }
        public String 도구경로 { get => Path.Combine(Global.환경설정.도구경로, ((Int32)모델구분).ToString("d2"), $"{도구명칭}.vpp"); }
        public String 마스터경로 { get => Path.Combine(Global.환경설정.마스터사진, $"{((Int32)모델구분).ToString("d2")}.{도구명칭}.bmp"); }

        public CogJob Job = null;
        public CogToolBlock ToolBlock = null;
        public CogToolBlock AlignTools { get => this.GetTool("AlignTools") as CogToolBlock; }
        public ICogImage InputImage { get => this.Input<ICogImage>("InputImage"); set => this.Input("InputImage", value); }
        public ICogImage OutputImage { get => Output<ICogImage>(this.AlignTools, "OutputImage"); }
        public String ViewerRecodName { get => "AlignTools.Fixture.OutputImage"; }

        private DateTime 검사시작 = DateTime.Today;
        private DateTime 검사종료 = DateTime.Today;
        public Double 검사시간 { get { return (this.검사종료 - this.검사시작).TotalMilliseconds; } }
        public RecordDisplay Display = null;

        public 비전도구(모델구분 모델, 카메라구분 카메라)
        {
            this.모델구분 = 모델;
            this.카메라 = 카메라;
        }

        public static Point2d PointTransform(ICogImage OrignImage, String TargetSpaceName, Double OriginX, Double OriginY)
        {
            if (OrignImage == null) return new Point2d() { X = OriginX, Y = OriginY };
            CogTransform2DLinear trans = OrignImage.GetTransform(TargetSpaceName, ".") as CogTransform2DLinear;
            Point2d p = new Point2d();
            trans.MapPoint(OriginX, OriginY, out p.X, out p.Y);
            return p;
        }

        public static ICogTool GetTool(CogToolBlock block, String name)
        {
            if (block == null || !block.Tools.Contains(name)) return null;
            return block.Tools[name];
        }
        public static T Input<T>(CogToolBlock block, String name)
        {
            if (block == null || !block.Inputs.Contains(name)) return default(T);
            Object v = block.Inputs[name].Value;
            if (v == null) return default(T);
            return (T)v;
        }
        public static Boolean Input(CogToolBlock block, String name, Object value)
        {
            if (block == null || !block.Inputs.Contains(name)) return false;
            block.Inputs[name].Value = value;
            return true;
        }
        public static T Output<T>(CogToolBlock block, String name)
        {
            if (block == null || !block.Outputs.Contains(name)) return default(T);
            Object v = block.Outputs[name].Value;
            if (v == null) return default(T);
            return (T)v;
        }
        public static Boolean Output(CogToolBlock block, String name, Object value)
        {
            if (block == null || !block.Outputs.Contains(name)) return false;
            block.Outputs[name].Value = value;
            return true;
        }

        public ICogTool GetTool(String name) => GetTool(ToolBlock, name);
        public T Input<T>(String name) => Input<T>(ToolBlock, name);
        public Boolean Input(String name, Object value) => Input(ToolBlock, name, value);
        public T Output<T>(String name) => Output<T>(ToolBlock, name);

        public static void AddInput(CogToolBlock block, String name, Type type)
        {
            if (block == null || block.Inputs.Contains(name)) return;
            block.Inputs.Add(new CogToolBlockTerminal(name, type));
        }
        public static void AddInput(CogToolBlock block, String name, Object value)
        {
            if (block == null || block.Inputs.Contains(name)) return;
            block.Inputs.Add(new CogToolBlockTerminal(name, value));
        }
        public static void AddOutput(CogToolBlock block, String name, Type type)
        {
            if (block == null || block.Outputs.Contains(name)) return;
            block.Outputs.Add(new CogToolBlockTerminal(name, type));
        }
        public static void AddOutput(CogToolBlock block, String name, Object value)
        {
            if (block == null || block.Outputs.Contains(name)) return;
            block.Outputs.Add(new CogToolBlockTerminal(name, value));
        }

        public void Init() => this.Load();

        public void Load()
        {
            Debug.WriteLine(this.도구경로, this.카메라.ToString());
            if (File.Exists(this.도구경로))
            {
                this.Job = CogSerializer.LoadObjectFromFile(this.도구경로) as CogJob;
                this.Job.Name = $"Job{도구명칭}";
                this.ToolBlock = (this.Job.VisionTool as CogToolGroup).Tools[0] as CogToolBlock;
            }
            else
            {
                this.Job = new CogJob($"Job{도구명칭}");
                CogToolGroup group = new CogToolGroup() { Name = $"Group{도구명칭}" };
                this.ToolBlock = new CogToolBlock();
                this.ToolBlock.Name = this.도구명칭;
                group.Tools.Add(this.ToolBlock);
                this.Job.VisionTool = group;
                this.ToolBlock.Tools.Add(new CogToolBlock() { Name = "AlignTools" });
                this.Save();
            }

            if (this.ToolBlock != null) this.ToolBlock.DataBindings.Clear();
            else this.ToolBlock = new CogToolBlock();
            this.ToolBlock.Name = this.도구명칭;
            if (this.카메라 == 카메라구분.Cam10) return;

            // 파라미터 체크
            AddInput(this.ToolBlock, "InputImage", typeof(CogImage8Grey));
            AddInput(this.AlignTools, "InputImage", typeof(CogImage8Grey));
            AddInput(this.AlignTools, "CalibX", 0.020d);
            AddInput(this.AlignTools, "CalibY", 0.020d);
            AddInput(this.AlignTools, "Width", 107.7d);
            AddInput(this.AlignTools, "Height", 538d);
            AddInput(this.AlignTools, "OriginX", 1024d);
            AddInput(this.AlignTools, "OriginY", 10000d);
            AddOutput(this.AlignTools, "OutputImage", typeof(CogImage8Grey));
            AddOutput(this.AlignTools, "Perspective", typeof(String));
            AddOutput(this.AlignTools, "Detected", false);
            AddOutput(this.AlignTools, "ResolutionX", 0.0d);
            AddOutput(this.AlignTools, "ResolutionY", 0.0d);
            SetCalib();

            // Output 파라메터 설정, 일단 CTQ 항목만
            검사설정 자료 = Global.모델자료.GetItem(this.모델구분)?.검사설정;
            if (자료 == null) return;
            List<검사정보> 목록 = 자료.Where(e => (Int32)e.검사장치 == (Int32)this.카메라 && !String.IsNullOrEmpty(e.변수명칭)).ToList();
            foreach (검사정보 검사 in 목록)
            {
                if (검사.검사그룹 == 검사그룹.CTQ) { 
                    AddOutput(this.ToolBlock, 검사.변수명칭, typeof(Double));
                }
                if (검사.검사그룹 == 검사그룹.Etc)
                {
                    AddOutput(this.ToolBlock, 검사.변수명칭, typeof(Double));
                }
                if (검사.검사그룹 == 검사그룹.Surface)
                {
                    AddOutput(this.ToolBlock, 검사.변수명칭, typeof(Double));
                }
                else { } 
            }
            
        }

        public void Save()
        {
            CogSerializer.SaveObjectToFile(this.Job, this.도구경로, typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
            Global.정보로그(this.로그영역, "Save", $"[{Utils.GetDescription(this.카메라)}] It was saved.", true);
        }

        public void SetCalib() => SetCalib(Global.그랩제어.GetItem(this.카메라));
        public void SetCalib(그랩장치 장치)
        {
            if (장치 == null) return;
            Input(this.AlignTools, "CalibX", 장치.교정X / 1000);
            Input(this.AlignTools, "CalibY", 장치.교정Y / 1000);
        }
        #endregion

        #region Run
        public Boolean IsAccepted()
        {
            foreach (ICogTool tool in this.AlignTools.Tools)
                if (tool.RunStatus.Result != CogToolResultConstants.Accept) return false;
            foreach (ICogTool tool in this.ToolBlock.Tools)
                if (tool.RunStatus.Result != CogToolResultConstants.Accept) return false;
            return true;
        }

        public Boolean Run(ICogImage image, 검사결과 검사)
        {
            try
            {
                if (image != null) this.InputImage = image;
                if (this.InputImage == null) return false;
                this.검사시작 = DateTime.Now;
                this.ToolBlock.Run();
                //this.표면검사(검사);
                검사?.SetResults(this.카메라, this.GetResults());
                DisplayResult(검사);
                this.검사종료 = DateTime.Now;
                if (Global.장치상태.자동수동 && 검사 != null)
                    Global.캘리브?.AddNew(this.ToolBlock, this.카메라, 검사.검사코드);
                return this.IsAccepted();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cognex Run Fails");
                return false;
            }
        }

        public Dictionary<String, Object> GetResults()
        {
            Dictionary<String, Object> results = new Dictionary<String, Object>();
            foreach (CogToolBlockTerminal terminal in this.ToolBlock.Outputs)
            {
                if (terminal.ValueType != typeof(Double)) continue;
                results.Add(terminal.Name, terminal.Value == null ? Double.NaN : (Double)terminal.Value);
            }


            //results.Add(ResultAttribute.VarName(검사항목.Scratchs), SurfaceResults(검사항목.Scratchs));
            //results.Add(ResultAttribute.VarName(검사항목.Dents), SurfaceResults(검사항목.Dents));


            //results.Add(ResultAttribute.VarName(검사항목.Scratchs), SurfaceResults(검사항목.Scratchs));
            //results.Add(ResultAttribute.VarName(검사항목.Dents), SurfaceResults(검사항목.Dents));


            return results;
        }

        public List<불량영역> SurfaceResults(검사항목 항목)
        {
            String name = ResultAttribute.VarName(항목);
            List<불량영역> list = new List<불량영역>();
            CogToolBlock surfacetools = this.GetTool("SurfaceTools") as CogToolBlock;
            if (surfacetools == null) return list;
            String json = Output<String>(surfacetools, name);
            if (String.IsNullOrEmpty(json)) return list;
            List<List<Double>> rects = JsonConvert.DeserializeObject<List<List<Double>>>(json);
            foreach (List<Double> rect in rects)
                list.Add(new 불량영역(this.카메라, 항목, rect));
            return list;
        }

        // Deep Learning
        //private void 표면검사(검사결과 검사)
        //{
        //    if (this.카메라 != 카메라구분.Cam01) return;
        //    if (this.InputImage == null || this.OutputImage == null || this.BaseTools == null) return;
        //    String json = this.BaseTools.Outputs["Perspective"].Value.ToString();
        //    if (String.IsNullOrEmpty(json)) return;
        //    UfaVision.RectanglePerspectiveTransform trans = new UfaVision.RectanglePerspectiveTransform();
        //    if (!trans.Load(json)) return;
        //    Point2d p = PointTransform(this.OutputImage, "@", trans.Left, trans.Top);
        //    Rect2d rect = new Rect2d(p.X, p.Y, trans.Width, trans.Height);
        //    사진분할 분할 = new 사진분할();
        //    사진분할자료 자료 = 분할.분할하기(this.InputImage, rect, out String path, out Exception ex);
        //    if (ex != null) { Debug.WriteLine(ex.ToString()); return; }

        //    IEnumerable<ImageData> images = Learning.LoadImagesFromDirectory(path);
        //    if (images.Count() < 1) return;
        //    IEnumerable<ModelOutput> predictions = Learning.RunInspect(images, path);
        //    List<ModelOutput> Bads = predictions.Where(prediction => prediction.PredictedLabel != Schemas.LableType.C.ToString()).ToList();
        //    List<String> files = Bads.Select(s => s.ImagePath).ToList();
        //    List<Rect2d> rects = 자료.GetRegions(files);
        //    foreach (Rect2d br in rects)
        //    {
        //        Point2d cp = PointTransform(this.InputImage, "Fixture", br.X, br.Y);
        //        Rect2d cr = new Rect2d(cp.X, cp.Y, br.Width, br.Height);
        //        Debug.WriteLine(cr.ToString());
        //        //검사.표면불량.Add(new 불량영역(this.카메라, 검사항목.스크레치B, cr.ToRect()));
        //        this.DrawRectangle(this.Display, cr, 비전도구.GraphicColor(결과구분.NG));
        //    }
        //    Directory.Delete(path, true);
        //}
        #endregion

        #region 도구설정, 마스터
        public void 도구설정() => 비전검사.도구설정(this);
        public Boolean 마스터저장()
        {
            if (this.InputImage == null) return false;
            //png로 저장할 경우 이미지 변형 발생확인
            //Boolean r = Common.ImageSavePng(this.InputImage, this.마스터경로, out String error);
            Boolean r = Common.ImageSaveBmp(this.InputImage, this.마스터경로, out String error);

            if (!r) Utils.WarningMsg("마스터 이미지 등록실패!!!\n" + error);
            return r;
        }
        public Boolean 마스터로드(Boolean autoCalibration = false)
        {
            Boolean r = 이미지로드(this.마스터경로);
            if (r && Output<Boolean>(AlignTools, "Detected"))
            {
                Double rX = Output<Double>(AlignTools, "ResolutionX");
                Double rY = Output<Double>(AlignTools, "ResolutionY");
                //Debug.WriteLine($"CalibX={rX}, CalibY={rY}", this.카메라.ToString());
                if (autoCalibration && rX > 0 && rY > 0)
                {
                    그랩장치 장치 = Global.그랩제어.GetItem(카메라);
                    if (장치 != null)
                    {
                        장치.교정X = rX; 장치.교정Y = rY;
                        Input(AlignTools, "CalibX", rX / 1000);
                        Input(AlignTools, "CalibY", rY / 1000);
                        CogTransform2DLinear transform = (GetTool(AlignTools, "Fixture") as CogFixtureNPointToNPointTool)?.RunParams.RawFixturedFromFixturedTransform as CogTransform2DLinear;
                        if (transform != null)
                        {
                            //Debug.WriteLine($"OriginX={transform.TranslationX}, OriginY={transform.TranslationY}", this.카메라.ToString());
                            Input(AlignTools, "OriginX", Math.Round(transform.TranslationX, 9));
                            Input(AlignTools, "OriginY", Math.Round(transform.TranslationY, 9));
                        }
                    }
                }
            }
            return r;
        }
        public Boolean 이미지로드() => 이미지로드(Common.GetImageFile());
        public Boolean 이미지로드(String path)
        {
            if (!File.Exists(path)) return false;
            return 이미지검사(Common.LoadImage(path, this.카메라 == 카메라구분.Cam10));
        }
        public Boolean 이미지검사(ICogImage image)
        {
            if (image == null) return false;
            Global.검사자료.수동검사.Reset(this.카메라);
            this.Run(image, Global.검사자료.수동검사);
            Global.검사자료.수동검사결과(this.카메라, Global.검사자료.수동검사);
            return true;
        }
        public Boolean 다시검사()
        {
            Global.검사자료.수동검사.Reset(this.카메라);
            this.Run(null, Global.검사자료.수동검사);
            Global.검사자료.수동검사결과(this.카메라, Global.검사자료.수동검사);
            return true;
        }
        #endregion

        #region Display
        public void DisplayResult(검사결과 검사) => this.DisplayResult(this.Display, 검사);
        public void DisplayResult(RecordDisplay display, 검사결과 검사)
        {
            if (display == null) return;
            ICogRecord records = this.ToolBlock.CreateLastRunRecord();
            try
            {
                ICogRecord record = null;
                if (records != null && records.SubRecords != null && records.SubRecords.ContainsKey(this.ViewerRecodName))
                    record = records.SubRecords[this.ViewerRecodName];
                List<ICogGraphic> graphics = new List<ICogGraphic>();
                if (검사 != null)
                    검사.불량영역(this.카메라).ForEach(e => graphics.Add(e.GetRectangleAffine(GraphicColor(결과구분.NG), 2)));

                if (this.OutputImage != null) display.SetImage(this.OutputImage, record, graphics);
                else display.SetImage(this.InputImage, record, graphics);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message, "DisplayResult"); }
        }

        public static CogColorConstants GraphicColor(결과구분 판정)
        {
            if (판정 == 결과구분.OK) return CogColorConstants.DarkGreen;
            if (판정 == 결과구분.NG) return CogColorConstants.Red;
            if (판정 == 결과구분.ER) return CogColorConstants.Magenta;
            return CogColorConstants.LightGrey;
        }

        //public static CogRectangle CreateRectangle(Rect2d rect, CogColorConstants color) =>
        //    new CogRectangle() { X = rect.X, Y = rect.Y, Width = rect.Width, Height = rect.Height, Color = color };

        //public static CogGraphicLabel CreateLabel(String text, Double x, Double y, Single size, CogColorConstants color, String spaceName, CogGraphicLabelAlignmentConstants alignment = CogGraphicLabelAlignmentConstants.TopCenter)
        //{
        //    CogGraphicLabel label = new CogGraphicLabel() { Text = text, X = x, Y = y, Color = color, SelectedSpaceName = spaceName, Alignment = alignment };
        //    label.Font = new Font(label.Font.FontFamily, size, FontStyle.Bold);
        //    return label;
        //}
        #endregion

        #region 이미지 저장
        public void SaveDisplayImage(String path)
        {
            if (this.Display == null) return;
            try 
            {
                this.Display.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(path, ImageFormat.Jpeg);
            }
            catch(Exception e) { Debug.WriteLine(e.Message, "결과 이미지 저장 오류"); }
        }
        #endregion
    }
}
