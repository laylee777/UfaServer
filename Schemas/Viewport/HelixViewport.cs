using HelixToolkit.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DSEV.Schemas
{
    public partial class Viewport : HelixViewport3D
    {
        #region 기본설정
        public Viewport() { }

        internal ModelVisual3D ModelVisual = new ModelVisual3D();
        internal Model3DGroup ModelGroup = new Model3DGroup();
        internal GeometryModel3D MainModel = null;
        public virtual String StlPath => String.Empty;
        public virtual String StlFile => String.Empty;
        public virtual Double Scale => 1;
        public virtual Material FrontMaterial => MaterialHelper.CreateMaterial(Colors.LightGray, 0.8);
        public virtual Material BackMaterial => MaterialHelper.CreateMaterial(Colors.Silver, 0.8);
        public virtual Material GridMaterial => MaterialHelper.CreateMaterial(Colors.LightGray, 0.5);
        public virtual Material BlackMaterial => MaterialHelper.CreateMaterial(Colors.Black, 0.5);
        public virtual Material RedMaterial => MaterialHelper.CreateMaterial(Colors.Red, 0.8);

        public virtual Point3D CameraPosition { get; set; } = default(Point3D);
        public virtual Vector3D CameraLookDirection { get; set; } = default(Vector3D);
        public virtual Vector3D CameraUpDirection { get; set; } = default(Vector3D);

        public Boolean Init(out String error)
        {
            ModelGroup.SetName(nameof(ModelGroup));
            PanGesture.MouseAction = MouseAction.LeftClick;
            PanGesture2.MouseAction = MouseAction.LeftClick;
            MouseDoubleClick += ViewportMouseDoubleClick;
            //CameraChanged += ViewportCameraChanged;

            error = String.Empty;
            try { LoadStl(); }
            catch (Exception ex)
            {
                error = ex.Message;
                Debug.WriteLine(ex.Message);
            }

            ModelVisual.Content = ModelGroup;
            Children.Add(new DefaultLights());
            Children.Add(ModelVisual);
            InitModel();
            InitCamera();
            return String.IsNullOrEmpty(error);
        }
        internal virtual void LoadStl()
        {
            if (!File.Exists(StlFile)) return;
            StLReader reader = new StLReader();
            Model3DGroup groups = reader.Read(StlFile);
            MainModel = groups.Children[0] as GeometryModel3D;

            Point3D p = Center3D();
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(p.X * Scale, p.Y * Scale, 0 * Scale));
            transform.Children.Add(new ScaleTransform3D(Scale, Scale, Scale));
            MainModel.Transform = transform;
            //Debug.WriteLine(p.ToString(), "p");
            //Debug.WriteLine(MainModel.Transform.Value.ToString(), "Transform");

            MainModel.SetName(nameof(MainModel));
            MainModel.Material = FrontMaterial;
            MainModel.BackMaterial = BackMaterial;
            ModelGroup.Children.Add(MainModel);
        }
        internal virtual void ViewportMouseDoubleClick(object sender, MouseButtonEventArgs e) => InitCamera();

        public ElementHost CreateHost() =>
            new ElementHost() { Child = this, Dock = System.Windows.Forms.DockStyle.Fill };
        public virtual void Refresh() => InvalidateVisual();

        public virtual Point3D Center3D()
        {
            if (MainModel == null) return new Point3D(0, 0, 0);
            Rect3D rect = MainModel.Bounds;
            Debug.WriteLine($"{rect.X + rect.SizeX / 2},{rect.Y + rect.SizeY / 2}, {rect.Z + rect.SizeZ / 2}");
            return new Point3D(rect.X + rect.SizeX / 2, rect.Y + rect.SizeY / 2, rect.Z + rect.SizeZ / 2);
        }

        internal virtual void InitModel() { }
        internal void InitCamera()
        {
            if (CameraPosition != default(Point3D)) Camera.Position = CameraPosition;
            if (CameraLookDirection != default(Vector3D)) Camera.LookDirection = CameraLookDirection;
            if (CameraUpDirection != default(Vector3D)) Camera.UpDirection = CameraUpDirection;
            CameraRotationMode = CameraRotationMode.Trackball;
        }
        #endregion

        #region 이벤트
        internal virtual void ViewportCameraChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Camera is PerspectiveCamera cam)
            {
                Debug.WriteLine(Center3D().ToString(), "Center");
                DebugPoint(cam.Position, "Position");
                DebugVector(cam.LookDirection, "LookDirection");
                DebugVector(cam.UpDirection, "UpDirection");
            }
        }
        private void DebugPoint(Point3D p, String name) =>
            Debug.WriteLine($"Camera.{name} = new Point3D({Math.Round(p.X, 3)}, {Math.Round(p.Y, 3)}, {Math.Round(p.Z)});");
        private void DebugVector(Vector3D p, String name) =>
            Debug.WriteLine($"Camera.{name} = new Vector3D({Math.Round(p.X, 3)}, {Math.Round(p.Y, 3)}, {Math.Round(p.Z)});");
        #endregion

        internal virtual void AddArrowLine(Point3D s, Point3D e, Color color)
        {
            Point3D center = new Point3D((s.X + e.X) / 2, (s.Y + e.Y) / 2, (s.Z + e.Z) / 2);
            Children.Add(CreateArrowLine(center, s, color));
            Children.Add(CreateArrowLine(center, e, color));
        }
        internal virtual void AddStaticLine(Point3D s, Point3D e, Color color) =>
            Children.Add(new LinesVisual3D { Points = new Point3DCollection { s, e }, Color = color });
        internal virtual void AddText3D(Point3D point, String text, Double size, Color color) =>
            Children.Add(CreateText3D(point, text, size, color));
        internal virtual void AddLabel(Point3D point, String text, Double size, Color color) =>
            Children.Add(CreateLabel(point, text, size, color));

        public static Color ToColor(System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);

        public static BillboardTextVisual3D CreateText3D(Point3D point, String text, Double size, Color color) =>
            new BillboardTextVisual3D()
            {
                Text = text,
                Position = point,
                FontSize = size,
                Padding = new System.Windows.Thickness(4, 2, 4, 2),
                Foreground = new SolidColorBrush(color),
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };

        public static TextVisual3D CreateLabel(Point3D point, String text, Double height, Color color) =>
            new TextVisual3D()
            {
                Text = text,
                Position = point,
                Height = height,
                TextDirection = new Vector3D(1, 0, 0),
                UpDirection = new Vector3D(0, 1, 0),
                FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(600),
                Padding = new System.Windows.Thickness(4, 0, 4, 2),
                Foreground = new SolidColorBrush(color),
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };

        public static ArrowVisual3D CreateArrowLine(Point3D s, Point3D e, Color color) =>
            new ArrowVisual3D() { Point1 = s, Point2 = e, Diameter = 1, HeadLength = 4, Fill = new SolidColorBrush(color) };

        public static RectangleVisual3D CreateRectangle(Point3D prigin, Double width, Double height, Color color, Vector3D normal) =>
            new RectangleVisual3D() { Origin = prigin, Width = width, Length = height, Normal = normal, Fill = new SolidColorBrush(color) };

        public static PieSliceVisual3D CreateCircle(Point3D center, Double radius, Color color) =>
            new PieSliceVisual3D() { Center = center, InnerRadius = 0, OuterRadius = radius, Fill = new SolidColorBrush(color), StartAngle = 0, EndAngle = 360, UpVector = new Vector3D(0, 1, 0) };
    }

    public static class MajorColors
    {
        public static Color FrameColor => Colors.Yellow;
        public static Color StaticColor => Colors.Cyan;
        public static Color GoodColor => Colors.Green;
        public static Color BadColor => Colors.Red;
        public static Color WarningColor => Colors.Magenta;
    }
}
