using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BoreholeHistogram.Control.Text
{
    /// <summary>
    /// 文本类
    /// </summary>
    class TextFormat
    {
        #region 属性

        /// <summary>
        /// 文字字体
        /// </summary>
        public Font FontD { get { return fontD; } set { fontD = value; } }

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color FontColor { get { return fontColor; } set { fontColor = value; } }

        /// <summary>
        /// 文字位置
        /// </summary>
        public BoreStyle.AlignStyle FontAlign { get { return fontAlign; } set { fontAlign = value; } }

        #endregion

        #region 私有字段

        /// <summary>
        /// 文字字体
        /// </summary>
        private Font fontD;
        /// <summary>
        /// 文字颜色
        /// </summary>
        private Color fontColor;
        /// <summary>
        /// 文字位置
        /// </summary>
        private BoreStyle.AlignStyle fontAlign;

        #endregion

        #region 构造函数

        public TextFormat()
        {
            this.fontD = new Font("宋体",9);
            this.fontColor = Color.Black;
            this.fontAlign = BoreStyle.AlignStyle.CenterCenter;
        }

        public TextFormat(Font fontd, Color fontcolor, BoreStyle.AlignStyle fontalign)
        {
            this.fontD = fontd;
            this.fontColor = fontcolor;
            this.fontAlign = fontalign;
        }

        #endregion

        #region 封装方法

        /// <summary>
        /// 设置文字格式，主要是位置
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public StringFormat GetFormat(BoreStyle.AlignStyle align)
        {
            StringFormat stringFormat = new StringFormat();

            switch (align)
            {
                case BoreStyle.AlignStyle.NearNear:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case BoreStyle.AlignStyle.NearCenter:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case BoreStyle.AlignStyle.NearFar:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case BoreStyle.AlignStyle.CenterNear:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case BoreStyle.AlignStyle.CenterCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case BoreStyle.AlignStyle.CenterFar:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case BoreStyle.AlignStyle.FarNear:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case BoreStyle.AlignStyle.FarCenter:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case BoreStyle.AlignStyle.FarFar:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                default:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
            }
            return stringFormat;
        }

        #endregion
    }
}
