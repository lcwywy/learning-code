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

    /// <summary>
    /// 岩性描述类
    /// </summary>
    public class LithText : Control
    {
        private AlignStyle horizontalAlign, verticalAlign;
        private Font fontStyleD;
        private float tempTop; //临时顶界，参与计算底界
        private float[] primitiveBottom; //初始默认底界
        private float[] bottoms;          //实际底界
        private float boreHoleElevation; //孔口高程
        private int validLayer = -1; //能显示在该页的起始地层,初始设为-1
        int count;//能显示在该页地层的数
        
        /// <summary
        /// 岩性描述构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public LithText(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            this.horizontalAlign = AlignStyle.Center;
            this.verticalAlign = AlignStyle.Center;
            this.fontStyleD = new Font("宋体", 8);
        }

        public AlignStyle HorizontalAlign
        {
            get { return horizontalAlign; }
            set { horizontalAlign = value; }
        }

        public AlignStyle VerticalAlign
        {
            get { return verticalAlign; }
            set { verticalAlign = value; }
        }

        public Font FontStyleD
        {
            get { return fontStyleD; }
            set { fontStyleD = value; }
        }

        /// <summary>
        /// 绘制描述信息
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        public override void Draw(Graphics g, BoreData data)
        {
            base.Draw(g, data);
          
            List<LayerInfo> layers = data.LayerInfoList;
            this.boreHoleElevation = data.BasicBoreInfo.DrillZ;
            tempTop = Top;
            GeneratePrimitiveBottoms(layers);
            GenerateBottoms(layers, g);

            int index = 0; //画该页第几层
            for (int i = validLayer; i < validLayer + count; i++)
            {
                drawDescription(layers[i], index++, g);
            }
            
        }

        /// <summary>
        /// 生成初始底界
        /// </summary>
        /// <param name="layers"></param>
        private void GeneratePrimitiveBottoms(List<LayerInfo> layers)
        {
            //float totalDepth=Math.Abs(layers[layers.Count-1].LayerBottom-boreHoleElevation);
            count = 0; //属于该页的文本描述行计数
            float totalDepth = Math.Abs(beginDepth - endDepth);

            float[] primitive = new float[layers.Count];

            for (int i = 0; i < layers.Count; i++)
            {
                if (Math.Abs(layers[i].LayerBottom) > Math.Abs(beginDepth))//必须大于顶界
                {
                    if (validLayer == -1)
                        validLayer = i; //第一个有效地址为初始地址
                    //primitiveBottom[i] = Top + Math.Abs(layers[i].LayerBottom - boreHoleElevation) * Height / totalDepth;
                    primitive[count] = Top + Math.Abs(layers[i].LayerBottom - beginDepth) * Height / totalDepth;

                    if (primitive[count] >= Top + Height)
                    {
                        primitive[count] = Top + Height; //不得超过页面底界
                        count++;
                        //根据底板生成 初始底界
                        break;                            //已经达到底界了，停止循环
                    }
                    count++;
                }
            }
            primitiveBottom = new float[count];
            for (int i = 0; i < count; i++)
            {
                primitiveBottom[i] = primitive[i];
            }
        }

        /// <summary>
        /// 生成底界
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        private void GenerateBottoms(List<LayerInfo> layers, Graphics g)
        {
            bottoms = new float[count];
            int index = 0;
            for (int i = validLayer; i < validLayer + count; i++)
            {
                SetBottom(layers[i], g, index++);
            }

        }

        private void SetBottom(LayerInfo layerInfo, Graphics g, int index)
        {
            string str = layerInfo.EngName;
            SizeF s = g.MeasureString(str, this.fontStyleD);
            int stringlines = (int)(s.Width * 10 / (Width * 9)) + 1; //测量所需要的行数（由于要装在十分之九的框中）
            float verticalSpace = s.Height * stringlines * 4 / 3; //写下文字所需空间
            if (tempTop + verticalSpace > primitiveBottom[index])
            {
                bottoms[index] = tempTop + verticalSpace;
            }
            else
            {
                bottoms[index] = primitiveBottom[index];
            }
            if (tempTop + verticalSpace > this.Top + this.Height && drillGlobal.currentPage != drillGlobal.totalPage)
                bottoms[index] = this.Top + this.Height;//如果超过了底界，且不是最后一页则以底界为准
            tempTop = bottoms[index];
        }

        /// <summary>
        /// 分层对描述信息进行绘制
        /// </summary>
        /// <param name="layer"></param>
        private void drawDescription(LayerInfo layer, int index, Graphics g)
        {
            float topElevation;
            if (index == 0)
                topElevation = Top;
            else
                topElevation = bottoms[index - 1];
            float bottomElevation;
            if (bottoms[index] > primitiveBottom[index])   //需要拉线
            {
                bottomElevation = bottoms[index];
                g.DrawString(layer.EngName, this.fontStyleD, new SolidBrush(Color.Black), new RectangleF(Left + Width / 20, topElevation + (bottomElevation - topElevation) / 20+5, Width * 9 / 10, bottomElevation - topElevation));
                g.DrawLine(new Pen(Color.Black), new PointF(Left, primitiveBottom[index]), new PointF(Left + Width / 20, bottomElevation));
                g.DrawLine(new Pen(Color.Black), new PointF(Left + Width / 20, bottomElevation), new PointF(Left + Width, bottomElevation));
            }
            else                                           //不需要拉线
            {
                bottomElevation = primitiveBottom[index];
                g.DrawString(layer.EngName, this.fontStyleD, new SolidBrush(Color.Black), new RectangleF(Left + Width / 20, topElevation, Width * 9 / 10+5, bottomElevation - topElevation));
                g.DrawLine(new Pen(Color.Black), new PointF(Left, bottomElevation), new PointF(Left + Width, bottomElevation));

            }


        }
    }
}
