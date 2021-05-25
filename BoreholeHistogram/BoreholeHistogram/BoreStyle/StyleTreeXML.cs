using System.Xml;
using System.Collections.Generic;

namespace SmartGeoLogical.Render.BoreStyle
{
    /// <summary>
    /// 样式树XML文件，封装对样式树的操作
    /// </summary>
    public class StyleTreeXML
    {

        /// <summary>
        /// 初始化一个XML文档
        /// </summary>
        public StyleTreeXML()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "");//XML文档声明
            xmlDocument.AppendChild(xmlDeclaration);
            XmlElement root = xmlDocument.CreateElement("StyleModel");
            xmlDocument.AppendChild(root);
        }

       /// <summary>
       /// 根据已知布局元素（Item）往XML文档添加布局元素
       /// </summary>
       /// <param name="item">已知布局元素</param>
       /// <param name="xmlDocument">XML文档</param>
       /// <param name="parentNode">插入节点</param>
       /// <returns></returns>
        public XmlNode CreateElement(Bore.BoreStyle.Item item, XmlDocument xmlDocument, XmlNode parentNode)
        {
            XmlNode headNode = this.AddNode(xmlDocument, parentNode, "Item");
            XmlElement HeadNode = (XmlElement)headNode;
            this.SetLocationAttribute(item.Location, HeadNode);//给Item节点设置location属性

            this.AddNode(xmlDocument, HeadNode, "name", item.Name);//在Item节点上添加name、type节点
            this.AddNode(xmlDocument, HeadNode, "type", item.Type);

            this.SetParameterAttribute(item.Parameters, xmlDocument, HeadNode);//设置参数节点

            this.SetSubElementLayout(item.ItemLayout,  xmlDocument, HeadNode);//设置子布局元素的布局值属性
            if (!item.Items.IsNullOrEmpty())
            {
                XmlNode SubElement = this.AddNode(xmlDocument, HeadNode, "Items");
                return SubElement;//若有下一级元素，返回下一个布局元素（若有）的首部节点
            }//添加下一个布局元素
            else return null;//若没有下一级元素，则返空值            
        }

        #region CreateElement方法所引用的方法

        /// <summary>
        /// 为父节点添加一个无文本子节点
        /// </summary>
        /// <param name="xmlDocument">XML文档</param>
        /// <param name="parentNode">插入节点</param>
        /// <param name="name">标签名</param>
        /// <returns>子节点，xmlnode类型</returns>
        public XmlNode AddNode(XmlDocument xmlDocument, XmlNode parentNode, string name)
        {
            XmlNode node = xmlDocument.CreateElement(name);
            parentNode.AppendChild(node);

            return node;
        }

        /// <summary>
        /// 重载，为父节点添加一个带文本的子节点
        /// </summary>
        /// <param name="xmlDocument">xml文档</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="name">标签名</param>
        /// <param name="text">标签内容</param>
        /// <returns>子节点，xmlnode类型</returns>
        public XmlNode AddNode(XmlDocument xmlDocument, XmlNode parentNode, string name, string text)
        {
            XmlNode node = xmlDocument.CreateElement(name);
            node.InnerText = text;
            parentNode.AppendChild(node);

            return node;
        }

        /// <summary>
        /// 给Item节点设置location属性，该属性为行、列、行跨、列跨
        /// </summary>
        /// <param name="location">位置属性</param>
        /// <param name="node">插入节点</param>
        public void SetLocationAttribute(Bore.BoreStyle.CtrLocation location, XmlElement node)
        {
            if (location.Row == -1) { }
            else node.SetAttribute("row", location.Row.ToString());

            if (location.Col == -1) { }
            else node.SetAttribute("col", location.Col.ToString());

            if (location.RowSpan <= 1) { }
            else node.SetAttribute("rowspan",location.RowSpan.ToString());

            if (location.ColSpan <= 1) { }
            else node.SetAttribute("colspan", location.ColSpan.ToString());
        }

      /// <summary>
      /// 给Item节点设置parameters子节点，再根据参数列设置次级子节点
      /// </summary>
      /// <param name="parameters">参数列</param>
      /// <param name="xmlDocument">xml文档</param>
      /// <param name="node">插入节点</param>
        public void SetParameterAttribute(List<Bore.Column.Parameter> parameters, XmlDocument xmlDocument, XmlElement node)
        {
            if (parameters.IsNullOrEmpty()) { }
            else
            {
                XmlElement node1 = (XmlElement)this.AddNode(xmlDocument, node, "parameters");
                foreach (Bore.Column.Parameter n in parameters)
                {
                    XmlElement node2 = (XmlElement)this.AddNode(xmlDocument, node1, "Parameter");
                    node2.SetAttribute("value", n.value.ToString());
                    node2.SetAttribute("name",n.key);
                }
            }
        }

        /// <summary>
        /// 插入（下一级元素的）布局节点及设置其布局属性
        /// </summary>
        /// <param name="layout">下一级元素布局</param>
        /// <param name="xmlDocument">XML文档</param>
        /// <param name="node">插入节点</param>
        public void SetSubElementLayout(Bore.BoreStyle.Layout layout, XmlDocument xmlDocument, XmlElement node)
        {
            if (layout == null) { }
            else{
                switch (layout.Type)
                {
                    case Bore.BoreStyle.LayOutType.Vertical:
                        XmlElement node1 = (XmlElement)this.AddNode(xmlDocument, node, "Layout", "垂直布局");
                        this.SetVPLayoutValue(layout, node1);
                        break;

                    case Bore.BoreStyle.LayOutType.Horizontal:
                        XmlElement node2 = (XmlElement)this.AddNode(xmlDocument, node, "Layout", "水平布局");
                        this.SetHPLayoutValue(layout, node2);
                        break;
                    case Bore.BoreStyle.LayOutType.Grid:
                        XmlElement node3 = (XmlElement)this.AddNode(xmlDocument, node, "Layout", "格网布局");
                        this.SetVPLayoutValue(layout, node3);
                        this.SetHPLayoutValue(layout, node3);
                        break;
                    default: break;
                }
            }           
        }

        /// <summary>
        /// 在垂直布局节点中设置垂直布局的属性
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="node"></param>
        public void SetVPLayoutValue(Bore.BoreStyle.Layout layout, XmlElement node)
        {
            string[] strList = new string[layout.HorizontalPercent.Length - 1];
            string value;
            for (int i = 1; i < layout.HorizontalPercent.Length; i++)
            {
                if (layout.HPunittype[i] == 0)
                {
                    strList[i - 1] = (layout.HorizontalPercent[i] * 100).ToString();
                }
                else if (layout.HPunittype[i] == 1)
                {
                    strList[i - 1] =layout.HorizontalPercent[i].ToString()+"px";
                }
            }
            value = string.Join(",",strList);
            node.SetAttribute("HorizontalPercent", value);
        }

        /// <summary>
        /// 在水平布局节点中设置水平布局的属性
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="node"></param>
        public void SetHPLayoutValue(Bore.BoreStyle.Layout layout, XmlElement node)
        {
            string[] strList = new string[layout.VerticalPercent.Length - 1];
            string value;
            for (int i = 1; i < layout.VerticalPercent.Length; i++)
            {
                if (layout.VPunittype[i] == 0)
                {
                    strList[i - 1] = (layout.VerticalPercent[i] * 100).ToString();
                }
                else if (layout.VPunittype[i] == 1)
                {
                    strList[i - 1] = layout.VerticalPercent[i].ToString() + "px";
                }
            }
            value = string.Join(",", strList);
            node.SetAttribute("VerticalPercent", value);
        }

        #endregion

        /// <summary>
        /// 保存样式树xml文件
        /// </summary>
        /// <param name="xmlDocument">xml文档</param>
        /// <param name="SavePath">保存路径</param>
        public void SaveStyleTreeXML(XmlDocument xmlDocument, string SavePath)
        {
            xmlDocument.Save(@"SavePath");
        }


    }
}
