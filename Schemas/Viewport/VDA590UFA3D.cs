using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DSEV.Schemas
{
    public class VDA590UFA3D : Viewport
    {
        #region 초기화
        public override String StlPath => Global.환경설정.기본경로;
        public override String StlFile => Path.Combine(StlPath, "VDA590UFA.stl");
        public override Double Scale => 1;
        internal override void LoadStl()
        {
            if (!File.Exists(StlFile)) return;
            StLReader reader = new StLReader();
            Model3DGroup groups = reader.Read(StlFile);
            MainModel = groups.Children[0] as GeometryModel3D;
            //Debug.WriteLine(groups.Children.Count, "Groups Count");

            Point3D p = Center3D();
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(p.X * Scale, p.Y * Scale, 0 * Scale));
            transform.Children.Add(new ScaleTransform3D(Scale, Scale, Scale));
            MainModel.Transform = transform;
            //MainModel.Transform = new TranslateTransform3D(p.X, p.Y, 0);
            //Debug.WriteLine(p.ToString(), "p");
            //Debug.WriteLine(MainModel.Transform.Value.ToString(), "Transform");

            MainModel.SetName(nameof(MainModel));
            MainModel.Material = FrontMaterial;
            MainModel.BackMaterial = BackMaterial;
            ModelGroup.Children.Add(MainModel);
        }
        #endregion

        #region 기본 설정
        List<Base3D> InspItems = new List<Base3D>();
        internal String InspectionName(검사항목 항목)
        {
            검사정보 정보 = Global.모델자료.선택모델.검사설정.GetItem(항목);
            if (정보 == null) return String.Empty;
            return 정보.검사명칭;
        }
        internal override void InitModel()
        {
            if (MainModel == null) return;
            //Children.Add(new GridLinesVisual3D
            //{
            //    MajorDistance = 10, // 주 그리드 간격
            //    MinorDistance = 5,  // 보조 그리드 간격
            //    Thickness = 1, // Scale,    // 그리드 두께
            //    Center = new Point3D(0, 0, 0),
            //    Material = GridMaterial,
            //});

            Rect3D r = MainModel.Bounds;
            Debug.WriteLine($"{r.SizeY}, {r.SizeX}, {r.SizeZ}", "Rectangle3D"); // 217, 562.16
            Double hx = r.SizeX / 2;
            Double hy = r.SizeY / 2;
            Double hz = r.SizeZ / 2;
            Double zz = r.SizeZ;
            Double tz = 2.5;

            AddText3D(new Point3D(-hx - 60, 0, 0), "R", 48, MajorColors.FrameColor);
            AddText3D(new Point3D(+hx + 60, 0, 0), "F", 48, MajorColors.FrameColor);
            AddArrowLine(new Point3D(-hx, 0, tz), new Point3D(hx, 0, tz), MajorColors.FrameColor); // Front ~ Rear Center
            AddArrowLine(new Point3D(0, -hy, tz), new Point3D(0, hy, tz), MajorColors.FrameColor); // Width Center
            AddArrowLine(new Point3D(0, -hy - 0.5,  0), new Point3D(0, -hy - 0.5, zz), MajorColors.FrameColor); // Front ~ Rear Center
            AddArrowLine(new Point3D(0, +hy + 0.5,  0), new Point3D(0, +hy + 0.5, zz), MajorColors.FrameColor); // Width Center

            InspItems.Add(new Width3D(검사항목.DistC3C4)  { Point = new Point3D(-200, -hy / 2, 7.2), PointS = new Point3D(-200, -hy, 7), PointE = new Point3D(-200, hy, 7), Name = "C3C4", LabelS = "C4", LabelE = "C3", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Width3D(검사항목.DistC2C5)  { Point = new Point3D(   0, -hy / 2, 7.2), PointS = new Point3D(   0, -hy, 7), PointE = new Point3D(   0, hy, 7), Name = "C2C5", LabelS = "C5", LabelE = "C2", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Width3D(검사항목.DistC1C6)  { Point = new Point3D(+200, -hy / 2, 7.2), PointS = new Point3D(+200, -hy, 7), PointE = new Point3D(+200, hy, 7), Name = "C1C6", LabelS = "C6", LabelE = "C1", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Length3D(검사항목.B1) { Origin = new Point3D(-268.5 - 10, +hy + 5, 65), PointS = new Point3D(0, +hy, 65), PointE = new Point3D(-268.5, +hy, 65), Name = "B1" });
            InspItems.Add(new Length3D(검사항목.B2) { Origin = new Point3D(-268.5 - 10, -hy - 5, 65), PointS = new Point3D(0, -hy, 65), PointE = new Point3D(-268.5, -hy, 65), Name = "B2" });
            InspItems.Add(new Length3D(검사항목.B3) { Origin = new Point3D(-268.5 - 10, +hy - 5, 15), PointS = new Point3D(0, +hy, 15), PointE = new Point3D(-268.5, +hy, 15), Name = "B3" });
            InspItems.Add(new Length3D(검사항목.B4) { Origin = new Point3D(-268.5 - 10, -hy + 5, 15), PointS = new Point3D(0, -hy, 15), PointE = new Point3D(-268.5, -hy, 15), Name = "B4" });
            InspItems.Add(new Length3D(검사항목.B8) { Origin = new Point3D(+268.5 + 10, -hy - 5, 65), PointS = new Point3D(0, -hy, 65), PointE = new Point3D(+268.5, -hy, 65), Name = "B8", LabelStyle = NamePrintType.Right });
            InspItems.Add(new Length3D(검사항목.B7) { Origin = new Point3D(+268.5 + 10, +hy + 5, 65), PointS = new Point3D(0, +hy, 65), PointE = new Point3D(+268.5, +hy, 65), Name = "B7", LabelStyle = NamePrintType.Right });
            InspItems.Add(new Length3D(검사항목.B10) { Origin = new Point3D(+268.5 + 10, -hy + 5, 15), PointS = new Point3D(0, -hy, 15), PointE = new Point3D(+268.5, -hy, 15), Name = "B10", LabelStyle = NamePrintType.Right });
            InspItems.Add(new Length3D(검사항목.B9) { Origin = new Point3D(+268.5 + 10, +hy - 5, 15), PointS = new Point3D(0, +hy, 15), PointE = new Point3D(+268.5, +hy, 15), Name = "B9", LabelStyle = NamePrintType.Right });
            //InspItems.Add(new Label3D(검사항목.THKL1)   { Point = new Point3D(-hx, +hy / 2, 1), Origin = new Point3D(-hx - 30, +hy / 2, 1), Name = "L1" });
            //InspItems.Add(new Label3D(검사항목.THKL2)   { Point = new Point3D(-hx, -hy / 2, 1), Origin = new Point3D(-hx - 30, -hy / 2, 1), Name = "L2" });
            //InspItems.Add(new Label3D(검사항목.THKR1)   { Point = new Point3D(+hx, -hy / 2, 1), Origin = new Point3D(+hx + 30, -hy / 2, 1), Name = "R1", LabelStyle = NamePrintType.Right });
            //InspItems.Add(new Label3D(검사항목.THKR2)   { Point = new Point3D(+hx, +hy / 2, 1), Origin = new Point3D(+hx + 30, +hy / 2, 1), Name = "R2", LabelStyle = NamePrintType.Right });
            //InspItems.Add(new Label3D(검사항목.DistD1)  { Point = new Point3D(-260, -hy, 78), Origin = new Point3D(-260, -hy - 30, 78), Name = "D1", LabelStyle = NamePrintType.Down });
            //InspItems.Add(new Label3D(검사항목.DistD2)  { Point = new Point3D(   0, -hy, 78), Origin = new Point3D(   0, -hy - 30, 78), Name = "D2", LabelStyle = NamePrintType.Down });
            //InspItems.Add(new Label3D(검사항목.DistD3)  { Point = new Point3D(+260, -hy, 78), Origin = new Point3D(+260, -hy - 30, 78), Name = "D3", LabelStyle = NamePrintType.Down });
            //InspItems.Add(new Label3D(검사항목.DistD4)  { Point = new Point3D(-260, +hy, 78), Origin = new Point3D(-260, +hy + 30, 78), Name = "D4", LabelStyle = NamePrintType.Up });
            //InspItems.Add(new Label3D(검사항목.DistD5)  { Point = new Point3D(   0, +hy, 78), Origin = new Point3D(   0, +hy + 30, 78), Name = "D5", LabelStyle = NamePrintType.Up });
            //InspItems.Add(new Label3D(검사항목.DistD6)  { Point = new Point3D(+260, +hy, 78), Origin = new Point3D(+260, +hy + 30, 78), Name = "D6", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.A1) { Point = new Point3D(-230, +90, tz), Name = "A1", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.A2) { Point = new Point3D(+230, +90, tz), Name = "A2", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.A3) { Point = new Point3D(-230, -90, tz), Name = "A3", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.A4) { Point = new Point3D(+230, -90, tz), Name = "A4", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a1) { Point = new Point3D(-200, +90, tz), Name = "a1", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a2) { Point = new Point3D(-200, 0, tz), Name = "a2", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a3) { Point = new Point3D(-200, -90, tz), Name = "a3", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a4) { Point = new Point3D(0, -90, tz), Name = "a4", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a5) { Point = new Point3D(0, 0, tz), Name = "a5", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a6) { Point = new Point3D(0, +90, tz), Name = "a6", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a7) { Point = new Point3D(+200, +90, tz), Name = "a7", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a8) { Point = new Point3D(+200, 0, tz), Name = "a8", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Circle3D(검사항목.a9) { Point = new Point3D(+200, -90, tz), Name = "a9", LabelStyle = NamePrintType.Up });
            InspItems.Add(new Label3D(검사항목.Flatness) { Name = InspectionName(검사항목.Flatness), Point = new Point3D(130, 96, tz) });


            //InspItems.Add(new Rectangle3D(검사항목.QrLegibility) { Point = new Point3D(98.5, -hy - 0.5, 40), Width = 34, Height = 24, Normal = new Vector3D(0, 1, 0), Name = String.Empty, FontHeight = 20, Value = Decimal.MinValue, LabelStyle = NamePrintType.Center });
            InspItems.Add(new Label3D(검사항목.ShapeB1B4) { Name = InspectionName(검사항목.ShapeB1B4), Point = new Point3D(130, 84, tz) });
            InspItems.Add(new Label3D(검사항목.ShapeB7B10) { Name = InspectionName(검사항목.ShapeB7B10), Point = new Point3D(130, 72, tz) });
            InspItems.Add(new Label3D(검사항목.ShapeH1H8) { Name = InspectionName(검사항목.ShapeH1H8), Point = new Point3D(130, 60, tz) });
            InspItems.Add(new Label3D(검사항목.ShapeH9H16) { Name = InspectionName(검사항목.ShapeH9H16), Point = new Point3D(130, 48, tz) });
            //InspItems.Add(new Label3D(검사항목.Profile4) { Name = InspectionName(검사항목.Profile4), Point = new Point3D(130, 58, tz) });
            //InspItems.Add(new Label3D(검사항목.Scratchs) { Name = InspectionName(검사항목.Scratchs), Point = new Point3D(130, 46, tz) });
            //InspItems.Add(new Label3D(검사항목.Dents)    { Name = InspectionName(검사항목.Dents),    Point = new Point3D(130, 34, tz) });
            InspItems.ForEach(e => e.Create(Children));
        }
        #endregion

        public virtual Color GetColor(결과구분 결과) => 결과 == 결과구분.OK ? MajorColors.GoodColor : MajorColors.BadColor;
        public void SetResults(검사결과 결과)
        {
            foreach(Base3D 항목 in InspItems)
            {
                검사정보 정보 = 결과.GetItem(항목.Type);
                if (정보 == null) continue;
                if (항목.Type == 검사항목.QrLegibility) 항목.Draw(Decimal.MinValue, 결과.큐알결과());
                else 항목.Draw(정보.결과값, 정보.측정결과);
            }
        }
    }
}
