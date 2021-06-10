using System.Collections.Generic;

namespace BoreholeHistogram.BoreStyle
{
    /// <summary>
    /// 钻孔样式类
    /// </summary>
    public class BoreStyle
    {
        //子控件集合
        public List<Item> Items { get; set;}
        //布局
        public Layout BoreLayout;

        /// <summary>
        /// 钻孔样式类构造函数
        /// </summary>
        /// <param name="items">子控件集合</param>
        /// <param name="boreLayout">布局</param>
        public BoreStyle(List<Item> items, Layout boreLayout)
        {
            this.Items = items;
            this.BoreLayout = boreLayout;
        }

        public BoreStyle()
        {
        }
    }
}
