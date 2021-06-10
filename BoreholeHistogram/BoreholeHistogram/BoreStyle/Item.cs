using System.Collections.Generic;

namespace BoreholeHistogram.BoreStyle
{
    public class Item
    {
        //控件名，类型
        public string Name,Type;
        //控件参数
        public List<Column.Parameter> Parameters;
        //子控件
        public List<Item> Items;
        //当前控件占所在布局的位置
        public CtrLocation Location;

        //子控件布局
        public Layout ItemLayout;
        /// <summary>
        /// 控件构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类别</param>
        /// <param name="parameters">参数</param>
        /// <param name="location">当前控件占所在布局的位置</param>
        /// <param name="itemLayout">子控件布局</param>
        public Item(string name, string type, List<Column.Parameter> parameters, CtrLocation location,Layout itemLayout)
        {
            this.Name = name;
            this.Type = type;
            this.Parameters = parameters;
            this.Items = new List<Item>();
            this.ItemLayout = itemLayout;
            this.Location = location;
        }
    }
}
