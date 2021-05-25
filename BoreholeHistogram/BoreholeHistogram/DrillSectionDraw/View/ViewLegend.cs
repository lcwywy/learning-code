using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using SmartGeoLogical.Core.DrillSection;
using SmartGeoLogical.Render.DrillSectionDraw.Models;

namespace SmartGeoLogical.Render.DrillSectionDraw.Draw.View
{
    /// <summary>
    /// 图例
    /// </summary>
    class ViewLegend
    {
        private readonly List<int> AllLists;

        private Rectangle polygonsrec;

        private readonly string Name;

        //单个图例的属性
        private float width;
        private float height;
        public float wordwidth;
        DrawScaling drawScaling;

        public ViewLegend(string name, List<int> allists, DrawScaling drawScaling)
        {
            this.Name = name;
            this.AllLists = allists;
            this.width = 10;
            this.height = 5;
            this.wordwidth = 10;
            this.drawScaling = drawScaling;
        }

        public List<PolygonF> GetData()
        {
            //调整图例的大小
            int n = this.AllLists.Count;
            int rows = (n / 10) + 1;
            this.height = 30;
            this.width = 45;
            this.wordwidth = (float)((drawScaling.Width - 9 * this.width - 300) / 8); 

            string layerName = this.Name;

            //设置第一个图例出现的位置
            float x = 150;
            float y = (float)(drawScaling.Height - rows * 75);

            List<PolygonF> temppolygons = this.CreateLegends(x, y);
            return temppolygons;
        }

        private List<PolygonF> CreateLegends(float headx, float heady)
        {
            float x = headx;
            float y = heady;
            int n = this.AllLists.Count;
            int rows = (n / 9) + 1;

            List<PolygonF> Polygons = new List<PolygonF>();
            PolygonF polygon0 = this.CreateLegend(x, y, AllLists[0]);
            Polygons.Add(polygon0);
            bool second = true;
            x += this.width + this.wordwidth;
            for (int i = 0; i < rows; i++)
            {
                if (second)
                {
                    if (i == rows - 1)
                    {
                        int columns = n % 9;
                        for (int j = 1; j < columns ; j++)
                        {
                            PolygonF polygon = this.CreateLegend(x, y, AllLists[i * 9 + j]);
                            Polygons.Add(polygon);
                            x += this.width + this.wordwidth;
                        }
                    }
                    else
                    {
                        for (int j = 1; j < 9; j++)
                        {
                            PolygonF polygon = this.CreateLegend(x, y, AllLists[i * 9 + j]);
                            Polygons.Add(polygon);
                            x += this.width + this.wordwidth;
                        }
                    }
                    second = false;
                }
                else
                if (i == rows - 1)
                {
                    int columns = n % 9;
                    for (int j = 0; j < columns; j++)
                    {
                        PolygonF polygon = this.CreateLegend(x, y, AllLists[i * 9 + j]);
                        Polygons.Add(polygon);
                        x += this.width + this.wordwidth;
                    }
                }
                else
                {
                    for (int j = 0; j < 9; j++)
                    {
                        PolygonF polygon = this.CreateLegend(x, y, AllLists[i * 9 + j]);
                        Polygons.Add(polygon);
                        x += this.width + this.wordwidth;
                    }
                }
                x = 150;
                y += 75;
            }

            return Polygons;
        }

        private PolygonF CreateLegend(float x, float y, int lith)
        {
            List<PointF> Points = new List<PointF>();
            PointF point = new PointF(x, y);
            Points.Add(point);
            PointF point1 = new PointF(x + this.width, y);
            Points.Add(point1);
            PointF point2 = new PointF(x + this.width, y + this.height);
            Points.Add(point2);
            PointF point3 = new PointF(x, y + this.height);
            Points.Add(point3);
            PolygonF polygon = new PolygonF(lith, Points);
            return polygon;
        }
    }
}
