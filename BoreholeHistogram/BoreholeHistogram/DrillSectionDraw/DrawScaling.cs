using SmartGeoLogical.Core.DrillSection;
using SmartGeoLogical.Render.DrillSectionDraw.Draw.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.DrillSectionDraw.Draw
{

    public class DrawScaling
    {
        /// <summary>
        /// X缩放
        /// </summary>
        public double ScalingX { get; private set; }
        /// <summary>
        /// Y缩放
        /// </summary>
        public double ScalingY { get; private set; }
        /// <summary>
        /// X平移
        /// </summary>
        public double DisplacementX { get; private set; }
        /// <summary>
        /// Y平移
        /// </summary>
        public double DisplacementY { get; private set; }
        /// <summary>
        /// 画布宽
        /// </summary>
        public float Width { get; private set; }
        /// <summary>
        /// 画布高
        /// </summary>
        public float Height { get; private set; }
        /// <summary>
        /// 岩性
        /// </summary>
        public string Texture { get; private set; }


        public double XYPercent { get; private set; }

        public double compensateX { get; private set; } = 1;
        public double minX = 9999999;
        public double minY = 9999999;
        public double maxX = 0;
        public double maxY = 0;
        /// <summary>
        /// 计算位移和缩放，先计算缩放，再计算位移
        /// </summary>
        /// <param name="heigth"></param>
        /// <param name="sPolygons"></param>
        /// <param name="percent"></param>
        public void CalculateParm(int heigth, List<SPolygon> sPolygons, double percent, string texture, int legendCount)
        {
            Height = heigth;
            XYPercent = percent;
            Texture = texture;

            sPolygons.ForEach(delegate (SPolygon sPolygon)
            {
                sPolygon.Points.ForEach(delegate (SPoint sPoint)
                {
                    minX = sPoint.X < minX ? sPoint.X : minX;
                    minY = sPoint.Y < minY ? sPoint.Y : minY;
                    maxX = sPoint.X > maxX ? sPoint.X : maxX;
                    maxY = sPoint.Y > maxY ? sPoint.Y : maxY;
                });
            });

            ScaleSection.PolygonRectangle.Bottom = minY;
            ScaleSection.PolygonRectangle.Top = maxY;
            ScaleSection.PolygonRectangle.Left = minX;
            ScaleSection.PolygonRectangle.Right = maxX;

            float distanceX = (float)(maxX - minX);
            float distanceY = (float)(maxY - minY);
            //最小刻度
            double min;
            if (ScaleSection.PolygonRectangle.Bottom >= 0)
                min = (int)(ScaleSection.PolygonRectangle.Bottom % 5 - 5 + ScaleSection.PolygonRectangle.Bottom);
            else
                min = (int)(-5 - ScaleSection.PolygonRectangle.Bottom % 5 + ScaleSection.PolygonRectangle.Bottom);
            //最大刻度
            double max = (int)(5 - ScaleSection.PolygonRectangle.Top % 5 + ScaleSection.PolygonRectangle.Top);

            int rows = (legendCount / 9) + 1;
            ScalingY = (max - min) / (heigth - 140 - rows * 75 - 10);
            ScalingX = ScalingY * percent;
            Width = (float)(distanceX / ScalingX);

            if (heigth > Width)
            {
                compensateX = Width / heigth;
                XYPercent = compensateX * percent;
                ScalingX = XYPercent * ScalingY;
                Width = (float)(distanceX / ScalingX);
            }



            Width += 300;
            DisplacementX = 150;
            DisplacementY = -min / ScalingY + rows * 75 + 10;
        }
        public void ConvertXY(ref double x, ref double y)
        {
            if (x >= 0)
            {
                x = x / ScalingX + DisplacementX;
            }
            else
            {
                x = -x;
            }
            y = y / ScalingY + DisplacementY;
            y = Height - y;
        }


    }
}
