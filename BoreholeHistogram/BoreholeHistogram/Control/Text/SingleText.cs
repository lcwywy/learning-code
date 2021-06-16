using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace BoreholeHistogram.Control.Text
{
    /// <summary>
    /// 单值文本
    /// </summary>
    class SingleText:Control
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
        /// 初始化单值文本控件的参数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        /// <param name="name"></param>
        public SingleText(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            text = new TextFormat();
        }

        #endregion

        #region 单值文本绘制

        /// <summary>
        /// 绘制单值文本
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        public override void Draw(Graphics g, BoreData.BoreData data)
        {
            base.Draw(g, data);

            string singleText;
            singleText = GetSingleText(data);

            g.DrawString(singleText, text.FontD, new SolidBrush(text.FontColor), Rect, text.GetFormat(text.FontAlign));
        }

        /// <summary>
        /// 从基本信息或自定义信息中获取单值文本
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetSingleText(BoreData.BoreData data)
        {
            Type t = data.BasicBoreInfo.GetType();
            foreach (FieldInfo field in t.GetFields())
                if (field.Name == this.CtrName)
                    return field.GetValue(data.BasicBoreInfo).ToString();

            foreach (BoreData.UserDefined userDef in data.UserDefinedBoreInfo.UserDefinedList)
            {
                if (userDef.UName == this.CtrName)
                {
                    return userDef.Value.ToString();
                }
            }
            return "";
        }

        #endregion

    }
}
