namespace SmartGeoLogical.Render.Bore.BoreStyle
{
   /// <summary>
   /// 布局类型：水平、垂直、格网
   /// </summary>
    public enum LayOutType
    {
        Horizontal=1,
        Vertical=2,
        Grid=3
    }
    public class Layout
    {
        //布局类型：水平，垂直，格网
        public LayOutType Type;
        //各行各列所占整个布局的比例
        public float[] VerticalPercent, HorizontalPercent;
        /// <summary>
        /// 布局单位类型储存，1为px，0为相对
        /// </summary>
        public int[] VPunittype,HPunittype;

        public Layout()
        {
        }

        /// <summary>
        /// 布局构造函数
        /// </summary>
        /// <param name="type">布局类型</param>
        /// <param name="verticalPercent">各行所占布局比例，适用水平布局、格网布局</param>
        /// <param name="horizontalPercent">各列所占布局比例，适用垂直布局、格网布局</param>
        public Layout(LayOutType type, float[] verticalPercent, float[] horizontalPercent,int[] vpunittype,int[] hpunittype )
        {
            this.Type = type;
            this.VerticalPercent = verticalPercent;
            this.HorizontalPercent = horizontalPercent;
            this.VPunittype = vpunittype;
            this.HPunittype = hpunittype;
        }
    }
}
