using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 曲线
    /// </summary>
    public class Curve:Control
    {
        #region 私有字段

        private float max, min;
        private bool isFillFromL2R,isDrawFromL2R,isDrawCenter;
        private bool drawGrid;  
        private GridStyle curveGridStyle;
        private CurveStyle curStyle;

        #endregion 私有字段

        #region 属性

        /// <summary>
        /// 最大值
        /// </summary>
        public float Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public float Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// 是否从左向右填充
        /// </summary>
        public bool IsFillFromL2R
        {
            get { return isFillFromL2R; }
            set { isFillFromL2R = value; }
        }


        /// <summary>
        /// 是否从左向右绘制
        /// </summary>
        public bool IsDrawFromL2R
        {
            get { return isDrawFromL2R; }
            set { isDrawFromL2R = value; }
        }
      
        /// <summary>
        /// 是否中间开始绘制
        /// </summary>
        public bool IsDrawCenter
        {
            get { return isDrawCenter; }
            set { isDrawCenter = value; }
        }
      
        /// <summary>
        /// 是否绘制格网
        /// </summary>
        public bool DrawGrid
        {
            get { return drawGrid; }
            set { drawGrid = value; }
        }

        /// <summary>
        /// 曲线格网样式
        /// </summary>
        public GridStyle CurveGridStyle
        {
            get { return curveGridStyle; }
            set { curveGridStyle = value; }
        }

        /// <summary>
        /// 曲线样式
        /// </summary>
        public CurveStyle CurStyle
        {
            get { return curStyle; }
            set { curStyle = value; }
        }

        #endregion 属性

        /// <summary>
        /// 曲线构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Curve(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            max = 0;
            min = 10;
            isFillFromL2R = true;
            isDrawFromL2R = true;
            isDrawCenter = false;
            drawGrid = true;
            curveGridStyle = new GridStyle();
            curStyle = new CurveStyle(drillGlobal);
        }

        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g,BoreData data)
        {
            base.Draw(g,data);
         
            List<SamplingInfo> sampling = data.GetSamplingDataByName(this.CtrName);
            float boreholeElevation = data.BasicBoreInfo.DrillZ;
            float totalDepth = data.BasicBoreInfo.totalDepth;

            if (drawGrid)
            {
                this.curveGridStyle.DrawGrid(g, new RectangleF(this.Left, this.Top, this.Width, this.Height));
            }
            PointF[] points = GetPointsPosition(sampling, boreholeElevation, totalDepth);
            //绘制曲线实体
            this.curStyle.DrawCurveByPoints(points, g);
                
        
        }

        /// <summary>
        /// 获取样点位置列表
        /// </summary>
        /// <param name="sampling"></param>
        /// <param name="boreholeElevation"></param>
        /// <param name="totalDepth"></param>
        /// <returns></returns>
        private PointF[] GetPointsPosition(List<SamplingInfo> sampling, float boreholeElevation, float totalDepth)
        {
            PointF[] points = new PointF[sampling.Count + 4]; 
            
            int count = 0;//计算个数
            //初始化
            for (int k = 0; k < points.Length; k++)
            {
                points[k].X = 32767;
                points[k].Y = 32767;
            }
            float moveLen = 0;
            if (IsDrawCenter)
            {
                this.Left += this.Width / 2;
                moveLen = this.Width / 2;
            }
            // if (isDrawFromL2R)//是否从左向右绘制
            {
                if (this.curStyle.PlotStyle == CurveCatalog.FillPoint || this.curStyle.PlotStyle == CurveCatalog.Fill)
                {
                    if (!isFillFromL2R)
                    {
                        points[0] = new PointF(this.Left + this.Width, this.Top);
                        points[points.Length - 1] = new PointF(this.Left + this.Width, this.Top + this.Height);

                    }
                    else
                    {
                        points[0] = new PointF(this.Left, this.Top);
                        points[points.Length - 1] = new PointF(this.Left, this.Top + this.Height);
                    }
                    count = 2;
                }
                else
                {
                    if (drillGlobal.currentPage == 1) //第一页则设左上点
                    {
                        points[0] = new PointF(this.Left, this.Top);
                        //points[points.Length - 1] = new PointF(this.Left, this.Top + this.Height);
                        count++;

                    }
                    if (drillGlobal.currentPage == drillGlobal.totalPage)//最后一页则设左下点
                    {
                        points[points.Length - 1] = new PointF(this.Left, this.Top + this.Height);
                        count++;
                    }
                }
                for (int i = 0; i < sampling.Count; i++)
                {
                    PointSamplingInfo sample = (PointSamplingInfo)sampling[i];
                    float depth = sample.Depth;
                    if (Math.Abs(depth) > Math.Abs(beginDepth) && Math.Abs(depth) < Math.Abs(endDepth))
                    {
                        float value = float.Parse(sample.SampleValueList[0].SamplingValue.ToString());
                        float x = this.Left + (value - this.min) * this.Width / (this.max - this.min)- moveLen;
                        //float y = this.Top + Math.Abs(depth - boreholeElevation) * this.Height / Math.Abs(totalDepth - boreholeElevation);
                        float y = this.Top + Math.Abs(depth - beginDepth) * this.Height / Math.Abs(interval);
                        points[i+2] = new PointF(x, y);
                        count++;
                    }
                }
            }
            //else
            //{
            //    if (this.curStyle.PlotStyle == CurveCatalog.FillPoint || this.curStyle.PlotStyle == CurveCatalog.Fill)
            //    {
            //        if (isFillFromL2R)
            //        {
            //            points[0] = new PointF(this.Left, this.Top);
            //            points[points.Length - 1] = new PointF(this.Left, this.Top + this.Height);

            //        }
            //        else
            //        {
            //            points[0] = new PointF(this.Left + this.Width, this.Top);
            //            points[points.Length - 1] = new PointF(this.Left + this.Width, this.Top + this.Height);
            //        }
            //        count = 2;
            //    }
            //    else// if(BGlobal.currentPage==1) //第一页则设左上点
            //    {
            //        points[0] = new PointF(this.Left + this.Width, this.Top);
            //        count = 2;

            //    }
            //    //else if (BGlobal.currentPage == BGlobal.totalPage)//最后一页则设左下点
            //    //{
            //    //    points[points.Length - 1] = new PointF(this.Left + this.Width, this.Top + this.Height);
            //    //    count++;
            //    //}
            //    for (int i = 0; i < sampling.Count; i++)
            //    {
            //        BoreData.PointSamplingInfo sample = (BoreData.PointSamplingInfo)sampling[i];
            //        float depth = sample.Depth;
            //        if (Math.Abs(depth) > Math.Abs(beginDepth) && Math.Abs(depth) < Math.Abs(endDepth))
            //        {
            //            float value = float.Parse(sample.SampleValueList[0].SamplingValue.ToString());
            //            float x = this.Left + this.Width - (value - this.min) * this.Width / (this.max - this.min);
            //            //float y = this.Top + Math.Abs(depth - boreholeElevation) * this.Height / Math.Abs(totalDepth - boreholeElevation);
            //            float y = this.Top + Math.Abs(depth - beginDepth) * this.Height / Math.Abs(interval);
            //            points[i + 1] = new PointF(x, y);
            //            count++;
            //        }
            //    }
            //}
            //最后预处理,人为增加2点
            if(drillGlobal.currentPage!=1)             //不是第一页，增加一点
            for(int i=2;i<points.Length;i++)
                if (points[i].X != 32767)
                {
                    points[1].X = points[i].X;
                    points[1].Y = this.Top+5;
                    count++;
                    break;
                }
            if(drillGlobal.currentPage!=drillGlobal.totalPage) //不是最后一页，增加一点
            for (int i = points.Length - 3; i > 0; i--)
            {
                if (points[i].X != 32767)
                {
                    points[points.Length - 2].X = points[i].X;
                    points[points.Length - 2].Y = this.Top + this.Height;
                    count++;
                    break;
                }
            }

            //筛选符合条件的点
            PointF[] pointsChose = new PointF[count]; //再设两个辅助点
            count=0;
            foreach (PointF poi in points)
            {
                if (poi.X != 32767)
                {
                    pointsChose[count].X = poi.X;
                    pointsChose[count++].Y = poi.Y;
                }
            }
            
            

            return pointsChose;
        }
    }
}
