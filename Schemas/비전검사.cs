using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.QuickBuild;
using Cogutils;
using DevExpress.Utils.Extensions;
using MvLibs;
using MvUtils;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace DSEV.Schemas
{
    public class 비전검사 : Dictionary<카메라구분, 비전도구>
    {
        #region 도구초기화
        private CogJobManager Manager = null;
        public 비전검사() { InitPath(); }

        private void InitPath()
        {
            foreach (모델구분 모델 in Enum.GetValues(typeof(모델구분)))
            {
                if (모델 == 모델구분.None) continue;
                String path = Path.Combine(Global.환경설정.도구경로, ((Int32)모델).ToString("d2"));
                if (Directory.Exists(path)) continue;
                Directory.CreateDirectory(path);
            }
        }

        public void Init() => this.Init(Global.환경설정.선택모델);

        public void Init(모델구분 모델)
        {
            this.Close();
            this.Manager = new CogJobManager("JobManager") { GarbageCollection = true, GarbageCollectionInterval = 4 };
            Debug.WriteLine($"GarbageCollection={this.Manager.GarbageCollection}, Interval={this.Manager.GarbageCollectionInterval}", "비젼검사");

            foreach (카메라구분 구분 in 그랩제어.대상카메라)
            {
                비전도구 도구 = new 비전도구(모델, 구분);
                도구.Init();
                this.Add(구분, 도구);
                this.Manager.JobAdd(도구.Job);
            }
        }

        public void Close()
        {
            foreach(비전도구 도구 in this.Values)
                도구.Job?.Shutdown();
            this.Manager?.Shutdown();
            this.Manager = null;
            this.Clear();
            GC.Collect();
        }

        public void SetCalib() => this.Values.ForEach(e => e.SetCalib());

        public void SetDisplay(카메라구분 카메라, RecordDisplay display)
        {
            if (!this.ContainsKey(카메라)) return;
            this[카메라].Display = display;
        }

        public static void 도구설정(비전도구 도구)
        {
            UI.Forms.CogToolEdit viewForm = new UI.Forms.CogToolEdit();
            viewForm.Init(도구);
            viewForm.Show(Global.MainForm);
        }
        public void 도구설정(카메라구분 구분)
        {
            if (!this.ContainsKey(구분)) return;
            도구설정(this[구분]);
        }
        #endregion

        #region Run
        public static String SaveImagePath(카메라구분 카메라, DateTime 검사시간)
        {
            List<String> paths = new List<String> { Global.환경설정.사진저장, Utils.FormatDate(검사시간, "{0:yyyy-MM-dd}"), Global.환경설정.선택모델.ToString(), 카메라.ToString() };
            return Common.CreateDirectory(paths);
            
        }
        public static String SaveImageFile(카메라구분 카메라, DateTime 검사시간, Int32 검사번호, ImageFormat 포맷, 결과구분 결과 = 결과구분.WA)
        {
            String path = SaveImagePath(카메라, 검사시간);
            if (String.IsNullOrEmpty(path)) return String.Empty;
            String ext = 포맷.ToString();
            if (포맷 == ImageFormat.Jpeg) ext = "jpg";
            String name = $"{검사번호.ToString("d4")}_{Utils.FormatDate(검사시간, "{0:HHmmss}")}.{ext}";
            return Path.Combine(path, name);
        }

        public void RunMaster()
        {
            this.Values.ForEach(도구 => 도구.마스터로드());
            //// 결과 이미지 저장 테스트
            //this.Values.ForEach(도구 => {
            //    Stopwatch sw = new Stopwatch();
            //    sw.Start();

            //    String file = SaveImageFile(도구.카메라, DateTime.Now, 0, ImageFormat.Jpeg);
            //    Common.ImageSaveJpeg(도구.InputImage, file, out String error, 70);
            //    /*
            //     * Mat JpegQuality: 60%, JpegOptimize: false
            //      저장시간: Cam01 => 3,659ms, 38.83MB, 0000_193624.Jpeg
            //      저장시간: Cam02 =>   144ms, 10.74MB, 0000_193627.Jpeg
            //      저장시간: Cam03 =>   144ms, 10.79MB, 0000_193628.Jpeg

            //      Mat JpegQuality: 70%, JpegOptimize: true
            //      저장시간: Cam01 => 1,509ms, 44.94MB, 0000_203608.Jpeg
            //      저장시간: Cam02 =>   364ms, 12.78MB, 0000_203609.Jpeg
            //      저장시간: Cam03 =>   351ms, 12.86MB, 0000_203610.Jpeg

            //      저장시간: Cam01 => 1,533ms, 44.94MB, 0000_205206.Jpeg
            //      저장시간: Cam02 =>   361ms, 12.78MB, 0000_205207.Jpeg
            //      저장시간: Cam03 =>   354ms, 12.86MB, 0000_205208.Jpeg

            //     * Cognex
            //      저장시간: Cam01 => 8,611ms, 239.44MB, 0000_185042.Jpeg
            //      저장시간: Cam02 => 1,831ms,  60.44MB, 0000_185051.Jpeg
            //      저장시간: Cam03 => 1,817ms,  61.66MB, 0000_185053.Jpeg
            //    */

            //    //String file = SaveImageFile(도구.카메라, DateTime.Now, 0, ImageFormat.Png);
            //    //Common.ImageSavePng(도구.InputImage, file, out String error);
            //    /*
            //     * Cognex
            //      저장시간: Cam01 => 33,499ms, 215.78MB
            //      저장시간: Cam02 =>  5,170ms,  32.60MB
            //      저장시간: Cam03 =>  5,479ms,  33,51MB
            //    */

            //    sw.Stop();
            //    FileInfo fileInfo = new FileInfo(file);
            //    Debug.WriteLine($"{도구.카메라} => {sw.ElapsedMilliseconds.ToString("#,0")}ms, {Math.Round(fileInfo.Length / 1000000d, 2).ToString("#,0.00")}MB, {fileInfo.Name}", "저장시간");
            //});
        }
        public Boolean RunMaster(카메라구분 카메라)
        {
            if (!this.ContainsKey(카메라)) return false;
            return this[카메라].마스터로드();
        }

        public Boolean Run(그랩장치 장치, 검사결과 결과)
        {
            Debug.WriteLine($"{Utils.FormatDate(DateTime.Now, "{0:HHmmss.fff}")} [검사수행] {장치.구분}");
            if (결과 == null) return false;
            Boolean r = Run(장치.구분, 장치.CogImage(), 결과);
            Global.사진자료.SaveImage(장치, 결과);
            return r;
        }
        public Boolean Run(카메라구분 카메라, ICogImage image, 검사결과 검사)
        {
            if (image == null)
            {
                Global.오류로그("비전검사", "이미지없음", $"{카메라} 검사할 이미지가 없습니다.", true);
                return false;
            }
            if (!this.ContainsKey(카메라)) return false;
            비전도구 도구 = this[카메라];
            return 도구.Run(image, 검사);
        }
        #endregion
    }


    public class PerspectiveCropResize
    {
        public enum FixedSide { None, Left, Right }

        public Size2d Size { get; set; } = new Size2d(0, 0);
        public Double Scale { get; set; } = 0.5;
        public FixedSide Fixed { get; set; } = FixedSide.None;

        public PerspectiveCropResize() { }
        public PerspectiveCropResize(Size2d size, FixedSide @fixed = FixedSide.None)
        {
            this.Size = size;
            this.Fixed = @fixed;
        }

        public static RectanglePoints GetCurrentRegion(CogFixtureNPointToNPointTool Fixture) =>
            new RectanglePoints(
                new PointD(Fixture.RunParams.GetUnfixturedPointX(0), Fixture.RunParams.GetUnfixturedPointY(0)),
                new PointD(Fixture.RunParams.GetUnfixturedPointX(1), Fixture.RunParams.GetUnfixturedPointY(1)),
                new PointD(Fixture.RunParams.GetUnfixturedPointX(2), Fixture.RunParams.GetUnfixturedPointY(2)),
                new PointD(Fixture.RunParams.GetUnfixturedPointX(3), Fixture.RunParams.GetUnfixturedPointY(3))
            );

        public static RectanglePoints GetOriginRegion(CogFixtureNPointToNPointTool Fixture) =>
            new RectanglePoints(
                new PointD(Fixture.RunParams.GetRawFixturedPointX(0), Fixture.RunParams.GetRawFixturedPointY(0)),
                new PointD(Fixture.RunParams.GetRawFixturedPointX(1), Fixture.RunParams.GetRawFixturedPointY(1)),
                new PointD(Fixture.RunParams.GetRawFixturedPointX(2), Fixture.RunParams.GetRawFixturedPointY(2)),
                new PointD(Fixture.RunParams.GetRawFixturedPointX(3), Fixture.RunParams.GetRawFixturedPointY(3))
            );

        public Mat CreateImage(CogImage8Grey cogImage, CogFixtureNPointToNPointTool fixture)
        {
            if (cogImage == null) return null;
            using (Mat source = Base.ToMat(cogImage))
                return CreateImage(source, fixture);
        }
        public Mat CreateImage(Mat source, CogFixtureNPointToNPointTool fixture)
        {
            if (source == null || fixture == null) return null;
            RectanglePoints cur = GetCurrentRegion(fixture);
            RectanglePoints org = GetOriginRegion(fixture);

            using (Mat matrix = Cv2.GetPerspectiveTransform(cur.ToArray(), org.ToArray()))
            using (Mat aligned = source.WarpPerspective(matrix, new Size(source.Width, source.Height)))
            {
                Rect region = CropRegion(org);
                using (Mat croped = Base.CopyRectangle(aligned, region, Scalar.Black))
                    return Base.Resize(croped, Scale);
            }
        }

        private Rect CropRegion(RectanglePoints org)
        {
            Double w = org.Width();
            Double h = org.Height();
            if (this.Size.Width < w || this.Size.Height < h)
                return new Rect2d(org.LT.X, org.LT.Y, w, h).ToRect();

            Double x = org.LT.X;
            if (Fixed == FixedSide.None) x -= (this.Size.Width - w) / 2;
            else if (Fixed == FixedSide.Right) x -= (this.Size.Width - w);
            Double y = org.LT.Y - (this.Size.Height - h) / 2;
            return new Rect2d(x, y, this.Size.Width, this.Size.Height).ToRect();
        }

        public static Mat ConcatHorizontal(Mat image1, Mat image2, Int32 padding = 0)
        {
            if (image1 == null || image2 == null) return null;
            Int32 height = Math.Max(image1.Height, image2.Height);
            Mat image = new Mat(height, image1.Width + image2.Width, MatType.CV_8UC1);
            Rect region = new Rect(0, 0, image1.Width, image1.Height);
            image[region] = image1;
            region = new Rect(image1.Width, 0, image2.Width, image2.Height);
            image[region] = image2;
            //Cv2.HConcat(image1, image2, image);
            if (padding <= 0) return image;
            Mat padded = new Mat(image.Rows + padding * 2, image.Cols + padding * 2, MatType.CV_8UC1, Scalar.Black);
            region = new Rect(padding, padding, image.Width, image.Height);
            padded[region] = image;
            image.Dispose();
            return padded;
        }
    }
}
