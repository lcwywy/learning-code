using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using BoreholeHistogram.BoreData;
using System.Reflection;

namespace BoreholeHistogram.Control.Text
{
    class MultivaluedText:Control
    {

        #region 属性

        /// <summary>
        /// 文本值格式
        /// </summary>
        public TextFormat Text { get { return text; } set { text = value; } }

        #endregion

        #region 私有字段

        /// <summary>
        /// 文本值格式        
        /// </summary>
        private TextFormat text;

        /// <summary>
        /// 孔口高程
        /// </summary>
        private float boreHoleElevation;
        private float tempTop;
        private float[] primitiveBottom; //初始默认底界
        private float[] bottoms;          //实际底界
        private int validLayer = -1; //能显示在该页的起始地层,初始设为-1
        int count;//能显示在该页地层的数

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化多值文本的参数
        /// </summary>
        public MultivaluedText(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
             : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            text = new TextFormat(new Font("宋体",8), Color.Black, BoreStyle.AlignStyle.CenterCenter);
        }

        #endregion

        #region 绘制多值文本

        public override void Draw(Graphics g, BoreData.BoreData data)
        {
            base.Draw(g, data);

            List<LayerInfo> layers = data.LayerInfoList;            
            boreHoleElevation = data.BasicBoreInfo.DrillZ;
            tempTop = Rect.Top;
            GeneratePrimitiveBottoms(layers);

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
                    primitive[count] = Rect.Top + Math.Abs(layers[i].LayerBottom - beginDepth) * Rect.Height / totalDepth;

                    if (primitive[count] >= Rect.Top + Rect.Height)
                    {
                        primitive[count] = Rect.Top + Rect.Height; //不得超过页面底界
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
            string str = GetText(layerInfo);
            SizeF s = g.MeasureString(str, this.text.FontD);
            int stringlines = (int)(s.Width * 10 / (Rect.Width * 9)) + 1; //测量所需要的行数（由于要装在十分之九的框中）
            float verticalSpace = s.Height * stringlines * 4 / 3; //写下文字所需空间
            if (tempTop + verticalSpace > primitiveBottom[index])
            {
                bottoms[index] = tempTop + verticalSpace;
            }
            else
            {
                bottoms[index] = primitiveBottom[index];
            }
            if (tempTop + verticalSpace > this.Rect.Top + this.Rect.Height && drillGlobal.currentPage != drillGlobal.totalPage)
                bottoms[index] = this.Rect.Top + this.Rect.Height;//如果超过了底界，且不是最后一页则以底界为准
            tempTop = bottoms[index];
        }

        private string GetText(LayerInfo layerInfo)
        {
            Type type = layerInfo.GetType();
            PropertyInfo prop = type.GetProperty(this.CtrName);
            string value = prop?.GetValue(layerInfo).ToString();
            return value;
        }

        /// <summary>
        /// 分层对描述信息进行绘制
        /// </summary>
        /// <param name="layer"></param>
        private void drawDescription(LayerInfo layer, int index, Graphics g)
        {
            float topElevation;
            if (index == 0)
                topElevation = Rect.Top;
            else
                topElevation = bottoms[index - 1];
            float bottomElevation;
            if (bottoms[index] > primitiveBottom[index])   //需要拉线
            {
                bottomElevation = bottoms[index];
                g.DrawString(GetText(layer), this.text.FontD, new SolidBrush(Color.Black), new RectangleF(Rect.Left + Rect.Width / 20, topElevation + (bottomElevation - topElevation) / 20, Rect.Width * 9 / 10, bottomElevation - topElevation), Text.GetFormat(this.text.FontAlign));
                g.DrawLine(new Pen(Color.Black), new PointF(Rect.Left, primitiveBottom[index]), new PointF(Rect.Left + Rect.Width / 20, bottomElevation));
                g.DrawLine(new Pen(Color.Black), new PointF(Rect.Left + Rect.Width / 20, bottomElevation), new PointF(Rect.Left + Rect.Width - Rect.Width / 20, bottomElevation));

                g.DrawLine(new Pen(Color.Black), new PointF(Rect.Left + Rect.Width - Rect.Width / 20, bottomElevation), new PointF(Rect.Left + Rect.Width, primitiveBottom[index]));
            }
            else                                           //不需要拉线
            {
                bottomElevation = primitiveBottom[index];
                g.DrawString(GetText(layer), this.text.FontD, new SolidBrush(Color.Black), new RectangleF(Rect.Left + Rect.Width / 20, topElevation, Rect.Width * 9 / 10, bottomElevation - topElevation), Text.GetFormat(this.text.FontAlign));
                g.DrawLine(new Pen(Color.Black), new PointF(Rect.Left, bottomElevation), new PointF(Rect.Left + Rect.Width, bottomElevation));

            }


        }

        #endregion
    }
}
