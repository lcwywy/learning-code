using SmartGeoLogical.Core.DrillSection;
using SmartGeoLogical.Core.ViewModels;
using SmartGeoLogical.Render.DrillSectionDraw.Draw;
using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;

namespace SmartGeoLogical.Render.DrillSectionDraw
{
    public  class VirtualDrill
    {
        private List<StratumModel> _stratumModels { set; get; }

        private List<SPolygon> _polygons { set; get; }

        private DrawScaling _drawScaling { set; get; }
        
        Dictionary<int, List<string>> _legends { set; get; }

        public VirtualDrill( List<SPolygon> polygons,DrawScaling drawScaling, Dictionary<int, List<string>> legends)
        {
            _polygons = polygons;
            _drawScaling = drawScaling;
            _legends = legends;
        }
        /// <summary>
        /// 获取虚拟钻孔地层
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<StratumModel> GetVirtualDrill(double distance)
        {
            _stratumModels = new List<StratumModel>();
            StratumModel stratumModel = null;
            SLine line = GetDrillLine(distance);
            LineString lineString = SLineToLineString(line);
            foreach (var item in _polygons)
            {
                Geometry geometry=SPolygonToGeometry(item);
                if (geometry.Intersects(lineString))
                {
                    Geometry interGeo = geometry.Intersection(lineString);
                    MultiPoint multiPoint = interGeo.Boundary as MultiPoint;
                    if (multiPoint.Count==2)
                    {
                        Coordinate coordinate1 = multiPoint.Coordinates[0];
                        Coordinate coordinate2 = multiPoint.Coordinates[1];

                        stratumModel = new StratumModel();
                        _legends.TryGetValue(item.Attitude, out List<string> legend);

                        stratumModel.LithCode = legend[0];
                        stratumModel.ZUp = coordinate1.Y > coordinate2.Y ? coordinate1.Y : coordinate2.Y;
                        stratumModel.ZDown = coordinate1.Y < coordinate2.Y ? coordinate1.Y : coordinate2.Y;
                        stratumModel.Height = Math.Abs(coordinate1.Y - coordinate2.Y);
                        _stratumModels.Add(stratumModel);
                    }
                }
            }
            //排序
            _stratumModels.Sort();
            for (int i = 0; i < _stratumModels.Count; i++)
            {
                _stratumModels[i].LithId = i + 1;
            }
            return _stratumModels;
        }
        /// <summary>
        /// 获取钻孔线
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private SLine GetDrillLine(double distance)
        {
            SPoint statrPoint = new SPoint(distance, _drawScaling.minY);
            SPoint endPoint = new SPoint(distance, _drawScaling.maxY);
            SLine sLine = new SLine(3, statrPoint, endPoint,0);
            return sLine;
        }
        /// <summary>
        /// 获取与钻孔线相交的点
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<SPoint> GetIntersectPoints(SPolygon polygon, SLine line )
        {
            polygon.PointsSorft();
            List<SPoint> list = new List<SPoint>();
            foreach (var point in polygon.Points)
            {
                SLine tempLine = new SLine(polygon.Id, point, point.Next, polygon.Attitude);
                if (tempLine.IntersectPoint(line, out SPoint IntersectPoint))
                {
                    list.Add(IntersectPoint);
                }
            }
            return list;
        }
        private  Geometry SPolygonToGeometry(SPolygon inPolygon)
        {
            try
            {
                inPolygon.PointsSorft();
                Coordinate[] coordinates = new Coordinate[inPolygon.Points.Count + 1];
                Coordinate coordinate = null;
                for (int i = 0; i <= inPolygon.Points.Count; i++)
                {
                    if (i< inPolygon.Points.Count)
                    {
                        SPoint point = inPolygon.Points[i];
                        coordinate = new Coordinate(point.X, point.Y);
                        coordinates[i] = coordinate;
                    }
                    else
                    {
                        SPoint point = inPolygon.Points[0];
                        coordinate = new Coordinate(point.X, point.Y);
                        coordinates[i] = coordinate;
                    }
                }
                LinearRing linearRing = new LinearRing(coordinates);
                Polygon outPolygoin = new Polygon(linearRing);
            
                return outPolygoin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
        }
        private LineString SLineToLineString(SLine inLine)
        {
            Coordinate[] coordinates = new Coordinate[2] 
            {
                new Coordinate(inLine.Startpnt.X, inLine.Startpnt.Y),
                new Coordinate(inLine.Endpnt.X, inLine.Endpnt.Y) 
            };
            LineString lineString = new LineString(coordinates);
            return lineString;
        }
   



    }
}
