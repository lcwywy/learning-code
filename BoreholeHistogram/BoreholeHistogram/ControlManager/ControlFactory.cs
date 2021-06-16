using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Reflection;
using BoreholeHistogram.Control;
using BoreholeHistogram.BoreStyle;

namespace BoreholeHistogram.ControlManager
{
    public class ControlFactory
    {

        public void LoadControls(XmlDocument document)
        {
            var ctrList = new List<Control.Control>();
            XmlNode root = document.DocumentElement;
            

        }

        #region 根据样式加载控件的方法

        public void loadcontrol(List<Control.Control> ctrList, XmlNode node ,DrillGlobal drillGlobal, RectangleF rect)
        {
            string name = "", type;
            switch (node.Name)
            {
                case "Pannel":

                case "Label":
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "name")
                             name = childNode.InnerText;                  
                    }                                        
                    Control.Control ctr = CreateControl(drillGlobal, rect, name, "Label");
                    ctrList.Add(ctr);
                    break;

                default:
                    string labelName;
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "name")
                            name = childNode.InnerText;
                        if (childNode.Name == "Column")
                        {
                            foreach (XmlNode node1 in childNode.ChildNodes)
                            {

                            }
                        }
                        else if (childNode.Name == "Row")
                        { }
                    }
                    type = node.Name;
                    labelName = node.Attributes["name"].Value;
                    Control.Control ctr1 = CreateControl(drillGlobal, rect, name, type);
                    Control.Control ctr2 = CreateControl(drillGlobal, rect, labelName, "Label");

                    break;
            }
        }

        #endregion


        public Control.Control CreateControl(DrillGlobal drillGlobal, RectangleF parentRect, string name, string type)
        {
            Control.Control ctr = MakeControl(name, type, parentRect);

            return ctr;
        }

        #region 构建Control的方法

        private static Control.Control MakeControl(string name, string type, RectangleF rect)
        {
            switch (type.ToUpper())
            {
                case "PANEL": return MakePanel(name, rect);
                case "LABEL": return MakeLabel(name, rect);                                            
                case "SINGLETEXT": return MakeSingleText(name, rect);              
                case "MULTIVALUEDTEXT": return MakeMultivaluedText(name, rect);
                case "COMBINATORIALTEXT": return MakeCombinatorialText(name, rect);
                case "LITHORITY": return MakeLithority(name, rect);
                case "HISTOGRAM": return MakeHistogram(name, rect);
                default: return MakeWithPlugin(name, type);
            }
        }

        private static Control.Control MakePanel(string name, RectangleF rect)
        {
            return new Control.Pannel.Pannel(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeLabel(string name, RectangleF rect)
        {
            return new Control.Label.Label(rect, Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeSingleText(string name, RectangleF rect)
        {
            return new Control.Text.SingleText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeMultivaluedText(string name, RectangleF rect)
        {
            return new Control.Text.MultivaluedText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeCombinatorialText(string name, RectangleF rect)
        {
            return new Control.Text.CombinatorialText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeLithority(string name, RectangleF rect)
        {
            return new Control.Fill.Lithority(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeHistogram(string name, RectangleF rect)
        {
            return new Control.Chart.Histogram(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeWithPlugin(string name, string type)
        {
            Control.Control obj = null;
            string[] assemble = type.Split(new char[] { '.' });
            try
            {
                Assembly a = Assembly.LoadFrom(assemble[0] + ".dll");
                Type[] mytypes = a.GetTypes();
                Type ht = null; ;
                foreach (Type t in mytypes)
                    if (t.ToString() == type)
                        ht = t;
                object[] paras = new object[1];
                paras[0] = name;
                obj = (Control.Control)Activator.CreateInstance(ht, paras);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n插件加载错误");
            }
            return obj;
        }

        #endregion

    }
}