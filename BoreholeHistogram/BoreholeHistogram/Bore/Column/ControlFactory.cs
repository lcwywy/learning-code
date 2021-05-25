using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.Bore;
namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 控件工厂
    /// </summary>
    public class ControlFactory
    {
       
       /// <summary>
       /// 通过Item创建相应的控件
       /// </summary>
       /// <param name="drillGlobal">全局变量</param>
       /// <param name="item">布局元素</param>
       /// <param name="parentLayout">父级元素布局信息</param>
       /// <param name="parentRect">父级矩形框</param>
       /// <returns>控件</returns>
        public static Control CreateControl(DrillGlobal drillGlobal, BoreStyle.Item item, BoreStyle.Layout parentLayout, RectangleF parentRect)
        {
           Control ctr= MakeControl(item.Name,item.Type);
           SetParameter(ctr,item.Parameters);

           RectangleF rect=CalculateClienteRect(parentLayout, item.Location, parentRect);
         
           //将列控件的开始高度和深度存入全局变量，为鼠标移动事件自动改变状态做基础
           if (item.Type.ToUpper() == "DEPTH" || item.Type.ToUpper() == "LITHORITY" || item.Type.ToUpper() == "CURVE" || item.Type.ToUpper() == "BAR")
           {
                drillGlobal.columnBeginDepth = rect.Top;
               drillGlobal.columnHeight = rect.Height;
           }
           ctr.Left = rect.Left;
           ctr.Top = rect.Top;
           ctr.Width = rect.Width;
           ctr.Height = rect.Height;

           return ctr;
        }

        /// <summary>
        /// 根据元素位置、上级元素布局信息及其所在的矩形框，得到该元素所在的矩形框
        /// </summary>
        /// <param name="parentLayout">上级元素对其子元素集合的布局</param>
        /// <param name="ctrLocation">该矩形框相对于上级元素的位置</param>
        /// <param name="parentRect">上级元素所在的矩形框</param>
        /// <returns>矩形框</returns>
        private static RectangleF CalculateClienteRect(BoreStyle.Layout parentLayout, BoreStyle.CtrLocation ctrLocation, RectangleF parentRect)
        {
            switch (parentLayout.Type)
            {
                case BoreStyle.LayOutType.Horizontal: return GetHorizontalRect(parentLayout,ctrLocation,parentRect);
                case BoreStyle.LayOutType.Vertical: return GetVerticalRect(parentLayout, ctrLocation, parentRect);
                case BoreStyle.LayOutType.Grid: return GetGridRect(parentLayout, ctrLocation, parentRect);
                default: return new RectangleF();
            }
           
        }

        private static RectangleF GetGridRect(BoreStyle.Layout parentLayout, BoreStyle.CtrLocation ctrLocation, RectangleF parentRect)
        {
            float widthPercen = GetPercentLength(parentLayout.HorizontalPercent, parentRect.Width);
            float heightPercen = GetPercentLength(parentLayout.VerticalPercent, parentRect.Height);


            float left = parentRect.Left + parentLayout.HorizontalPercent[ctrLocation.Col] * parentRect.Width;
            float top = parentRect.Top + parentLayout.VerticalPercent[ctrLocation.Row] * parentRect.Height;
            float width = GetLocation(parentLayout.HorizontalPercent, widthPercen, ctrLocation.Col + ctrLocation.ColSpan)- GetLocation(parentLayout.HorizontalPercent, widthPercen, ctrLocation.Col);// parentRect.Width * (parentLayout.HorizontalPercent[ctrLocation.Col + ctrLocation.ColSpan] - parentLayout.HorizontalPercent[ctrLocation.Col]);
            float height = GetLocation(parentLayout.VerticalPercent, heightPercen, ctrLocation.Row + ctrLocation.RowSpan)- GetLocation(parentLayout.VerticalPercent, heightPercen, ctrLocation.Row);// parentRect.Height*(parentLayout.VerticalPercent[ctrLocation.Row + ctrLocation.RowSpan] - parentLayout.VerticalPercent[ctrLocation.Row]);
            return new RectangleF(left,top,width,height);
        }

        private static RectangleF GetVerticalRect(BoreStyle.Layout parentLayout, BoreStyle.CtrLocation ctrLocation, RectangleF parentRect)
        {
            //if (ctrLocation.Col >= parentLayout.HorizontalPercent.Length - 1)
            //    ctrLocation.Col -= 1;

        
            float widthPercen = GetPercentLength(parentLayout.HorizontalPercent, parentRect.Width);


            float left = parentRect.Left + GetLocation(parentLayout.HorizontalPercent, widthPercen, ctrLocation.Col);//parentLayout.HorizontalPercent[ctrLocation.Col] * parentRect.Width;
            float top = parentRect.Top;
            float width =  GetLocation(parentLayout.HorizontalPercent, widthPercen, ctrLocation.Col+1) - GetLocation(parentLayout.HorizontalPercent, widthPercen, ctrLocation.Col);
            float height = parentRect.Height;
            return new RectangleF(left, top, width, height);
        }

        private static RectangleF GetHorizontalRect(BoreStyle.Layout parentLayout, BoreStyle.CtrLocation ctrLocation, RectangleF parentRect)
        {
            float width = parentRect.Width;

            float heightPercen = GetPercentLength(parentLayout.VerticalPercent, parentRect.Height);

            float left= parentRect.Left;
            float top = parentRect.Top + GetLocation(parentLayout.VerticalPercent, heightPercen, ctrLocation.Row);//parentLayout.VerticalPercent[ctrLocation.Row] * parentRect.Height;   // parentLayout.VerticalPercent[ctrLocation.Row] 为其在该布局中所占的位置（百分比）
            float height =  GetLocation(parentLayout.VerticalPercent, heightPercen, ctrLocation.Row + 1)- GetLocation(parentLayout.VerticalPercent, heightPercen, ctrLocation.Row);//(parentLayout.VerticalPercent[ctrLocation.Row + 1] - parentLayout.VerticalPercent[ctrLocation.Row]); //parentLayout.VerticalPercent[ctrLocation.Row+1] 为后一控件在布局中的位置
            return new RectangleF(left, top, width, height);
        }
        /// <summary>
        /// 总长度减去固定长度
        /// </summary>
        /// <param name="PercentOrPixel"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static float GetPercentLength(float[] PercentOrPixel, float length)
        {
            for (int i = 0; i < PercentOrPixel.Length; i++)
            {
                if (PercentOrPixel[i] > 1)
                {
                    length -= PercentOrPixel[i];
                }
            }

            return length;
        }
        private static float GetLocation(float[] PercentOrPixel,float length,int index) 
        {
        
            float location = 0;
            for (int i = 0; i < PercentOrPixel.Length; i++)
            {
                if (index < i)
                {
                    break;
                }
                if (PercentOrPixel[i]>1)
                {
                    location += PercentOrPixel[i];
                }
                else
                {
                     location += PercentOrPixel[i] * length;
                }
            }
            return location;
        }

        /// <summary>
        /// 根据传递的名称，类型构建控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeControl(string name, string type)
        {
            switch (type.ToUpper())
            {
                case "LABEL": return MakeLabel(name);
                case "DEPTH": return MakeDepth(name); 
                case "CURVE": return MakeCurve(name);
                case "PANEL": return MakePanel(name); 
                case "BAR": return MakeBar(name); 
                case "BARVALUE": return MakeBarValue(name); 
                case "LITHORITY": return MakeLithority(name);
                case "PICTURE": return MakePicture(name); 
                case "TEXT": return MakeText(name);
                case "DESCRIPTION": return MakeDescription(name);
                case "WATEROILDESCRIPTION": return MakeWaterOilDes(name);
                case "PLOTHEAD": return MakePlotHead(name);
                case "LITHTEXT": return MakeLithText(name);
                case "BEGINTIME": return MakeBeginTime(name);
                default: return MakeWithPlugin(name, type); 
            }
        }

        private static Control MakeBeginTime(string name)
        {
            return new BeginTime(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control MakeLithText(string name)
        {
            return new LithText(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control MakeWaterOilDes(string name)
        {
            return new WaterOilDescription(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        private static Control MakePlotHead(string name)
        {
            return new PlotHead(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造描述控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeDescription(string name)
        {
            return new Description(new RectangleF(), Color.Blue, Color.White,true, false, name);
        }

        /// <summary>
        /// 构造文本控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeText(string name)
        {
            return new Text(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造图片控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakePicture(string name)
        {
            return new Picture(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造岩性纹理控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeLithority(string name)
        {
            return new Lithority(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造填充图值控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeBarValue(string name)
        {
            return new BarValue(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造填充图
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeBar(string name)
        {
            return new Bar(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造面板
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakePanel(string name)
        {
            return new Panel(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造曲线
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeCurve(string name)
        {
            return new Curve(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造深度控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeDepth(string name)
        {
            return new Depth(new RectangleF(), Color.Blue, Color.White, true, false, name);
        }

        /// <summary>
        /// 构造文本
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeLabel(string name)
        {
           return new Label(new RectangleF(), Color.Blue, Color.White, true,false, name);
           // throw new Exception();
        }

        /// <summary>
        /// 通过插件构造控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Control MakeWithPlugin(string name,string type)
        {
            Control obj = null;
            string[] assemble = type.Split(new char[] { '.' });
            try
            {
                Assembly a = Assembly.LoadFrom(assemble[0]+ ".dll");
                Type[] mytypes = a.GetTypes();
                Type ht = null; ;
                foreach (Type t in mytypes)
                    if (t.ToString() == type)
                        ht = t;
                object[] paras = new object[1];
                paras[0] = name;
                obj = (Control)Activator.CreateInstance(ht, paras);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n插件加载错误");
            }
            return obj;
        }

        /// <summary>
        /// 改变控件部分默认参数
        /// </summary>
        /// <param name="ctr"></param>
        /// <param name="parameters"></param>
        private static void SetParameter(Control ctr, List<Parameter> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
                SetProperty(ctr, parameters[i]);
        }

        /// <summary>
        /// 设置单个参数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameter"></param>
        private static void SetProperty(object obj, object parameter)
        {
            Parameter para = (Parameter)parameter;
            //目标底层对象
            object objclass = obj; //初始化为传过来的对象
            //目标底层属性
            PropertyInfo objProperty;

            Type t;
            string key = para.key;
            object value = para.value;
            string[] path = key.Split(new char[] { '.' });
            //找到最底层属性，例如属性的key值为Bar下的 BarGridStyle.DrawHorizontalGrid
            //则找到 BarGridStyle的子属性DrawHorizontalGrid
            //最终把需要设置的对象和属性赋给 objclass与objProperty
            for (int i = 0; i < path.Length - 1; i++)
            {
                t = objclass.GetType();
                objProperty = t.GetProperty(path[i]);
                objclass = objProperty.GetValue(objclass, null);
            }

            //设置底层属性
            PropertyInfo prop = objclass.GetType().GetProperty(path[path.Length - 1]);
            //根据属性的不同类型设值

            value = GetValueByType(value, prop.PropertyType.ToString());

            prop.SetValue(objclass, value, null);
        }

        /// <summary>
        /// 根据不同的类型将value转化为相应的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object GetValueByType(object value, string type)
        {
            switch (type)
            {
                case "System.Int32": return int.Parse(value.ToString());
                case "System.Single" :return float.Parse(value.ToString());
                case "System.Boolean": return Boolean.Parse(value.ToString());
                case "System.Drawing.Font": return GetFont(value);
                case "System.Drawing.Color": return Color.FromName(value.ToString());
                case "System.String": return value;
                default: return int.Parse(value.ToString());//针对枚举等类型
            }
        }

       

        /// <summary>
        /// 构造字体
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object GetFont(object value)
        {
            string fontFamily="";
            int fontSize=8;
            string[] strLst = value.ToString().Split(new char[] { ','});
            fontFamily = strLst[0];
            fontSize = int.Parse(strLst[1]);
            return new Font(fontFamily,fontSize);
        }

    }
}
