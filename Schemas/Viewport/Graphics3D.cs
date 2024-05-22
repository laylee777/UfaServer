using HelixToolkit.Wpf;
using MvUtils;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DSEV.Schemas
{
    public partial class Viewport
    {
        public enum NamePrintType { Left, Right, Up, Down, Center }

        public abstract class Base3D
        {
            public readonly 검사항목 Type = 검사항목.None;
            public virtual String Name { get; set; } = String.Empty;
            public virtual String Label { get; set; } = String.Empty;
            public virtual NamePrintType LabelStyle { get; set; } = NamePrintType.Left;
            public virtual Decimal Value { get; set; } = 0;
            public virtual Color Color { get; set; } = ToColor(환경설정.ResultColor(결과구분.OK));
            public virtual Color BackColor => Color.FromArgb(64, Color.R, Color.G, Color.B);
            public virtual Point3D Point { get; set; } = new Point3D(Double.NaN, Double.NaN, Double.NaN);
            public virtual Boolean HasPoint => !(Double.IsNaN(Point.X) || Double.IsNaN(Point.Y) || Double.IsNaN(Point.Z));

            public Base3D(검사항목 항목) { Type = 항목; }
            public abstract void Create(Visual3DCollection collectioin);
            public abstract void Draw();
            public virtual void Draw(Decimal value, 결과구분 결과)
            {
                Value = value;
                Color = ToColor(환경설정.ResultColor(결과));
                Label = CreateLabelText();
                Draw();
            }
            public virtual String CreateLabelText()
            {
                if (String.IsNullOrEmpty(Name) && Value <= Decimal.MinValue) return String.Empty;
                if (Value <= Decimal.MinValue) return Name;

                String val = Utils.FormatNumeric(Value, Global.환경설정.결과표현);
                if (String.IsNullOrEmpty(Name)) return val;
                if (LabelStyle == NamePrintType.Up || LabelStyle == NamePrintType.Down) return val;
                return $"{Name}: {val}";
            }
        }

        public class Label3D : Base3D
        {
            public virtual Double FontHeight { get; set; } = 14;
            public virtual Point3D Origin { get; set; } = new Point3D(Double.NaN, Double.NaN, Double.NaN);
            public virtual Boolean HasOrigin => !(Double.IsNaN(Origin.X) || Double.IsNaN(Origin.Y) || Double.IsNaN(Origin.Z));
            public TextVisual3D TextName = null;
            public TextVisual3D TextLabel = null;
            public ArrowVisual3D Indicator = null;

            public Label3D(검사항목 항목) : base(항목) { }
            public override void Create(Visual3DCollection collectioin)
            {
                Label = CreateLabelText();
                Point3D p = new Point3D();
                if (HasPoint) p = Point;
                if (HasOrigin && HasPoint)
                {
                    p = Origin;
                    Indicator = CreateArrowLine(Origin, Point, Color);
                    collectioin.Add(Indicator);
                }
                p.Z += 0.6;
                if (Indicator != null)
                {
                    if (LabelStyle == NamePrintType.Up) p.Y += FontHeight * 0.3;
                    else if (LabelStyle == NamePrintType.Down) p.Y -= FontHeight * 0.3;
                }
                TextLabel = Schemas.Viewport.CreateLabel(p, Label, FontHeight, Color);
                collectioin.Add(TextLabel);
                if (!String.IsNullOrEmpty(Name))
                {
                    if (LabelStyle == NamePrintType.Up)
                        TextName = Schemas.Viewport.CreateLabel(new Point3D(p.X, p.Y + FontHeight * 0.6, p.Z + 0.2), Name, FontHeight, Color);
                    else if (LabelStyle == NamePrintType.Down)
                        TextName = Schemas.Viewport.CreateLabel(new Point3D(p.X, p.Y - FontHeight * 0.6, p.Z + 0.2), Name, FontHeight, Color);
                    if (TextName != null) collectioin.Add(TextName);
                    else
                    {
                        if (LabelStyle == NamePrintType.Left) TextLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        else if (LabelStyle == NamePrintType.Right) TextLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    }
                }
            }

            public override void Draw()
            {
                SolidColorBrush brush = new SolidColorBrush(Color);
                TextLabel.Text = Label;
                TextLabel.Foreground = brush;
                if (TextName != null) TextName.Foreground = brush;
                if (Indicator != null) Indicator.Fill = brush;
            }
        }

        public class Length3D : Label3D
        {
            public Point3D PointS { get; set; } = new Point3D();
            public Point3D PointE { get; set; } = new Point3D();
            public Point3D Center => new Point3D((PointS.X + PointE.X) / 2, (PointS.Y + PointE.Y) / 2, (PointS.Z + PointE.Z) / 2);
            public ArrowVisual3D ArrowLine = null;

            public Length3D(검사항목 항목) : base(항목) { }
            public override void Create(Visual3DCollection collectioin)
            {
                if (!HasPoint) Point = new Point3D(PointE.X, PointE.Y, PointE.Z);
                base.Create(collectioin);
                ArrowLine = CreateArrowLine(PointS, PointE, Color);
                collectioin.Add(ArrowLine);
            }

            public override void Draw()
            {
                base.Draw();
                ArrowLine.Fill = new SolidColorBrush(Color);
            }
        }

        public class Width3D : Length3D
        {
            public String LabelS { get; set; } = String.Empty;
            public String LabelE { get; set; } = String.Empty;
            public Double LabelMargin { get; set; } = 16;
            public ArrowVisual3D ArrowLine2 = null;
            public TextVisual3D Text3DS = null;
            public TextVisual3D Text3DE = null;

            public Width3D(검사항목 항목) : base(항목) { }
            public override void Create(Visual3DCollection collectioin)
            {
                base.Create(collectioin);
                ArrowLine2 = CreateArrowLine(Center, PointE, Color);
                collectioin.Add(ArrowLine2);
                if (!String.IsNullOrEmpty(LabelS))
                {
                    Point3D point = new Point3D(PointS.X, PointS.Y, PointS.Z);
                    if (LabelStyle == NamePrintType.Left || LabelStyle == NamePrintType.Right) point.X -= LabelMargin;
                    else point.Y -= LabelMargin;
                    Text3DS = Schemas.Viewport.CreateLabel(point, LabelS, base.FontHeight, Color);
                    collectioin.Add(Text3DS);
                }
                if (!String.IsNullOrEmpty(LabelE))
                {
                    Point3D point = new Point3D(PointE.X, PointE.Y, PointS.Z);
                    if (LabelStyle == NamePrintType.Left || LabelStyle == NamePrintType.Right) point.X += LabelMargin;
                    else point.Y += LabelMargin;
                    Text3DE = Schemas.Viewport.CreateLabel(point, LabelE, base.FontHeight, Color);
                    collectioin.Add(Text3DE);
                }
            }

            public override void Draw()
            {
                base.Draw();
                ArrowLine2.Fill = new SolidColorBrush(Color);
                if (Text3DS != null) Text3DS.Foreground = new SolidColorBrush(Color);
                if (Text3DE != null) Text3DE.Foreground = new SolidColorBrush(Color);
            }
        }

        public class Rectangle3D : Label3D
        {
            public Vector3D Normal { get; set; } = new Vector3D(1, 0, 0);
            public Double Width { get; set; } = 10;
            public Double Height { get; set; } = 10;
            public RectangleVisual3D Rect = null;

            public Rectangle3D(검사항목 항목) : base(항목) { }
            public override void Create(Visual3DCollection collectioin)
            {
                base.Create(collectioin);
                Rect = CreateRectangle(new Point3D(Point.X, Point.Y, Point.Z + 1.0), Width, Height, BackColor, Normal);
                collectioin.Add(Rect);
            }
            public override void Draw()
            {
                base.Draw();
                Rect.Fill = new SolidColorBrush(BackColor);
            }
        }

        public class Circle3D : Label3D
        {
            public Double Radius { get; set; } = 14;
            public PieSliceVisual3D Circle = null;

            public Circle3D(검사항목 항목) : base(항목) { }
            public override void Create(Visual3DCollection collectioin)
            {
                base.Create(collectioin);
                Circle = CreateCircle(new Point3D(Point.X, Point.Y, Point.Z + 1.0), Radius, BackColor);
                collectioin.Add(Circle);
            }
            public override void Draw()
            {
                base.Draw();
                Circle.Fill = new SolidColorBrush(BackColor);
            }
        }
    }
}
