using System.Drawing;
using SmartGeoLogical.Render.DrawInterface;

namespace SmartGeoLogical.Render.Bore.Column
{
   

    /// <summary>
    /// 填充类型，为Bar和Curve的聚集类
    /// </summary>
    public class FillType
    {
        //填充模式：单色填充，基于值的填充
        private FillMode figFillMode;
        //单填充色，基于值的填充色的起始、终止值
        private Color singleColor, valueBasedBegin, valueBasedEnd;
        /// <summary>
        /// 填充模式：单色填充/基于值的填充
        /// </summary>
        public FillMode FigFillMode
        {
            get { return figFillMode; }
            set { figFillMode = value; }
        }

        /// <summary>
        /// 单填充色
        /// </summary>
        public Color SingleColor
        {
            get { return singleColor; }
            set { singleColor = value; }
        }

        /// <summary>
        /// 基于值的填充起始色
        /// </summary>
        public Color ValueBasedBegin
        {
            get { return valueBasedBegin; }
            set { valueBasedBegin = value; }
        }

        /// <summary>
        /// 基于值的填充终止色
        /// </summary>
        public Color ValueBasedEnd
        {
            get { return valueBasedEnd; }
            set { valueBasedEnd = value; }
        }

        /// <summary>
        /// 填充类型构造函数
        /// </summary>
        /// <param name="figFillMode">填充模式，单填充/基于值的填充</param>
        /// <param name="singleColor">单填充色</param>
        /// <param name="valueBasedBegin">值填充起始色</param>
        /// <param name="valueBasedEnd">值填充终止色</param>
        public FillType(FillMode figFillMode,Color singleColor, Color valueBasedBegin, Color valueBasedEnd)
        {
            this.figFillMode = figFillMode;
            this.singleColor = singleColor;
            this.valueBasedBegin = valueBasedBegin;
            this.valueBasedEnd = valueBasedEnd;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FillType()
        {
            this.figFillMode = FillMode.ValueBased;
            this.singleColor = Color.Yellow;
            this.valueBasedEnd = Color.Red;
            this.valueBasedBegin = Color.Yellow;
        }
    }
}
