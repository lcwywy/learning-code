using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 文本
    /// </summary>
    public class Text:Control
    {
        private Font fontD;
        private AlignStyle horizontalAlignStyle;
        private float compensationHeigth=0;
        /// <summary>
        /// 上一个采样点的文字下边界
        /// </summary>
        private float underLineHeigth = 0;
        /// <summary>
        /// 文本构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Text(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner,name)
        {
            this.fontD = new Font("宋体", 8);
            this.horizontalAlignStyle = AlignStyle.Center;
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

        /// <summary>
        /// 绘制描述信息
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        public override void Draw(Graphics g, BoreData data)
        {
            base.Draw(g, data);
          
            List<SamplingInfo> sampling = data.GetSamplingDataByName(this.CtrName);
            float boreholeElevation = data.BasicBoreInfo.DrillZ;
            float totalDepth = data.BasicBoreInfo.totalDepth;
            foreach (SamplingInfo sample in sampling)
            {
                DrawNameAndValue(sample, g, 0f, totalDepth);
            }
        }

        /// <summary>
        /// 绘制点采样信息
        /// </summary>
        /// <param name="sampling"></param>
        private void DrawText(SamplingInfo sample,Graphics g,float boreholeElevation,float totalDepth)
        {
            float depth;
            PointSamplingInfo sampling = (PointSamplingInfo)sample;
            depth = sampling.Depth;
            SizeF s = g.MeasureString(sampling.SampleValueList[0].SamplingValue.ToString(), this.fontD);
            float horizontalBegin=this.Left; //水平起始绘制位置
            switch (this.horizontalAlignStyle)
            {
                case AlignStyle.Left: horizontalBegin = this.Left; break;
                case AlignStyle.Right: horizontalBegin = this.Left +this.Width- s.Width; break;
                case AlignStyle.Center: horizontalBegin = this.Left + (this.Width - s.Width) / 2; break;
            }

            //垂直绘制高度
            float verticalBegin = this.Top + Math.Abs(depth - boreholeElevation) * this.Height / Math.Abs(totalDepth - boreholeElevation);
            g.DrawString(sampling.SampleValueList[0].SamplingValue.ToString(), fontD, new SolidBrush(Color.Black), new PointF(horizontalBegin, verticalBegin));

        }
        /// <summary>
        /// 绘制  采样点名称加值的
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="g"></param>
        /// <param name="boreholeElevation"></param>
        /// <param name="totalDepth"></param>
        private void DrawNameAndValue(SamplingInfo sample,Graphics g , float boreholeElevation, float totalDepth)
        {
            AreaSamplingInfo sampling = (AreaSamplingInfo)sample;
            
            SizeF s = g.MeasureString(sampling.SampleValueList[0].SamplingValue.ToString(), this.fontD);
            float horizontalBegin = this.Left; //水平起始绘制位置
            float verticalBegin = this.Top + Math.Abs(sampling.BeginDepth) * this.Height / Math.Abs(totalDepth)+ compensationHeigth;

            SampleValue sampleValue =sampling.SampleValueList[0];
            string overValue = sampleValue.SamplingValue.ToString();
            string value = $"{sampling.BeginDepth}-{sampling.EndDepth}";
            SizeF nameSize= g.MeasureString(overValue, this.fontD);
            SizeF valueSize = g.MeasureString(value, this.fontD);
            g.DrawString(overValue, this.fontD, new SolidBrush(Color.Black), GetRectangle(nameSize, ref verticalBegin, true), GetFormat( AlignStyle.CenterFar));
            g.DrawLine(new Pen(Color.Black), new PointF(Left, verticalBegin), new PointF(Left + Width, verticalBegin));
            g.DrawString(value, this.fontD, new SolidBrush(Color.Black), GetRectangle(valueSize, ref verticalBegin, false),GetFormat( AlignStyle.CenterNear));
        }
        /// <summary>
        /// 获取文字的外框
        /// </summary>
        /// <param name="sizeF"></param>
        /// <param name="verticalBegin"></param>
        /// <param name="IsOver"></param>
        /// <returns></returns>
        private RectangleF GetRectangle(SizeF sizeF, ref float verticalBegin   , bool IsOver)
        {
            //计算高度不够时，补偿高度
           int row= (int)Math.Ceiling(sizeF.Width / this.Width);
            int fHeigth = (int)(row * sizeF.Height);
            if (underLineHeigth == 0)
            {
                underLineHeigth = this.Top;
            }
        
            if(verticalBegin- underLineHeigth< fHeigth)
            {
                compensationHeigth = fHeigth - (verticalBegin - underLineHeigth);
                verticalBegin += compensationHeigth;
            }
            else
            {
                if (compensationHeigth>0)
                {
                    float distance = compensationHeigth- ((verticalBegin - underLineHeigth)- fHeigth);
                    if (distance > 0)
                    {
                        verticalBegin -= (verticalBegin - underLineHeigth) - fHeigth;
                        compensationHeigth = compensationHeigth - distance;
                    }
                    else
                    {
                        verticalBegin -= compensationHeigth;
                        compensationHeigth = 0;
                    }

                }
            }

            RectangleF rectangleF ;
            if (IsOver)
            {
                rectangleF = new RectangleF(this.Left, verticalBegin - row * sizeF.Height, this.Width, row * sizeF.Height);
            }
            else
            {
                rectangleF = new RectangleF(this.Left, verticalBegin, this.Width, row * sizeF.Height);
                underLineHeigth = verticalBegin + row * sizeF.Height;
            }

            return rectangleF;
        }

    }
}
