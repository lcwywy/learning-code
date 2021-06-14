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

        public void LoadControl()
        { 
        
        }



        public Control.Control CreateControl(DrillGlobal drillGlobal, XmlDocument document, RectangleF parentRect)
        {
            Control.Control ctr ;

            return ctr;
        }

        #region 构建Control的方法

        private static Control.Control MakeControl(string name, string type)
        {
            switch (type.ToUpper())
            {
                case "PANEL": return MakePanel(name);
                case "LABEL": return MakeLabel(name);                                            
                case "SINGLETEXT": return MakeSingleText(name);              
                case "MULTIVALUEDTEXT": return MakeMultivaluedText(name);
                case "COMBINATORIALTEXT": return MakeCombinatorialText(name);
                case "LITHORITY": return MakeLithority(name);
                case "HISTOGRAM": return MakeHistogram(name);
                default: return MakeWithPlugin(name, type);
            }
        }

        private static Control.Control MakePanel(string name)
        {
            return new Control.Pannel.Pannel(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeLabel(string name)
        {
            return new Control.Label.Label(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeSingleText(string name)
        {
            return new Control.Text.SingleText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeMultivaluedText(string name)
        {
            return new Control.Text.MultivaluedText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeCombinatorialText(string name)
        {
            return new Control.Text.CombinatorialText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeLithority(string name)
        {
            return new Control.Fill.Lithority(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control.Control MakeHistogram(string name)
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
