using System;
using System.Drawing;

namespace BoreholeHistogram.Control.Label
{
    class Label :Control
    {
        #region 属性
        
        /// <summary>
        /// 标签文本
        /// </summary>
        public Text.Text LabelText { get { return labelText; } set { labelText = value; } }

        #endregion

        #region 私有字段
        
        /// <summary>
        /// 标签文本
        /// </summary>
        private Text.Text labelText;

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 标签构造函数，初始化一个标签的属性设置
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
            labelText = new Text.Text();
        }

        #endregion

        #region 标签绘制

        public override void Draw(Graphics g, BoreData.BoreData data)
        {
            base.Draw(g, data);
        }

        #endregion

    }
}
