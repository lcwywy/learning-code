using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartGeoLogical.Render.DrillSectionDraw.Draw.View
{

    /// <summary>
    /// 表达模型中点的数据结构
    /// </summary>
    class VPoint
    {
        public double X;//横坐标
        public int Y;//纵坐标
    }

    /// <summary>
    /// 表达模型中刻度线的数据结构
    /// </summary>
    class VLine
    {
        public VPoint StartP;
        public VPoint EndP;
        //记录刻度线的真实值
        public string attribute;
    }
    class ViewSection
    {
        /// <summary>
        /// 剖面表达模型
        /// 在这里因为模型的设计想法还不够成熟，只是在这里进行了试验，
        /// 没有将剖面表达模型全部设计完，只是做了部分进行设计，
        /// 其中地层表达还是使用逻辑模型进行表达
        /// </summary>
        private ViewScale LeftViewScale;
        private ViewScale RightViewScale;
        private ViewLegend VLegend;

        private int xScale;
        private int yScale;

    }
}
