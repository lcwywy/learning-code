using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using BoreholeHistogram.BoreData;

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

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化多值文本的参数
        /// </summary>
        public MultivaluedText()
        {
            text = new TextFormat(new Font("宋体",8), Color.Black, BoreStyle.AlignStyle.CenterCenter);
        }

        #endregion

        #region 绘制多值文本

        public override void Draw(Graphics g, BoreData.BoreData data)
        {
            base.Draw(g, data);

            List<LayerInfo> layers = data.LayerInfoList;


        }

        #endregion
    }
}
