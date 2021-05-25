using System.Drawing;
using SmartGeoLogical.Render.DrawInterface;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 面板
    /// </summary>
    public class Panel:Control
    {
        /// <summary>
        /// 面板构造函数,默认边框黑色，底色为白，其他控件同
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Panel(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            :base(rect,outLineColor, fillColor, drawOutLine, fillInner,name)
        {
        }
    }
}
