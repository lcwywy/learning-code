using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;
using FillMode = SmartGeoLogical.Render.DrawInterface.FillMode;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 填充图
    /// </summary>
    public class Bar:Control
    {
        private float max, min;
        private bool isFillFromL2R;
        private bool drawGrid;
        private Color fillBackgroundColor;
        private Color fillOutLineColor;
        private FillType barFillType;
        private GridStyle barGridStyle;

        /// <summary>
        /// 填充图构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Bar(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner,string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner,name)
        {
            barGridStyle = new GridStyle();
            barFillType = new FillType();
            fillBackgroundColor = Color.White;
            fillOutLineColor = Color.Blue;
            max = 1;
            min = 0;
            isFillFromL2R = true;
            this.DrawOutLine = false;
            drawGrid = false;
        }

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
        /// 是否是从左向右填充
        /// </summary>
        public bool IsFillFromL2R
        {
            get { return isFillFromL2R; }
            set { isFillFromL2R = value; }
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
        /// 背景色
        /// </summary>
        public Color FillBackgroundColor
        {
            get { return fillBackgroundColor; }
            set { fillBackgroundColor = value; }
        }

        /// <summary>
        /// 外框色
        /// </summary>
        public Color FillOutLineColor
        {
            get { return fillOutLineColor; }
            set { fillOutLineColor = value; }
        }

        /// <summary>
        /// 填充类型
        /// </summary>
        public FillType BarFillType
        {
            get { return barFillType; }
            set { barFillType = value; }
        }

        /// <summary>
        /// 格网样式
        /// </summary>
        public GridStyle BarGridStyle
        {
            get { return barGridStyle; }
            set { barGridStyle = value; }
        }

        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="g"></param>
        public override async void Draw(Graphics g,BoreData data)
        {
             base.Draw(g, data);
            //to do
            List<SamplingInfo> sampling = data.GetSamplingDataByName(this.CtrName);
            float boreholeElevation = data.BasicBoreInfo.DrillZ;
            float totalDepth = data.BasicBoreInfo.totalDepth;

            if (drawGrid)
            {
                this.barGridStyle.DrawGrid(g, new RectangleF(this.Left, this.Top, this.Width, this.Height));
            }

            RectangleF[] rects = GetRectsPosition(sampling, boreholeElevation, totalDepth);
            foreach (RectangleF rect in rects)
                DrawBar(rect, g);
          
           
        }

        
        private void DrawBar(RectangleF rect,Graphics g)
        {
            Brush myBrush;
            if (this.barFillType.FigFillMode == FillMode.ValueBased)
                myBrush = new LinearGradientBrush(new PointF( rect.Left,rect.Top),new PointF( rect.Left + rect.Width,rect.Top), barFillType.ValueBasedBegin, barFillType.ValueBasedEnd);
            else
                myBrush = new SolidBrush(this.fillBackgroundColor);
            g.FillRectangle(myBrush, rect);
            g.DrawRectangle(new Pen(this.OutLineColor), rect.Left,rect.Top,rect.Width,rect.Height);
           
        }

        /// <summary>
        /// 获取样点位置列表
        /// </summary>
        /// <param name="sampling"></param>
        /// <param name="boreholeElevation"></param>
        /// <param name="totalDepth"></param>
        /// <returns></returns>
        private RectangleF[] GetRectsPosition(List<SamplingInfo> sampling, float boreholeElevation, float totalDepth)
        {
            RectangleF[] rects = new RectangleF[sampling.Count];
            int count = 0;
            for (int i = 0; i < sampling.Count; i++)
                rects[i] = new RectangleF(32767, 0, 1, 1);
            for (int i = 0; i < sampling.Count; i++)
            {
               AreaSamplingInfo sample = (AreaSamplingInfo)sampling[i];
               float beginDepth1 = sample.BeginDepth;//与点的开始深度区分开
               float endDepth1 = sample.EndDepth;
               if ((Math.Abs(beginDepth1) >= Math.Abs(beginDepth) && Math.Abs(beginDepth1) < Math.Abs(endDepth))||
                   (Math.Abs(endDepth1) > Math.Abs(beginDepth) && Math.Abs(endDepth1) < Math.Abs(endDepth)))
               {
                   float beginOffset = this.Top + Math.Abs(beginDepth1 - beginDepth) * this.Height / Math.Abs(interval);
                   if (Math.Abs(beginDepth1) < Math.Abs(beginDepth))//如果上页就已经有了，切该页里还有
                       beginOffset = this.Top;
                   float endOffset = this.Top + Math.Abs(endDepth1 - beginDepth) * this.Height / Math.Abs((interval));
                   if (endOffset > this.Top + this.Height)
                       endOffset = this.Top + this.Height;//超过该页长度则取该页最底端
                   float value = float.Parse(sample.SampleValueList[0].SamplingValue.ToString());
                   float horizontalOffset = (value - this.min) * this.Width / (this.max - this.min);
                   rects[i] = new RectangleF(this.Left, beginOffset, horizontalOffset, endOffset - beginOffset);
                   count++;
               }
            }

            RectangleF[] choseRect = new RectangleF[count];
            count=0;
            for(int i=0;i<sampling.Count;i++)
                if (rects[i].Left != 32767)
                {
                    choseRect[count++] = new RectangleF(rects[i].Left, rects[i].Top, rects[i].Width, rects[i].Height);
                }
           
            return choseRect;
        }
    }
}
