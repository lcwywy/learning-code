using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.Bore.Column
{
   
    /// <summary>
    /// 标签
    /// </summary>
    public class Label : Control
    {
        #region 私有字段

        private Font fontD;
        private Color fontColor;
        private AlignStyle align;
        private string displayTyoe;

        #endregion

        #region 属性

        /// <summary>
        /// 字体
        /// </summary>
        public Font FontD
        {
            get { return fontD; }
            set { fontD = value; }
        }

        /// <summary>
        /// 对齐方式
        /// </summary>
        public AlignStyle Align
        {
            get { return this.align; }
            set { this.align = value; }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }

        /// <summary>
        /// 信息显示方式
        /// </summary>
        public string DisplayTyoe 
        { 
            get => displayTyoe; set => displayTyoe = value;
        }

        #endregion

        /// <summary>
        /// 使用默认参数初始化标签对象
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Label(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            this.fontColor = Color.Black;//默认字体颜色黑色
            this.fontD = new Font("宋体", 8);//默认8号宋体
            this.align = AlignStyle.Center;//默认标签位置居中
        }

        public override  void Draw(Graphics g, BoreData data)
        {

            string labelText = "";
             base.Draw(g, data);
            
                switch (DisplayTyoe)
                {
                    case "值":
                        labelText = GetLabelData(data);
                        break;
                    case "文本、值":
                        labelText = this.CtrName + " : " + labelText; //动态文本
                        break;
                    default:
                        labelText = this.CtrName;
                        break;
                }

                Size s = GetStringSize(labelText, g);
                switch (this.align)
                {
                    case AlignStyle.Left:
                        g.DrawString(labelText, this.fontD, new SolidBrush(this.fontColor), this.Left, this.Top + (this.Height - s.Height) / 2);                               //左
                        break;
                    case AlignStyle.Center:
                        g.DrawString(labelText, this.fontD, new SolidBrush(this.fontColor), this.Left + (this.Width - s.Width) / 2, this.Top + (this.Height - s.Height) / 2); //居中显示
                        break;
                    case AlignStyle.Right:
                        g.DrawString(labelText, this.fontD, new SolidBrush(this.fontColor), this.Left + this.Width - s.Width, this.Top + (this.Height - s.Height) / 2); //右
                        break;
                    case AlignStyle.CenterCenter:
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        RectangleF rectangleF = new RectangleF(this.Left, this.Top, this.Width, this.Height);
                        //labelText = labelText + System.Environment.NewLine + labelText;
                        g.DrawString_2(labelText, this.fontD, new SolidBrush(this.fontColor), rectangleF, stringFormat);

                        break;
                }
        }

        private string GetLabelData(BoreData data)
        {
            Type t = data.BasicBoreInfo.GetType();
            foreach (FieldInfo field in t.GetFields())
                if (field.Name == this.CtrName)
                    return field.GetValue(data.BasicBoreInfo).ToString();


            string filedName = "";
           
            foreach (UserDefined userDef in data.UserDefinedBoreInfo.UserDefinedList)
            {
                if (userDef.UName == this.CtrName)
                { 
                    return userDef.Value.ToString();
                }
            }
            return "";
        }

        //测量标号为i的字符串
        public Size GetStringSize(string str, Graphics g)
        {
            
            return g.MeasureString(str, fontD).ToSize();
        }
        
    }
}
