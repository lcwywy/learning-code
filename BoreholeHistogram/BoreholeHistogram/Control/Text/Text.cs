using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BoreholeHistogram.Control.Text
{
    /// <summary>
    /// 文本类
    /// </summary>
    class Text
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

        public Text()
        {
            this.fontD = new Font("宋体",9);
            this.fontColor = Color.Black;
            this.fontAlign = BoreStyle.AlignStyle.CenterCenter;
        }

        public Text(Font fontd, Color fontcolor, BoreStyle.AlignStyle fontalign)
        {
            this.fontD = fontd;
            this.fontColor = fontcolor;
            this.fontAlign = fontalign;
        }

        #endregion
    }
}
