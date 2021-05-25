using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace SmartGeoLogical.Render.Bore.Sys
{
    /// <summary>
    /// 样式加载器
    /// </summary>
    public class StyleLoader
    {
        //私有BoreStyle类型 style
        private BoreStyle.BoreStyle style;
        
        /// <summary>
        /// 属性 BoreStyle类型 Style，并设置为只读
        /// </summary>
        public BoreStyle.BoreStyle Style
        {
            get { return style; }
        }

        /// <summary>
        /// 初始化样式器—将该类属性Style初始化
        /// </summary>
        public StyleLoader()
        {
            this.style = new Bore.BoreStyle.BoreStyle();
        }

        /// <summary>
        ///  加载控件
        /// </summary>
        /// <param name="dataFile"></param>
        public void Load(string dataFile)
        {
            XmlImplementation objXImp = new XmlImplementation();
            XmlDocument doc = objXImp.CreateDocument();

            doc.Load(dataFile);

            XmlNode node = doc.GetElementsByTagName("Layout")[0];         
            style.BoreLayout= LoadLayout(node);
            
            XmlNode node1 = doc.GetElementsByTagName("Items")[0];
            style.Items= LoadItems(node1);
        }

        #region 加载柱状图样式所引用的方法

        /// <summary>
        /// 加载子控件，通过节点获取子节点。
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<BoreStyle.Item>  LoadItems(XmlNode node)
        {
            
            List<BoreStyle.Item> items = new List<Bore.BoreStyle.Item>();
            foreach (XmlNode n in node.ChildNodes)
            {
                BoreStyle.Item item = LoadItem(n);
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 加载布局,根据布局属性设置控件布局，获取行列划分百分比并返回一个Layout对象
        /// </summary>
        /// <param name="node"></param>
        private BoreStyle.Layout LoadLayout(XmlNode node)
        {
            
            string layout = node.InnerText;
            BoreStyle.LayOutType layoutType=BoreStyle.LayOutType.Vertical;
            float[] verticalPercent, horizontalPercent;
            int[] vpunittype, hpunittype;//声明并赋空值
            vpunittype = hpunittype = null;
            verticalPercent = horizontalPercent = null;

            //根据布局属性设置的控件布局
            switch (layout)
            {
                case "垂直布局": layoutType = BoreStyle.LayOutType.Vertical; break;
                case "水平布局": layoutType = BoreStyle.LayOutType.Horizontal; break;
                case "格网布局": layoutType = BoreStyle.LayOutType.Grid; break;
                default: break;
            }

            //获取 行/列划分 百分比
           XmlAttribute verticalAtt = node.Attributes["VerticalPercent"];
           XmlAttribute horizontalAtt = node.Attributes["HorizontalPercent"];
           if (verticalAtt != null)
           {
               string[] strs = verticalAtt.Value.Split(new char[] { ',' });
               verticalPercent = new float[strs.Length+1];
              
                vpunittype = new int[strs.Length + 1];//单位类型

                verticalPercent[0] = 0;
                vpunittype[0] = 0;//第一值设为0，但是不从第一个值算起
                for (int i = 0; i < strs.Length; i++)
               {
                    if (strs[i].Contains("px"))
                    {
                        verticalPercent[i + 1] = float.Parse(strs[i].Replace("px", ""));
                        vpunittype[i + 1] = 1;
                    }
                    else
                    {
                        float last = verticalPercent[i];
                        verticalPercent[i + 1] =  float.Parse(strs[i]) / 100;
                        vpunittype[i + 1] = 0;
                    }
                }
           }

           if (horizontalAtt != null)
           {
               string[] strLst =horizontalAtt.Value.Split(new char[] { ',' });
               horizontalPercent = new float[strLst.Length+1];
              
                hpunittype = new int[strLst.Length + 1];//单位类型
               
                horizontalPercent[0] = 0;
                hpunittype[0] = 0;//第一值设为0，但是不从第一个值算起
                for (int i = 0; i < strLst.Length; i++)
               {
                    if (strLst[i].Contains("px"))
                    {
                        horizontalPercent[i + 1] = float.Parse(strLst[i].Replace("px", ""));
                        hpunittype[i + 1] = 1;
                    }
                    else
                    {
                        horizontalPercent[i + 1] =  float.Parse(strLst[i]) / 100;
                        hpunittype[i + 1] = 0;
                    }
                }
           }

           return new Bore.BoreStyle.Layout(layoutType,verticalPercent,horizontalPercent, vpunittype, hpunittype);
           
        }

        /// <summary>
        /// 加载 Item
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private BoreStyle.Item LoadItem(XmlNode node)
        {
            string itemName, itemType;
            itemName = itemType = "";
            List<Column.Parameter> paras = new List<Bore.Column.Parameter>();
            BoreStyle.CtrLocation location = null;
            BoreStyle.Layout layout = new Bore.BoreStyle.Layout();
            List<BoreStyle.Item> items = new List<Bore.BoreStyle.Item>();

            // 设置 Item的 属性
            foreach (XmlNode n in node.ChildNodes)
            {
                switch (n.Name)
                {
                    case "name": itemName = n.InnerText; break;
                    case "type": itemType = n.InnerText; break;
                    case "parameters": paras = GetItemParameter(n); break;
                    case "Layout": layout = LoadLayout(n); break;
                    case "Items": items = LoadItems(n); break;
                    default: break;
                }
            }

            //获取位置参数
            location = GetItemLocation(node);

            BoreStyle.Item item=new  Bore.BoreStyle.Item(itemName, itemType, paras, location, layout);
            item.Items = items;
            return item;
        }
       
        /// <summary>
        /// 获取 Item属性
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<Column.Parameter> GetItemParameter(XmlNode node)
        {
            //对每一个parameter结点
            List<Column.Parameter> parameters = new List<Bore.Column.Parameter>();
            foreach (XmlNode n in node.ChildNodes)
            {
                Column.Parameter parameter = new Bore.Column.Parameter(n.Attributes["name"].Value,n.Attributes["value"].Value);
                parameters.Add(parameter);
            }
            return parameters;
        }

        //private BoreStyle.Layout GetItemLayout(XmlNode node)
        //{
        ////    return null;
        //}

        /// <summary>
        /// 根据 Item的位置属性设置 Item的 行，列，行跨度，列跨度等
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private BoreStyle.CtrLocation GetItemLocation(XmlNode node)
        {
            int row, col, rowspan, colspan;
            row = col = -1;
            rowspan = colspan = 1; //初始化

             // 设值
            XmlAttribute rowAtt = node.Attributes["row"];
            XmlAttribute colAtt = node.Attributes["col"];
            XmlAttribute rowSpanAtt = node.Attributes["rowspan"];
            XmlAttribute colSpanAtt = node.Attributes["colspan"];

            if (rowAtt != null)
                row = int.Parse(rowAtt.Value);
            if (colAtt != null)
                col = int.Parse(colAtt.Value);
            if (rowSpanAtt != null)
                rowspan = int.Parse(rowSpanAtt.Value);
            if (colSpanAtt != null)
                colspan = int.Parse(colSpanAtt.Value);

            return new Bore.BoreStyle.CtrLocation(row, col, colspan, rowspan);
        }

        #endregion
    }
}
