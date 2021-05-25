using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SmartGeoLogical.Core.DrillSection;

namespace SmartGeoLogical.Render.DrillSectionDraw.Draw
{
    public class DrawHelper
    {
        DrawScaling drawScaling;
        public DrawHelper(DrawScaling drawScaling)
        {
            this.drawScaling = drawScaling;
        }
        /// <summary>
        /// 绘制比例尺刻度文字
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="color"></param>
        /// <param name="fontD"></param>
        /// <param name="format"></param>
        public void DrawString(Graphics graphics, string text,SPoint startPoint, SPoint endPoint, Color color, Font fontD, StringFormat format)
        {
            PointF start =  GetPointF(startPoint.X, startPoint.Y);
            PointF point ;
            if (startPoint.X> endPoint.X)
            {
                point = start;
                point.X -= 5;
                format.Alignment = StringAlignment.Far;
            }
            else
            {
                point = start;
                point.X += 5;
                format.Alignment = StringAlignment.Near;
            }
            graphics.DrawString(text, fontD, new SolidBrush(color), point, format);
        }
        public  void DrawLine(Graphics graphics, SPoint startPoint, SPoint endPoint, Color color )
        {
            graphics.DrawLine(new Pen(color), GetPointF(startPoint.X, startPoint.Y), GetPointF(endPoint.X, endPoint.Y));
        }
        public void DrawLines(Graphics graphics, List<SPoint> points, Color color)
        {
            PointF[] pointFs = new PointF[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                PointF point = GetPointF(points[i].X, points[i].Y);
                pointFs[i] = point;
            }

            graphics.DrawLines(new Pen(color), pointFs);
        }
        /// <summary>
        /// 绘制不规则多变形
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="sPoints"></param>
        /// <param name="imagePath"></param>
        public void DrawFillPolygon(Graphics graphics, List<SPoint> sPoints, string imagePath)
        {
            using Image image = Image.FromFile(imagePath);
            PointF[] pointFs = new PointF[sPoints.Count];
            for (int i = 0; i < sPoints.Count; i++)
            {
                pointFs[i] = GetPointF(sPoints[i].X, sPoints[i].Y);
            }
            Size size = new Size(50, 30);
            using Bitmap bitmap = new Bitmap(image, size);
            using Brush brush =new  TextureBrush(bitmap);
            graphics.FillPolygon(brush, pointFs, 0);
            Pen pen = new Pen(Color.Black);
            graphics.DrawPolygon(pen, pointFs);
        }
        /// <summary>
        /// 绘制矩形的图例
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="sPoints"></param>
        /// <param name="imagePath"></param>
        public void DrawFillRectangleF(Graphics graphics, List<PointF> sPoints, string imagePath)
        {
            using  Image image = Image.FromFile(imagePath);
            PointF[] tfArray = sPoints.ToArray();//new PointF[sPoints.Count];
            //for (int i = 0; i < sPoints.Count; i++)
            //{
            //    tfArray[i] = this.GetPointF(sPoints[i].X, sPoints[i].Y);
            //}
            Size size = new Size(50, 30);
            if (size.Height == 0)
            {
                size.Height = 1;
            }
            using Bitmap bitmap = new Bitmap(image, size);
            
            RectangleF rectangleF = RectangleF.FromLTRB(tfArray[0].X, tfArray[0].Y, tfArray[2].X, tfArray[2].Y);
            using (new TextureBrush(bitmap, 0))
            {
                graphics.DrawImage(bitmap, rectangleF);
                Pen pen = new Pen(Color.Black);
                graphics.DrawPolygon( pen, tfArray);
            }
        }

        public PointF GetPointF(double x, double y)
        {
            drawScaling.ConvertXY(ref x, ref y);
            return new PointF((float)x, (float)y);
        }


        public Color GetRGBLith(string lith)
        {
            Color color;
            switch (lith)
            {
                case "1":
                    color = Color.FromArgb(255, 0, 0);
                    break;
                case "2":
                    color = Color.FromArgb(0, 255, 0);
                    break;
                case "3":
                    color = Color.FromArgb(0, 0, 255);
                    break;
                case "4":
                    color = Color.FromArgb(255, 255, 0);
                    break;
                case "5":
                    color = Color.FromArgb(255, 0, 255);
                    break;
                case "6":
                    color = Color.FromArgb(200, 10, 100);
                    break;
                case "7":
                    color = Color.FromArgb(25, 0, 100);
                    break;
                default:
                    color = Color.FromArgb(10, 200, 200);
                    break;
            }
            return color;
        }

    }
}
