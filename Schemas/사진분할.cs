using Cognex.VisionPro;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace DSEV.Schemas
{
    public class 사진분할
    {
        public Double 축소비율 { get; set; } = 0.1;
        public static Int32 가로크기 { get; set; } = 256;
        public static Int32 세로크기 { get; set; } = 256;
        public String 자료경로 = @"C:\IVM\SurfaceDetect\Assets";//@"C:\IVM\UfaServer\Asserts";
        public 사진분할자료 분할자료 = new 사진분할자료();

        public 사진분할자료 분할하기(ICogImage cogImage, Rect2d rect, out String path, out Exception exception)//Int32 index, 
        {
            this.분할자료.Width = 가로크기;
            this.분할자료.Height = 세로크기;
            this.분할자료.Images.Clear();
            Int32 padding = 10;
            this.분할자료.Region = new Rect2d(rect.X - padding, rect.Y - padding, rect.Width + padding * 2, rect.Height + padding * 2).ToRect();
            using (Mat source = Common.ToMat(cogImage))
            using (Mat crop = new Mat(source, this.분할자료.Region))
            {
                Double width = Math.Ceiling(crop.Width * 축소비율 / 가로크기) * 가로크기;
                this.분할자료.Scale = width / crop.Width;
                Debug.WriteLine(crop.Size().ToString(), "CropSize");
                using (Mat resized = Common.Resize(crop, this.분할자료.Scale))
                {
                    Debug.WriteLine($"{this.분할자료.Scale} => {resized.Size().ToString()}", "ResizedSize");
                    Int32 rows = (Int32)Math.Ceiling((Single)resized.Height / 세로크기);
                    Int32 cols = (Int32)Math.Ceiling((Single)resized.Width / 가로크기);
                    using (Mat mat = new Mat(rows * 세로크기, cols * 가로크기, resized.Type(), Scalar.Black))
                    {
                        this.분할자료.StartX = (mat.Width - resized.Width) / 2;
                        this.분할자료.StartY = (mat.Height - resized.Height) / 2;
                        Rect roi = new Rect(this.분할자료.StartX, this.분할자료.StartY, resized.Width, resized.Height);
                        resized.CopyTo(mat[roi]);
                        분할하기(mat, out path, out exception);
                    }
                }
            }
            return 분할자료;
        }
        private void 분할하기(Mat image, out String path, out Exception exception)
        {
            try
            {
                path = Common.CreateDirectory(new List<String>() { 자료경로, MvUtils.Utils.FormatDate(DateTime.Now, "{0:yyMMddHHmmss}") });
                Int32 rows = image.Height / 세로크기;
                Int32 cols = image.Width / 가로크기;
                for (Int32 r = 0; r < rows; r++)
                {
                    for (Int32 c = 0; c < cols; c++)
                    {
                        Int32 y = r * 세로크기;
                        Int32 x = c * 가로크기;
                        Rect roi = new Rect(x, y, 가로크기, 세로크기);
                        사진분할정보 정보 = new 사진분할정보() { Index = 분할자료.Images.Count + 1, Row = r, Col = c, Directory = path };
                        using (Mat copyed = new Mat(image, roi))
                            Common.ImageSaveJpeg(copyed, 정보.FullPath, out String error, 100);
                        분할자료.Images.Add(정보);
                    }
                }
                exception = null;
            }
            catch(Exception ex)
            {
                path = String.Empty;
                exception = ex;
            }
        }
    }

    public enum LableType : UInt32
    {
        [Description("None")]
        N = 0,
        [Description("Clean")]
        C = 1,
        [Description("Scratch")]
        S = 2,
        [Description("Dent")]
        D = 3,
        [Description("Stain")]
        T = 4,
    }

    public class 사진분할정보
    {
        public Int32 Index = 0;
        public Int32 Row = 0;
        public Int32 Col = 0;
        public String Directory = String.Empty;
        public String FileName { get => $"{Index.ToString("d4")}.{Row.ToString("d2")}_{Col.ToString("d2")}.{LableType.C.ToString()}.jpg"; }
        public String FullPath { get => Path.Combine(Directory, FileName); }
        public static void GetRowColByName(String file, out Int32 row, out Int32 col)
        {
            String name = Path.GetFileName(file);
            String[] names = name.Split('.');
            if (names.Length < 3 || !names[1].Contains("_")) { row = -1; col = -1; return; }
            String[] rowcol = names[1].Split('_');
            row = Convert.ToInt32(rowcol[0]);
            col = Convert.ToInt32(rowcol[1]);
        }
    }

    public class 사진분할자료
    {
        public Rect Region = new Rect();
        public Double Scale = 1;
        public Int32 StartX = 0;
        public Int32 StartY = 0;
        public Int32 Width = 사진분할.가로크기;
        public Int32 Height = 사진분할.세로크기;
        public List<사진분할정보> Images = new List<사진분할정보>();

        public List<Rect2d> GetRegions(List<String> files)
        {
            List<Rect2d> regions = new List<Rect2d>();
            foreach (String file in files)
            {
                사진분할정보.GetRowColByName(file, out Int32 row, out Int32 col);
                if (row < 0 || col < 0) continue;
                Debug.WriteLine($"row={row}, col={col}, {file}");
                Double y = (row * Height + StartY) / Scale;
                Double x = (col * Width + StartX) / Scale;
                Double w = Width / Scale;
                Double h = Height / Scale;
                regions.Add(new Rect2d(x, y, w, h));
            }
            return regions;
        }
    }
}
