using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;

namespace SmartGeoLogical.Render.Bore.Column
{
    public class BarValue:Control
    {
        private Font fontD;
        private AlignStyle horizontalAlignStyle;
        private object[] barValue=new object[100];//设置一个足够大的数组存填充图值
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        /// <param name="name"></param>
        public BarValue(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner,name)
        {
            this.fontD = new Font("宋体", 8);
            this.horizontalAlignStyle = AlignStyle.Left;
            this.DrawOutLine = false;
        }


        /// <summary>
        /// 字体
        /// </summary>
        public Font FontD
        {
            get { return fontD; }
            set { fontD = value; }
        }

        /// <summary>
        /// 水平对齐方式
        /// </summary>
        public AlignStyle HorizontalAlignStyle
        {
            get { return horizontalAlignStyle; }
            set { horizontalAlignStyle = value; }
        }

        public override  void Draw(Graphics g, BoreData data)
        {
            base.Draw(g, data);
          
            List<SamplingInfo> sampling = data.GetSamplingDataByName(this.CtrName);
            float boreholeElevation = data.BasicBoreInfo.DrillZ;
            float totalDepth = data.BasicBoreInfo.totalDepth;
            PointF[] points = GetPointsPosition(sampling, boreholeElevation, totalDepth, g);
            for (int i = 0; i < points.Length; i++)
                g.DrawString(barValue[i].ToString(), this.fontD, new SolidBrush(Color.Black), points[i]);
        }

        private PointF[] GetPointsPosition(List<SamplingInfo> sampling, float boreholeElevation, float totalDepth,Graphics g)
        {
            int count = 0;
            PointF[] points = new PointF[sampling.Count];
            for (int i = 0; i < sampling.Count; i++)
            {
                points[i].X = 32767;
                points[i].Y = 32767;
            }
            for (int i = 0; i < sampling.Count; i++)
            {
                
                AreaSamplingInfo sample = (AreaSamplingInfo)sampling[i];
                SizeF s = g.MeasureString(sample.SampleValueList[0].SamplingValue.ToString(), this.fontD);

                float left=this.Left;
                switch (this.horizontalAlignStyle)
                {
                    case AlignStyle.Left: left = this.Left; break;
                    case AlignStyle.Right: left = this.Left + this.Width - s.Width; break;
                    case AlignStyle.Center: left = this.Left + (this.Width - s.Width) / 2; break;
                }

                float depth = (sample.BeginDepth + sample.EndDepth) / 2 ;
                if (Math.Abs(depth) > Math.Abs(beginDepth)&&Math.Abs(depth)<Math.Abs(endDepth))
                {
                    float offset = this.Top + Math.Abs(depth-beginDepth) * this.Height / Math.Abs(interval) - s.Height / 2;
                    if (offset > this.Top + this.Height)
                        offset = this.Top + this.Height - s.Height ;//超过则放该页最底下
                    points[i] = new PointF(left, offset);
                    barValue[count] = sample.SampleValueList[0].SamplingValue;
                    count++;
                }
            }
            PointF[] chosePoint = new PointF[count];
            int j = 0;
            foreach (PointF poi in points)
            {
                if (poi.X != 32767)
                {
                    chosePoint[j].X = poi.X;
                    chosePoint[j++].Y = poi.Y;
                }
            }
            return chosePoint;

        } 


    }
}
