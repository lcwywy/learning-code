
using SmartGeoLogical.Core.DrillSection;
using SmartGeoLogical.Core.Entities;
using SmartGeoLogical.Core.Helpers;
using SmartGeoLogical.Core.ViewModels;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrillSectionDraw.Draw.View;
using SmartGeoLogical.Render.DrillSectionDraw.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.DrillSectionDraw.Draw
{
    public class ProfileControl
    {

        public LogicSection logicSection;
        DrawHelper drawHelper;
        List<DrillModel> drillList;
        ProfileParameter parameter;
        public Dictionary<int, List<string>> legends;
        Color color = Color.Black;
        public DrawScaling drawScaling;

        public ProfileControl(LogicSection logicSection, List<DrillModel> drillList, ProfileParameter parameter, DrawScaling drawScaling)
        {
            this.logicSection = logicSection;
            this.drillList = drillList;
            this.parameter = parameter;
            this.drawScaling = drawScaling;
            this.drawHelper = new DrawHelper(drawScaling);
            legends = GetLegendMessage();
        }
        /// <summary>
        /// 绘制钻孔剖面图
        /// 绘制原则：按照先画面再划线。
        /// </summary>
        /// <param name="graphics"></param>
        public Task Draw(Graphics graphics)
        {
            try
            {
                return Task.Run(() =>
                {
                    DrawLayer(graphics);
                    DrawScale(graphics, true);
                    DrawScale(graphics, false);
                    DrawDrill(graphics);
                    DrawLegend(graphics);
                    DrawHead(graphics);
                    DrawDirectionRight(graphics);
                    DrawDirectionLeft(graphics);
                    DrawDemLine(graphics);
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 绘制钻孔线
        /// </summary>
        /// <param name="graphics"></param>
        public void DrawDrill(Graphics graphics)
        {
            foreach (LogicBore logicBore in logicSection.Bores)
            {
                foreach (SLine line in logicBore.Borelinepoints.mlines)
                {
                    drawHelper.DrawLine(graphics, line.Startpnt, line.Endpnt, color);
                }
                SPoint point = logicBore.Borelinepoints.mpoints[0];
                string height = Math.Round(point.Y, 2).ToString();

                Font fontD = new Font("宋体", 12);
                SizeF sizeFHeight = graphics.MeasureString(height, fontD);
                Font drillFont = new Font("宋体", 12, FontStyle.Underline);

                string drillId = GetDrillId(logicBore.BoreID);
                SizeF sizeFDrillCode = graphics.MeasureString(drillId, drillFont);
                PointF pointTemp = drawHelper.GetPointF(point.X, point.Y);
                PointF pointHeight = new PointF(pointTemp.X, pointTemp.Y - sizeFHeight.Height / 2 - 5);
                PointF pointDrill = new PointF(pointTemp.X, pointTemp.Y - sizeFHeight.Height - sizeFDrillCode.Height / 2 - 5);

                //文字位置
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                //绘制钻孔ID和钻孔高度
                graphics.DrawString(height, fontD, new SolidBrush(color), pointHeight, format);
                graphics.DrawString(drillId, drillFont, new SolidBrush(color), pointDrill, format);

            }
        }

        public void DrawDemLine(Graphics graphics)
        {
            if (parameter.DrillLineElevation != null)
            {
                //绘制线
                List<SPoint> points = new List<SPoint>();

                for (int i = 0; i < logicSection.Bores.Count - 1; i++)
                {
                    LogicBore BoreLeft = logicSection.Bores[i];
                    LogicBore BoreRight = logicSection.Bores[i + 1];

                    //高程值
                    List<double> elevations = parameter.DrillLineElevation[BoreLeft.BoreID];
                    int prevelevation = (int)BoreLeft.Y;
                    for (double j = 0; j < elevations.Count; j++)
                    {
                        SPoint point = null;
                        if (j == 0)
                        {
                            point = new SPoint(BoreLeft.X, BoreLeft.Y);
                        }
                        else if (j == elevations.Count - 1)
                        {
                            point = new SPoint(BoreRight.X, BoreRight.Y);
                        }
                        else
                        {
                            point = new SPoint(BoreLeft.X + j / elevations.Count * (BoreRight.X - BoreLeft.X), elevations[(int)j]);
                        }
                        points.Add(point);
                    }
                }

                drawHelper.DrawLines(graphics, points,  Color.Green);
                #region 画面的
                //for (int i = 0; i < logicSection.Bores.Count - 1; i++)
                //{
                //    LogicBore BoreLeft = logicSection.Bores[i];
                //    LogicBore BoreRight = logicSection.Bores[i + 1];

                //    //高程值
                //    List<double> elevations = parameter.DrillLineElevation[BoreLeft.BoreID];
                //    SPoint point1=null, point2=null, point3=null, point4=null;
                //    int prevelevation = (int)BoreLeft.Y;
                //    for (double j = 1; j < elevations.Count; j++)
                //    {
                //        double gap = elevations[(int)j] - prevelevation;
                //        if (j==1)
                //        {
                //            point1 = new SPoint(BoreLeft.X, BoreLeft.Y);
                //            point2 = null;

                //            if (gap>0)
                //            {
                //                point3 = new SPoint(BoreLeft.X + j / elevations.Count * (BoreRight.X - BoreLeft.X), elevations[(int)j]);

                //                point4 = new SPoint(BoreLeft.X + j / elevations.Count * (BoreRight.X - BoreLeft.X),
                //                    BoreLeft.Y + j / elevations.Count * (BoreRight.Y - BoreLeft.Y));
                //                List<SPoint> points = new List<SPoint>();
                //                points.Add(point1);
                //                points.Add(point3);
                //                points.Add(point4);

                //                SPolygon polygon = new SPolygon(0,9999, points);
                //                foreach (var item in MSPolygons)
                //                {
                //                    var unionPolygon = ntsHelper.UnionPolygon(item, polygon);
                //                    if (unionPolygon!=null)
                //                    {
                //                        polygon = new SPolygon(unionPolygon.Id, unionPolygon.Attitude, points);
                //                        demPolygons.Add(polygon);
                //                        break;
                //                    }
                //                }

                //            }

                //        }
                //        else if (j == elevations.Count-1)
                //        {

                //        }
                //        else
                //        {
                //            if (point4!=null)
                //            {
                //                point1 = point4;

                //            }
                //            if (point3 != null)
                //            {
                //                point2 = point3;

                //            }

                //            if (gap > 0)
                //            {
                //                point3 = new SPoint(BoreLeft.X + j / elevations.Count * (BoreRight.X - BoreLeft.X), elevations[(int)j]);

                //                point4 = new SPoint(BoreLeft.X + j / elevations.Count * (BoreRight.X - BoreLeft.X),
                //                               BoreLeft.Y + j / elevations.Count * (BoreRight.Y - BoreLeft.Y));


                //                List<SPoint> points = new List<SPoint>();
                //                points.Add(point1);
                //                if (point2!=null)
                //                {
                //                    points.Add(point2);
                //                }
                //                points.Add(point3);
                //                points.Add(point4);

                //                SPolygon polygon = new SPolygon(0, 9999, points);
                //                foreach (var item in MSPolygons)
                //                {
                //                    var unionPolygon = ntsHelper.UnionPolygon(item, polygon);
                //                    if (unionPolygon != null)
                //                    {
                //                        polygon = new SPolygon(unionPolygon.Id, unionPolygon.Attitude, points);
                //                        demPolygons.Add(polygon);
                //                        break;

                //                    }
                //                }

                //            }

                //        }
                //    }

                //}
                #endregion
            }
        }
        /// <summary>
        /// 绘制图层
        /// </summary>
        /// <param name="graphics"></param>
        public void DrawLayer(Graphics graphics)
        {
            try
            {
                NTSHelper ntsHelper = new NTSHelper();

                List<SPolygon> MSPolygons = logicSection.SectionPloygons;
                List<int> pIDList = new List<int>();
                for (int i = 0; i < MSPolygons.Count; i++)
                {
                    if (pIDList.Contains(MSPolygons[i].Attitude) == false)
                    {
                        pIDList.Add(MSPolygons[i].Attitude);
                    }
                }
                List<SPolygon> demPolygons = new List<SPolygon>();
                //增加地势起伏的面
             
                MSPolygons.AddRange(demPolygons);
                //绘制面
                for (int i = 0; i < pIDList.Count; i++)
                {
                    SPolygon lastPolygon = null;
                   
                    for (int j = 0; j < MSPolygons.Count; j++)
                    {
                  
                        if (pIDList[i] == MSPolygons[j].Attitude)
                        {
                            if (lastPolygon == null)
                            {
                                lastPolygon = MSPolygons[j];
                            }
                            else                                             
                            {
                                SPolygon unionPolygon = null;
                                if (lastPolygon.Points.Count>0)
                                {
                                    unionPolygon = ntsHelper.UnionPolygon(lastPolygon, MSPolygons[j]);
                                }
                                if (unionPolygon==null)
                                {
                                    if (lastPolygon.Points.Count>0)
                                    {
                                        DrawPolgon(graphics, pIDList[i], lastPolygon);
                                    }
                                    lastPolygon = MSPolygons[j];
                                }
                                else
                                {
                                    lastPolygon = unionPolygon;
                                }
                               //  lastPolygon.Merge(MSPolygons[j], out int intersectCount);
                               //// if (intersectCount < 2&& lastPolygon.Points.Count>0)
                               // {
                               //     DrawPolgon(graphics, pIDList[i], unionPolygon);
                               //     lastPolygon = MSPolygons[j];
                               // }
                            }
                        }
                    }
                    if (lastPolygon.Points.Count>0)
                    {
                        DrawPolgon(graphics, pIDList[i], lastPolygon);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 按照点集绘制面
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="id"></param>
        /// <param name="lastPolygon"></param>
        private void DrawPolgon(Graphics graphics, int id, SPolygon lastPolygon)
        {
            legends.TryGetValue(id, out List<string> legend);

            string legendPath = GetTexturePath(legend);

            drawHelper.DrawFillPolygon(graphics, lastPolygon.Points, legendPath);
        }

        private string GetTexturePath(List<string> legend)
        {
            var legendPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrillSet", "Legend", drawScaling.Texture, legend[0]);
            if (System.IO.File.Exists(legendPath + ".jpg"))
            {
                legendPath += ".jpg";
            }
            else if (System.IO.File.Exists(legendPath + ".png"))
            {
                legendPath += ".png";
            }

            return legendPath;
        }

        /// <summary>
        /// 绘制图例
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawLegend(Graphics graphics)
        {
            //获取图例的位置
            ViewLegend viewLegend = new ViewLegend("", logicSection.AllList, drawScaling);
            List<PolygonF> LegendPolygons = viewLegend.GetData();

            //绘制图例
            foreach (PolygonF sPolygon in LegendPolygons)
            {
                //获取对应的图例编号和图例信息，并用图例（图片）填充；
                legends.TryGetValue(sPolygon.Id, out List<string> legend);
                var legendPath  = GetTexturePath(legend);

                drawHelper.DrawFillRectangleF(graphics, sPolygon.Points, legendPath);

                //描写图例的图层信息
                Font fontD = new Font("宋体", 11);
                Brush brush = new SolidBrush(color);
                PointF pointF0 = sPolygon.Points[0];
                PointF pointF2 = sPolygon.Points[2];
                RectangleF rectangleF = new RectangleF(pointF0.X - viewLegend.wordwidth/2, pointF2.Y, pointF2.X - pointF0.X + viewLegend.wordwidth, pointF2.Y - pointF0.Y+10);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                graphics.DrawString(string.Join("", legend.ToArray()), fontD, brush, rectangleF, stringFormat);
            }
        }
        /// <summary>
        /// 绘制比例尺的刻度
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="left"></param>
        private void DrawScale(Graphics graphics, bool left)
        {
            ViewScale viewScale = new ViewScale(drawScaling, "LefeScale", left ? logicSection.LeftScale : logicSection.RightScale, logicSection.Bores, 0, 0, true);
            viewScale.GetGraduations(left);
            foreach (VLine vLine in viewScale.mGraduations)
            {
                SPoint startPoint = GetPoint(vLine.StartP);
                SPoint endPoint = GetPoint(vLine.EndP);

                drawHelper.DrawLine(graphics, startPoint, endPoint, color);
                if (startPoint.Y == endPoint.Y)
                {
                    Font fontD = new Font("宋体", 10);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    drawHelper.DrawString(graphics, vLine.attribute, startPoint, endPoint, color, fontD, format);
                }
            }
            //绘制数轴刻度线
            VPoint vPoint1, vPoint2;
            vPoint1 = viewScale.mGraduations[0].StartP;
            vPoint2 = viewScale.mGraduations[viewScale.mGraduations.Count - 1].StartP;
            drawHelper.DrawLine(graphics, GetPoint(vPoint1), GetPoint(vPoint2), color);
        }
        private SPoint GetPoint(VPoint vPoint)
        {
            return new SPoint(0, vPoint.X, vPoint.Y);
        }
        /// <summary>
        /// 绘制钻孔剖面图标题
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawHead(Graphics graphics)
        {
            View.Rectangle polygonsrec = ScaleSection.PolygonRectangle;
            string text = $"{GetDrillId(logicSection.MapHead[0])}-{GetDrillId(logicSection.MapHead[logicSection.MapHead.Count - 1])}钻孔对比剖面图";

            //计算位置
            Font fontD = new Font("宋体", 25);
            SizeF sizeF = graphics.MeasureString(text, fontD);
            float x = (float)(drawScaling.Width / 2);//(polygonsrec.Right + polygonsrec.Left) / 2;
            float y = 0;//polygonsrec.Top + (polygonsrec.Top - polygonsrec.Bottom) / 3;
            PointF pointF = new PointF(x, y);
            pointF.X = pointF.X;
            pointF.Y = pointF.Y + sizeF.Height / 2;

            //绘制标题
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            graphics.DrawString(text, fontD, new SolidBrush(color), pointF, format);

            //绘制缩放比例
            DrawScale(graphics, pointF, sizeF);
        }
        private string GetDrillId(string id)
        {
            DrillModel drill = drillList.Find(r => r.Id == id);
            drill.Extensions.TryGetValue("drillid", out string drillid);
            return drillid;
        }
        /// <summary>
        /// 绘制比例尺
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawScale(Graphics graphics, PointF pointF, SizeF sizeF)
        {
            string vertical = $"水平比例 1：{parameter.ScaleX}   垂直比例 1：{parameter.ScaleY} ";
            double x = pointF.X;
            double y1 = pointF.Y + sizeF.Height * 5 / 4;

            Font fontD = new Font("宋体", 13);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            graphics.DrawString(vertical, fontD, new SolidBrush(color), new PointF((float)x, (float)y1), format);
        }
        /// <summary>
        /// 绘制剖面走势
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawDirectionRight(Graphics graphics)
        {
            DirectionType directionType = GetDirection(false);

            //计算起始位置
            View.Rectangle allRectangle = ScaleSection.PolygonRectangle;
            float x = (drawScaling.Width / 2)+225;//(allRectangle.Right + allRectangle.Left) / 2 + (allRectangle.Right - allRectangle.Left) / 5 * 2; 
            float y = 95;//allRectangle.Top + (allRectangle.Top - allRectangle.Bottom) / 6;

            float distanceX = drawScaling.Width / 13;
            PointF pointStart = new PointF(x, y);
            PointF pointEnd = new PointF(pointStart.X + distanceX, y);

            //计算箭头长度
            float height = drawScaling.Height / 40;//(allRectangle.Top - allRectangle.Bottom) / 20;
            float length = distanceX / 9;
            PointF pointOver = new PointF(pointEnd.X - length, y - height);

            //构造箭头
            graphics.DrawLine(new Pen(color), pointStart, pointEnd);
            graphics.DrawLine(new Pen(color), pointOver, pointEnd);

            //描述箭头指向
            Font fontD = new Font("宋体", 15);
            string text = directionType.ToString();
            SizeF sizeF = graphics.MeasureString(text, fontD);

            PointF pointF = new PointF(x - sizeF.Width, y);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            graphics.DrawString(text, fontD, new SolidBrush(color), pointF, format);
        }
        private void DrawDirectionLeft(Graphics graphics)
        {
            DirectionType directionType = GetDirection(true);

            //计算起始位置
            View.Rectangle allRectangle = ScaleSection.PolygonRectangle;
            float x = (drawScaling.Width / 2) - 225;//(allRectangle.Right + allRectangle.Left) / 2 - (allRectangle.Right - allRectangle.Left) / 5 * 2;
            float y = 95;//allRectangle.Top + (allRectangle.Top - allRectangle.Bottom) / 6;

            float distanceX = drawScaling.Width / 13;
            PointF pointStart = new PointF(x, y);
            PointF pointEnd = new PointF(pointStart.X - distanceX, y);

            //计算箭头长度
            float height = drawScaling.Height / 40;//(allRectangle.Top - allRectangle.Bottom) / 20;
            float length = distanceX / 9;//(pointStart.X - pointEnd.X) / 5;
            PointF pointOver = new PointF(pointEnd.X + length, y - height);

            graphics.DrawLine(new Pen(color), pointStart, pointEnd);
            graphics.DrawLine(new Pen(color), pointOver, pointEnd);

            //描述箭头指向
            Font fontD = new Font("宋体", 15);
            string text = directionType.ToString();
            SizeF sizeF = graphics.MeasureString(text, fontD);

            PointF pointF =new PointF( x + sizeF.Width, y);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            graphics.DrawString(text, fontD, new SolidBrush(color), pointF, format);
        }
        /// <summary>
        /// 获取图例信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, List<string>> GetLegendMessage()
        {
            Dictionary<int, List<string>> legends = new Dictionary<int, List<string>>();
            if (drawScaling.Texture=="南京")
            {
                drillList.ForEach(delegate (DrillModel drill)
                {
                    foreach (var item in drill.StratumList)
                    {
                        int lith = InitSection.StratumLith(item.LithCode);
                        if (!legends.Keys.Contains(lith))
                        {
                            var code = GetLithCode(lith);
                            if (!string.IsNullOrWhiteSpace(code))
                                legends.Add(lith, new List<string>() { code, item.LithDescribe });
                        }
                    }
                });
            }
            if (drawScaling.Texture == "郑州")
            {
                foreach (var drill in drillList)
                {
                    foreach (var item in drill.StratumList)
                    {
                        int lith = InitSection.StratumLith(item.LithCode);
                        if (!legends.Keys.Contains(lith))
                        {
                            legends.Add(lith, new List<string>() { lith.ToString(), item.LithDescribe });
                        }
                    }
                }
                
              

            }


            return legends;
        }

        private string GetLithCode(int lith)
        {
            switch (lith)
            {
                case 1:
                    return "\x2460";
                case 2:
                    return "\x2461";
                case 3:
                    return "\x2462";
                case 4:
                    return "\x2463";
                case 5:
                    return "\x2464";
                case 6:
                    return "\x2465";
                case 7:
                    return "\x2466";
                case 8:
                    return "\x2467";
                case 9:
                    return "\x2468";
                default:
                    return "";
            }
        }

        private DirectionType GetDirection(bool isLeft)
        {
            SPoint point1;
            SPoint point2;
            if (isLeft)
            {
                point1 = new SPoint(0, drillList[drillList.Count - 1].X, drillList[drillList.Count - 1].Y);
                point2 = new SPoint(0, drillList[0].X, drillList[0].Y);
            }
            else
            {
                point1 = new SPoint(0, drillList[0].X, drillList[0].Y);
                point2 = new SPoint(0, drillList[drillList.Count - 1].X, drillList[drillList.Count - 1].Y);
            }


            double angle = Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            double theta = 180 / Math.PI * angle;

            if (theta < 0)
            {
                theta += 360;
            }
            if (theta == 0)
            {
                return DirectionType.正东;
            }
            else if (theta < 90)
            {
                return DirectionType.东北;

            }
            else if (theta == 90)
            {
                return DirectionType.正北;
            }
            else if (theta < 180)
            {
                return DirectionType.西北;
            }
            else if (theta == 180)
            {
                return DirectionType.正西;
            }
            else if (theta < 270)
            {
                return DirectionType.西南;
            }
            else if (theta == 270)
            {
                return DirectionType.正南;
            }
            else
            {
                return DirectionType.东南;
            }
        }

    }

}
