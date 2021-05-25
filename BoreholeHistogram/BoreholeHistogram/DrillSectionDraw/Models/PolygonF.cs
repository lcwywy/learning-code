using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SmartGeoLogical.Render.DrillSectionDraw.Models
{
    public class PolygonF
    {
        public readonly int Id;
        public readonly List<PointF> Points;
        public PolygonF(int id, List<PointF> points)
        {
            this.Id = id;
            this.Points = points;
        }
    }
}
