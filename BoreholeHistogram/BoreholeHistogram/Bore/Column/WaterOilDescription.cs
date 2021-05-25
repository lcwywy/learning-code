using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.Bore.Column
{
    public class WaterOilDescription : Control
    {
        private Font fontD;
        private AlignStyle horizontalAlignStyle;
        private object[] barValue = new object[100];//设置一个足够大的数组存填充图值
        private int barCounts;
        private List<RectangleF> rects = new List<RectangleF>(); //油层界限
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        /// <param name="name"></param>
        public WaterOilDescription(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
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

        //public override void Draw(Graphics g, BoreData data)
        //{
        //    base.Draw(g, data);
        //    List<SamplingInfo> sampling = data.GetSamplingDataByName(this.CtrName);
        //    float boreholeElevation = data.BasicBoreInfo.孔口高程;
        //    float totalDepth = data.BasicBoreInfo.完井深度;
        //    PointF[] points = GetPointsPosition(sampling, boreholeElevation, totalDepth, g);
        //    GetLayerRects(points);
        //    points = MergePoints(points);
        //    for (int i = 0; i < points.Length; i++)
        //        g.DrawString(barValue[i].ToString(), this.fontD, new SolidBrush(Color.Black), points[i]); 
        //}

        private void GetLayerRects(PointF[] points)
        {
            for (int i = 0; i < points.Length-1; i++)
            {
                RectangleF rec = new RectangleF(this.Left, points[i].Y, this.Width, points[i + 1].Y - points[i].Y);
                rects.Add(rec);
            }
        }

        public override void Draw(Graphics g, BoreData data)
        {
            base.Draw(g, data);

            List<SamplingInfo> sampling = data.GetSamplingDataByName(this.CtrName);
            float boreholeElevation = data.BasicBoreInfo.DrillZ;
            float totalDepth = data.BasicBoreInfo.totalDepth;
            PointF[] points = GetPointsPosition(sampling, boreholeElevation, totalDepth, g);
            points = MergePoints(points);
            GetLayerRects(points);
            Image img;
            for (int i = 0; i < rects.Count; i++)
            {
                if (barValue[i].ToString() == "0")
                    img = Bitmap.FromFile("water" + ".jpg");
                else if (barValue[i].ToString() == "1")
                    img = Bitmap.FromFile("oillayer" + ".jpg");
                else
                    img = Bitmap.FromFile("dry" + ".jpg");
                g.DrawImage(img, rects[i]);
                g.DrawRectangle(new Pen(Color.Black), rects[i].Left, rects[i].Top, rects[i].Width, rects[i].Height);
            }

              
        }





        //public override void Draw(Graphics g, BoreData data)
        //{
        //    base.Draw(g, data);

        //    //tempTop = data.BasicBoreInfo.孔口高程;
        //    tempTop = beginDepth;

        //    List<LayerInfo> layers = data.LayerInfoList;
        //    //float totalDepth = Math.Abs(layers[layers.Count-1].LayerBottom - data.BasicBoreInfo.孔口高程);
        //    float totalDepth = Math.Abs(endDepth - beginDepth);//总深度：开始深度-结束深度

        //    foreach (LayerInfo layer in layers)
        //    {
        //        //if(Math.Abs(layer.LayerBottom)>Math.Abs(beginDepth))
        //        DrawLayer(layer, totalDepth, data.BasicBoreInfo.孔口高程, g);
        //    }

        //}

        //private void DrawLayer(LayerInfo layer, float totalDepth, float holeElevation, Graphics g)
        //{
        //    if (Math.Abs(layer.LayerBottom) < Math.Abs(beginDepth))
        //        return;                                          //底界必须大于最小深度，否则不绘制
        //    if (Math.Abs(layer.LayerBottom) > Math.Abs(endDepth))
        //        layer.LayerBottom = endDepth;                    //底界大于该页最大深度，画到底界为止

        //    float layerHeight = Math.Abs(layer.LayerBottom - tempTop) * this.Height / totalDepth;
        //    //float layerTop = this.Top+Math.Abs(tempTop - holeElevation) * this.Height / totalDepth;
        //    float layerTop = this.Top + Math.Abs(tempTop - beginDepth) * this.Height / totalDepth;
        //    Image img = Bitmap.FromFile(layer.LithName + ".jpg");

        //    g.DrawImage(img, new RectangleF(this.Left, layerTop, this.Width, layerHeight));
        //    g.DrawRectangle(new Pen(Color.Black), this.Left, layerTop, this.Width, layerHeight);
        //    this.tempTop = layer.LayerBottom;
        //}

        private PointF[] GetPointsPosition(List<SamplingInfo> sampling, float boreholeElevation, float totalDepth, Graphics g)
        {
            int count = 0;
            PointF[] points = new PointF[sampling.Count];
           
            for (int i = 0; i < sampling.Count; i++)
            {
                
                PointSamplingInfo sample = (PointSamplingInfo)sampling[i];
                barValue[count] = sample.SampleValueList[0].SamplingValue;
                
                    float depth = sample.Depth;
               
                    float value = float.Parse(sample.SampleValueList[0].SamplingValue.ToString());
                    float x = this.Left + this.Width / 2;
                    //float y = this.Top + Math.Abs(depth - boreholeElevation) * this.Height / Math.Abs(totalDepth - boreholeElevation);
                    //设置为水油层中间的位置
                    
                    float y = this.Top + Math.Abs(depth - beginDepth) * this.Height / Math.Abs(interval);
                    points[i] = new PointF(x, y);
                    
                    count++;
                             
              
            }
            barCounts = count;
            return points;

        }

        private  PointF[] MergePoints(PointF[] points)
        {
            List<object> valueList = new List<object>();

            List<PointF> pointsF = new List<PointF>();
            valueList.Add(barValue[0]);
            int valueCount = 1;
            pointsF.Add(points[0]);
            for (int i = 1; i < barCounts; i++)
            {
                if (barValue[i].ToString() != valueList[valueCount - 1].ToString())
                {
                    valueList.Add(barValue[i]);
                    pointsF.Add(points[i]);
                    valueCount++;
                }
                else
                {
                    pointsF[pointsF.Count - 1] = points[i];                    
                }
            }
            PointF[] pf = new PointF[pointsF.Count];
            for (int i = 0; i < pointsF.Count; i++)
                pf[i] = pointsF[i];
            for (int i = 0; i < pointsF.Count; i++)
            {
                barValue[i] = valueList[i];
            }
            return pf;
        }

    }
}
