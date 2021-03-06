using System;
using System.Drawing;

namespace BoreholeHistogram.Control.Label
{
    /// <summary>
    /// 标签类
    /// </summary>
    class Label :Control
    {
        #region 属性
        
        /// <summary>
        /// 标签文本
        /// </summary>
        public Text.TextFormat Text { get { return text; } set { text = value; } }

        #endregion

        #region 私有字段
        
        /// <summary>
        /// 标签文本
        /// </summary>
        private Text.TextFormat text;

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 初始化标签的参数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        /// <param name="name"></param>
        public Label(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            :base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            text = new Text.TextFormat();
        }

        #endregion

        #region 标签绘制

        /// <summary>
        /// 绘制标签
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        public override void Draw(Graphics g, BoreData.BoreData data)
        {
            //绘制矩形框
            base.Draw(g, data);

            //标签文本
            string labelText;
            labelText = this.CtrName;

            g.DrawString(labelText, text.FontD, new SolidBrush(text.FontColor), Rect, text.GetFormat(text.FontAlign));   //暂时没考虑下标签有些文本换行问题
        }

        #endregion

    }
}
