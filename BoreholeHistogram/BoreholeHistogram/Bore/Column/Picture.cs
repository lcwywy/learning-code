using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 图片
    /// </summary>
    public class Picture:Control
    {
        /// <summary>
        /// 图片构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Picture(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner,name)
        {
        }

        public override void Draw(Graphics g, BoreData data)
        {
           
            var legendPath = System.AppDomain.CurrentDomain.BaseDirectory + $"DrillSet{Path.DirectorySeparatorChar}{this.CtrName}.jpg";
            Image img = Bitmap.FromFile(legendPath);
            g.DrawImage(img, new RectangleF(this.Left, this.Top, this.Width, this.Height));
            base.Draw(g, data);

        }
    }
}
