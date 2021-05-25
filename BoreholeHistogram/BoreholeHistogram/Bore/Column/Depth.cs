using System;
using System.Drawing;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;

namespace SmartGeoLogical.Render.Bore.Column
{
   
    /// <summary>
    /// 深度
    /// </summary>
    public class Depth:Control
    {
        private Font fontD;
        private Color fontColor;
        private AlignStyle numDisplayPosition;
        private AngleStyle numDisplayAngle;
        private float  largeScale, smallScale;
        private int validNum;
        private RulePosition largeRulePosition, smallRulePosion;

        private float topOffset=0;//顶位移

        /// <summary>
        /// 深度构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Depth(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner,name)
        {
            this.fontD = new Font("宋体", 8);
            this.numDisplayAngle = AngleStyle.Horizontal;
            this.numDisplayPosition = AlignStyle.Center;
            this.validNum = 2;
            this.largeScale = 5;
            this.smallScale = 2.5f;
            this.largeRulePosition = RulePosition.Left;
            this.smallRulePosion = RulePosition.Left;
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
         /// 数字显示位置
         /// </summary>
         public AlignStyle NumDisplayPosition
         {
             get { return numDisplayPosition; }
             set { numDisplayPosition = value; }
         }
         
        /// <summary>
        /// 数字显示角度
        /// </summary>
         public AngleStyle NumDisplayAngle
         {
             get { return numDisplayAngle; }
             set { numDisplayAngle = value; }
         }

         /// <summary>
         /// 有效数字
         /// </summary>
         public int  ValidNum
         {
             get { return validNum; }
             set { validNum = value; }
         }
         
        /// <summary>
        /// 大格标尺步长
        /// </summary>
         public float LargeScale
         {
             get { return largeScale; }
             set { largeScale = value; }
         }
         
        /// <summary>
        /// 小格标尺步长
        /// </summary>
         public float SmallScale
         {
             get { return smallScale; }
             set { smallScale = value; }
         }
         
        /// <summary>
        /// 大格标尺位置
        /// </summary>
         public RulePosition LargeRulePosition
         {
             get { return largeRulePosition; }
             set { largeRulePosition = value; }
         }

         /// <summary>
         /// 小格标尺位置
         /// </summary>
         public RulePosition SmallRulePosition
         {
             get { return smallRulePosion; }
             set { smallRulePosion = value; }
         }

         /// <summary>
         /// 字体颜色
         /// </summary>
         public Color FontColor
         {
             get { return fontColor; }
             set { fontColor = value; }
         }

         public override void Draw(Graphics g, BoreData data)
         {

             base.Draw(g, data);
   
            float totalDepth = Math.Abs(endDepth - beginDepth);
            DrawCenterLine(g);
            DrawLScale(g, totalDepth);
            DrawSScale(g, totalDepth);

            
         }

         private int GetTotalDepth(BoreData data)
         {
             return (int)(data.BasicBoreInfo.totalDepth - data.BasicBoreInfo.DrillZ);
         }

        /// <summary>
        /// 绘制小尺度标尺及数值
        /// </summary>
         private void DrawSScale(Graphics g,float totalDepth)
         {
             totalDepth = Math.Abs(totalDepth);
             float offsetLeft = 0; //标尺距离中心的左位移
             float offsetRight = 0; //标尺距离中心的右位移
             switch (this.smallRulePosion)
             {
                 case RulePosition.Left: offsetLeft = this.Width/15>7?7:this.Width / 15; break; //不大于7
                 case RulePosition.Right: offsetRight = this.Width /15> 7 ? 7 : this.Width/15; break;
                 case RulePosition.BothSides: offsetLeft = this.Width /15> 7 ? 7 : this.Width/15; offsetRight=this.Width/15 > 7 ? 7 : this.Width/15; break;
             }
             float center = this.Left + Width / 2;
             for (int i = 1; i < totalDepth / SmallScale; i++)
             {
                 g.DrawLine(new Pen(Color.Black), new PointF(center - offsetLeft, topOffset + i * smallScale*Height*19/20/totalDepth), new PointF(center + offsetRight, topOffset + i * smallScale*Height*19/20/totalDepth));
                 //位置=标尺起始高度+小标尺所在深度相对于总深度的百分比*标尺的实际深度（这里为总高度的4/5）
             }
         }

         /// <summary>
         /// 绘制大尺度标尺及数值
         /// </summary>
         private void DrawLScale(Graphics g,float totalDepth)
         {
             totalDepth = Math.Abs(totalDepth);
             float offsetLeft = 0; //标尺距离中心的左位移
             float offsetRight = 0; //标尺距离中心的右位移
             switch (this.smallRulePosion)
             {
                 case RulePosition.Left: offsetLeft = this.Width / 8 > 10 ? 10 : this.Width / 8; break; //不大于7
                 case RulePosition.Right: offsetRight = this.Width / 8> 10 ? 10 : this.Width / 8; break;
                 case RulePosition.BothSides: offsetLeft = this.Width / 8 > 10 ? 10 : this.Width / 8; offsetRight = this.Width / 15 > 7 ? 7 : this.Width / 15; break;
             }
             float center = this.Left + Width / 2;
             for (int i = 1; i < totalDepth / largeScale; i++)
             {
                 g.DrawLine(new Pen(Color.Black), new PointF(center - offsetLeft, topOffset + i * largeScale * Height * 19 / 20 / totalDepth), new PointF(center + offsetRight, topOffset + i * largeScale * Height * 19 / 20 / totalDepth));
                 //位置=标尺起始高度+小标尺所在深度相对于总深度的百分比*标尺的实际深度（这里为总高度的4/5）
                 float num=i*largeScale+Math.Abs(beginDepth);
                 SizeF strSize = g.MeasureString(num.ToString(), fontD);
                 g.DrawString(num.ToString(), fontD, new SolidBrush(fontColor), new PointF(center + offsetRight, topOffset + i * largeScale * Height * 19 / 20 / totalDepth-strSize.Height/2)); //取文本垂直位置为 横线位置 向上 半个字大小
             }
         }

         /// <summary>
         /// 绘制中线
         /// </summary>
         private void DrawCenterLine(Graphics g)
         {
             float center = this.Left + Width / 2;
             float heightBegin = this.Top + Height / 40;
             this.topOffset = heightBegin;
             float heightEnd = this.Top + Height * 39 / 40;
             g.DrawLine(new Pen(Color.Black), new PointF(center, heightBegin), new PointF(center, heightEnd));
         }
    }
}
